using Assets.FantasyMonsters.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    // Singleton
    private static WaveManager _instance;
    public static WaveManager Instance { get { return _instance; } }

    public LevelData levelData;
    public GameObject[] spawnPos;

    public GameObject fullSreenPopUp;
    public Transform mainCanvas;

    public enum WaveStatus
    {
        Stop,
        Preparing,
        Battle,
        Waiting
    }
    public WaveStatus currentWaveStatus;

    private float waveStartTime;
    public int waveNow = 0;
    // ׼����ս��ʣ���ʱ�䣬�Թ�����
    public float leftTime;

    public List<GameObject> livingMonsters;

    private IEnumerator coroutine = null;

    private void Awake()
    {
        // Singleton
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }

        livingMonsters = new List<GameObject>();

        currentWaveStatus = WaveStatus.Stop;
    }

    private void Update()
    {
        CalculateWaveProcess();
    }

    // ���ⲿ���ã����¿�ʼ�˹ؿ�
    public void InitWave()
    {
        waveStartTime = Time.time;
        currentWaveStatus = WaveStatus.Preparing;

        // ���ñ���
        livingMonsters.Clear();
        waveNow = 0;
        coroutine = null;
    }

    // ���ⲿ���ã�ֹͣ�˹ؿ�
    public void StopWave()
    {
        currentWaveStatus = WaveStatus.Stop;
        foreach(var monster in livingMonsters)
        {
            Destroy(monster.gameObject);
        }
        if (coroutine != null) StopCoroutine(coroutine);
    }

    // ���㵱ǰ��״̬����һ������׼��ʱ�仹��ս��ʱ��
    private void CalculateWaveProcess()
    {
        float timePassed = Time.time - waveStartTime;

        /*
            ���ܴ��ڵ�����״̬�仯
            1. ��ǰΪ׼��״̬��ʱ�䳬��׼��ʱ�䣬��ʼս��ˢ�֣����ɵ���
            2. ��ǰΪս��״̬��ʱ�䳬��ս��ˢ��ʱ��
                (1). ս�����в���������ȴ�״̬��ֹͣˢ��
                (2). ս���޹��������һ��׼��״̬
            3. ��ǰΪ�ȴ�״̬��ս���޹֣�������һ����׼��״̬
        */

        switch(currentWaveStatus)
        {
            case WaveStatus.Preparing:
                // ״̬�仯1
                if (timePassed >= levelData.wavePrepareTime[waveNow])
                {
                    GoodToStartBattle();
                }
                break;

            case WaveStatus.Battle:
                // ״̬�仯2������
                if (timePassed >= levelData.waveTime[waveNow] + levelData.wavePrepareTime[waveNow])
                {
                    // ״̬�仯2.1
                    if (livingMonsters.Count > 0)
                    {
                        GoodToStartWaiting();
                    } else
                    // ״̬�仯2.2
                    {
                        GoodToNextWave();
                    }
                }
                break;

            case WaveStatus.Waiting:
                // ״̬�仯3
                if (livingMonsters.Count == 0)
                {
                    GoodToNextWave();
                }
                break;
        }

        // ���㵱ǰ״̬��ʣ��ʱ��
        switch(currentWaveStatus)
        {
            case WaveStatus.Preparing:
                leftTime = levelData.wavePrepareTime[waveNow] - timePassed;
                break;
            case WaveStatus.Battle:
                leftTime = levelData.waveTime[waveNow] + levelData.wavePrepareTime[waveNow] - timePassed;
                break;
            case WaveStatus.Waiting:
                leftTime = 0;
                break;
        }
    }

    private void GoodToStartBattle()
    {
        currentWaveStatus = WaveStatus.Battle;

        // ��ʼ�µ�һ��coroutine
        coroutine = SpawnMonsterCoroutine(waveNow);
        StartCoroutine(coroutine);

        GameObject gameObject =  Instantiate(fullSreenPopUp, mainCanvas);
        gameObject.GetComponent<FullScreenPopUp>().showText = ConstantText.nextWaveComing;
    }

    private void GoodToStartWaiting()
    {
        currentWaveStatus = WaveStatus.Waiting;
        if (coroutine != null) StopCoroutine(coroutine);
        coroutine = null;
    }

    private void GoodToNextWave()
    {
        if (waveNow + 1 == levelData.waveCount)
        {
            // GAME END...
            Debug.Log("Game end - success");

        }
        else
        {
            currentWaveStatus = WaveStatus.Preparing;
            waveNow++;
            waveStartTime = Time.time;
        }
        if (coroutine != null) StopCoroutine(coroutine);
        coroutine = null;

    }

    IEnumerator SpawnMonsterCoroutine(int wave)
    {
        while (true)
        {
            int spawnPosId = Random.Range(0, spawnPos.Length);
            int spawnMonsterId = Random.Range(0, levelData.waves[wave].monsters.Length);
            GameObject newMonster = Instantiate(levelData.waves[wave].monsters[spawnMonsterId], spawnPos[spawnPosId].transform);
            newMonster.transform.localScale = new Vector3(0.25f, 0.25f, 1);
            livingMonsters.Add(newMonster);

            yield return new WaitForSeconds(Random.Range(levelData.waves[wave].spawnIntervalBase - levelData.waves[wave].spawnIntervalRandom, levelData.waves[wave].spawnIntervalBase + levelData.waves[wave].spawnIntervalRandom));
        }
    }
}
