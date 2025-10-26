using System.Collections;
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

    private CapsuleCollider capsuleCollider;
    private float originalHeight;
    private Vector3 originalCenter;

    public bool coinMagnetActive = false;
    public float magnetRadius = 5f;
    public float magnetForce = 2000f;

    public bool hasShield = false;

    private Animator animator;

    [SerializeField] private float JumpForce = 100;
    [SerializeField] private LayerMask GroundMask;


    public float laneDistance = 3f; // Distance entre les lanes
    private int currentLane = 1;    // 0 = gauche, 1 = centre, 2 = droite

    public float laneSwitchSpeed = 10f;
    public float forwardSpeed = 10f;

    private Vector3 targetPosition;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        targetPosition = transform.position;
    }


    private void FixedUpdate()
    {
        if (!isGameStarted)
        {
            return;
        }

        if (isAlive)
        {
            Vector3 forwardMove = Vector3.forward * RunSpeed * Time.fixedDeltaTime;
            Vector3 lateralMove = Vector3.right * (targetPosition.x - transform.position.x);
            lateralMove = Vector3.ClampMagnitude(lateralMove, HorizontalSpeed * Time.fixedDeltaTime);

            rb.MovePosition(rb.position + forwardMove + lateralMove);
        }

        if (coinMagnetActive)
        {
            Collider[] coins = Physics.OverlapSphere(transform.position, magnetRadius);
            foreach (Collider coin in coins)
            {
                if (coin.CompareTag("Coin"))
                {
                    Vector3 direction = (transform.position - coin.transform.position).normalized;
                    coin.GetComponent<Rigidbody>().AddForce(direction * magnetForce);
                }
            }
        }

    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        originalHeight = capsuleCollider.height;
        originalCenter = capsuleCollider.center;
    }

    // Update is called once per frame
    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        float playerHeight = GetComponent<Collider>().bounds.size.y;
        bool isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight / 2 + 0.1f, GroundMask);

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            isGameStarted = true;
            animator.SetBool("isRunning", true);
            Destroy(startingText);
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && isAlive )
        {
            Jump();
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            animator.SetBool("isSliding", true);

            // Réduction du collider
            capsuleCollider.height = originalHeight / 2f;
            capsuleCollider.center = new Vector3(originalCenter.x, originalCenter.y / 2f, originalCenter.z);

        }

        if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            capsuleCollider.height = originalHeight;
            capsuleCollider.center = originalCenter;
            animator.SetBool("isSliding", false);
        }


        if (Input.GetKeyDown(KeyCode.LeftArrow) && currentLane > 0)
        {
            currentLane--;
            UpdateTargetPosition();
        }

        if (Input.GetKeyDown(KeyCode.RightArrow) && currentLane < 2)
        {
            currentLane++;
            UpdateTargetPosition();
        }

    }

    void UpdateTargetPosition()
    {
        float xPos = (currentLane - 1) * laneDistance;
        targetPosition = new Vector3(xPos, transform.position.y, transform.position.z);
    }


    public void ActivateShield()
    {
        StartCoroutine(Shield());
    }

    private IEnumerator Shield()
    {
        hasShield = true;
        yield return new WaitForSeconds(15);
        hasShield = false;
    }


    public void ActivateMagnet()
    {
        StartCoroutine(Magnet());
    }

    private IEnumerator Magnet()
    {
        coinMagnetActive = true;
        yield return new WaitForSeconds(15);
        coinMagnetActive = false;
    }

    public void ReduceHorizontalSpeed()
    {
        StartCoroutine(ReduceSpeedTemporarily());
    }

    private IEnumerator ReduceSpeedTemporarily()
    {
        float originalSpeed = HorizontalSpeed;
        HorizontalSpeed = 2.5f;
        yield return new WaitForSeconds(10);
        HorizontalSpeed = originalSpeed;
    }


    public void Jump()
    {
        rb.AddForce(Vector3.up * JumpForce);
    }

    public void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.name == "Graphic")
        {
            if (hasShield)
            {
                hasShield = false;
                Destroy(collision.gameObject); // Ignore la mort
            }
            else
            {
                Die();
            }
        }

        if (collision.gameObject.name == "Coin(Clone)")
        {
            Destroy(collision.gameObject);
            GameManager.MyInstance.score++;
            if (GameManager.MyInstance.score % 30 == 0)
                RunSpeed += speedIncrease;
        }

        if (collision.gameObject.name == "SpeedReduce(Clone)")
        {
            Destroy(collision.gameObject);
            ReduceHorizontalSpeed();
        }

        if (collision.gameObject.name == "Magnet(Clone)")
        {
            Destroy(collision.gameObject);
            ActivateMagnet();
        }

        if (collision.gameObject.name == "Shield(Clone)")
        {
            Destroy(collision.gameObject);
            ActivateShield();
        }
    }

    public void Die()
    {
        isAlive = false;
        GameManager.MyInstance.GameOverPanel.SetActive(true);
    }
}
