using System.Collections;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    // Singleton
    private static LevelManager _instance;
    public static LevelManager Instance { get { return _instance; } }

    /// <summary>
    /// 关卡推进流程
    /// 开启游戏，开始关卡0
    ///     1.关卡成功：
    ///         (1).当前关卡不是最后一关，出现popup提示(levelSuccess)并等待3秒，进入下一关
    ///         (2).当前关卡时最后一关，出现popup提示(gameSuccess)并等待10秒，重新开始第一关
    ///     2.关卡失败：
    ///         出现popup提示(gameFail)，重新开始第一关
    ///         
    /// </summary>

    public int levelNow = 0;
    public LevelData[] levelData;

    // prefabs to Instantiate
    public GameObject fullScreenPopUp;

    // some transform as Instantiate parents
    public Transform mainCanvas;

    // 设置关卡背景图片的引用
    public SpriteRenderer bgSpriteRenderer;

    // 是否在重新开始等待时间
    public bool preparingRestart = false;
    public float preparingTime = 10.0f;
    public FullScreenPopUp popUpCountDown;

    private float preparingStartTime;
    private const float refreshDiffcultyFactorIntervalTime = 5.0f;
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
        // 重新开始准备时间
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

    // 进入level后进行初始化
    private void InitLevel(int level)
    {
        bgSpriteRenderer.sprite = levelData[level].levelBackground;
        WaveManager.Instance.levelData = levelData[level];
        WaveManager.Instance.InitWave();
    }


    // 开启当前关卡
    public void StartLevel(int level)
    {
        // 显示开启关卡信息
        GameObject popUpObject = Instantiate(fullScreenPopUp, mainCanvas);
        popUpObject.GetComponent<FullScreenPopUp>().showText = levelData[level].levelName + ConstantText.startLevel;

        InitLevel(level);
    }

    // 由Event Listener组件调用
    public void LevelSuccess()
    {
        // 显示关卡成功信息
        GameObject popUpObject = Instantiate(fullScreenPopUp, mainCanvas);
        popUpObject.GetComponent<FullScreenPopUp>().showText = ConstantText.levelSuccess;

        StartCoroutine(NextLevelCoroutine());
    }

    IEnumerator NextLevelCoroutine()
    {
        yield return new WaitForSeconds(3f);

        if (levelNow + 1 == levelData.Length)
        {
            // 所有关卡通过后
            GameObject popUpObject = Instantiate(fullScreenPopUp, mainCanvas);
            popUpObject.GetComponent<FullScreenPopUp>().showText = ConstantText.gameSuccess;

            StartCoroutine(RestartGameCoroutine());
        }
        else
        {
            levelNow++;
        }

        // 初始化下一关
        InitLevel(levelNow);
    }

    // 由Event Listener组件调用
    public void GameFail()
    {
        // 显示游戏失败信息
        GameObject popUpObject = Instantiate(fullScreenPopUp, mainCanvas);
        popUpObject.GetComponent<FullScreenPopUp>().showText = ConstantText.gameFail;

        StartCoroutine(RestartGameCoroutine());
    }

    IEnumerator RestartGameCoroutine()
    {
        yield return new WaitForSeconds(5.0f);
        preparingRestart = true;
        preparingStartTime = Time.time;
        popUpCountDown = Instantiate(fullScreenPopUp, mainCanvas).GetComponent<FullScreenPopUp>();
        popUpCountDown.dieTime = preparingTime;

        levelNow = 0;
        InitLevel(levelNow);
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