using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public bool isAlive = true;
    public float RunSpeed;
    public int speedIncrease;
    public float HorizontalSpeed;
    public Rigidbody rb;
    float horizontalInput;
    public bool isGameStarted = false;
    public TextMeshProUGUI startingText;

    private Animator animator;

    [SerializeField] private float JumpForce = 350;
    [SerializeField] private LayerMask GroundMask;

        

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            isGameStarted = true;
            Destroy(startingText);
        }

        if (!isGameStarted)
        {
            return;
        }

        if (isAlive)
        {
            Vector3 forwardMove = transform.forward * RunSpeed * Time.fixedDeltaTime;
            Vector3 horizontalMove = transform.right * horizontalInput * HorizontalSpeed * Time.fixedDeltaTime;
            rb.MovePosition(rb.position + forwardMove + horizontalMove);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        float playerHeight = GetComponent<Collider>().bounds.size.y;
        bool isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight / 2 + 0.1f, GroundMask);


        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && isAlive )
        {
            Jump();
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            animator.SetBool("isSliding", true);
            animator.SetBool("isSliding", false);
        }

    }

    public void Jump()
    {
        rb.AddForce(Vector3.up * JumpForce);
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Graphic")
        {
            Debug.Log("Collision: " + collision.gameObject.name);
            Die();
        }

        if (collision.gameObject.name == "Coin(Clone)")
        {
            Destroy(collision.gameObject);
            GameManager.MyInstance.score++;
            RunSpeed += speedIncrease;
        }
    }

    public void Die()
    {
        isAlive = false;
        GameManager.MyInstance.GameOverPanel.SetActive(true);
    }
}
