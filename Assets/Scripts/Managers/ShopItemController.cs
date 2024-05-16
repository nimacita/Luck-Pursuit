using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static ShopItemController;

public class ShopItemController : MonoBehaviour
{

    [Header("Item Settings")]
    [SerializeField]
    private string itemName;

    [Header("Coin Item")]
    [SerializeField]
    private int coinPrice;

    //для покупки скина карты
    [Space]
    [Header("Card Skin Reward")]
    [SerializeField]
    private Sprite currentCardSkin;
    [SerializeField]
    private Sprite currentBackCardSkin;
    [SerializeField]
    [Space(5), Tooltip("индекс скина для покупки")]
    [Range(0, 2)]
    private int cardSkinSelectedInd;

    [Space]
    [Header("Editor")]
    [SerializeField]
    private ShopManager shopManager;
    [SerializeField]
    private TMPro.TMP_Text itemNameTxt;
    [SerializeField]
    private GameObject itemIcon;
    [SerializeField]
    private GameObject backIcon;
    [SerializeField]
    private GameObject shopBtn;
    [SerializeField]
    private GameObject coinTxt;
    [SerializeField]
    private GameObject equiped;

    void Start()
    {
        itemNameTxt.text = itemName;
        coinTxt.SetActive(false);
        coinTxt.GetComponent<TMPro.TMP_Text>().text = $"{coinPrice}";
        gameObject.GetComponent<Button>().interactable = true;
        gameObject.GetComponent<Button>().onClick.AddListener(ShopItemBtnClick);
        itemIcon.GetComponent<Image>().sprite = currentCardSkin;
        backIcon.GetComponent<Image>().sprite = currentBackCardSkin;

        UpdateItemView();
    }

    private void FixedUpdate()
    {
        UpdateItemView();
    }

    //сохраненное значение куплен ли товар
    private bool IsShopItemPurchased
    {
        get
        {
            if (PlayerPrefs.HasKey($"IsShopItemPurchased{itemName}"))
            {
                if (PlayerPrefs.GetInt($"IsShopItemPurchased{itemName}") == 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                if (cardSkinSelectedInd == 0)
                {
                    PlayerPrefs.SetInt($"IsShopItemPurchased{itemName}", 1);
                    return true;
                }
                else
                {
                    PlayerPrefs.SetInt($"IsShopItemPurchased{itemName}", 0);
                    return false;
                }
            }
        }

        set
        {
            if (value)
            {
                PlayerPrefs.SetInt($"IsShopItemPurchased{itemName}", 1);
            }
            else
            {
                PlayerPrefs.SetInt($"IsShopItemPurchased{itemName}", 0);
            }
        }
    }

    //текущий скин персонажа
    private int CurrentCardSkinInd
    {
        get
        {
            if (PlayerPrefs.HasKey($"CurrentCardInd"))
            {
                return PlayerPrefs.GetInt($"CurrentCardInd");
            }
            else
            {
                PlayerPrefs.SetInt($"CurrentCardInd", 0);
                return PlayerPrefs.GetInt($"CurrentCardInd");
            }
        }

        set
        {
            PlayerPrefs.SetInt($"CurrentCardInd", value);
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
        if (!IsShopItemPurchased)
        {
            //если не куплен
            coinTxt.SetActive(true);
            equiped.SetActive(false);
        }
        else
        {
            //если куплен
            coinTxt.SetActive(false);
            equiped.SetActive(IsEquiped());
        }

    }

    //проверяем если скин экипирован
    private bool IsEquiped()
    {
        bool isEquiped = false;
        //проверяем значение имени спрайта с сохраненным значением
        if (cardSkinSelectedInd == CurrentCardSkinInd)
        {
            isEquiped = true;
        }
        return isEquiped;
    }

    //можем ли купить, выводим нужные экраны
    private void CanClaim()
    {
        if (!IsShopItemPurchased) {
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
                IsShopItemPurchased = true;
            }
        }
        else
        {
            //иначе по нажатию - экипируем
            EquipedSelectProduct();
        }
    }

    //экипируем или снимаем
    private void EquipedSelectProduct()
    {
        //скина персонажа
        if (cardSkinSelectedInd != CurrentCardSkinInd)
        {
            //экипируем 
            CurrentCardSkinInd = cardSkinSelectedInd;
        }
        else
        {
            //снимаем 
            CurrentCardSkinInd = 0;
        }
    }

    //нажатие на кнопку покупки
    public void ShopItemBtnClick()
    {
        CanClaim();
    }
    
}
