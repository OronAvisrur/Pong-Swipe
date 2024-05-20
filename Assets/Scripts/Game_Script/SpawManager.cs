using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using System.Threading.Tasks;

public class SpawnManager : MonoBehaviour
{
    [Header("Game Component")]
    [SerializeField] private GameObject _powerupsContainer;
    [SerializeField] private GameObject powerup;

    private GameScript _gameScript;

    private bool _stopSpawning = false; 
    
    public bool StopSpawning{ get{ return _stopSpawning; } set { _stopSpawning = value; } }
   
   void Start()
   {
        _gameScript = GameObject.Find("GameScript").GetComponent<GameScript>();
        if (_gameScript is null) Debug.LogError("GameScript Not Found!");
   }

    public void StartSpawning()
    {
        StartCoroutine(SpawnPowerupRoutine());
    }

    IEnumerator SpawnPowerupRoutine() 
    {
        //wait 3 seconds before its start spawning the PowerUps game object
        yield return new WaitForSeconds(3.0f);

        while (_stopSpawning == false)
        {
            if(_gameScript.CurrentPowerup == 0)
            {
                Vector3 posToSpawn = new Vector3(Random.Range(-8.0f, 8.0f), 7, 0);
                GameObject newPoweup = Instantiate(powerup, posToSpawn, Quaternion.identity);
                newPoweup.transform.parent = _powerupsContainer.transform; 
            }

            yield return new WaitForSeconds(Random.Range(3, 30));
        }
    }
}
