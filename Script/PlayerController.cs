using Newtonsoft.Json.Linq;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEditor.PackageManager.UI;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer spriteRenderer;

    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask whatIsGround;

    private bool isGrounded;
    private bool isFacingRight = true;

    private bool isStop;
    private bool isMoving;


    [SerializeField] private float groundCheckRadius;
    [SerializeField] private float jumpForce;
    [SerializeField] private float movementSpeed;

    private float movementInputDirection;

    public float friction = 0.1f;

    void InputController()
    {
        if(Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            JumpMethod();
        }

        if(Input.GetAxis("Horizontal") != 0)
        {
            Flip();
            MoveMethod();
        }
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim= GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }


    private void Update()
    {
        InputController();
        Check();
        MoveMethod();
        BooleanCheckMethods();
        AnimationController();
        Dash();

    }

    private void Check()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround); 
    }

    private void Flip()
    {
        if (movementInputDirection > 0)
        {
            if (!isFacingRight)
            {
                FlipMethod();
            }
        }
        else if (movementInputDirection < 0)
        {
            if (isFacingRight)
            {
                FlipMethod();
            }
        }
    }

    private void JumpMethod()
    {
        rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
    }


    private void FlipMethod()
    {
        isFacingRight = !isFacingRight;
        Vector3 Scaler = transform.localScale;
        Scaler.x *= -1;
        transform.localScale = Scaler;
    }

    private void MoveMethod()
    {
        movementInputDirection = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(movementInputDirection * movementSpeed, rb.velocity.y);
    }


    private float dashSpeed = 3f;
    private float dashTime = 0.1f;
    private float dashTimer;
    private bool isDashing = false;


    private void Dash()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && !isDashing)
        {
            isDashing = true;
            dashTimer = dashTime;
        }

        if (isDashing)
        {
            if (dashTimer > 0)
            {
                rb.velocity = new Vector2(movementSpeed * dashSpeed, rb.velocity.y);
                dashTimer -= Time.deltaTime;
            }
            else
            {
                isDashing = false;
            }
        }
    }


    private void BooleanCheckMethods()
    {
        if(rb.velocity.x == 0)
        {
            isStop = true;
            isMoving= false;
        }
        else if(rb.velocity.x != 0)
        {
            isStop = false;
            isMoving = true;
        }
    }


    private void AnimationController()
    {
        anim.SetBool("isWalking", isMoving);
        anim.SetBool("idLe", isStop);
    }
}
