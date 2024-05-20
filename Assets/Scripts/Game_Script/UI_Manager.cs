using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UI_Manager : MonoBehaviour
{
    [Header("Text Components")]
    [SerializeField] private TMP_Text _scoreText;
    [SerializeField] private TMP_Text _coinsText;

    [Header("Panels")]
    [SerializeField] private GameObject _pasuePanel;
    [SerializeField] private GameObject _gameOverPanel;

    [Header("Sprites")]
    [SerializeField] private GameObject[] _lifeSprites;
    [SerializeField] private Sprite _soundOn;
    [SerializeField] private Sprite _soundOff;

    [Header("Buttons")]
    [SerializeField] private GameObject _soundButton;

    private GameManager _gameManager;
    private GameScript _gameScript;

    void Start()
    {
        _gameScript = GameObject.Find("GameScript").GetComponent<GameScript>();
        if (_gameScript is null) Debug.LogError("GameScript Not Found!");

        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        if (_gameManager is null) Debug.LogError("Game Manager Not Found!");
        
        _scoreText.text = "0";
        _pasuePanel.gameObject.SetActive(false);
        _coinsText.text = PlayerPrefs.GetInt("Coins", 0).ToString();

        _soundButton.GetComponent<Image>().sprite = PlayerPrefs.GetInt("SoundState", 1) == 1 ? _soundOn : _soundOff;
    }

    void Update()
    {
        LifeBar();
        _coinsText.text = PlayerPrefs.GetInt("Coins", 0).ToString();
    }

    public void UpdateScore(int playerScore)
    {
        _scoreText.text = playerScore.ToString();
    }

    public void PasueGame()
    {
        _gameScript.SaveGame();
        _pasuePanel.gameObject.SetActive(true);
        Time.timeScale = 0;
    }
    public void UnPauseGame()
    {
        _pasuePanel.gameObject.SetActive(false);
        Time.timeScale = 1;
    }
    public void GameOver()
    {
        _gameOverPanel.SetActive(true);

        var finalScore = _gameOverPanel.transform.GetChild(4).GetComponent<TMP_Text>();
        finalScore.text = "Score: " + _gameScript.Score.ToString();
        
        var finalHighScore = _gameOverPanel.transform.GetChild(5).GetComponent<TMP_Text>();
        finalHighScore.text = "Highest Score: " +  _gameScript.CalculateHighScore().ToString();
    }
    public void LifeBar()
    {
        switch (_gameScript.Lifes)
        {
            case 0:
                _lifeSprites[0].SetActive(false);  
                _lifeSprites[1].SetActive(false);
                _lifeSprites[2].SetActive(false);
                break;
            case 1:
                _lifeSprites[0].SetActive(true); 
                _lifeSprites[1].SetActive(false);
                _lifeSprites[2].SetActive(false);
                break;
            case 2:
                _lifeSprites[0].SetActive(true);
                _lifeSprites[1].SetActive(true);
                _lifeSprites[2].SetActive(false);
                break;
            case 3:
                _lifeSprites[0].SetActive(true);
                _lifeSprites[1].SetActive(true);
                _lifeSprites[2].SetActive(true);
                break;
        }
    }

    public void SetSoundButtonSprite(bool state)
    {
        if(state) _soundButton.GetComponent<Image>().sprite = _soundOn;
        else _soundButton.GetComponent<Image>().sprite = _soundOff;
    }

}
