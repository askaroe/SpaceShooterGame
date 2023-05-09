using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private bool _isGameOver;
    [SerializeField]
    private GameObject _pauseMenuPanel;

    private Animator _pauseAnimator;

    private void Start()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        int buildIndex = currentScene.buildIndex;
        if(buildIndex == 1)
        {
            _pauseAnimator = GameObject.Find("Pause_menu_panel").GetComponent<Animator>();
            _pauseAnimator.updateMode = AnimatorUpdateMode.UnscaledTime;
        }
    }

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
            pauseMenu();
        }
    }

    public void GameOver()
    {
        _isGameOver = true;
    }

    public void pauseMenu()
    {
        _pauseMenuPanel.SetActive(true);
        _pauseAnimator.SetBool("isPause", true);
        Time.timeScale = 0;
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
