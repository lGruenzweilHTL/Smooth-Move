using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class FirstPersonMovement : MonoBehaviour
{
    [Space, SerializeField] private float speed = 5f;

    [SerializeField] private float sprintSpeedMultiplier = 1.5f;
    [SerializeField] private float sneakSpeedMultiplier = 0.5f;
    [SerializeField] private float airControlMultiplier = 1f;

    [Space, SerializeField] private float jumpStrength = 1.5f;
    [SerializeField] private float jumpBuffer = 0.2f;

    [Space, SerializeField] private float gravity = -20f;


    [Space, SerializeField] private Transform groundCheckTransform;
    [SerializeField] private float groundCheckDistance = 0.2f;
    [SerializeField] private LayerMask groundLayers;

    #region Variables

    private CharacterController controller;

    private Vector2 input;

    private float yMovement;
    private float currentGroundSpeed;

    private float jumpBufferTimer;

    private bool isGrounded;

    private bool isSprinting;
    private bool isSneaking;

    #endregion

    private void Awake()
    {
        controller = GetComponent<CharacterController>();

        currentGroundSpeed = speed;
    }

    void Update()
    {
        #region set basic variables

        isGrounded = Physics.CheckSphere(groundCheckTransform.position, groundCheckDistance, groundLayers);
        isSneaking = Input.GetKey(KeyCode.LeftControl);
        isSprinting = Input.GetKey(KeyCode.LeftShift);

        currentGroundSpeed = speed;
        if (isSneaking) //Sneaking has the most priority
        {
            currentGroundSpeed *= sneakSpeedMultiplier;
        }
        else if (isSprinting) //Sprinting has less priority than sneaking
        {
            currentGroundSpeed *= sprintSpeedMultiplier;
        }
        //if not grounded (in the air)
        else if (!isGrounded) //Being grounded doesn't change the currentGroundSpeed value
        {
            currentGroundSpeed *= airControlMultiplier;
        }

        #endregion

        #region resetting gravity force when grounded

        if (isGrounded && yMovement < 0)
        {
            yMovement = -2f;
        }

        #endregion

        #region get Input

        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");

        #endregion

        #region calculate values

        Vector3 move = transform.right * input.x + transform.forward * input.y;

        #endregion

        #region jumping & gravity

        //jumping
        jumpBufferTimer -= Time.deltaTime;
        if (Input.GetButtonDown("Jump"))
        {
            jumpBufferTimer = jumpBuffer;
        }
        if (jumpBufferTimer > 0f && isGrounded)
        {
            //real life gravity formula
            yMovement = Mathf.Sqrt(jumpStrength * -2f * gravity);
        }

        //adding gravity
        yMovement += gravity * Time.deltaTime;

        #endregion

        #region applying movement

        controller.Move(currentGroundSpeed * Time.deltaTime * move);
        controller.Move(Time.deltaTime * yMovement * transform.up);

        #endregion
    }

    #region Gizmos

    private void OnDrawGizmosSelected()
    {
        if (groundCheckTransform == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheckTransform.position, groundCheckDistance);
    }

    #endregion
}