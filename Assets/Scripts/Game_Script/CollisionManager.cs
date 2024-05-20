using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionManager : MonoBehaviour
{
    [Header("Audio Component")]
    [SerializeField] private AudioClip _scoreAudioClip;
    private AudioSource[] _audioSources;
    private AudioSource _scoreAudioSource;

    private Ball ball;
    private GameScript _gameScript;
    private GateKeeper _gateKeeper;

    void Start()
    {
        ball = gameObject.GetComponent<Ball>();
        if (ball is null) Debug.LogError("Ball Component Not Found!");

        _gameScript = GameObject.Find("GameScript").GetComponent<GameScript>();
        if (_gameScript is null) Debug.LogError("Game Script Not Found!");

        _gateKeeper = GameObject.Find("GateKeeper").GetComponent<GateKeeper>();  
        if (_gateKeeper is null) Debug.LogError("Gate Keeper Not Found!");

        _audioSources = this.GetComponents<AudioSource>();
        if (_audioSources is null) Debug.LogError("Audio Sources Not Found!");

        _scoreAudioSource = _audioSources[1];
        _scoreAudioSource.clip = _scoreAudioClip;

    }

    void BounceFromObject(GameObject objCollision){

        Vector3 ballPosition = this.transform.position;
        Vector3 racketPosition = objCollision.transform.position;

        float x = objCollision.name == "Blade" ? -1 : 1;
        //Check which height to go:
        float y = ballPosition.y;

        this.ball.MoveByDircetion(new Vector2(x, y));
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        //If ball hit wall give player score and start the game again otherwise check which player to bounce from:
        if(collision.gameObject.name == "Blade" || collision.gameObject.name == "GateKeeper" ){
            this.BounceFromObject(collision.gameObject);
        }
        else if (collision.gameObject.name == "TargetGate")
        {
            if(PlayerPrefs.GetInt("SoundState", 1) == 1 ? true : false) _scoreAudioSource.Play();
            _gameScript.Score += 1;
            StartCoroutine(this.ball.Initiate(true, true));
		}
		else if (collision.gameObject.name == "PlayerGate")
		{
            _gameScript.LowerLifePoint();    
            StartCoroutine(this.ball.Initiate(false, true));
		}
    }
}
