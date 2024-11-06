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

    /// <summary>
    /// Deactivates the panels and activates the panel clicked
    /// </summary>
    /// <param name="activePanel"></param>
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

    /// <summary>
    /// Panel manager to deactivate all panels at start
    /// </summary>
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

    /// <summary>
    /// Button manager to assign the levels to the buttons
    /// </summary>
    private void AssignButtons()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            int index = i;
            buttons[index].GetComponent<Button>().onClick.AddListener(() =>
            {
                ChangeScene(index+1);
            });
            Transform levelNumberParent = buttons[index].transform.GetChild(0);
            var levelStars = levelNumberParent.GetComponentsInChildren<SpriteRenderer>();
            for (int j = 0; j < levelStars.Length; j++)
            {
                if (PlayerPrefs.GetInt(GameConstants.LEVEL_STARS+(index+1))<j+1)
                {
                    levelStars[j].color = Color.black;
                }
            }
        }
    }

    /// <summary>
    /// Change scene to the specified level
    /// </summary>
    /// <param name="level"></param>
    private void ChangeScene(int level)
    {
        SceneManager.LoadScene("Level" + level);
    }


    /// <summary>
    /// Exits the game
    /// </summary>
    public void ExitGame()
    {
        Application.Quit();
    }
}
