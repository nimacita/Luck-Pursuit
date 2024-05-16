using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BonusMenuManager : MonoBehaviour
{

    [SerializeField]
    private GameObject bonusView;
    [SerializeField]
    private GameObject menuView;
    [SerializeField]
    private TMPro.TMP_Text keyText;
    [SerializeField]
    private GameObject bonusClaimPanel;
    public TMPro.TMP_Text coinClaimTxt;
    public GameObject bonusClaimImg;

    [Header("Button Settings")]
    public Button homeBtn;
    public Button closeClaimPanelBtn;


    void Start()
    {
        bonusClaimPanel.SetActive(false);
        ButtonSettings();
        UpdateKeyCount();
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

    //��������� �����
    private void ButtonSettings()
    {
        homeBtn.onClick.AddListener(MainMenu);
        closeClaimPanelBtn.onClick.AddListener(CloseBonusClaimPanel);
    }

    private void Update()
    {
        UpdateKeyCount();
    }

    public void UpdateKeyCount()
    {
        keyText.text = $"{KeyReward}";
    }

    public void CloseBonusClaimPanel()
    {
        bonusClaimPanel.SetActive(false );
        UpdateKeyCount();
    }

    public void OpenClaimPanel(int coinsCount, Sprite bonusImg)
    {
        bonusClaimImg.GetComponent<Image>().sprite = bonusImg;
        coinClaimTxt.text = $"X{coinsCount}";
        bonusClaimPanel.SetActive(true);
    }

    //����� � ����
    public void MainMenu()
    {
        menuView.SetActive(true);
        bonusView.SetActive(false);
    }
}
