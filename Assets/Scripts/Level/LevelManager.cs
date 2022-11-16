using System.Collections;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    // Singleton
    private static LevelManager _instance;
    public static LevelManager Instance { get { return _instance; } }

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


    // 开启当前关卡
    public void StartLevel(int level)
    {
        // 显示开启关卡信息
        GameObject popUpObject = Instantiate(fullScreenPopUp, mainCanvas);
        popUpObject.GetComponent<FullScreenPopUp>().showText = levelData[level].levelName + ConstantText.startLevel;

        WaveManager.Instance.InitWave();
    }

    // 由外部调用
    public void GameFail()
    {
        // 显示游戏失败信息
        GameObject popUpObject = Instantiate(fullScreenPopUp, mainCanvas);
        popUpObject.GetComponent<FullScreenPopUp>().showText = ConstantText.gameFail;

        WaveManager.Instance.StopWave();
        StartCoroutine(RestartGame());
    }

    IEnumerator RestartGame()
    {
        yield return new WaitForSeconds(5.0f);
        preparingRestart = true;
        preparingStartTime = Time.time;
        popUpCountDown = Instantiate(fullScreenPopUp, mainCanvas).GetComponent<FullScreenPopUp>();
        popUpCountDown.dieTime = preparingTime;
    }

    // 由WaveManager单实例调用
    public void LevelSuccess()
    {
        // 显示关卡成功信息
        GameObject popUpObject = Instantiate(fullScreenPopUp, mainCanvas);
        popUpObject.GetComponent<FullScreenPopUp>().showText = ConstantText.gameSuccess;

        StartCoroutine(GoodToNextLevel());
    }

    IEnumerator GoodToNextLevel()
    {
        yield return new WaitForSeconds(3f);

        if (levelNow+1 == levelData.Length)
        {
            Debug.Log("game success");
            StartCoroutine(RestartGame());
            yield break;
        }

        levelNow++;

        // 初始化下一关
        bgSpriteRenderer.sprite = levelData[levelNow].levelBackground;
        WaveManager.Instance.levelData = levelData[levelNow];
        WaveManager.Instance.InitWave();
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