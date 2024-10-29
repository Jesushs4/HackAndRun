using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance;

    [SerializeField] GameObject gameOverPanel;
    [SerializeField] GameObject winGamePanel;

    private void Awake()
    {

        Instance = this;

        gameOverPanel.SetActive(false);
        winGamePanel.SetActive(false);

        Time.timeScale = 1;
    }

    public void WinGame()
    {
        winGamePanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void GameOver()
    {
        gameOverPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    /*public void ChangeScene(string sceneName)
    {
        if (sceneName.Equals(GameConstants.MAINMENU_KEY))
        {
            if (LevelManager.Instance.PlayerScore > Getint(GameConstants.HIGHSCORE_KEY))
            {
                SetInt(GameConstants.HIGHSCORE_KEY, LevelManager.Instance.PlayerScore);
            }
            LevelManager.Instance.PlayerScore = 0;
        }
        SceneManager.LoadScene(sceneName);

    }*/

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
