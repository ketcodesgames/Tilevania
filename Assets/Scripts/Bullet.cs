using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float bulletSpeed = 20f;
    PlayerMovement player;
    Rigidbody2D bulletRigidBody;
    float xSpeed;

    void Start()
    {
        bulletRigidBody = GetComponent<Rigidbody2D>();
        player = FindFirstObjectByType<PlayerMovement>();
        xSpeed = player.transform.localScale.x * bulletSpeed;
        transform.localScale = new Vector2(player.isFacingRight ? 1 : -1, 1f);
    }

    void Update()
    {
        bulletRigidBody.linearVelocity = new Vector2(xSpeed, 0f);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Destroy(collision.gameObject);
        }
        Destroy(gameObject);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject, 1f);
    }
}
