using System.Collections;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    // Singleton
    private static LevelManager _instance;
    public static LevelManager Instance { get { return _instance; } }

    public LevelData levelData;

    // prefabs to Instantiate
    public GameObject fullScreenPopUp;

    // some transform as Instantiate parents
    public Transform mainCanvas;

    // �Ƿ������¿�ʼ�ȴ�ʱ��
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
        // demo����uiʱ
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


    // ������ǰ�ؿ�
    public void StartLevel()
    {
        GameObject popUpObject = Instantiate(fullScreenPopUp, mainCanvas);
        popUpObject.GetComponent<FullScreenPopUp>().showText = levelData.levelName + ConstantText.startLevel;
        WaveManager.Instance.InitWave();
    }

    // ���ⲿ����
    public void GameFail()
    {
        GameObject popUpObject = Instantiate(fullScreenPopUp, mainCanvas);
        popUpObject.GetComponent<FullScreenPopUp>().showText = ConstantText.gameFail;
        WaveManager.Instance.StopWave();
        StartCoroutine(RestartLevel());
    }

    // ��WaveManager��ʵ������
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

    // ������wavemaneger�����Ѷ�ϵ��
    IEnumerator RefreshDiffcultyFactor()
    {
        while (true)
        {
            // ��ʱ���ѶȻ��ֻ��ƣ��ȴ���ϸ�߻�
            float cp = playerCPIntReference.value;
            float factor = 1.0f;
            if (cp < 375)
            {
                factor = 0.5f;
            } else if (cp < 750)
            {
                factor = 0.75f;
            } else if (cp < 1000)
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
