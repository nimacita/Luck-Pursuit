using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{

    [Header("Editor")]
    [SerializeField]
    private TMPro.TMP_Text coinValue;
    [SerializeField]
    private TMPro.TMP_Text moneyValue;
    [SerializeField]
    private TMPro.TMP_Text lightningValue;
    [SerializeField]
    private GameObject purchaseView;
    [SerializeField]
    private GameObject successPurchase, errorPurchase;
    [SerializeField]
    private GameObject menuView;
    [SerializeField]
    private GameObject shopView;

    [Header("Button Settings")]
    public Button homeBtn;
    public Button purchaseDisableBtn;

    [Header("Anim Settings")]
    [SerializeField]
    private float animStep;
    [SerializeField]
    private int panleOffsetY;
    [SerializeField]
    private bool isAnim = false;
    public GameObject content;
    public GameObject cardItemsPanel;
    public GameObject lightningPanel;
    public GameObject shopAntiScrollBtn;
    private GameObject toScrollPanel = null;

    private LightningController lightningController;

    void Start()
    {
        lightningController = LightningController.instanceLightning;
        UpdateCoinTxt();
        ButtonSettings();
        purchaseView.SetActive(false);
    }

    private void Update()
    {
        UpdateCoinTxt();
        AnimSlideToCards(toScrollPanel);
    }

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

    //����������� �������� �����
    private int Money
    {
        get
        {
            if (PlayerPrefs.HasKey("Money"))
            {
                return PlayerPrefs.GetInt("Money");
            }
            else
            {
                PlayerPrefs.SetInt("Money", 0);
                return 0;
            }
        }
        set
        {
            PlayerPrefs.SetInt("Money", value);
        }
    }

    //��������� ������
    private void ButtonSettings()
    {
        homeBtn.onClick.AddListener(MainMenu);
        purchaseDisableBtn.onClick.AddListener(PurchaseDisable);
    }

    //�������� ���� �������
    public void PurchaseViewEnable(bool success)
    {
        purchaseView.SetActive(true);
        UpdateCoinTxt();
        if (success)
        {
            successPurchase.SetActive(true);
            errorPurchase.SetActive(false);
        }
        else
        {
            successPurchase.SetActive(false);
            errorPurchase.SetActive(true);
        }
    }

    //��������� ���� �������
    public void PurchaseDisable()
    {
        purchaseView.SetActive(false);
        UpdateCoinTxt();
    }

    //�������� ������ 
    private void AnimSlideToCards(GameObject toScrollPanel)
    {
        shopAntiScrollBtn.SetActive(isAnim);
        if (isAnim && toScrollPanel != null) 
        {
            float step = animStep + Time.deltaTime;
            if (toScrollPanel.GetComponent<RectTransform>().position.y < panleOffsetY) 
            {
                content.GetComponent<RectTransform>().position += new Vector3(0, step, 0);   
            }
            else
            {
                isAnim = false;
            }
        }
    }

    //������ �������� �� ����
    public void StartToCardAnim()
    {
        isAnim = true;
        toScrollPanel = cardItemsPanel;
    }

    //������ �������� �� ������
    public void StartToLightningAnim()
    {
        isAnim = true;
        toScrollPanel = lightningPanel;
    }

    //������������� ��������
    public void StopAnim()
    {
        isAnim = false;
        toScrollPanel = null;
    }

    //��������� ����� ������
    public void UpdateCoinTxt()
    {
        coinValue.text = $"{Coins}";
        moneyValue.text = $"{Money}";
        lightningValue.text = $"{lightningController.Lightning}";
    }

    //����� � ����
    public void MainMenu()
    {
        menuView.SetActive(true);
        shopView.SetActive(false);
    }

}
