using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct Collection 
{ 
    public String name;
    public CollectionCardController[] cardInCollection;
}

public class CollectionManager : MonoBehaviour
{

    [Header("Collection")]
    [Tooltip("Карты из разных коллекций")]
    public Collection[] collections;


    [Header("Animation Settings")]
    [SerializeField] private float animTime;
    [SerializeField] private float animStep;
    public GameObject animPanel;
    public GameObject animItem;
    public Button animBtn;
    private bool isAnim;
    private Vector3 itemPos;
    private Vector3 startPos;
    private Vector3 currentPos;
    private GameObject currentObj;

    [Header("Sound Settings")]
    public AudioSource cardItemFlipSound;
    public AudioSource cardItemBackSound;

    [Header("Editor")]
    public GameObject collectionView;
    public GameObject menuView;
    public GameObject shopView;
    public Button homeBtn;
    public GameObject collectionCompletePanel;
    public Button collectionCompleteBtn;
    public GameObject[] collectionsImgs;
    private int currentCollectionInd;

    public ShopManager shopManager;

    void Start()
    {
        CheckCollectionToComplete();

        itemPos = animItem.GetComponent<RectTransform>().position;
        isAnim = false;
        animPanel.SetActive(false);
        animBtn.onClick.AddListener(EndForBtn);

        homeBtn.onClick.AddListener(CloseCollection);

        collectionCompleteBtn.onClick.AddListener(CompleteCollectionChecked);
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

    private void Update()
    {
        AnimPos();
    }

    //собрана ли коллекция
    private bool IsCollection(int ind)
    {

        if (!PlayerPrefs.HasKey($"IsCollection{ind}"))
        {
            PlayerPrefs.SetInt($"IsCollection{ind}", 0);
        }
        if (PlayerPrefs.GetInt($"IsCollection{ind}") == 0)
        {
            return false;
        }
        else
        {
            return true;
        }
        
    }

    //закрываем панель коллекционных карточек
    private void CloseCollection()
    {
        menuView.SetActive(true);
        collectionView.SetActive(false);
    }

    //проверяем есть ли собранные коллекции
    public void CheckCollectionToComplete()
    {
        currentCollectionInd = -1;
        collectionCompletePanel.SetActive(false);
        bool isCompletePanel;
        for (int i = 0; i < collections.Length; i++) 
        {
            collectionsImgs[i].SetActive(false);
            isCompletePanel = true;
            for (int j = 0; j < collections[i].cardInCollection.Length; j++) 
            {
                //если хоть одна карта не собранна не запускаем
                if (!collections[i].cardInCollection[j].IsCardCollected)
                {
                    isCompletePanel = false;
                }
            }
            if (isCompletePanel && !IsCollection(i))
            {
                currentCollectionInd = i;
                collectionsImgs[i].SetActive(true);
                collectionCompletePanel.SetActive(true);
                return;
            }
        }
    }

    //приняли что коллекция собранна
    private void CompleteCollectionChecked()
    {
        collectionCompletePanel.SetActive(false);
        //сохраняем знаение просмотренности
        PlayerPrefs.SetInt($"IsCollection{currentCollectionInd}", 1);
        currentCollectionInd = -1;
    }

    //анимация спуска 
    private void AnimPos()
    {
        if (isAnim)
        {
            animItem.GetComponent<RectTransform>().position = Vector3.MoveTowards(animItem.GetComponent<RectTransform>().position, currentPos, 
                animStep);
        }
    }

    //наичнаем анимацию с карточки
    public void GoToAnim(Vector3 startPosition, Sprite itemSprite, GameObject obj)
    {
        animItem.GetComponent<RectTransform>().position = startPosition;
        startPos = startPosition;
        animItem.GetComponent<Image>().sprite = itemSprite;
        currentObj = obj;
        StartCoroutine(StartAnim());
    }

    //начинаем анимацию
    private IEnumerator StartAnim()
    {
        animPanel.GetComponent<Animation>().Play("CollectItemStartAnim");
        if(Sound) cardItemFlipSound.Play();
        currentObj.SetActive(false);
        animPanel.SetActive(true);
        currentPos = itemPos;
        isAnim = true;
        animBtn.interactable = false;
        yield return new WaitForSeconds(animTime);
        animBtn.interactable = true;
        isAnim = false;
    }

    //заканчиваем анимацию
    private IEnumerator EndAnim()
    {
        animBtn.interactable = false;
        animPanel.GetComponent<Animation>().Play("CollectItemEndAnim");
        if (Sound) cardItemBackSound.Play();
        currentPos = startPos;
        isAnim = true;
        yield return new WaitForSeconds(animTime);
        currentObj.SetActive(true);
        animPanel.SetActive(false);
        isAnim = false;
        animItem.GetComponent<RectTransform>().position = itemPos;
        currentObj = null;
    }

    private void EndForBtn()
    {
        StartCoroutine(EndAnim());
    }

    //открываем магаз
    public void OpenShop()
    {
        //добавить анимацию спуска к карточкам
        shopView.SetActive(true);
        shopManager.StartToCardAnim();
        collectionView.SetActive(false);
    }
    
}
