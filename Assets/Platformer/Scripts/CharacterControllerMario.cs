using UnityEngine;
using TMPro;

public class CharacterControllerMario : MonoBehaviour
{
    public float acceleration = 3f;
    public float maxSpeed = 10f;
    public float jumpImpulse = 8f;
    public float jumpBoostForce = 8f;
    public GameObject brick;
    public GameObject question;
    public TextMeshProUGUI points;
    public TextMeshProUGUI coinAmount;
    public TextMeshProUGUI gameOverText;

    [Header("Debug Stuff")]
    public bool isGrounded;
    public bool isUnderBlock;

    private int count;
    private int pointAmount;
    private bool hitBlock;
    Animator animator;
    Rigidbody rb;
    CapsuleCollider capsuleCollider;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();
    }

    void UpdateAnimation()
    {
        animator.SetFloat("Speed", Mathf.Abs(rb.linearVelocity.x));
        animator.SetBool("In Air", !isGrounded);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Goal"))
        {
            gameOverText.text = "You Win!";
        }

        if (other.CompareTag("Fire"))
        {
            gameOverText.text = "Game Over\nYou Died";
        }
    }

    void BlockRaycast()
    {
        //Test if character on ground surface
        Collider c = GetComponent<Collider>();
        Vector3 startPoint = transform.position;
        float castDistance = c.bounds.extents.y * 1.99f;
        RaycastHit hit;
        isUnderBlock = Physics.Raycast(startPoint, Vector3.up, out hit, castDistance);

        //Color color = (isUnderBlock) ? Color.blue : Color.red;
        //Debug.DrawLine(startPoint, startPoint + castDistance * Vector3.up, color, 0f, false);
        
        if (isUnderBlock)
        {
            if (hit.transform.gameObject.tag == "Brick")
            {
                Destroy(hit.transform.gameObject, 0.1f);
                pointAmount += 100;
                points.text = $"Mario\n{pointAmount.ToString("000000")}";
            }
            else if (hit.transform.gameObject.tag == "Question")
            {
                count++;
                coinAmount.text = $"x{count.ToString("00")}";
                pointAmount += 100;
                points.text = $"Mario\n{pointAmount.ToString("000000")}";
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Tap jump
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            // apply an impulse force upward
            rb.AddForce(Vector3.up * jumpImpulse, ForceMode.VelocityChange);
        }
    }

    void FixedUpdate()
    {
        UpdateAnimation();
        BlockRaycast();
        OnTriggerEnter(capsuleCollider);
        //horizontal movement
        float horizontalAmount = Input.GetAxis("Horizontal");
        if (isGrounded)
        {
            rb.linearVelocity += Vector3.right * (horizontalAmount * Time.fixedDeltaTime * acceleration);
        
            //horizontal velocity clamping (used to limit max speed)
            float horizontalSpeed = rb.linearVelocity.x;
            horizontalSpeed = Mathf.Clamp(horizontalSpeed, -maxSpeed, maxSpeed);

            Vector3 newVelocity = rb.linearVelocity;
            newVelocity.x = horizontalSpeed;
            rb.linearVelocity = newVelocity;

        }
        else
        {
            float airAcceleration = acceleration * 0.5f;
            rb.linearVelocity += Vector3.right * (horizontalAmount * Time.fixedDeltaTime * airAcceleration);
            
            //horizontal velocity clamping (used to limit max speed)
            float horizontalSpeed = rb.linearVelocity.x;
            horizontalSpeed = Mathf.Clamp(horizontalSpeed, -maxSpeed, maxSpeed);

            Vector3 newVelocity = rb.linearVelocity;
            newVelocity.x = horizontalSpeed;
            rb.linearVelocity = newVelocity;
        }

        //Test if character on ground surface
        Collider c = GetComponent<Collider>();
        Vector3 startPoint = transform.position;
        float castDistance = c.bounds.extents.y / 2f;

        isGrounded = Physics.Raycast(startPoint, Vector3.down, castDistance);

        //Color color = (isGrounded) ? Color.green : Color.red;
        //Debug.DrawLine(startPoint, startPoint + castDistance * Vector3.down, color, 0f, false); ;
        
        
        //Hold jump (Goes higher)
        if (Input.GetKey(KeyCode.Space) && !isGrounded)
        {
            if (rb.linearVelocity.y > 0)
            {
                rb.AddForce(Vector3.up * (jumpBoostForce * Time.fixedDeltaTime), ForceMode.VelocityChange);
            }
        }
        
        //slows character down until stopped when no horizontal input is pressed
        if (horizontalAmount == 0f)
        {
            Vector3 decayedVelocity = rb.linearVelocity;
            decayedVelocity.x *= 1f - Time.deltaTime * 4f;
            rb.linearVelocity = decayedVelocity;
        }
        //if horizontal input is pressed, checks which direction mario is going in to face him the right way
        else
        {
            float yawRotation = (horizontalAmount > 0f) ? 90f : -90f;
            Quaternion rotation = Quaternion.Euler(0f, yawRotation, 0f);
            transform.rotation = rotation;
        }
    }
}
