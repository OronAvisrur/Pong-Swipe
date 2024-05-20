using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class Menu : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TMP_Text _coinsText;
    [SerializeField] private GameObject _soundButton;

    [Header("Sprites")]
    [SerializeField] private Sprite _soundOn;
    [SerializeField] private Sprite _soundOff;

    private SoundSystem soundSystem;

    void Start()
    {
        _coinsText.text = PlayerPrefs.GetInt("Coins", 0).ToString();

        soundSystem = GameObject.Find("Background_Music").GetComponent<SoundSystem>();
        if (soundSystem is null) Debug.LogError("Sound System is NULL");

        if(!SoundSystem.IsPlaying && PlayerPrefs.GetInt("SoundState", 1) == 1 ? true : false) soundSystem.Play(true);
        _soundButton.GetComponent<Image>().sprite = PlayerPrefs.GetInt("SoundState", 1) == 1 ? _soundOn : _soundOff;
    }

    public void LoadGame() 
    {
        SceneManager.LoadScene(1);
    }

    public void LoadShop() 
    {
        SceneManager.LoadScene(2);
    }

    public void SoundButtomClick()
    {
        soundSystem.Play(PlayerPrefs.GetInt("SoundState", 1) == 1 ? false : true);
        SetSoundButtonSprite(ChangeSoundState());
    }

    public void SetSoundButtonSprite(bool state)
    {
        if(state) _soundButton.GetComponent<Image>().sprite = _soundOn;
        else _soundButton.GetComponent<Image>().sprite = _soundOff;
    }

    public bool ChangeSoundState()
    {
        bool currentState = PlayerPrefs.GetInt("SoundState", 1) == 1 ? true : false;
        PlayerPrefs.SetInt("SoundState", currentState ? 0 : 1);

        return !currentState;
    }
}
