using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateKeeper : GameCharacter
{
    [Header("Game Component")]
    [SerializeField] private GameObject _iceBlock;

    private Ball ball;
    private GameScript _gameScript;

    private float _maxY = 2.7f;
    private float _minY = -2.7f;
    private float _topGate = 1.7f;
    private float _bottomGate = -1.7f;
    private bool isFreeze = false;

    void Start()
    {
        ball = GameObject.Find("Ball").GetComponent<Ball>();
        if (ball is null) Debug.LogError("Ball Component Not Found!");

        _gameScript = GameObject.Find("GameScript").GetComponent<GameScript>();
        if (_gameScript is null) Debug.LogError("GameScript Not Found!");

        movementSpeed = 2.0f;
        StartCoroutine(ImproveAccuracy());

    }

    private void FixedUpdate()
    {
        if(!isFreeze)
        {
            if (Mathf.Abs(transform.position.y - ball.transform.position.y) > 0.1)
            {
                if(transform.position.y < ball.transform.position.y)
                {
                    //Go Up
                    if(transform.position.y <= _maxY) GetComponent<Rigidbody2D>().velocity = new Vector2(0,1) * movementSpeed;
                
                }
                else
                {
                    //Go Down
                    if(transform.position.y >= _minY) GetComponent<Rigidbody2D>().velocity = new Vector2(0,-1) * movementSpeed;
                }
            }
            else GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        } 
    }

    public IEnumerator ImproveAccuracy()
    {
        yield return new WaitForSeconds(10);

        if(!isFreeze)
        {
            movementSpeed += 0.1f;
            if(_maxY > _topGate) _maxY -= 0.5f;
            if(_minY < _bottomGate) _minY += 0.1f; 
        }
    }

    public void ActivateFreeze()
    {
        isFreeze = true;
        StartCoroutine(Freeze());
    }

    public void ActivateMakeBigger()
    {
        StartCoroutine(MakeBigger());
    }

    public IEnumerator Freeze()
    {
        _iceBlock.SetActive(true);
        float currSpeed = movementSpeed;
        movementSpeed = 0;
        yield return new WaitForSeconds(10);
        movementSpeed = currSpeed; 
        _iceBlock.SetActive(false);
        isFreeze = false;

        _gameScript.CurrentPowerup = 0;
    }

    public IEnumerator MakeBigger()
    {
        transform.localScale = new Vector3(4.0f, 4.0f, 1.0f);  
        yield return new WaitForSeconds(10);
        transform.localScale = new Vector3(2.0f, 2.0f, 1.0f);

        _gameScript.CurrentPowerup = 0;
    }
}