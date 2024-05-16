using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LightningBalanceController : MonoBehaviour
{

    [Header("Editor")]
    public GameObject lightningBalanceView;
    public TMPro.TMP_Text timerTxt;
    public Button exitBtn;
    public Button goShopButton;

    public GameObject shopView;
    public ShopManager shopManager;


    private LightningController lightningController;

    void Start()
    {
        lightningController = LightningController.instanceLightning;

        UpdateTimerTxt();

        exitBtn.onClick.AddListener(CloseView);
        goShopButton.onClick.AddListener(GoToShop);
    }

    void Update()
    {
        UpdateTimerTxt();
    }

    //обновляем текст таймера
    private void UpdateTimerTxt()
    {
        TimeSpan sub = new TimeSpan(0, lightningController.GetMinutesToWait(), 0).
            Subtract(new TimeSpan(0,lightningController.GetRemainingMinutes(),lightningController.GetRemainingSeconds()));

        string txt = $"{sub.Minutes:D2}m {sub.Seconds:D2}s";
        timerTxt.text = $"{txt}";
    }

    //выключаем панель
    private void CloseView()
    {
        lightningBalanceView.SetActive(false);
    }

    //идем в магазин
    private void GoToShop()
    {
        shopView.SetActive(true);
        shopManager.StartToLightningAnim();
        lightningBalanceView.SetActive(false);
    }
}
