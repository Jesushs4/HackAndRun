using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    public void AttackAudio()
    {
        AudioManager.Instance.Attack();
    }
}
