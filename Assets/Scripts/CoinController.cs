using UnityEngine;

public class CoinController : MonoBehaviour
{
    [SerializeField] AudioClip collectSound;

    bool wasCollected = false;
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player") && !wasCollected)
        {
            wasCollected = true;

            AudioSource.PlayClipAtPoint(collectSound, transform.position);
            Destroy(gameObject);
        }
    }
}
