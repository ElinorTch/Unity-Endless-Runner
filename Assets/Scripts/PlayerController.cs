using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public bool isAlive = true;
    public float RunSpeed;
    public float HorizontalSpeed;
    public Rigidbody rb;
    float horizontalInput;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
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
        
    }

    // Update is called once per frame
    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
    }
}
