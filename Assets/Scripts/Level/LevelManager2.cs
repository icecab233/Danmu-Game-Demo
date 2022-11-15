﻿using System.Collections;
using UnityEngine;
/*
public class LevelManager2 : MonoBehaviour
{
    // Singleton
    private static LevelManager2 _instance;
    public static LevelManager2 Instance { get { return _instance; } }

    public int levelNow = 0;
    public LevelData[] levelData;

    // prefabs to Instantiate
    public GameObject fullScreenPopUp;

    // some transform as Instantiate parents
    public Transform mainCanvas;

    // 是否在重新开始等待时间
    public bool preparingRestart = false;
    public float preparingTime = 10.0f;
    public FullScreenPopUp popUpCountDown;

    private const float refreshDiffcultyFactorIntervalTime = 5.0f;
    private float preparingStartTime;
    private IntReference playerCPIntReference;

    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }
        playerCPIntReference = GetComponent<IntReference>();
    }

    void Start()
    {
        // demo，无ui时
        StartLevel(0);

        StartCoroutine(RefreshDiffcultyFactor());
    }

    void Update()
    {
        if (preparingRestart)
        {
            float leftTime = preparingTime - (Time.time - preparingStartTime);
            if (leftTime <= 0)
            {
                preparingRestart = false;
                StartLevel(0);
                return;
            }
            popUpCountDown.showText = "Restarting GAME\nIn " + leftTime.ToString("0.000") + " Seconds";
        }
    }


    // 开启当前关卡
    public void StartLevel(int level)
    {
        GameObject popUpObject = Instantiate(fullScreenPopUp, mainCanvas);
        popUpObject.GetComponent<FullScreenPopUp>().showText = levelData[level].levelName + ConstantText.startLevel;
        WaveManager.Instance.InitWave();
    }

    // 由外部调用
    public void GameFail()
    {
        GameObject popUpObject = Instantiate(fullScreenPopUp, mainCanvas);
        popUpObject.GetComponent<FullScreenPopUp>().showText = ConstantText.gameFail;
        WaveManager.Instance.StopWave();
        StartCoroutine(RestartLevel());
    }

    // 由WaveManager单实例调用
    public void GameSuccess()
    {
        GameObject popUpObject = Instantiate(fullScreenPopUp, mainCanvas);
        popUpObject.GetComponent<FullScreenPopUp>().showText = ConstantText.gameSuccess;
        StartCoroutine(RestartLevel());
    }

    IEnumerator RestartLevel()
    {
        yield return new WaitForSeconds(3.0f);
        preparingRestart = true;
        preparingStartTime = Time.time;
        popUpCountDown = Instantiate(fullScreenPopUp, mainCanvas).GetComponent<FullScreenPopUp>();
        popUpCountDown.dieTime = preparingTime;
    }

    // 定期向wavemaneger更新难度系数
    IEnumerator RefreshDiffcultyFactor()
    {
        while (true)
        {
            // 临时的难度划分机制，等待详细策划
            float cp = playerCPIntReference.value;
            float factor = 1.0f;
            if (cp < 310)
            {
                factor = 0.5f;
            }
            else if (cp < 610)
            {
                factor = 0.75f;
            }
            else if (cp < 1010)
            {
                factor = 1.0f;
            }
            else
            {
                factor = 1.5f;
            }
            WaveManager.Instance.diffcultyFactor = factor;
            yield return new WaitForSeconds(refreshDiffcultyFactorIntervalTime);
        }
    }
}
*/