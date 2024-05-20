using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Item : MonoBehaviour
{
    private int _id;
    private int _price;
    private Sprite _mySprite;
    private string _name;

    private Shop _shop;

    void Start()
    {
        _shop = GameObject.Find("UI").GetComponent<Shop>();
        if (_shop == null) Debug.LogError("Shop Is Not Found!");

        Button _button = transform.GetChild(4).GetComponent<Button>();
        _button.onClick.AddListener(() => BuyButtonClicked());

    }

    public int ID{ get { return _id; } set { _id = value; }}
    public int Price
    { 
        get 
        { 
            return _price; 
            } 
            set 
            { 
                _price = value; 
                if(value == 1) transform.GetChild(4).GetComponentInChildren<TMP_Text>().text = "In Use";
                else if(value == 0) transform.GetChild(4).GetComponentInChildren<TMP_Text>().text = "Use";
                else transform.GetChild(4).GetComponentInChildren<TMP_Text>().text = value.ToString();
            }
        }
    public string Name
    { 
        get { return _name; } 
        set 
        {
            _name = value;
            transform.GetChild(1).GetComponent<TMP_Text>().text = value; 
        }
    }
    public Sprite MySprite
    { 
        get { return _mySprite; } 
        set 
        { 
            _mySprite = value;
            transform.GetChild(3).GetComponent<Image>().sprite = value; 
        }
    }

    public void BuyButtonClicked()
    {
        if(Price == 0) _shop.SwitchBlade(ID);
        else if(Price != 1) _shop.BuyItem(ID, Price);
    }

}
