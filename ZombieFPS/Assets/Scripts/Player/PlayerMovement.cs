using UnityEngine;

namespace Assets.Scripts.Player
{
    public class PlayerMovement : MonoBehaviour
    {
        public GameObject modelCapsul;
        public Transform cameraPosition;
        public GameObject knife;
        [Header("Movement")]
        private float moveSpeed;
        public float walkSpeed;
        [SerializeField] private float sprintSpeed;
        [SerializeField] private float groundDrag;
        [Header("Jumping")]
        [SerializeField] private float jumpForce;
        [SerializeField] private float jumpCoolDown;
        [SerializeField] private float airMultiplier;
        [SerializeField] private bool readyToJump;
        [SerializeField] private float jumpGravity = -30;
        [SerializeField] private float realGravity = -9.81f;
        [Header("Crouching")]
        [SerializeField] private float crouchSpeed;
        [SerializeField] private float crouchYscale;
        private float startYscale;
        [Header("Ground check")]
        [SerializeField] private float playerHeight;
        [SerializeField] private LayerMask WhatIsGround;
        [SerializeField] private bool grounded;
        [Header("Slope Handling")]
        public float maxSlopeAngle;
        private RaycastHit slopeHit;
        private bool exitingSlope;

        public MovementState state;
        public enum MovementState { walking, sprinting, croucing, air }
        [SerializeField] private Transform orientation;
        private float horizontalInput;
        private float verticalInput;
        private Vector3 moveDirection;
        private Rigidbody rb;
        private CapsuleCollider capsuleCollider;

        private void Start()
        {
            rb = GetComponent<Rigidbody>();
            capsuleCollider = GetComponent<CapsuleCollider>();
            readyToJump = true;
            startYscale = transform.localScale.y;
        }

        private void Update()
        {
            grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, WhatIsGround);
            MyInput();
            SpeedControl();
            StateHandler();
            if (grounded) 
            {
                rb.drag = groundDrag; 
            }
            else 
            {
                rb.drag = 0; 
            }
        }

        private void FixedUpdate()
        {
            MovePlayer();
        }

        private void MyInput()
        {
            horizontalInput = Input.GetAxisRaw("Horizontal");
            verticalInput = Input.GetAxisRaw("Vertical");
            if(Input.GetKey(PlayerInput.instance.jump) && readyToJump && grounded) 
            {
                readyToJump = false; Jump();
                Invoke(nameof(ResetJump), jumpCoolDown);
            }
            if (Input.GetKeyDown(PlayerInput.instance.crouch))
            {
                capsuleCollider.height = 1;
                capsuleCollider.center = new Vector3(0, -0.5f,0);
                cameraPosition.localPosition = new Vector3(0, 0, 0);
            }
            if (Input.GetKeyUp(PlayerInput.instance.crouch))
            {
                capsuleCollider.height = 1.64f;
                capsuleCollider.center = new Vector3(0, -0.17f, 0);
                cameraPosition.localPosition = new Vector3(0, 0.3f, 0);
            }
        }

        private void StateHandler()
        {
            if (Input.GetKey(PlayerInput.instance.crouch))
            {
                state = MovementState.croucing; 
                moveSpeed = crouchSpeed;
            }
            else if (grounded && Input.GetKey(PlayerInput.instance.sprint)) 
            {
                state = MovementState.sprinting; 
                moveSpeed = sprintSpeed; 
            }
            else if (grounded) 
            { 
                state = MovementState.walking; 
                moveSpeed = walkSpeed; 
            }
            else 
            {
                state = MovementState.air;
            }
        }

        private void MovePlayer()
        {
            moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
            if (OnSlope() && !exitingSlope)
            {
                rb.AddForce(20f * moveSpeed * GetSlopeMoveDirection(), ForceMode.Force);
                if(rb.velocity.y > 0)
                {
                    rb.AddForce(Vector3.down * 80f, ForceMode.Force);
                }
            }
            if (grounded)
            {
                rb.AddForce(10f * moveSpeed * moveDirection.normalized, ForceMode.Force);
                Physics.gravity = new Vector3(0, realGravity, 0); 
            }
            else 
            { 
                rb.AddForce(10f * airMultiplier * moveSpeed * moveDirection.normalized, ForceMode.Force);
                Physics.gravity = new Vector3(0, jumpGravity, 0); 
            }
            rb.useGravity = !OnSlope();
        }

        private void SpeedControl()
        {
            if (OnSlope() && !exitingSlope)
            {
                if (rb.velocity.magnitude > moveSpeed)
                {
                    rb.velocity = rb.velocity.normalized * moveSpeed;
                }
            }
            else
            {
                Vector3 flatVelocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
                if (flatVelocity.magnitude > moveSpeed)
                {
                    Vector3 limitedVelocity = flatVelocity.normalized * moveSpeed;
                    rb.velocity = new Vector3(limitedVelocity.x, rb.velocity.y, limitedVelocity.z);
                }
            }
        }

        private void Jump()
        {
            exitingSlope = true;
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        }

        private void ResetJump()
        {
            readyToJump = true;
            exitingSlope = false;
        }

        private bool OnSlope()
        {
            if(Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.3f))
            {
                float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
                return angle < maxSlopeAngle && angle != 0;
            }
            return false;
        }

        private Vector3 GetSlopeMoveDirection()
        {
            return Vector3.ProjectOnPlane(moveDirection, slopeHit.normal).normalized;
        }
    }
}