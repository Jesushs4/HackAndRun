
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance;

    [SerializeField] GameObject gameOverPanel;
    [SerializeField] GameObject winGamePanel;
    [SerializeField] GameObject hackPanel;
    [SerializeField] GameObject pausePanel;

    private HackMinigame hackMinigame;

    [SerializeField] int[] starsTimes = new int[] { 120, 120, 60 };
    private Transform starsWinObject;
    private Image[] starsWinImages;

    private Transform starsPauseObject;
    private Image[] starsPauseImages;


    [SerializeField] private int health = 3;
    private float timer = 0;

    public int Health { get => health; set => health = value; }
    public float Timer { get => timer; set => timer = value; }

    private void Awake()
    {

        Instance = this;

        gameOverPanel.SetActive(false);
        winGamePanel.SetActive(false);

        starsWinObject = winGamePanel.transform.Find("Stars");
        starsWinImages = starsWinObject.GetComponentsInChildren<Image>();

        starsPauseObject = pausePanel.transform.Find("Stars");
        starsPauseImages = starsPauseObject.GetComponentsInChildren<Image>();



        for (int i = 0; i < starsPauseImages.Length && i < starsTimes.Length; i++)
        {
            int timeInSeconds = starsTimes[i];

            string formattedTime = (i == 0)
                ? "+" + string.Format("{0:D2}:{1:D2}", timeInSeconds / 60, timeInSeconds % 60)
                : string.Format("{0:D2}:{1:D2}", timeInSeconds / 60, timeInSeconds % 60);

            starsPauseImages[i].transform.GetChild(0).GetComponent<TMP_Text>().text = formattedTime;
        }


        for (int i = 0; i < starsWinImages.Length && i < starsTimes.Length; i++)
        {
            int timeInSeconds = starsTimes[i];

            string formattedTime = (i == 0)
                ? "+" + string.Format("{0:D2}:{1:D2}", timeInSeconds / 60, timeInSeconds % 60)
                : string.Format("{0:D2}:{1:D2}", timeInSeconds / 60, timeInSeconds % 60);

            starsWinImages[i].transform.GetChild(0).GetComponent<TMP_Text>().text = formattedTime;
        }



        hackMinigame = hackPanel.GetComponent<HackMinigame>();

        Time.timeScale = 1;
    }

    private void Update()
    {
        ManagePause();
    }

    private void ManagePause()
    {

        if (starsTimes[1] <= timer)
        {
            starsPauseImages[2].color = Color.black;
        }
        if (starsTimes[0] <= timer)
        {
            starsPauseImages[1].color = Color.black;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pausePanel.activeSelf)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    private void SaveStars(int stars)
    {
        if (PlayerPrefs.GetInt(GameConstants.LEVEL_STARS + SceneManager.GetActiveScene().buildIndex) < stars)
        {
            PlayerPrefs.SetInt(GameConstants.LEVEL_STARS + SceneManager.GetActiveScene().buildIndex, stars);
        }
    }

    public void WinGame()
    {
        if (SceneManager.GetActiveScene().buildIndex > PlayerPrefs.GetInt(GameConstants.MAXLEVEL_KEY))
        {
            PlayerPrefs.SetInt(GameConstants.MAXLEVEL_KEY, SceneManager.GetActiveScene().buildIndex);
        }
        
        winGamePanel.SetActive(true);
        if (starsTimes[1] > timer)
        {
            SaveStars(3);
            starsWinImages[2].color = Color.white;
        }
        if (starsTimes[0] > timer) {
            SaveStars(2);
            starsWinImages[1].color = Color.white;
        }
        SaveStars(1);
        Time.timeScale = 0f;
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        pausePanel.SetActive(true);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        pausePanel.SetActive(false);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void HackMinigame()
    {
        hackPanel.SetActive(true);
        hackMinigame.StartHack();
    }

    public void GameOver()
    {
        gameOverPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        Time.timeScale = 1;
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}
