using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private bool _isGameOver;
    [SerializeField]
    private GameObject _pauseMenuPanel;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && _isGameOver == true)
        {
            LoadMainMenu();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        if (Input.GetKeyDown(KeyCode.P)){
            _pauseMenuPanel.SetActive(true);
            Time.timeScale = 0;
            
        }
    }

    public void GameOver()
    {
        _isGameOver = true;
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        _pauseMenuPanel.SetActive(false);
    }
}
