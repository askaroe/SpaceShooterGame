using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text _scoreText;
    [SerializeField]
    private Image _LivesImg;
    [SerializeField]
    private Sprite[] _liveSprites;
    [SerializeField]
    private Text _gameOverText;
    [SerializeField]
    private Text _restartText;

    public Text bestText;
    public int score;
    public int bestScore;

    private GameManager _gameManager;
    // Start is called before the first frame update
    void Start()
    {
        _scoreText.text = "Score: " + 0;
        _gameOverText.gameObject.SetActive(false);
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();

        bestScore = PlayerPrefs.GetInt("HighScore", 0);
        bestText.text = "Best: " + bestScore;
        
        if(_gameManager == null)
        {
            Debug.LogError("Your gameManager component is NULL");
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Player player = GameObject.Find("Player").GetComponent<Player>();
        //_scoreText.text = "Score: " + player.AddingScore();
    }

    public void AddingScoresUI(int score_text)
    {
        score = score_text;
        _scoreText.text = "Score: " + score_text.ToString();
    }

    public void CheckForBestScore()
    {
        if(score > bestScore)
        {
            bestScore = score;
            PlayerPrefs.SetInt("HighScore", bestScore);
            bestText.text = "Best: " + bestScore.ToString();
        }
    }

    public void UpdateLives(int currentLives)
    {
        _LivesImg.sprite = _liveSprites[currentLives];

        if(currentLives == 0)
        {
            GameOverSequence();
        }
    }

    void GameOverSequence()
    {
        _gameManager.GameOver();
        _gameOverText.gameObject.SetActive(true);
        StartCoroutine(GameOverFlickerRoutine());
        _restartText.gameObject.SetActive(true);
    }

    IEnumerator GameOverFlickerRoutine()
    {
        while (true)
        {
            _gameOverText.text = "GAME OVER";
            yield return new WaitForSeconds(0.5f);
            _gameOverText.text = "";
            yield return new WaitForSeconds(0.5f);
        }
    }

    public void ResumePlay()
    {
        _gameManager.ResumeGame();
    }

    public void BackToMainMenu()
    {
        _gameManager.LoadMainMenu();
    }
}
