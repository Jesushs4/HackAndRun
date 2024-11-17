using UnityEngine;

public class BulletController : MonoBehaviour
{

    private Rigidbody2D rb;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        rb.velocity = Vector3.right * 10f * transform.localScale.x;
        Destroy(gameObject, 3f);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.gameObject.GetComponentInParent<IDamageable>().TakeDamage();
        }

    }

}
