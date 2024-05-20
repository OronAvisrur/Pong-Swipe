using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScript : MonoBehaviour
{
    [Header("UI Component")]
    [SerializeField] private UI_Manager _UIManager;

    [Header("Game Component")]
    [SerializeField] private GameObject _shield; 
    [SerializeField] private Ball _ball;
    [SerializeField] private GateKeeper _gateKeeper;
    
    private SoundSystem soundSystem;
    private SpawnManager spawnManager;

    private int score = 0;
    private int lifes = 3;
    private float waitForCoin;
    private int currentPowerup = 0;
    private bool dataLoaded = false;

    public float WaitForCoin{ get { return waitForCoin; } set { waitForCoin = value; } }
    public int Lifes{ get { return lifes; } set { lifes = value; } }
    public int CurrentPowerup{ get { return currentPowerup; } set { currentPowerup = value; } }
    public int Score
    { 
        get { return score; } 
        set 
        { 
            score = value;
            _UIManager.UpdateScore(score);
        } 
    }

    void Start()
    {
        Time.timeScale = 1;

        if (_ball is null) Debug.LogError("Ball Not Found!");
        if (_shield is null) Debug.LogError("Gate Shield Not Found!");
        if (_gateKeeper is null) Debug.LogError("Gate Keeper Not Found!");
        if (_UIManager is null) Debug.LogError("UI Manager Not Found!");

        soundSystem = GameObject.Find("Background_Music").GetComponent<SoundSystem>();
        if (soundSystem is null) Debug.LogError("Sound System is NULL");

        spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        if (spawnManager is null) Debug.LogError("Spawn Manager is NULL");

        StartCoroutine(IncreaseEarnableCoins());
    }

    void Update()
    {
        if(lifes == 0) GameOverSequence();
    }

    public void GameOverSequence()
    {
        Time.timeScale = 0;
        spawnManager.StopSpawning = true;
        _UIManager.GameOver();
        SavingSystem.DeleteData();
    }

    public void LowerLifePoint()
    {
        if(lifes > 0) lifes--;
    }

    public void ActivateShield()
    {
        StartCoroutine(Shield());
    }

    public IEnumerator Shield()
    {
        _shield.SetActive(true);
        _shield.GetComponent<Animator>().SetTrigger("Freeze");
        yield return new WaitForSeconds(10);
        _shield.SetActive(false); 
    }

    public IEnumerator IncreaseEarnableCoins()
    {
        yield return new WaitForSeconds(waitForCoin);
        
        int coins = coins = PlayerPrefs.GetInt("Coins", 0);
        coins++;
        PlayerPrefs.SetInt("Coins", coins);

        if(waitForCoin >= 5) waitForCoin -= 0.5f;
    }

    public int CalculateHighScore()
    {
        int highestScore = PlayerPrefs.GetInt("HighScore", 0);
        if(score > highestScore) 
        {
            PlayerPrefs.SetInt("HighScore", score);
            return score;   
        }
        else return highestScore;
    }

    public void LoadData()
    {
        GameData data = SavingSystem.LoadGame();
        if (data != null)
        {
            _gateKeeper.SetLocation(data.GoalKeeperLocation[0], data.GoalKeeperLocation[1]);
            _gateKeeper.MovementSpeed = data.GoalKeeperSpeed;
            _ball.SetLocation(data.BallLocation[0], data.BallLocation[1]);
            _ball.MovementSpeed = data.BallSpeed;
            waitForCoin = data.WaitTimeForCoins;
            lifes = data.PlayerLifes;

            if(data.PowerupID != 0)
            {
                //3 is Extra life point No Need to Add! 
                currentPowerup = data.PowerupID;
                switch(currentPowerup)
                {
                    case 0: 
                        //Freeze Gate keeper for 5 sec 
                        _gateKeeper.ActivateFreeze();
                        break;
                    case 1:
                        //Fire ball - extrmely fast ball for 10 sec
                        _ball.ActivateMakeFire();
                        break;
                    case 2:
                        //Gate shield for 10 sec
                        ActivateShield();
                        break;
                    case 4:
                        //Super gate keeper - become big and strong for 10 sec
                        _gateKeeper.ActivateMakeBigger();
                        break;
                    case 5:
                        //Small ball for 10 sec
                        _ball.ActivateMakeSmaller();
                        break;
                    default:
                        Debug.Log("Unknown Powerup");
                        break;
                }
            }
            dataLoaded = true;
        }
        else 
        {
            _gateKeeper.SetLocation(-5.5f, 0);
            _gateKeeper.MovementSpeed = 2.0f;
            _ball.SetLocation(0, 0);
            _ball.MovementSpeed = 5.0f;
            waitForCoin = 35.0f;
            lifes = 3;
            dataLoaded = false;
        }
    }

    public void SaveGame()
    {
        SavingSystem.SaveGame(this, _gateKeeper as GameCharacter, _ball as GameCharacter);
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)  SaveGame();
        else if (dataLoaded) LoadData();
    }

    public void SoundButtomClick()
    {
        soundSystem.Play(PlayerPrefs.GetInt("SoundState", 1) == 1 ? false : true);

        bool currentSoundState = PlayerPrefs.GetInt("SoundState", 1) == 1 ? true : false;
        PlayerPrefs.SetInt("SoundState", currentSoundState ? 1 : 0);
        _UIManager.SetSoundButtonSprite(currentSoundState);
    }

}



