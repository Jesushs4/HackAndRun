
using UnityEngine;

public class ShootController : MonoBehaviour
{

    [SerializeField] private GameObject bullet;
    private Transform shootPoint;

    private void Awake()
    {
        shootPoint = gameObject.transform.GetChild(0);

    }

    public void Shoot()
    {
        var newBullet = Instantiate(bullet, shootPoint.position, Quaternion.identity);
        newBullet.transform.localScale = transform.parent.transform.localScale;
    }

    
}
