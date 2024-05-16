using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


//������������� ���������� ������� ��� ������� ������
[System.Serializable]
public struct Rewards
{
    public Vector2 gemCountRange;
    public Vector2 moneyCountRange;
    public Vector2 lightningCountRange;  
    public Vector2 keyCountRange;  
}

//������� ��� �����
public enum CardItemReward
{
    gemReward = 1,
    moneyReward = 2,
    lightningReward = 3,
    keyReward = 4,
}

public class MainGameController : MonoBehaviour
{

    [Header("Game Settings")]
    [Tooltip("������ ���������� ������ �� ������ ������, ��������� �������� �� X �� Y")]
    public Rewards[] rewards;

    [Header("Level Settings")]
    [SerializeField] private int maxLvl;
    [SerializeField] private int currLvl;
    [SerializeField] private int lvlsWithoutBomb;
    public LevelPanelController levelPanelController;

    [Header("Cards")]
    public GameObject[] cardItems;
    private CardItemController[] cardItemsController;
    private Button[] cardItemsButton;
    [Tooltip("������������ �������� ���������� ����� ��� �������")]
    [SerializeField] private int cardEnableAnimDurability;

    [Header("Skins")]
    public Sprite[] cardSkins;
    public Sprite[] cardBackSkins;
    public Sprite[] bombSkins;

    [Header("Sound Settings")]
    public AudioSource claimSound;
    public AudioSource clownSound;
    public AudioSource winSound;

    [Header("Game Panel")]
    public Button quitBtn;
    public Button pauseBtn;
    public TMPro.TMP_Text gameLightTxt, gameMoneyTxt, gameGemTxt, gameKeyTxt;
    public TMPro.TMP_Text currentLightTxt;

    [Header("Current Reward Panel")]
    public GameObject currRewardPanel;
    public GameObject currRGemImg, currRMoneyImg, currRLightningImg, currRKeyImg;
    public TMPro.TMP_Text currRText;
    public Button currRNextBtn;

    [Header("Victory Panel")]
    public GameObject victoryPanel;
    public Button victoryNextBtn;
    public TMPro.TMP_Text victoryLightTxt, victoryMoneyTxt, victoryGemTxt, victoryKeyTxt;

    [Header("Defeat Panel")]
    public GameObject defeatPanel;
    public Button defeatNextBtn, defeatRestartBtn;

    [Header("Pause Panel")]
    public GameObject pausePanel;
    public Button pauseContinueBtn, pauseExitBtn;
    public GameObject musicBtn;
    public GameObject soundBtn;
    [SerializeField]
    private Sprite musicOn;
    [SerializeField]
    private Sprite musicOff;
    [SerializeField]
    private Sprite soundOn, soundOff;

    [Header("Editor")]
    public GameObject mainCamera;

    [Header("Debug")]
    [SerializeField] private int currClaimGems;
    [SerializeField] private int currClaimMoney;
    [SerializeField] private int currClaimKey;
    [SerializeField] private int currClaimLightning;

    private LightningController lightningController;


    void Start()
    {
        lightningController = LightningController.instanceLightning;
        ButtonSettings();
        InitializedCards();
        UpdateCardsSkin();
        StartPanelSettings();

        UpdateBtnSprite();

        levelPanelController.InitializedLvlIcons(maxLvl);

        currLvl = 0;
        //������������� ������ �������
        SetCardsRewards(currLvl, false);
    }

