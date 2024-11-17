using UnityEngine;

public class ShootController : MonoBehaviour
{

    [SerializeField] private GameObject bullet;
    private Transform shootPoint;

    private void Awake()
    {
        shootPoint = gameObject.transform.GetChild(0);

    }

    /// <summary>
    /// Instantiates the bullet
    /// </summary>
    public void Shoot()
    {
        AudioManager.Instance.ShootAudio();
        var newBullet = Instantiate(bullet, shootPoint.position, Quaternion.identity);
        newBullet.transform.localScale = transform.parent.transform.localScale;
    }




}
