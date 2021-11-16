public class PlayerMovement : MonoBehaviour
{
    //variables

    [Header("Movement")]
    [SerializeField] float airMultiplier = 0.4f;
    public float moveSpeed = 6f;
    public float movementMultiplier = 10f;
    float horizontalMovement;
    float verticalMovement;

    [Header("Drag")]
    [SerializeField] float groundDrag = 6f;
    [SerializeField] float airDrag = 0.1f;

    [Header("Jumping")]
    public float jumpForce = 5f;
    public float gravity;
    float playerHeight;
    int doubleJump;
    bool isGrounded;

    [Header("Slide")]
    public float slideForce = 6f;
    public float slidePlayerHeight;
    
    private Vector3 moveDirection;

    Rigidbody rb;
    new CapsuleCollider collider;

    //\\//\\//\\//\\//\\//\\//\\//\\
    private void Start()
    {
        collider = GetComponentInChildren<CapsuleCollider>();
        rb = GetComponent<Rigidbody>();

        playerHeight = collider.height / 2 + 0.1f;
    }
    void Update()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight);
        
        MyInput();
        ControlDrag();
        
        Jump();
        Fall();

        Slide();
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
        if (isGrounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * movementMultiplier, ForceMode.Acceleration);
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
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
            doubleJump = 1;
        }
        else if (Input.GetKeyDown(KeyCode.Space) && !isGrounded && doubleJump == 1)
        {
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
            doubleJump = 0;           
        }
    }
    void Fall()
    {
        if (!isGrounded)
        {
            rb.AddForce(Vector3.down * gravity, ForceMode.Acceleration);
        }
        
    }
    void Slide()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl) && Input.GetKey(KeyCode.W) && isGrounded)
        {
            rb.AddForce(transform.forward * slideForce, ForceMode.VelocityChange);
        }
    }

    IEnumerator wait()
    {
        yield return new WaitForSeconds(3);
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public override bool Equals(object other)
    {
        return base.Equals(other);
    }

    public override string ToString()
    {
        return base.ToString();
    }
}