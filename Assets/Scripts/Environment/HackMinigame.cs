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
    [SerializeField] private TMP_Text hackingFail;
    private int hackedTimes = 0;

    // Cinemachine shake variables
    [SerializeField] private CinemachineVirtualCamera vCam;
    [SerializeField] private float shakeAmplitude = 1f;
    [SerializeField] private float shakeFrequency = 1f;
    [SerializeField] private float shakeDuration = 0.1f;

    private void Awake()
    {
        hackingFail.gameObject.SetActive(false);
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

    /// <summary>
    /// Starts the hacking minigame
    /// </summary>
    public void StartHack()
    {
        isNeedleMoving = true;
        randomZone = Random.Range(-0.4f, 0.4f);
        hitZone.localPosition = new Vector3(randomZone, hitZone.localPosition.y, hitZone.localPosition.z);
    }

    /// <summary>
    /// Checks needle position to throw success or failure
    /// </summary>
    private void NeedleCheck()
    {
        if (Input.GetKeyDown(KeyCode.E) && isNeedleMoving)
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
                hackingFail.gameObject.SetActive(true);
                hackingFail.rectTransform.localPosition = new Vector3(needle.localPosition.x, hackingFail.rectTransform.localPosition.y, hackingFail.rectTransform.localPosition.z);
                GameManager.Instance.Timer += 10f;
                ShakeCamera();
            }
        StartCoroutine(NewHacking());
        }
    }

    /// <summary>
    /// Makes the needle move in the minigame
    /// </summary>
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

    /// <summary>
    /// Starts a new hacking after success/failing with a cooldown
    /// </summary>
    /// <returns></returns>
    private IEnumerator NewHacking()
    {
        yield return new WaitForSeconds(1f);
        StartHack();
        hackingFail.gameObject.SetActive(false);
    }

    /// <summary>
    /// Shakes the camera after failing
    /// </summary>
    private void ShakeCamera()
    {
        var noise = vCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        noise.m_AmplitudeGain = shakeAmplitude;
        noise.m_FrequencyGain = shakeFrequency;

        StartCoroutine(StopShakeAfterDuration());
    }

    /// <summary>
    /// Stops the camera shake after a few seconds
    /// </summary>
    /// <returns></returns>
    private IEnumerator StopShakeAfterDuration()
    {
        yield return new WaitForSeconds(shakeDuration);
        var noise = vCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        noise.m_AmplitudeGain = 0f;
        noise.m_FrequencyGain = 0f;
    }

}
