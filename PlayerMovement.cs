using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //variables

    [Header("Movement")]
    [SerializeField] float airMultiplier = 0.4f;
    Vector3 moveDirection;
    Vector3 slopeMoveDirection;
    public float moveSpeed = 6f;
    public float movementMultiplier = 10f;
    float horizontalMovement;
    float verticalMovement;

    [Header("Drag")]
    [SerializeField] float groundDrag = 6f;
    [SerializeField] float airDrag = 0.1f;

    [Header("Jumping")]
    public float jumpForce = 5f;
    float fallMultiplier = 3f;
    int doubleJump;
    bool hasDoubleJumped;

    [Header("Ground detection")]
    [SerializeField] LayerMask groundMask;
    RaycastHit slopeHit;
    float playerHeight;
    float groundDistance = 0.4f;
    bool isGrounded;
  
    Rigidbody rb;
    new CapsuleCollider collider;

    //\\//\\//\\//\\//\\//\\//\\//\\
    void Awake()
    {

    }
    private void Start()
    {
        collider = GetComponentInChildren<CapsuleCollider>();
        rb = GetComponent<Rigidbody>();

        playerHeight = collider.height / 2 + 0.1f;
    }
    void Update()
    {
        isGrounded = Physics.CheckSphere(transform.position - new Vector3(0, 1, 0), groundDistance, groundMask );
        slopeMoveDirection = Vector3.ProjectOnPlane(moveDirection, slopeHit.normal);

        MyInput();
        ControlDrag();
        Jump();
    }
    private void FixedUpdate()
    {
        MovePlayer();
        Fall();
    }
    //\\//\\//\\//\\//\\//\\//\\//\\

    void MyInput()
    {
        horizontalMovement = Input.GetAxisRaw("Horizontal");
        verticalMovement = Input.GetAxisRaw("Vertical");

        moveDirection = transform.forward * verticalMovement + transform.right * horizontalMovement * 2;
    }
    void MovePlayer()
    {
        if (isGrounded && !OnSlope())
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * movementMultiplier, ForceMode.Acceleration);
        }
        else if (isGrounded && OnSlope())
        {
            rb.AddForce(slopeMoveDirection.normalized * moveSpeed * movementMultiplier, ForceMode.Acceleration);
        }
        else if (!isGrounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * movementMultiplier * airMultiplier, ForceMode.Acceleration);
        }
    }
    void ControlDrag()
    {
        if (isGrounded)
        {
            rb.drag = groundDrag;
        }
        else
        {
            rb.drag = airDrag;
        }
    }
    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            doubleJump = 1;
            hasDoubleJumped = false;
            fallMultiplier = 3f;
        }
        else if (Input.GetKeyDown(KeyCode.Space) && !isGrounded && doubleJump == 1)
        {
            hasDoubleJumped = true;
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            if (hasDoubleJumped)
            {
                fallMultiplier = 3.35f;
            }
            doubleJump = 0;
        }
    }
    void Fall()
    {
        if (rb.velocity.y < 1.5f)
        {
            rb.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }

    }
    private bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight))
        {
            if (slopeHit.normal != Vector3.up)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        
        return false;
    }

    IEnumerator wait()
    {
        yield return new WaitForSeconds(3);
    }
}
