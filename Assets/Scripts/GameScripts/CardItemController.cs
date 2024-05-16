using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CardItemController : MonoBehaviour
{

    [Header("Card Settings")]
    [SerializeField] private int cardIndex;
    [SerializeField] private bool isBomb;
    [SerializeField] private bool isfree;
    [SerializeField] private CardItemReward cardReward;
    [SerializeField] private int cardRewardCount;
    [SerializeField] private int backAnimTime;

    [Header("Editor")]
    public MainGameController gameController;

    public GameObject rewardPanel;
    public GameObject gemReward, moneyReward, lightingReward, keyReward;
    public TMPro.TMP_Text rewardTxt;

    public GameObject bombImg;

    public GameObject backImg;

    public AudioSource cardFlipSound;


    void Start()
    {
        GetComponent<Button>().onClick.AddListener(CardItemClick);
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

    //нажатие на карту
    private void CardItemClick()
    {
        gameController.CardItemClick(cardIndex);
        CardBackEnable(false);
    }

    //меняем отображение карты
    private void UpdateCardView()
    {
        if (isBomb)
        {
            rewardPanel.SetActive(false);
            bombImg.SetActive(true);
        }
        else
        {
            bombImg.SetActive(false);
            gemReward.SetActive(false);
            moneyReward.SetActive(false);
            keyReward.SetActive(false);
            lightingReward.SetActive(false);
            switch (cardReward)
            {
                case CardItemReward.gemReward:
                    gemReward.SetActive(true);
                    break;
                case CardItemReward.moneyReward:
                    moneyReward.SetActive(true);
                    break;
                case CardItemReward.keyReward:
                    keyReward.SetActive(true);
                    break;
                case CardItemReward.lightningReward: 
                    lightingReward.SetActive(true);
                    break;
            }
            rewardTxt.text = $"X{cardRewardCount}";
            rewardPanel.SetActive(true);
        }
    }

    //устанавливаем награду
    public void SetCardReward(CardItemReward itemReward, int rewardCount)
    {
        isfree = false;
        cardReward = itemReward;
        cardRewardCount = rewardCount;
        UpdateCardView();
    }

    //устанавливаем бомбу
    public void SetCardBomb()
    {
        isfree = false;
        isBomb = true;
        UpdateCardView();
    }

    //обнуляем карту
    public void ResetCardItem()
    {
        isBomb = false;
        isfree = true;
        cardRewardCount = -1;

        rewardPanel.SetActive(false);
        bombImg.SetActive(false);
    }

    //устанавлиавем скин
    public void SetCurrentSkin(Sprite cardBg, Sprite cardBack, Sprite bombSprite)
    {
        GetComponent<Image>().sprite = cardBg;
        backImg.GetComponent<Image>().sprite = cardBack;
        bombImg.GetComponent<Image>().sprite = bombSprite;
    }

    //возвращаемые значения
    public bool GetIsFree()
    {
        return isfree;
    }

    public bool GetIsBomb()
    {
        return isBomb;
    }

    public CardItemReward GetCardItemReward()
    {
        return cardReward;
    }

    public int GetCardRewardCount()
    {
        return cardRewardCount;
    }

    //выключаем или включаем рубашку карты
    public void CardBackEnable(bool value, bool isAim = true)
    {

        if (isAim)
        {
            //если включаем
            if (value)
            {
                //backImg.SetActive(value);
                StartCoroutine(BackAnim(value,"CardOpenAnim"));
            }
            else
            {
                //если выключаем
                StartCoroutine(BackAnim(value,"CardCloseAnim"));
            }
        }
        else
        {
            backImg.SetActive(value);
        }
    }

    //анимацию рубашки
    private IEnumerator BackAnim(bool value,string animName)
    {
        GetComponent<Animation>().Play(animName);
        if (Sound) cardFlipSound.Play();
        yield return new WaitForSeconds(0.15f);
        backImg.SetActive(value);
    }

}
