using UnityEngine;

public class DeathController : MonoBehaviour
{
    public void Death()
    {
        Destroy(transform.parent.gameObject);
    }
}
