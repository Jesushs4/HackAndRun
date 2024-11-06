
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance;

    [SerializeField] GameObject gameOverPanel;
    [SerializeField] GameObject winGamePanel;
    [SerializeField] GameObject hackPanel;

    private HackMinigame hackMinigame;

    [SerializeField] int[] starsTimer = new int[] { 120, 60 };
    private Transform starsObject;
    private Image[] starsImages;

    [SerializeField] private int health = 3;
    private float timer = 0;

    public int Health { get => health; set => health = value; }
    public float Timer { get => timer; set => timer = value; }

    private void Awake()
    {

        Instance = this;

        gameOverPanel.SetActive(false);
        winGamePanel.SetActive(false);

        starsObject = winGamePanel.transform.Find("Stars");
        starsImages = starsObject.GetComponentsInChildren<Image>();

        hackMinigame = hackPanel.GetComponent<HackMinigame>();

        Time.timeScale = 1;
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
        if (starsTimer[1] > timer)
        {
            SaveStars(3);
            starsImages[2].color = Color.white;
        }
        if (starsTimer[0] > timer) {
            SaveStars(2);
            starsImages[1].color = Color.white;
        }
        SaveStars(1);
        Time.timeScale = 0f;
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
