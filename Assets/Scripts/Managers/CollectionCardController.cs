using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollectionCardController : MonoBehaviour
{

    public enum CardCollection
    {
        JesterCollection = 1,
        GemsCollection = 2,
        RoayalCollection = 3,
    }
    [Header("Set Bonus Reward")]
    [SerializeField]
    private CardCollection cardCollection;
    [Tooltip("����� �����  � ���������, ����� ��� ���������� ������� �����")]
    [Range(0, 2)]
    public int cardNumber;
    public Sprite cardIcon;

    [Header("Editor")]
    public GameObject cardItemImg;
    public TMPro.TMP_Text itemNameTxt;
    public GameObject buttonTxt;
    public GameObject buttonCollectedImg;

    public CollectionManager collectionManager;

    void Start()
    {
        GetComponent<Button>().onClick.AddListener(ItemBtnClick);
        cardItemImg.GetComponent<Image>().sprite = cardIcon;
        itemNameTxt.text = CardName();
        UpdateItemView();
    }

    //���������� ��� �������� �� �� ���������
    private string CardName()
    {
        string name;
        switch (cardCollection)
        {
            case CardCollection.JesterCollection:
                name = "Jester Collection";
                break;
            case CardCollection.GemsCollection:
                name = "Gems Collection";
                break;
            case CardCollection.RoayalCollection:
                name = "Royal Collection";
                break;
            default:
                name = "default";
                break;
        }
        return name;
    }

    private void Update()
    {
        UpdateItemView();
    }

    //����������� �������� ���� �� �����
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

    //��������� ����������� ��������
    private void UpdateItemView()
    {
        if (IsCardCollected)
        {
            buttonTxt.SetActive(false);
            buttonCollectedImg.SetActive(true);
        }
        else
        {
            buttonTxt.SetActive(true);
            buttonCollectedImg.SetActive(false);
        }
    }

    //������� �� ������
    private void ItemBtnClick()
    {
        //���� �� �������
        if (!IsCardCollected)
        {
            collectionManager.OpenShop();
        }
        else
        {
            //���������� ��������
            collectionManager.GoToAnim(cardItemImg.GetComponent<RectTransform>().position, cardIcon, cardItemImg);
        }

    }
    
}
