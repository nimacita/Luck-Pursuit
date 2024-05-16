using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningController : MonoBehaviour
{

    public static LightningController instanceLightning;

    [Header("Settings")]
    [SerializeField]
    [Tooltip("������������ ����� ������, ������ �������� ���������� ��������")]
    private int maxLightningCount;
    [SerializeField]
    [Tooltip("����� � ������� �������� ����� ������")]
    private int minuteToWait;
    [SerializeField]
    private bool isWait;

    [Header("Debug")]
    [SerializeField]
    private int addMinute;
    [SerializeField]
    private int currentLightning;
    [SerializeField]
    private int mineuteToAdd, secondToAdd;

    private DateTime currentTime;

    void Awake()
    {
        if (!instanceLightning)
            instanceLightning = this;
        else
            Destroy(this.gameObject);


        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        currentTime = DateTime.Now.AddMinutes(addMinute);

        IsWaitLightning();
    }

    //����������� �������� ������
    public int Lightning
    {
        get
        {
            if (!PlayerPrefs.HasKey("lightning"))
            {
                PlayerPrefs.SetInt("lightning", maxLightningCount);
            }
            return PlayerPrefs.GetInt("lightning");
        }
        set
        {
            PlayerPrefs.SetInt("lightning", value);
        }
    }

    //����������� �������� ��������� ����� ������
    public DateTime LastSpendLightning
    {
        get
        {
            DateTime dateTime = new DateTime();
            if (!PlayerPrefs.HasKey("LastSpendLightning"))
            {
                return dateTime;
            }
            else
            {
                return DateTime.Parse(PlayerPrefs.GetString("LastSpendLightning"));
            }
        }
        set
        {
            PlayerPrefs.SetString("LastSpendLightning", value.ToString());
        }
    }

    void Update()
    {
        currentLightning = Lightning;
        currentTime = DateTime.Now.AddMinutes(addMinute);
        IsWaitLightning();
    }

    //���� �� ������
    private void IsWaitLightning()
    {
        WaitingLightning();
        if (Lightning < maxLightningCount)
        {
            isWait = true;
        }
        else
        {
            isWait = false;
        }
    }

    private void WaitingLightning()
    {
        if (isWait)
        {
            if (MinuteDifference() > 0)
            {
                mineuteToAdd = 0;
                secondToAdd = 0;
                int mdiff = MinuteDifference();
                Lightning += mdiff;
                if (Lightning > maxLightningCount)
                {
                    Lightning = maxLightningCount;
                }
                //���� ������ ��� ��� ���� �� ��������� �����
                if (Lightning < maxLightningCount)
                {
                    LastSpendLightning = LastSpendLightning.AddMinutes(minuteToWait * mdiff);
                }
                else
                {
                    isWait = false;
                }
            }
            else
            {
                mineuteToAdd = currentTime.Subtract(LastSpendLightning).Minutes;
                secondToAdd = currentTime.Subtract(LastSpendLightning).Seconds;
            }
        }
    }

    //������� ������ ����������
    private int MinuteDifference()
    {
        int mdiff = 0;
        if (LastSpendLightning.Year == currentTime.Year && LastSpendLightning.Month == currentTime.Month)
        {
            //���� ������� ������ ������� ���������� ����� �����
            if (currentTime.Hour - LastSpendLightning.Hour == 0 && currentTime.Day == LastSpendLightning.Day &&
                currentTime.Minute - LastSpendLightning.Minute < minuteToWait)
            {
                mdiff = 0;
            }
            else
            {
                //���� ���� ����
                if (LastSpendLightning.Day == currentTime.Day)
                {
                    TimeSpan subTime = currentTime.Subtract(LastSpendLightning);
                    mdiff += subTime.Hours * (60 / minuteToWait);
                    mdiff += subTime.Minutes / minuteToWait;
                }
                else
                {
                    //���� ������ ��� � ��������� ��������
                    if (LastSpendLightning.AddMinutes(minuteToWait).Month == currentTime.Month ||
                        currentTime.Subtract(LastSpendLightning).Days < 2)
                    {
                        TimeSpan lastSpan = new TimeSpan(LastSpendLightning.Hour, LastSpendLightning.Minute, LastSpendLightning.Second);
                        TimeSpan currentSpan = new TimeSpan(currentTime.Hour, currentTime.Minute, currentTime.Second);
                        TimeSpan subTime = (new TimeSpan(24, 0, 0) - lastSpan + currentSpan);
                        mdiff += subTime.Hours * (60 / minuteToWait);
                        mdiff += subTime.Minutes / minuteToWait;
                    }
                    else
                    {
                        mdiff = 0;
                    }
                }
            }
        }
        else
        {
            mdiff = maxLightningCount;
        }

        if (mdiff > maxLightningCount)
        {
            mdiff = maxLightningCount;
        }
        return mdiff;
    }

    //����� �� ��������� ������
    public bool IsCanSpend()
    {
        if (Lightning > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    //������ ������
    public void SpendLightning()
    {
        if (IsCanSpend())
        {
            Lightning--;
            //���� �� ���� � ������ ������ ��� �����������, �� ���������� ����� ����� ���������
            if (!isWait && Lightning < maxLightningCount)
            {
                LastSpendLightning = currentTime;
            }
        }
    }

    //���������� ������������ �������� ������
    public int GetMaxLightningCount()
    {
        return maxLightningCount;
    }

    //���������� ����� ��� ����� ������
    public int GetMinutesToWait()
    {
        return minuteToWait;
    }

    //���������� ���������� ����� � �������
    public int GetRemainingMinutes()
    {
        if (isWait)
        {
            return mineuteToAdd;
        }
        else
        {
            return 0;
        }
    }

    //���������� ���������� ����� � ��������
    public int GetRemainingSeconds()
    {
        if (isWait)
        {
            return secondToAdd;
        }
        else
        {
            return 0;
        }
    }


}
