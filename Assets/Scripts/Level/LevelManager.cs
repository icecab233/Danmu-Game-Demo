using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    // Singleton
    private static LevelManager _instance;
    public static LevelManager Instance { get { return _instance; } }

    public LevelData levelData;
    public WaveManager waveManager;

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
            Destroy(this.gameObject);
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
        StartLevel();

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
                StartLevel();
                return;
            }
            popUpCountDown.showText = "Restarting GAME\nIn " + leftTime.ToString("0.000") + " Seconds";
        }
    }


    // 开启当前关卡
    public void StartLevel()
    {
        GameObject popUpObject = Instantiate(fullScreenPopUp, mainCanvas);
        popUpObject.GetComponent<FullScreenPopUp>().showText = levelData.levelName + ConstantText.startLevel;
        waveManager.InitWave();
    }

    public void GameFail()
    {
        GameObject popUpObject = Instantiate(fullScreenPopUp, mainCanvas);
        popUpObject.GetComponent<FullScreenPopUp>().showText = ConstantText.gameFail;
        waveManager.StopWave();
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
            float cp = playerCPIntReference.value;
            float factor = 1.0f;
            if (cp < 750)
            {
                factor = 0.5f;
            } else if (cp < 1500)
            {
                factor = 1.0f;
            } else
            {
                factor = 1.5f;
            }
            waveManager.diffcultyFactor = factor;
            yield return new WaitForSeconds(refreshDiffcultyFactorIntervalTime);
        }
    }
}