    private bool Music
    {
        get
        {
            if (PlayerPrefs.HasKey("music"))
            {
                if (PlayerPrefs.GetInt("music") == 1)
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
                PlayerPrefs.SetInt("music", 1);
                return true;
            }
        }
        set
        {
            if (value)
            {
                PlayerPrefs.SetInt("music", 1);
            }
            else
            {
                PlayerPrefs.SetInt("music", 0);
            }
        }
    }
    private bool Sound
    {
        get
        {
            if (PlayerPrefs.HasKey("sound"))
            {
                if (PlayerPrefs.GetInt("sound") == 1)
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
                PlayerPrefs.SetInt("sound", 1);
                return true;
            }
        }
        set
        {
            if (value)
            {
                PlayerPrefs.SetInt("sound", 1);
            }
            else
            {
                PlayerPrefs.SetInt("sound", 0);
            }
        }
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

    //����������� �������� ������
    private int KeyReward
    {
        get
        {
            if (PlayerPrefs.HasKey("key"))
            {
                return PlayerPrefs.GetInt("key");
            }
            else
            {
                PlayerPrefs.SetInt("key", 0);
                return 0;
            }
        }
        set
        {
            PlayerPrefs.SetInt("key", value);
        }
    }

    //������� ���� ���������
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

    //��������� ������
    private void ButtonSettings()
    {
        currRNextBtn.onClick.AddListener(DisableCurrentRewardPanel);
        quitBtn.onClick.AddListener(VictoryPanel);
        quitBtn.interactable = true;

        victoryNextBtn.onClick.AddListener(GoHome);

        defeatNextBtn.onClick.AddListener(GoHome);
        defeatRestartBtn.onClick.AddListener(Restart);

        pauseBtn.onClick.AddListener(PauseOn);
        pauseContinueBtn.onClick.AddListener(PauseOff);
        pauseExitBtn.onClick.AddListener(GoHome);
        musicBtn.GetComponent<Button>().onClick.AddListener(MusicBtnClick);
        soundBtn.GetComponent<Button>().onClick.AddListener(SoundBtnClick);
    }

    //��������� ��������� �����
    private void StartPanelSettings()
    {
        UpdateCardRewardTxt();
        currRewardPanel.SetActive(false);
        victoryPanel.SetActive(false);
        defeatPanel.SetActive(false);

        currentLightTxt.text = $"{lightningController.Lightning}/{lightningController.GetMaxLightningCount()}";
    }

    //���������� ������� ����
    private void InitializedCards()
    {
        cardItemsController = new CardItemController[cardItems.Length];
        cardItemsButton = new Button[cardItems.Length];
        for (int i = 0; i < cardItems.Length; i++) 
        {
            cardItemsController[i] = cardItems[i].GetComponent<CardItemController>();
            cardItemsButton[i] = cardItems[i].GetComponent<Button>();
            cardItemsButton[i].interactable = false;
        }
    }

    //��������� ����� ����
    private void UpdateCardsSkin()
    {
        for (int i = 0; i < cardItemsController.Length; i++) 
        {
            cardItemsController[i].SetCurrentSkin(cardSkins[CurrentCardSkinInd], 
                cardBackSkins[CurrentCardSkinInd], 
                bombSkins[CurrentCardSkinInd]);
        }
    }

    //������������� ������� �� �����
    private void SetCardsRewards(int rewardInd,bool isBomb = true)
    {
        //��������
        ResetCards();
        //���� �� ������ ���� �����
        if (isBomb)
        {
            //������� �����
            int randBombInd = UnityEngine.Random.Range(0, cardItemsController.Length);
            cardItemsController[randBombInd].SetCardBomb();
        }

        //������������� �������
        RandomCardRewards(rewardInd);

        //�������� ������
        levelPanelController.OpenCurrentLvl(rewardInd);
    }

    //�������� ������� ��� ������ �����
    private void RandomCardRewards(int rewardInd)
    {
        for (int i = 0; i < cardItemsController.Length; i++) 
        {
            //���� �������� ��������
            if (cardItemsController[i].GetIsFree())
            {
                //���������� ��������� �������
                CardItemReward reward = RandomReward();
                //���� ����� ������� �� ����, �� ������, ���� ���� ������������� �����
                /*if (!CheckIsReward(reward))
                {
                    //������
                    cardItemsController[i].SetCardReward(reward, GetCurrRewardCount(reward, rewardInd));
                }
                else
                {
                    //�������������
                    RandomCardRewards(rewardInd);
                }*/
                //������������� ��������� ������� (������� ����� �����������)
                cardItemsController[i].SetCardReward(reward, GetCurrRewardCount(reward, rewardInd));
            }
        }
    }

    //���������� ��������� ������� �� ������
    private CardItemReward RandomReward()
    {
        CardItemReward randReward;
        int r = UnityEngine.Random.Range(1, 5);
        switch (r)
        {
            case 1:
                randReward = CardItemReward.gemReward;
                break;
            case 2:
                randReward = CardItemReward.moneyReward;
                break;
            case 3:
                randReward = CardItemReward.lightningReward;
                break;
            case 4:
                randReward = CardItemReward.keyReward;
                break;
            default:
                randReward = CardItemReward.gemReward;
                break;
        }
        return randReward;
    }

    //��������� ���� �� ��� ����� �������
    private bool CheckIsReward(CardItemReward reward)
    {
        for (int i = 0; i < cardItemsController.Length; i++)
        {
            if (cardItemsController[i].GetCardItemReward() == reward)
            {
                return true;
            }
        }
        return false;
    }

    //���������� ���������� ������� �� �����
    private int GetCurrRewardCount(CardItemReward reward, int ind)
    {
        int count = 0;
        switch (reward)
        {
            case CardItemReward.gemReward:
                count = (int)UnityEngine.Random.Range(rewards[ind].gemCountRange.x, rewards[ind].gemCountRange.y);
                break;
            case CardItemReward.moneyReward:
                count = (int)UnityEngine.Random.Range(rewards[ind].moneyCountRange.x, rewards[ind].moneyCountRange.y);
                break;
            case CardItemReward.lightningReward:
                count = (int)UnityEngine.Random.Range(rewards[ind].lightningCountRange.x, rewards[ind].lightningCountRange.y);
                break;
            case CardItemReward.keyReward:
                count = (int)UnityEngine.Random.Range(rewards[ind].keyCountRange.x, rewards[ind].keyCountRange.y);
                break;
        }
        return count;
    }

    //�������� ��� �����
    private void ResetCards()
    {
        for (int i = 0; i < cardItemsController.Length; i++)
        {
            //cardItemsController[i].CardBackEnable(true, false);
            cardItemsController[i].ResetCardItem();
            cardItemsButton[i].interactable = true;
        }
    }

    //��������� ������ ����
    private void DisableCardsButtons()
    {
        for (int i = 0; i < cardItemsButton.Length; i++) 
        {
            cardItemsButton[i].interactable = false;
        }
    }

    //������� �� �����  ���������
    public void CardItemClick(int ind)
    {
        DisableCardsButtons();
        quitBtn.interactable = false;

        //���� �� �����
        if (!cardItemsController[ind].GetIsBomb())
        {
            //�������� ������� � ���� ������
            cardItemsController[ind].CardBackEnable(false);
            StartCoroutine(ClaimCurrentReward(cardItemsController[ind].GetCardItemReward(), cardItemsController[ind].GetCardRewardCount(), 
                cardItemsController[ind]));
        }
        else
        {
            //�����������
            cardItemsController[ind].CardBackEnable(false);
            currClaimGems = currClaimKey = currClaimLightning = currClaimMoney = 0;
            StartCoroutine(Defeat());
        }
    }

    //���� ���������� ������� � ������� ���������� �������
    private IEnumerator ClaimCurrentReward(CardItemReward currReward,int currRewardCount, CardItemController cardItemController)
    {
        currRGemImg.SetActive(false);
        currRKeyImg.SetActive(false);
        currRLightningImg.SetActive(false);
        currRMoneyImg.SetActive(false);
        switch (currReward)
        {
            case CardItemReward.gemReward:
                currRGemImg.SetActive(true);
                currClaimGems += currRewardCount;
                break;
            case CardItemReward.moneyReward:
                currRMoneyImg.SetActive(true);
                currClaimMoney += currRewardCount;
                break;
            case CardItemReward.keyReward:
                currRKeyImg.SetActive(true);
                currClaimKey += currRewardCount;
                break;
            case CardItemReward.lightningReward:
                currRLightningImg.SetActive(true);
                currClaimLightning += currRewardCount;
                break;
        }
        currRText.text = $"X{currRewardCount}";
        yield return new WaitForSeconds(cardEnableAnimDurability);
        currRewardPanel.SetActive(true);
        PlaySound(claimSound);

        //�������������� ����� �������
        cardItemController.CardBackEnable(true);
    }

    //��������� ���� ���������� ������� � ��� ������
    private void DisableCurrentRewardPanel()
    {
        currRewardPanel.SetActive(false);
        currLvl++;
        if (!IsWin()) 
        {
            bool bomb;
            if (currLvl < lvlsWithoutBomb)
            {
                bomb = false;
            }
            else
            {
                bomb = true;
            }
            SetCardsRewards(currLvl, bomb);
            quitBtn.interactable = true;
        }
        UpdateCardRewardTxt();
    }

    //�������� ��, ������ �� ��������� �������
    private bool IsWin()
    {
        //���� ������ ��������� �������
        if (currLvl >= maxLvl)
        {
            VictoryPanel();
            return true;
        }
        return false;
    }

    //�������� ���� ���������
    private IEnumerator Defeat()
    {
        yield return new WaitForSeconds(cardEnableAnimDurability);
        UpdateCardRewardTxt();
        PlaySound(clownSound);
        //��������� ���������� ��� �����
        for (int i = 0; i < cardItemsController.Length; i++) 
        {
            if (!cardItemsController[i].GetIsBomb())
            {
                cardItemsController[i].CardBackEnable(false);
                yield return new WaitForSeconds(cardEnableAnimDurability/2f);
            }
        }
        yield return new WaitForSeconds(cardEnableAnimDurability);
        //���� ������ ��������� �� ������ �������
        defeatRestartBtn.interactable = lightningController.IsCanSpend();
        //�������� ������
        defeatPanel.SetActive(true);
    }

    //���� ������
    private void VictoryPanel()
    {
        PlaySound(winSound);
        victoryLightTxt.text = $"X{currClaimLightning}";
        victoryMoneyTxt.text = $"X{currClaimMoney}";
        victoryGemTxt.text = $"X{currClaimGems}";
        victoryKeyTxt.text = $"X{currClaimKey}";

        //���������� ������� � ��������
        Coins += currClaimGems;
        Money += currClaimMoney;
        KeyReward += currClaimKey;
        lightningController.Lightning += currClaimLightning;

        victoryPanel.SetActive(true);
    }

    //��������� ����� �� ������ ����
    private void UpdateCardRewardTxt()
    {
        gameLightTxt.text = $"X{currClaimLightning}";
        gameMoneyTxt.text = $"X{currClaimMoney}";
        gameGemTxt.text = $"X{currClaimGems}";
        gameKeyTxt.text = $"X{currClaimKey}";
    }

    //������� ���� ���� ������
    private void Restart()
    {
        lightningController.SpendLightning(); 
        StartCoroutine(openScene(SceneManager.GetActiveScene().name));
    }

    //������������ � ����
    private void GoHome()
    {
        StartCoroutine(openScene("main menu"));
    }

    //�������� ������ �����
    private void PauseOn()
    {
        pausePanel.SetActive(true);
    }

    //��������� ������ �����
    private void PauseOff()
    {
        pausePanel.SetActive(false); 
    }

    //������ ������
    public void MusicBtnClick()
    {
        if (Music)
        {
            Music = false;
        }
        else
        {
            Music = true;
        }
        UpdateBtnSprite();
    }

    //������ ������
    public void SoundBtnClick()
    {
        if (Sound)
        {
            Sound = false;
        }
        else
        {
            Sound = true;
        }
        UpdateBtnSprite();
    }

    //��������� ������� ������
    private void UpdateBtnSprite()
    {
        if (Music)
        {
            musicBtn.GetComponent<Image>().sprite = musicOn;
        }
        else
        {
            musicBtn.GetComponent<Image>().sprite = musicOff;
        }

        if (Sound)
        {
            soundBtn.GetComponent<Image>().sprite = soundOn;
        }
        else
        {
            soundBtn.GetComponent<Image>().sprite = soundOff;
        }
    }

    //������ ������ ����
    private void PlaySound(AudioSource source)
    {
        if(Sound) source.Play();
    }

    //��������� ����� ����� �������� ��� ��������
    IEnumerator openScene(string sceneName)
    {
        float fadeTime = mainCamera.GetComponent<Fading>().BeginFade(1);
        yield return new WaitForSeconds(fadeTime);
        SceneManager.LoadScene(sceneName);
    }
}
