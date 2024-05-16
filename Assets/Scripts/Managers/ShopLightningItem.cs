using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopLightningItem : MonoBehaviour
{

    [Header("Item Settings")]
    [SerializeField]
    private int lightningReward;

    [Header("Coin Item")]
    [SerializeField]
    private int coinPrice;

    [Space]
    [Header("Editor")]
    [SerializeField]
    private TMPro.TMP_Text coinTxt;
    [SerializeField]
    private TMPro.TMP_Text rewardTxt;
    [SerializeField]
    private ShopManager shopManager;
    private Button shopBtn;

    private LightningController lightningController;


    //����������� �������� �����
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

    void Start()
    {
        shopBtn = GetComponent<Button>();
        lightningController = LightningController.instanceLightning;
        shopBtn.onClick.AddListener(ShopItemBtnClick);
        UpdateItemView();
    }

    //��������� ��� ������
    private void UpdateItemView()
    {
        coinTxt.text = $"{coinPrice}";
        rewardTxt.text = $"{lightningReward}";
    }

    //������� �� ������ �������
    public void ShopItemBtnClick()
    {
        if (Coins < coinPrice)
        {
            //�� ����� ������
            shopManager.PurchaseViewEnable(false);
        }
        else
        {
            //����� ������
            Coins -= coinPrice;
            lightningController.Lightning += lightningReward;
            shopManager.PurchaseViewEnable(true);
        }
    }

}
