using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : GameCharacter
{
    [Header("Audio Component")]
    [SerializeField] private AudioClip _hitAudioClip;
    
    private SpriteRenderer sprite;
    private Animator anim;
    private GameScript _gameScript;
    new Rigidbody2D rigidbody2D;
    private AudioSource[] _audioSources;
    private AudioSource _collisionAudioSource;

    private float maxExtraSpeed = 10.0f;
    private float extraSpeedPerHit = 1.2f;
    public float rotationSpeed = 720;

    void Start()
    {
        _gameScript = GameObject.Find("GameScript").GetComponent<GameScript>();
        if (_gameScript is null) Debug.LogError("GameScript Not Found!");

        rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
        if (rigidbody2D is null) Debug.LogError("Rigidbody2D Not Found!");

        sprite = GetComponentInChildren<SpriteRenderer>();
        if (sprite is null) Debug.LogError("Sprite Not Found!");

        anim = GetComponentInChildren<Animator>();
        if (anim is null) Debug.LogError("Animator Not Found!");
        
        _audioSources = this.GetComponents<AudioSource>();
        if (_audioSources is null) Debug.LogError("Audio Sources Not Found!");
        _collisionAudioSource = _audioSources[0];
        _collisionAudioSource.clip = _hitAudioClip;

        StartCoroutine(Initiate());
        StartCoroutine(IncreaseSpeed());
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(PlayerPrefs.GetInt("SoundState", 1) == 1 ? true : false) _collisionAudioSource.Play();
        Vector3 currentPosition = transform.position;
        //Liner algebra calculation for direction vector:
        Vector2 direction = (rigidbody2D.velocity  - (Vector2)collision.transform.position).normalized;
        //Set the rotation
        transform.right = direction;
    }

    public IEnumerator Initiate(bool isStartingPlayer1 = true, bool isNewGame = false)
    {
        if(isNewGame) this.PositionBall();
        else this.GetComponent<Rigidbody2D>().velocity = new Vector2(0,0);

        yield return new WaitForSeconds(2.0f);

        //Move the ball to the player that start side:
        if (isStartingPlayer1) this.MoveByDircetion(new Vector2(1, 0));   
        else this.MoveByDircetion(new Vector2(-1, 0));
    }
    
    void PositionBall()
    {
        this.GetComponent<Rigidbody2D>().velocity = new Vector2(0,0);
        this.gameObject.transform.localPosition = new Vector3(0, 0, -1);
    }

    public void MoveByDircetion(Vector2 direction)
    {
        direction = direction.normalized;
        float speed = this.movementSpeed;

        rigidbody2D.velocity = direction * speed;
    }

    public IEnumerator IncreaseSpeed(){
        if(MovementSpeed * this.extraSpeedPerHit <= this.maxExtraSpeed){
            MovementSpeed *= this.extraSpeedPerHit;
            yield return new WaitForSeconds(20.0f);    
        }  
    }

    public void ActivateMakeSmaller()
    {
        StartCoroutine(MakeSmaller());
    }

    public void ActivateMakeFire()
    {
        StartCoroutine(MakeFire());
    }

    public IEnumerator MakeSmaller()
    {
        anim.SetBool("Flicker", true);
        transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);  
        yield return new WaitForSeconds(10);
        transform.localScale = new Vector3(2.0f, 2.0f, 1.0f);
        anim.SetBool("Flicker", false);

        _gameScript.CurrentPowerup = 0;

    }

    public IEnumerator MakeFire()
    {
        anim.SetBool("FireBall", true);  
        float currSpeed = movementSpeed;
        movementSpeed = 10.0f;
        yield return new WaitForSeconds(10);
        movementSpeed = currSpeed;
        anim.SetBool("FireBall", false);

        _gameScript.CurrentPowerup = 0;
    }
}
