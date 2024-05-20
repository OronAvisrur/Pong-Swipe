using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TrailRenderer))]
public class Blade : MonoBehaviour
{
    [Header("Collider Component")]   
    [SerializeField] private EdgeCollider2D _collider;
    
    [Header("Audio Component")]
    [SerializeField] private AudioClip _swipeAudioClip;
    
    private List<Vector2> _colliderPoints = new List<Vector2>();
    
    private TrailRenderer _trail;
    private GameScript _gameScript; 
    private AudioSource _audioSource;

    void Start()
    {
        _gameScript = GameObject.Find("GameScript").GetComponent<GameScript>();
        if (_gameScript is null) Debug.LogError("GameScript Not Found!");

        _trail = this.GetComponent<TrailRenderer>();
        if (_trail is null) Debug.LogError("Trail Not Found!");

        _audioSource = this.GetComponent<AudioSource>();
        if (_audioSource is null) Debug.LogError("Audio Source Not Found!");

        _audioSource.clip = _swipeAudioClip;
        SetColor();
    }

    void Update()
	{
        SetColliderPointsFromTrail(_trail, _collider);
        if(Input.GetMouseButtonDown(0))
        {
            _collider.enabled = true;
            if(PlayerPrefs.GetInt("SoundState", 1) == 1 ? true : false) _audioSource.Play();
            StartCoroutine(InstatiateTrail());
        }
	}

    IEnumerator InstatiateTrail()
    {
        transform.position = GetMousePosition();
        _trail.Clear();

        while(true)
        {
            transform.position = GetMousePosition();
            if(Input.GetMouseButtonUp(0))
            {
                _collider.enabled = false;
                yield break;
            }
            yield return null;
        }
    }

    Vector2 GetMousePosition()
    {
        Vector3 transformPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transformPos.z = 0;
        return transformPos;
    } 

    void SetColliderPointsFromTrail(TrailRenderer trail, EdgeCollider2D collider)
    {
        _colliderPoints.Clear();

        for(int i = 0; i < trail.positionCount; i++)
        {
            _colliderPoints.Add(trail.GetPosition(i));
        }
        collider.SetPoints(_colliderPoints);
    }  

    void SetColor()
    {
        switch (PlayerPrefs.GetInt("InUseBlade", 0))
        {
            case 0:
                _trail.material.color = Color.white;
                break;
            case 1:
                _trail.material.color = Color.black;
                break;
            case 2:
                _trail.material.color = Color.blue;
                break;
            case 3:
                _trail.material.color = Color.yellow;
                break;
            case 4:
                _trail.material.color = Color.green;
                break;
            case 5:
                _trail.material.color = Color.red;
                break;
        }
    }
}
