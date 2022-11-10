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
    // 准备或战斗剩余的时间，以供访问
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

    // 供外部调用，重新开始此关卡
    public void InitWave()
    {
        waveStartTime = Time.time;
        currentWaveStatus = WaveStatus.Preparing;

        // 重置变量
        livingMonsters.Clear();
        waveNow = 0;
        coroutine = null;
    }

    // 供外部调用，停止此关卡
    public void StopWave()
    {
        currentWaveStatus = WaveStatus.Stop;
        foreach(var monster in livingMonsters)
        {
            Destroy(monster.gameObject);
        }
        if (coroutine != null) StopCoroutine(coroutine);
    }

    // 计算当前的状态，哪一波，是准备时间还是战斗时间
    private void CalculateWaveProcess()
    {
        float timePassed = Time.time - waveStartTime;

        /*
            可能存在的三种状态变化
            1. 当前为准备状态，时间超过准备时间，开始战斗刷怪，生成敌人
            2. 当前为战斗状态，时间超过战斗刷怪时间
                (1). 战场还有残余怪物，进入等待状态，停止刷怪
                (2). 战场无怪物，进入下一波准备状态
            3. 当前为等待状态，战场无怪，进入下一波的准备状态
        */

        switch(currentWaveStatus)
        {
            case WaveStatus.Preparing:
                // 状态变化1
                if (timePassed >= levelData.wavePrepareTime[waveNow])
                {
                    GoodToStartBattle();
                }
                break;

            case WaveStatus.Battle:
                // 状态变化2的条件
                if (timePassed >= levelData.waveTime[waveNow] + levelData.wavePrepareTime[waveNow])
                {
                    // 状态变化2.1
                    if (livingMonsters.Count > 0)
                    {
                        GoodToStartWaiting();
                    } else
                    // 状态变化2.2
                    {
                        GoodToNextWave();
                    }
                }
                break;

            case WaveStatus.Waiting:
                // 状态变化3
                if (livingMonsters.Count == 0)
                {
                    GoodToNextWave();
                }
                break;
        }

        // 计算当前状态的剩余时间
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

        // 开始新的一波coroutine
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
