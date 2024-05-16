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

    //настройкка кнопок
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

    //настройки отображения экранов в начале
    private void StartViewSettings()
    {
        MenuView.SetActive(true);
        CollectionView.SetActive(false);
        SettingsView.SetActive(false);
        BonusView.SetActive(false);
        ShopView.SetActive(false);

        UpdateCoinTxt();
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

    //сохраненное значение монет
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

    //обновляем коин текст
    public void UpdateCoinTxt()
    {
        coinLabel.text = "" + Coins;
        moneyLabel.text = "" + Money;
        lightningLabel.text = "" + lightningController.Lightning;
    }

    //включаем экран ежедневной награды
    private void CollectionViewOn()
    {
        collectionManager.CheckCollectionToComplete();
        ViewOpen(CollectionView);
    }

    //включаем экран настроек
    private void SettingsViewOn()
    {
        SettingsView.SetActive(true);
    }

    //запуск игровых сцен
    private void start()
    {
        //если можем запустить
        if (lightningController.IsCanSpend())
        {
            //тратим молнию
            lightningController.SpendLightning();
            //запуск игры
            StartCoroutine(openScene("GameScene"));
        }
        else
        {
            //включаем меню нехватки молний
            LightningBalanceView.SetActive(true);
        }
    }

    //включаем сцену бонусов
    private void bonusMenu()
    {
        bonusMenuManager.UpdateKeyCount();
        ViewOpen(BonusView);
    }

    //включаем сцену магазина
    public void shop()
    {
        ViewOpen(ShopView);
    }

    //открываем магазин с молниями
    public void OpenShopLightning()
    {
        ViewOpen(ShopView);
        shopManager.StartToLightningAnim();
    }

    //открываем нужный экран
    private void ViewOpen(GameObject view)
    {
        view.SetActive(true);
        MenuView.SetActive(false);
    }

    //открываем сцену после задержки для перехода
    IEnumerator openScene(string sceneName)
    {
        float fadeTime = mainCamera.GetComponent<Fading>().BeginFade(1);
        yield return new WaitForSeconds(fadeTime);
        SceneManager.LoadScene(sceneName);
    }

}
