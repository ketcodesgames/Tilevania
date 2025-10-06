using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float runSpeed = 10f;
    [SerializeField] float jumpSpeed = 5f;
    [SerializeField] float climbSpeed = 10f;
    [SerializeField] Vector2 deathKick = new Vector2(10f, 20f);
    [SerializeField] GameObject bullet;
    [SerializeField] Transform gun;

    Vector2 moveInput;
    Rigidbody2D playerRigidbody;
    Animator playerAnimator;
    CapsuleCollider2D playerCapsuleCollider;
    BoxCollider2D playerBoxCollider;
    float gravityScale = 1f;
    bool isAlive = true;

    public bool isFacingRight = true;

    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
        playerCapsuleCollider = GetComponent<CapsuleCollider2D>();
        playerBoxCollider = GetComponent<BoxCollider2D>();

        gravityScale = playerRigidbody.gravityScale;
    }

    void Update()
    {
        if (isAlive)
        {
            Run();
            FlipSprite();
            Climb();
            Die();
        }
    }

    void OnMove(InputValue value)
    {
        if (isAlive)
        {
            moveInput = value.Get<Vector2>();
        }
    }

    void OnJump(InputValue value)
    {
        if (playerBoxCollider.IsTouchingLayers(LayerMask.GetMask("Ground")) &&
            value.isPressed &&
            isAlive)
        {
            playerRigidbody.linearVelocity += new Vector2(0f, jumpSpeed);
        }
    }

    void OnAttack(InputValue value)
    {
        if (value.isPressed && isAlive)
        {
            Instantiate(bullet, gun.position, transform.rotation);
        }
    }

    void Run()
    {
        Vector2 playerVelocity = new Vector2(moveInput.x * runSpeed, playerRigidbody.linearVelocity.y);
        playerRigidbody.linearVelocity = playerVelocity;

        bool hasHorizontalSpeed = Mathf.Abs(playerRigidbody.linearVelocity.x) > Mathf.Epsilon;
        playerAnimator.SetBool("isRunning", hasHorizontalSpeed);
    }

    void FlipSprite()
    {
        bool hasHorizontalSpeed = Mathf.Abs(playerRigidbody.linearVelocity.x) > Mathf.Epsilon;
        if (hasHorizontalSpeed)
        {
            float lineaVelocityXSign = Mathf.Sign(playerRigidbody.linearVelocity.x);
            isFacingRight = lineaVelocityXSign == 1 ? true : false;
            transform.localScale = new Vector2(lineaVelocityXSign, 1f);
        }
    }

    void Climb()
    {
        if (playerCapsuleCollider.IsTouchingLayers(LayerMask.GetMask("Climbing")))
        {
            playerRigidbody.gravityScale = 0f;
            Vector2 climbVelocity = new Vector2(playerRigidbody.linearVelocity.x, moveInput.y * climbSpeed);
            playerRigidbody.linearVelocity = climbVelocity;

            bool hasVerticalSpeed = Mathf.Abs(playerRigidbody.linearVelocity.y) > Mathf.Epsilon;
            playerAnimator.SetBool("isClimbing", hasVerticalSpeed);
        }
        else
        {
            playerRigidbody.gravityScale = gravityScale;
            playerAnimator.SetBool("isClimbing", false);
        }
    }

    void Die()
    {
        if (playerCapsuleCollider.IsTouchingLayers(LayerMask.GetMask("Enemies")) ||
            playerCapsuleCollider.IsTouchingLayers(LayerMask.GetMask("Water")) ||
            playerCapsuleCollider.IsTouchingLayers(LayerMask.GetMask("Hazards")))
        {
            isAlive = false;
            playerAnimator.SetTrigger("Dying");
            playerRigidbody.linearVelocity = deathKick;
        }
    }
}
