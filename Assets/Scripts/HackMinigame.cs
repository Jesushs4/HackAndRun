using Cinemachine;
using System.Collections;
using TMPro;
using UnityEngine;

public class HackMinigame : MonoBehaviour
{
    private Transform hitZone;
    private Transform needle;
    private float direction = 1;
    private float randomZone;
    private bool isNeedleMoving = true;
    [SerializeField] private int hackingNeeded = 3;
    [SerializeField] private TMP_Text hackingProgress;
    private int hackedTimes = 0;

    // Cinemachine shake variables
    [SerializeField] private CinemachineVirtualCamera vCam;
    [SerializeField] private float shakeAmplitude = 1f;
    [SerializeField] private float shakeFrequency = 1f;
    [SerializeField] private float shakeDuration = 0.1f;

    private void Awake()
    {
        hitZone = gameObject.transform.GetChild(0);
        needle = gameObject.transform.GetChild(1);
        hackingProgress.text = hackedTimes + "/" + hackingNeeded;
    }

    private void Update()
    {
        NeedleMove();
        NeedleCheck();

        if (hackedTimes == hackingNeeded)
        {
            gameObject.SetActive(false);
            GameManager.Instance.WinGame();
        }
    }

    public void StartHack()
    {
        isNeedleMoving = true;
        randomZone = Random.Range(-0.4f, 0.4f);
        hitZone.localPosition = new Vector3(randomZone, hitZone.localPosition.y, hitZone.localPosition.z);
    }

    private void NeedleCheck()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            isNeedleMoving = false;
            if (needle.localPosition.x >= randomZone - 0.05 && needle.localPosition.x <= randomZone + 0.05)
            {
                AudioManager.Instance.HackCorrect();
                hackedTimes++;
                hackingProgress.text = hackedTimes + "/" + hackingNeeded;
            }
            else
            {
                AudioManager.Instance.HackFail();
                GameManager.Instance.Timer += 10f;
                ShakeCamera();
            }
        StartCoroutine(NewHacking());
        }
    }

    private void NeedleMove()
    {
        if (isNeedleMoving)
        {
            needle.localPosition = new Vector3(needle.localPosition.x + 1 * Time.deltaTime * direction, needle.localPosition.y, needle.localPosition.z);
            if (needle.localPosition.x >= 0.5 || needle.localPosition.x <= -0.5)
            {
                direction *= -1;
            }
        }

    }

    private IEnumerator NewHacking()
    {
        yield return new WaitForSeconds(1f);
        StartHack();
    }

    private void ShakeCamera()
    {
        var noise = vCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        noise.m_AmplitudeGain = shakeAmplitude;
        noise.m_FrequencyGain = shakeFrequency;

        StartCoroutine(StopShakeAfterDuration());
    }

    private IEnumerator StopShakeAfterDuration()
    {
        yield return new WaitForSeconds(shakeDuration);
        var noise = vCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        noise.m_AmplitudeGain = 0f;
        noise.m_FrequencyGain = 0f;
    }

}
