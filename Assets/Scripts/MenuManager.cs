using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject[] panels;

    [SerializeField] private GameObject[] buttons;



    private void Start()
    {

        DeactivatePanels();
        AssignButtons();

    }

    public void ChangePanel(GameObject activePanel)
    {
        foreach (GameObject panel in panels)
        {
            if (panel.activeSelf) 
            {
                panel.SetActive(false);
                break;
            }
        }

        activePanel.SetActive(true);
    }

    private void DeactivatePanels()
    {
        foreach (var panel in panels)
        {
            if (panel.name == "GameMenu")
            {
                panel.SetActive(true);
            }
            else
            {
                panel.SetActive(false);
            }
        }
    }

    private void AssignButtons()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            int index = i;
            buttons[i].GetComponent<Button>().onClick.AddListener(() =>
            {
                ChangeScene(index+1);
            });
        }
    }

    private void ChangeScene(int level)
    {
        SceneManager.LoadScene("Level" + level);
    }


    public void ExitGame()
    {
        Application.Quit();
    }
}
