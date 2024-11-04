using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HackMinigame : MonoBehaviour
{
    private Transform hitZone;
    private Transform needle;
    private float direction = 1;
    private float randomZone;
    private bool isNeedleMoving = true;
    [SerializeField] private int hackingNeeded = 3;
    private int hackedTimes = 0;

    private void Awake()
    {
        hitZone = gameObject.transform.GetChild(0);
        needle = gameObject.transform.GetChild(1);
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
                Debug.Log("OK");
                // ADD TO HACK NEEDED NUM
                hackedTimes++;
            }
            else
            {
                Debug.Log("FAIL");
                // ADD TIME TO TIMER
                GameManager.Instance.Timer += 10f;
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
}
