using System;
using System.Collections;
using System.Collections.Generic;
using SpyroClone.Combat;
using SpyroClone.Saving;
using UnityEngine;

namespace SpyroClone.Player
{
    public class PlayerMovement : MonoBehaviour, ISaveable
    {

        [Header("Grounded Movement")]
        [SerializeField] float floorOffsetY;
        [SerializeField] float moveSpeed = 6f;
        [SerializeField] float rotationSpeed = 10f;

        [Header("Jump/Glide Movement")]
        [SerializeField] float jumpPower;
        [SerializeField] float glideSpeed = 7f;
        [SerializeField] float movingGlideForce = 200f;
        [SerializeField] float stationaryGlideForce = 100f;

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
        bool isGlidePressed;

        //Base movement
        Vector3 moveDir;
        float inputAmount;

        //Grounded/Jumping
        Vector3 raycastFloorPos;
        Vector3 floorMovement;
        Vector3 gravity;
        Vector3 combinedRaycast;
        readonly float jumpFallOff = 2f;
        readonly float lowJumpMulti = 1.5f;

        //Gliding
        readonly float glideFallOff = 0.5f;
        bool isInGlide;

        //Slopes
        float slopeAmount;
        Vector3 floorNormal;

        public float kRaycastLength = 1f;

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
            isGlidePressed = Input.GetKeyDown(KeyCode.Space) && !IsGrounded();

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

            if (isGlidePressed)
            {
                Glide();
            }

            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                SetTargetting();
            }

            if (isInGlide && IsGrounded())
            {
                animator.SetTrigger("Land");
                isInGlide = false;
            }

            animator.SetFloat("forwardSpeed", inputAmount, 0.2f, Time.deltaTime);
            animator.SetFloat("slopeNormal", slopeAmount, 0.2f, Time.deltaTime);
        }



        private void FixedUpdate()
        {
            //Apply gravity if not grounded           
            if (!IsGrounded() || slopeAmount >= 0.1f)
            {
                if (isInGlide)
                {
                    gravity += Vector3.up * Physics.gravity.y * (glideFallOff - 1) * Time.fixedDeltaTime;
                }
                gravity += Vector3.up * Physics.gravity.y * (jumpFallOff - 1) * Time.fixedDeltaTime;
            }
            else if (!Input.GetKey(KeyCode.Space))
            {
                gravity += Vector3.up * Physics.gravity.y * (lowJumpMulti - 1) * Time.fixedDeltaTime;
            }

            //Depending on if we're locked on or not, handle movement differently
            HandleTargetting();

            Vector3 velocity;
            if (isInGlide)
            {
                if (moveDir == Vector3.zero)
                {
                    velocity = transform.forward * glideSpeed;

                }
                else
                {
                    velocity = moveDir * glideSpeed * inputAmount;
                }
            }
            else
            {
                velocity = moveDir * GetMoveSpeed() * inputAmount;
            }

            rb.velocity = velocity + gravity;

            if (isInGlide)
            {
                if (moveDir == Vector3.zero)
                {
                    rb.AddForce(velocity * glideSpeed * stationaryGlideForce * Time.deltaTime);
                } else 
                {
                    rb.AddForce(velocity * glideSpeed * movingGlideForce * Time.deltaTime);
                }
                
            }

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

        private void HandleTargetting()
        {
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

        private void Glide()
        {
            if (!IsGrounded())
            {
                animator.SetTrigger("Glide");
                isInGlide = true;
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

            combinedRaycast = FloorRaycasts(0, 0, kRaycastLength);
            floorAverage +=
                (GetFloorAverage(raycastWidth, 0) + GetFloorAverage(-raycastWidth, 0) + GetFloorAverage(0, raycastWidth) + GetFloorAverage(0, -raycastWidth));

            return combinedRaycast / floorAverage;
        }

        private int GetFloorAverage(float offsetX, float offsetZ)
        {
            if (FloorRaycasts(offsetX, offsetZ, kRaycastLength) != Vector3.zero)
            {
                combinedRaycast += FloorRaycasts(offsetX, offsetZ, kRaycastLength);
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

            Debug.DrawRay(raycastFloorPos, Vector3.down * raycastLength, Color.magenta);

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

        [System.Serializable]
        struct SaveData
        {
            public SerializableVector3 position;
            public SerializableQuaternion rotation;
        }

        //Saving
        public object CaptureState()
        {
            SaveData data = new()
            {
                position = new SerializableVector3(transform.position),
                rotation = new SerializableQuaternion(transform.rotation)
            };
            return data;
        }

        public void RestoreState(object state)
        {
            SaveData data = (SaveData)state;
            transform.position = data.position.ToVector();
            transform.rotation = data.rotation.ToQuaternion();
        }
    }
}
