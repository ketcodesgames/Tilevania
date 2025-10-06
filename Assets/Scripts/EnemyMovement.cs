using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 10f;
    Rigidbody2D enemyRigidBody;
    BoxCollider2D enemyBoxCollider;

    void Start()
    {
        enemyRigidBody = GetComponent<Rigidbody2D>();
        enemyBoxCollider = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        enemyRigidBody.linearVelocity = new Vector2(moveSpeed, 0f);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        moveSpeed = -moveSpeed;
        FlipEnemyFacing();
    }

    void FlipEnemyFacing()
    {
        transform.localScale = new Vector2(-(Mathf.Sign(enemyRigidBody.linearVelocity.x)), 1f);
    }
}
