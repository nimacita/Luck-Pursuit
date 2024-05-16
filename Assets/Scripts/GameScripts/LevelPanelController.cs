using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelPanelController : MonoBehaviour
{

    [Header("Game Settings")]
    [SerializeField] private float animStep;
    [SerializeField] private int minLvlToRoadAnim;
    private GameObject[] lvlIcons;
    private int maxLvls;
    private int currLvl;

    [Header("Editor")]
    public GameObject lvls;
    private RectTransform lvlsRt;

    void Start()
    {
        currLvl = 0;
    }

    //объявляем все иконки
    public void InitializedLvlIcons(int max)
    {
        maxLvls = max;
        lvlIcons = new GameObject[maxLvls];
        for (int i = 0; i < maxLvls; i++) 
        {
            lvlIcons[i] = lvls.transform.GetChild(i + 1).gameObject;
            lvlIcons[i].transform.GetChild(0).gameObject.GetComponent<TMPro.TMP_Text>().text = $"{i + 1}";
            lvlIcons[i].transform.GetChild(1).gameObject.SetActive(true);
        }
    }

    //открываем текущий уровень
    public void OpenCurrentLvl(int lvl)
    {
        currLvl = lvl;

        if (currLvl > 0)
        {
            //открвыем уровень с анимацией
            StartCoroutine(LvlOpenAnim(currLvl));
        }
        else
        {
            //открываем первый уровень
            lvlIcons[currLvl].transform.GetChild(1).gameObject.SetActive(false);
        }
    }

    //анимация открытитя уровня
    IEnumerator LvlOpenAnim(int ind)
    {
        lvlIcons[ind].GetComponent<Animation>().Play("LvlItemOpen");
        yield return new WaitForSeconds(0.4f);
        lvlIcons[ind].transform.GetChild(1).gameObject.SetActive(false);
    }

    private void Update()
    {
        RoadAnimToLvl(currLvl);
    }

    //анимация спускания дороги у уровню
    private void RoadAnimToLvl(int lvl)
    {
        if (lvl >= minLvlToRoadAnim)
        {
            if (lvlIcons[lvl].GetComponent<RectTransform>().position.y > 0f)
            {
                lvls.GetComponent<RectTransform>().position += new Vector3(0, -animStep * Time.deltaTime, 0);
            }
        }
    }
}
