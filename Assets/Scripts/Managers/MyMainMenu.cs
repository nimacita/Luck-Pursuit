using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class MyMainMenu : MonoBehaviour
{

    [Space]
    [Header("Main settings")]
    public TMPro.TMP_Text coinLabel;
    public TMPro.TMP_Text moneyLabel;
    public TMPro.TMP_Text lightningLabel;

    [Space]
    [Header("Views")]
    public GameObject MenuView;
    public GameObject CollectionView;
    public GameObject SettingsView;
    public GameObject BonusView;
    public GameObject ShopView;
    public GameObject LightningBalanceView;

    [Space]
    [Header("Buttons Settings")]
    public Button startBtn;
    public Button shopBtn;
    public Button settingsBtn;
    public Button bonusBtn;
    public Button collectionBtn;
    public Button addCoinsBtn, addMoneyBtn, addLightningBtn;

    [Space]
    [Header("Editor")]
    [SerializeField]
    private GameObject mainCamera;
    [SerializeField]
    private BonusMenuManager bonusMenuManager;
    [SerializeField]
    private ShopManager shopManager;
    [SerializeField]
    private CollectionManager collectionManager;
    private LightningController lightningController;


    void Start()
    {
        lightningController = LightningController.instanceLightning;
        ButtonSettings();
        UpdateCoinTxt();
        StartViewSettings();
    }

    private void Update()
    {
        UpdateCoinTxt();
    }

    //���������� ������
    private void ButtonSettings()
    {
        startBtn.onClick.AddListener(start);
        shopBtn.onClick.AddListener(shop);
        settingsBtn.onClick.AddListener(SettingsViewOn);
        bonusBtn.onClick.AddListener(bonusMenu);
        collectionBtn.onClick.AddListener(CollectionViewOn);
        addCoinsBtn.onClick.AddListener(shop);
        addMoneyBtn.onClick.AddListener(shop);
        addLightningBtn.onClick.AddListener(OpenShopLightning);
    }

    //��������� ����������� ������� � ������
    private void StartViewSettings()
    {
        MenuView.SetActive(true);
        CollectionView.SetActive(false);
        SettingsView.SetActive(false);
        BonusView.SetActive(false);
        ShopView.SetActive(false);

        UpdateCoinTxt();
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

    //��������� ���� �����
    public void UpdateCoinTxt()
    {
        coinLabel.text = "" + Coins;
        moneyLabel.text = "" + Money;
        lightningLabel.text = "" + lightningController.Lightning;
    }

    //�������� ����� ���������� �������
    private void CollectionViewOn()
    {
        collectionManager.CheckCollectionToComplete();
        ViewOpen(CollectionView);
    }

    //�������� ����� ��������
    private void SettingsViewOn()
    {
        SettingsView.SetActive(true);
    }

    //������ ������� ����
    private void start()
    {
        //���� ����� ���������
        if (lightningController.IsCanSpend())
        {
            //������ ������
            lightningController.SpendLightning();
            //������ ����
            StartCoroutine(openScene("GameScene"));
        }
        else
        {
            //�������� ���� �������� ������
            LightningBalanceView.SetActive(true);
        }
    }

    //�������� ����� �������
    private void bonusMenu()
    {
        bonusMenuManager.UpdateKeyCount();
        ViewOpen(BonusView);
    }

    //�������� ����� ��������
    public void shop()
    {
        ViewOpen(ShopView);
    }

    //��������� ������� � ��������
    public void OpenShopLightning()
    {
        ViewOpen(ShopView);
        shopManager.StartToLightningAnim();
    }

    //��������� ������ �����
    private void ViewOpen(GameObject view)
    {
        view.SetActive(true);
        MenuView.SetActive(false);
    }

    //��������� ����� ����� �������� ��� ��������
    IEnumerator openScene(string sceneName)
    {
        float fadeTime = mainCamera.GetComponent<Fading>().BeginFade(1);
        yield return new WaitForSeconds(fadeTime);
        SceneManager.LoadScene(sceneName);
    }

}
