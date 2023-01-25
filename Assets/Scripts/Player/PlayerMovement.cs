using System;
using System.Collections;
using System.Collections.Generic;
using SpyroClone.Combat;
using UnityEngine;

namespace SpyroClone.Player
{
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


        //Movement Modes
        public enum MovementControlType
        {
            Normal, LockedOn
        }
        MovementControlType movementControlType;
        bool isLockedOn;

        //Refs
        Rigidbody rb;
        Animator animator;
        TargetTracker targetTracker;

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
        readonly float jumpFallOff = 2f;

        //Slopes
        float slopeAmount;
        Vector3 floorNormal;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            animator = GetComponent<Animator>();
            targetTracker = GetComponent<TargetTracker>();
        }

        // Update is called once per frame
        void Update()
        {
            moveDir = Vector3.zero;

            //Grab Inputs
            vertical = Input.GetAxis("Vertical");
            horizontal = Input.GetAxis("Horizontal");
            isJumpPressed = Input.GetKeyDown(KeyCode.Space);

            //Correct the inputs
            Vector3 correctedVert = vertical * Camera.main.transform.forward;
            Vector3 correctedHorizon = horizontal * Camera.main.transform.right;

            Vector3 combinedInput = correctedVert + correctedHorizon;

            moveDir = new Vector3((combinedInput).normalized.x, 0, (combinedInput).normalized.z);

            float inputMag = Mathf.Abs(horizontal) + Mathf.Abs(vertical);
            inputAmount = Mathf.Clamp01(inputMag);

            //Movement is applied to the rigidbody in FixedUpdate

            if (isJumpPressed)
            {
                Jump();
            }

            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                SetTargetting();
            }

            animator.SetFloat("forwardSpeed", inputAmount, 0.2f, Time.deltaTime);
            animator.SetFloat("slopeNormal", slopeAmount, 0.2f, Time.deltaTime);
        }



        private void FixedUpdate()
        {
            //Apply gravity if not grounded
            if (!IsGrounded() || slopeAmount >= 0.1f)
            {
                gravity += Vector3.up * Physics.gravity.y * jumpFallOff * Time.fixedDeltaTime;
            }

            switch (movementControlType)
            {
                case MovementControlType.Normal:
                    Vector3 targetDirNormal = moveDir;
                    if (moveDir == Vector3.zero)
                    {
                        targetDirNormal = transform.forward;
                    }
                    targetDirNormal.y = 0;

                    Quaternion rot = Quaternion.LookRotation(targetDirNormal);
                    Quaternion targetRot = Quaternion.Slerp(transform.rotation, rot, Time.fixedDeltaTime * inputAmount * rotationSpeed);
                    transform.rotation = targetRot;
                    break;
                case MovementControlType.LockedOn:
                    Vector3 targetDirLocked = Vector3.zero;
                    if (targetTracker.GetActiveTarget() != null)
                    {
                        targetDirLocked = targetTracker.GetActiveTarget().transform.position - transform.position;
                    }
                    else
                    {
                        targetDirLocked = Camera.main.transform.forward;
                    }

                    targetDirLocked.y = 0;

                    Quaternion trLockedRot = Quaternion.LookRotation(targetDirLocked);
                    Quaternion lockedTargetRot = Quaternion.Slerp(transform.rotation, trLockedRot, Time.fixedDeltaTime * rotationSpeed);
                    transform.rotation = lockedTargetRot;
                    break;
            }

            //Apply the movement to the rigidbody with some gravity
            rb.velocity = (moveDir * GetMoveSpeed() * inputAmount) + gravity;

            floorMovement = new Vector3(rb.position.x, FindFloor().y + floorOffsetY, rb.position.z);

            //Stick to the floor if we're grounded
            if (floorMovement != rb.position && IsGrounded() && rb.velocity.y <= 0)
            {
                rb.MovePosition(floorMovement);
                gravity.y = 0;
                if (!isLockedOn)
                {
                    movementControlType = MovementControlType.Normal;
                }
                else
                {
                    movementControlType = MovementControlType.LockedOn;
                }
            }
        }

        private float GetMoveSpeed()
        {
            float currentMoveSpeed = Mathf.Clamp(moveSpeed + (slopeAmount * slopeInfluence), 0, moveSpeed + 1);
            return currentMoveSpeed;
        }

        private void SetTargetting()
        {
            if (targetTracker.GetTargetList().Count < 0)
            {
                isLockedOn = false;
                movementControlType = MovementControlType.Normal;
                animator.SetBool("isTargetting", false);
                return;
            }

            isLockedOn = !isLockedOn;
            animator.SetBool("isTargetting", isLockedOn);

            if (isLockedOn)
            {
                movementControlType = MovementControlType.LockedOn;
            }
            else
            {
                movementControlType = MovementControlType.Normal;
            }
        }

        #region Jump/Grounded

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
            float raycastWidth = 0.5f;
            int floorAverage = 1;

            combinedRaycast = FloorRaycasts(0, 0, 1.6f);
            floorAverage +=
                (GetFloorAverage(raycastWidth, 0) + GetFloorAverage(-raycastWidth, 0) + GetFloorAverage(0, raycastWidth) + GetFloorAverage(0, -raycastWidth));

            return combinedRaycast / floorAverage;
        }

        private int GetFloorAverage(float offsetX, float offsetZ)
        {
            if (FloorRaycasts(offsetX, offsetZ, 1.6f) != Vector3.zero)
            {
                combinedRaycast += FloorRaycasts(offsetX, offsetZ, 1.6f);
                return 1;
            }
            else
            {
                return 0;
            }
        }

        private Vector3 FloorRaycasts(float offsetX, float offsetZ, float raycastLength)
        {
            raycastFloorPos = transform.TransformPoint(0 + offsetX, 0 + 0.5f, 0 + offsetZ);

            Debug.DrawRay(raycastFloorPos, Vector3.down, Color.magenta);

            if (Physics.Raycast(raycastFloorPos, -Vector3.up, out RaycastHit hit, raycastLength))
            {
                return hit.point;
            }
            else
            {
                return Vector3.zero;
            }
        }
        #endregion
    }
}
