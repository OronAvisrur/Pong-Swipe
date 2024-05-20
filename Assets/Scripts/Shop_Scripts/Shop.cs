using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class Shop : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Item _shopItem;
    [SerializeField] private GameObject _items_Container;
    [SerializeField] private TMP_Text _coinsText;

    [Header("Sprites")]
    [SerializeField] private Sprite[] _bladeSprites; 

    [Header("Audio Component")]
    [SerializeField] private AudioClip _purchaseAudioClip;

    private AudioSource _audioSource;

    private enum Colors { White, Black, Blue, Yellow, Green, Red};
    private List<int> ownedBlade;

    private Vector3 posToSpawn;
    private int rowDownCounter = 0;
    private int coinsAmount;
    private int inUseBlade = 0;

    void Start()
    {
        _audioSource = this.GetComponent<AudioSource>();
        if (_audioSource is null) Debug.LogError("Audio Source Not Found!");

        _audioSource.clip = _purchaseAudioClip;

        coinsAmount = PlayerPrefs.GetInt("Coins", 0);
        _coinsText.text = coinsAmount.ToString();

        inUseBlade = PlayerPrefs.GetInt("InUseBlade", 0);

        ownedBlade = SavingSystem.LoadBlades();
        if(ownedBlade is null)
        {
            ownedBlade = new List<int>();
            ownedBlade.Add(0);
            SavingSystem.SaveBladesList(ownedBlade);
        }

        LoadShopItems(6);
    }

    public void LoadShopItems(int amount = 6)
    {
        posToSpawn = new Vector3(-400, 90, 0);

        if(ownedBlade != null)
        {
            foreach (Colors blade in Enum.GetValues(typeof(Colors)))
            {
                rowDownCounter++;

                Item newItem = Instantiate(_shopItem, posToSpawn, Quaternion.identity);
                newItem.transform.SetParent(_items_Container.transform, false); 

                //Set blade ID
                newItem.ID = (int)blade;

                //Set blade image
                newItem.MySprite = _bladeSprites[(int)blade];

                //Set blade name
                switch (blade)
                {
                    case Colors.White:
                        newItem.Name = "White Blade";
                        newItem.Price = 1000;
                        break;
                    case Colors.Black:
                        newItem.Name = "Black Blade";
                        newItem.Price = 1500;
                        break;
                    case Colors.Blue:
                        newItem.Name = "Blue Blade";
                        newItem.Price = 3000;
                        break;
                    case Colors.Yellow:
                        newItem.Name = "Yellow Blade";
                        newItem.Price = 6000;
                        break;
                    case Colors.Green:
                        newItem.Name = "Green Blade";
                        newItem.Price = 7000;
                        break;
                    case Colors.Red:
                        newItem.Name = "Red Blade";
                        newItem.Price = 8500;
                        break;
                    default:
                        newItem.Name = "Unkown";
                        newItem.Price = 0;
                        break;
                }
                
                posToSpawn.x += 400;
                if(rowDownCounter % 3 == 0 && rowDownCounter != 0)
                {
                    rowDownCounter = 0;
                    posToSpawn.y -= 390;
                    posToSpawn.x = -400;
                }

            }

            LoadPricesAndInventory();
        }
        else Debug.LogError("Can't Load Shop!");

    }

    private void LoadPricesAndInventory()
    {
        for(int i = 0; i < ownedBlade.Count; i++)
        {
            if(inUseBlade == ownedBlade[i]) _items_Container.transform.GetChild(ownedBlade[i]).GetComponent<Item>().Price = 1;
            else  _items_Container.transform.GetChild(ownedBlade[i]).GetComponent<Item>().Price = 0;
        }
    }

    public void BuyItem(int itemID, int price)
    {
        if(coinsAmount > price)
        {
            if(PlayerPrefs.GetInt("SoundState", 1) == 1 ? true : false) _audioSource.Play();

            coinsAmount -= price;
            _coinsText.text = coinsAmount.ToString();
            PlayerPrefs.SetInt("Coins", coinsAmount);

            ownedBlade.Add(itemID);
            SavingSystem.SaveBladesList(ownedBlade);
            ownedBlade = SavingSystem.LoadBlades();
        }

        LoadPricesAndInventory();
        
    }

    public void SwitchBlade(int bladeID)
    {
        _items_Container.transform.GetChild(inUseBlade).GetComponent<Item>().Price = 0;
        _items_Container.transform.GetChild(bladeID).GetComponent<Item>().Price = 1;

        PlayerPrefs.SetInt("InUseBlade", bladeID);
        inUseBlade = bladeID;

        LoadPricesAndInventory();
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene(0);
    }
}
