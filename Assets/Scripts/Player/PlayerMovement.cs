using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Grounded Movement")]
    [SerializeField] float floorOffsetY;
    [SerializeField] float moveSpeed = 6f;
    [SerializeField] float rotationSpeed = 10f;

    [Header("Jump Movement")]
    [SerializeField] float jumpPower;

    [Header("Slope Tolerances")]
    [SerializeField] float slopeLimit = 45f;
    [SerializeField] float slopeInfluence = 5f;

    //Refs
    Rigidbody rb;
    Animator animator;

    //Input
    float vertical, horizontal;
    bool isJumpPressed;

    //Base movement
    Vector3 moveDir;
    float inputAmount;
    
    //Grounded/Jumping
    Vector3 raycastFloorPos;
    Vector3 floorMovement;
    Vector3 gravity;
    Vector3 combinedRaycast;
    float jumpFallOff = 2f;

    //Slopes
    float slopeAmount;
    Vector3 floorNormal;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        moveDir = Vector3.zero;

        //Grab Inputs
        GetInput();

        CalculateInput();

        //Movement is applied to the rigidbody in FixedUpdate

        ApplyRotation();

        if (isJumpPressed)
        {
            Jump();
        }

        UpdateAnimator();
    }



    private void FixedUpdate()
    {
        //Apply gravity if not grounded
        if (!IsGrounded() || slopeAmount >= 0.1f)
        {
            gravity += Vector3.up * Physics.gravity.y * jumpFallOff * Time.deltaTime;
        }

        //Apply the movement to the rigidbody with some gravity
        rb.velocity = (moveDir * GetMoveSpeed() * inputAmount) + gravity;

        floorMovement = new Vector3(rb.position.x, FindFloor().y + floorOffsetY, rb.position.z);

        //Stick to the floor if we're grounded
        if (floorMovement != rb.position && IsGrounded() && rb.velocity.y <= 0)
        {
            rb.MovePosition(floorMovement);
            gravity.y = 0;
        }
    }

    private void GetInput()
    {
        vertical = Input.GetAxis("Vertical");
        horizontal = Input.GetAxis("Horizontal");
        isJumpPressed = Input.GetKeyDown(KeyCode.Space);
    }

    private float GetMoveSpeed()
    {
        float currentMoveSpeed = Mathf.Clamp(moveSpeed + (slopeAmount * slopeInfluence), 0, moveSpeed + 1);
        return currentMoveSpeed;
    }

    private void ApplyRotation()
    {
        //Apply Rotations
        Quaternion rot = Quaternion.LookRotation(moveDir);
        Quaternion targetRot = Quaternion.Slerp(transform.rotation, rot, Time.fixedDeltaTime * inputAmount * rotationSpeed);
        transform.rotation = targetRot;
    }

    private void CalculateInput()
    {
        //Correct the inputs
        Vector3 correctedVert = vertical * Camera.main.transform.forward;
        Vector3 correctedHorizon = horizontal * Camera.main.transform.right;

        Vector3 combinedInput = correctedVert + correctedHorizon;

        moveDir = new Vector3((combinedInput).normalized.x, 0, (combinedInput).normalized.z);

        float inputMag = Mathf.Abs(horizontal) + Mathf.Abs(vertical);
        inputAmount = Mathf.Clamp01(inputMag);
    }

    private void Jump()
    {
        if (IsGrounded())
        {
            gravity.y = jumpPower;
            animator.SetTrigger("Jump");
        }
    }

    private bool IsGrounded()
    {
        if (FloorRaycasts(0, 0, 0.6f) != Vector3.zero)
        {
            slopeAmount = Vector3.Dot(transform.forward, floorNormal);
            return true;
        }
        return false;
    }

    private Vector3 FindFloor()
    {
        float raycastWidth = 0.25f;
        int floorAverage = 1;

        combinedRaycast = FloorRaycasts(0, 0, 1.6f);
        floorAverage +=
            GetFloorAverage(raycastWidth, 0) + GetFloorAverage(-raycastWidth, 0) + GetFloorAverage(0, raycastWidth) + GetFloorAverage(0, -raycastWidth);

        return combinedRaycast / floorAverage;
    }

    private int GetFloorAverage(float offsetX, float offsetZ)
    {
        if (FloorRaycasts(offsetX, offsetZ, 1.6f) == Vector3.zero) { return 0; }

        combinedRaycast += FloorRaycasts(offsetX, offsetZ, 1.6f);
        return 1;
    }

    private Vector3 FloorRaycasts(float offsetX, float offsetZ, float raycastLength)
    {
        raycastFloorPos = transform.TransformPoint(0 + offsetX, 0 + 0.5f, 0 + offsetZ);

        Debug.DrawRay(raycastFloorPos, Vector3.down, Color.magenta);

        if (Physics.Raycast(raycastFloorPos, -Vector3.up, out RaycastHit hit, raycastLength))
        {
            floorNormal = hit.point;

            if (Vector3.Angle(floorNormal, Vector3.up) < slopeLimit)
            {
                return hit.point;
            }
            else
            {
                return Vector3.zero;
            }
        }
        else
        {
            return Vector3.zero;
        }
    }

    private void UpdateAnimator()
    {
        animator.SetFloat("forwardSpeed", inputAmount, 0.2f, Time.deltaTime);
        animator.SetFloat("slopeNormal", slopeAmount, 0.2f, Time.deltaTime);
    }
}
