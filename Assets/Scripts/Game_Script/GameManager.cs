using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    private SpawnManager _spawnManager;
    private GameScript _gameScript;
    private UI_Manager _uiManager;

    void Start()
    {
        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        if (_spawnManager is null) Debug.LogError("Spawn Manager Not Found!");
        
        _gameScript = GameObject.Find("GameScript").GetComponent<GameScript>();
        if (_gameScript is null) Debug.LogError("GameScript Not Found!");

        _uiManager = GameObject.Find("UI").GetComponent<UI_Manager>();   
        if (_uiManager is null) Debug.LogError("UI Not Found!");

        _spawnManager.StartSpawning();
        _gameScript.LoadData();
    }

    public void LoadGame() 
    {
        SceneManager.LoadScene(1);
    }

    public void LoadMenu()
    {
        _gameScript.SaveGame();
        SceneManager.LoadScene(0);
    }

       public void Restart()
    {
        SavingSystem.DeleteData();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
    }
}
