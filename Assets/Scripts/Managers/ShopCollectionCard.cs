using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopCollectionCard : MonoBehaviour
{
    [Header("Item Settings")]
    [SerializeField]
    private string itemName;

    [Header("Coin Item")]
    [SerializeField]
    private int coinPrice;

    public enum CardCollection
    {
        JesterCollection = 1,
        GemsCollection = 2,
        RoayalCollection = 3,
    }
    [Header("Set Bonus Reward")]
    [SerializeField]
    private CardCollection cardCollection;
    [Tooltip("Номер карты  в коллекции, нужен для сохранения наличия карты")]
    [Range(0, 2)]
    public int cardNumber;
    public Sprite cardIcon;


    [Space]
    [Header("Editor")]
    [SerializeField]
    private ShopManager shopManager;
    [SerializeField]
    private TMPro.TMP_Text itemNameTxt;
    [SerializeField]
    private GameObject itemIcon;
    [SerializeField]
    private GameObject shopBtn;
    [SerializeField]
    private GameObject coinTxt;
    [SerializeField]
    private GameObject collected;

    void Start()
    {
        itemNameTxt.text = itemName;
        coinTxt.SetActive(false);
        coinTxt.GetComponent<TMPro.TMP_Text>().text = $"{coinPrice}";
        gameObject.GetComponent<Button>().interactable = true;
        gameObject.GetComponent<Button>().onClick.AddListener(ShopItemBtnClick);
        itemIcon.GetComponent<Image>().sprite = cardIcon;

        UpdateItemView();
    }


    private void FixedUpdate()
    {
        UpdateItemView();
    }

    //сохраненное значение есть ли карта
    public bool IsCardCollected
    {
        get
        {
            if (!PlayerPrefs.HasKey($"IsCardCollected{cardCollection}{cardNumber}"))
            {
                PlayerPrefs.SetInt($"IsCardCollected{cardCollection}{cardNumber}", 0);
            }
            if (PlayerPrefs.GetInt($"IsCardCollected{cardCollection}{cardNumber}") == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        set
        {
            if (!value)
            {
                PlayerPrefs.SetInt($"IsCardCollected{cardCollection}{cardNumber}", 0);
            }
            else
            {
                PlayerPrefs.SetInt($"IsCardCollected{cardCollection}{cardNumber}", 1);
            }
        }
    }

    //сохраненное значение монет
    private int Coins
    {
        get
        {
            if (PlayerPrefs.HasKey("Coins"))
            {
                return PlayerPrefs.GetInt("Coins");
            }
            else
            {
                PlayerPrefs.SetInt("Coins", 0);
                return 0;
            }
        }
        set
        {
            PlayerPrefs.SetInt("Coins", value);
        }
    }

    //определяем вид кнопки
    private void UpdateItemView()
    {
        itemIcon.SetActive(true);

        //проверить куплено ли
        if (!IsCardCollected)
        {
            //если не куплен
            coinTxt.SetActive(true);
            collected.SetActive(false);
        }
        else
        {
            //если куплен
            coinTxt.SetActive(false);
            collected.SetActive(true);
        }

    }

    //можем ли купить, выводим нужные экраны
    private void CanClaim()
    {
        if (!IsCardCollected)
        {
            //если не куплено - покупаем
            if (Coins < coinPrice)
            {
                //не можем купить
                shopManager.PurchaseViewEnable(false);
            }
            else
            {
                //можем купить
                Coins -= coinPrice;
                shopManager.PurchaseViewEnable(true);
                IsCardCollected = true;
            }
        }
    }

    //нажатие на кнопку покупки
    public void ShopItemBtnClick()
    {
        CanClaim();
    }
}
