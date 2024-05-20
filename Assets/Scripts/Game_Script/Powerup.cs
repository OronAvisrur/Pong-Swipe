using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [Header("Audio Component")]
    [SerializeField] private AudioClip _powerupAudioClip;
    
    private AudioSource _audioSource;
    private Ball _ball;
    private GateKeeper _gateKeeper;
    private GameScript _gameScript;
    private Animator _anim;

    private float _speed = 3.0f;
    private int powerupID;

    void Start()
    {
        powerupID = Random.Range(0, 6);

        _ball = GameObject.Find("Ball").GetComponent<Ball>();
        if (_ball is null) Debug.LogError("Enemy Not Found!");

        _gateKeeper = GameObject.Find("GateKeeper").GetComponent<GateKeeper>();
        if (_gateKeeper is null) Debug.LogError("Gate Keeper Not Found!");

        _gameScript = GameObject.Find("GameScript").GetComponent<GameScript>();
        if (_gameScript is null) Debug.LogError("GameScript Not Found!");

        _anim = GetComponentInChildren<Animator>();
        if (_anim is null) Debug.LogError("Animator Not Found!");

        _audioSource = this.GetComponent<AudioSource>();
        if (_audioSource is null) Debug.LogError("Audio Source Not Found!");
        _audioSource.clip = _powerupAudioClip;
    }

    void Update()
    {
        //move down and if it leave's the screen destroy it
        if (transform.position.y < -4.5f) Destroy(this.gameObject);
        else transform.Translate(Vector3.down * _speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.name == "BladeCollider")
        {
            if(PlayerPrefs.GetInt("SoundState", 1) == 1 ? true : false) _audioSource.Play();
            _anim.SetTrigger("Collected");
            _gameScript.CurrentPowerup = powerupID;

            ActivatePoweup(powerupID);

            Destroy(this.gameObject, 0.9f);
        }
    }

    public void ActivatePoweup(int powerupID)
    {
        switch(powerupID)
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
                _gameScript.ActivateShield();
                break;
            case 3:
                //Extra life point
                _gameScript.Lifes += 1;
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
}
