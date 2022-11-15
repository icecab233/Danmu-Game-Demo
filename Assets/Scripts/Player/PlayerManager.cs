using UnityEngine;
using Assets.HeroEditor.Common.CommonScripts;
using System.Collections.Generic;
using System.Collections;
using DanmuGame.events;

/// <summary>
/// TO DO:
///     1. 将refreshBot改为事件监听机制
/// </summary>

// Singleton Class
public class PlayerManager : MonoBehaviour
{
    // Singleton
    private static PlayerManager _instance;
    public static PlayerManager Instance { get { return _instance; } }

    public GameObject defaultPlayerPrefab;
    public GameObject[] spawnPos;

    /* 
     * 维护在线玩家的变量
     */
    // Player对应的int为玩家的posID
    public Dictionary<Player, int> playerPosMap;
    public List<Player> playerList;
    public bool[] posOccupied;
    // 存储玩家名字的set集合，用于查重，防止一个玩家加入两次
    public HashSet<string> playerNameSet;

    public Player[] bots;
    [SerializeField]
    private int lineCount;
    private const float updateTimeInterval = 5f;

    private int maxPlayerNum;

    public IntEvent OnPlayerOccupyPosEvent;
    public IntEvent OnPlayerFreePosEvent;

    public IntVariable playerSumCP;

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

        maxPlayerNum = spawnPos.Length;
        posOccupied = new bool[maxPlayerNum];
        playerList = new List<Player>();
        playerPosMap = new Dictionary<Player, int>();
        playerNameSet = new HashSet<string>();
    }

    void Start()
    {
        StartCoroutine(GetAllCPCoroutinue());
        RefreshBots();
    }

    void Update()
    {
        // 仅供测试
        if (Input.GetKeyDown(KeyCode.A))
        {
            AddNewPlayer("BOT"+Random.Range(10,100));
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            RageModeForAll(20.0f);
        }
    }

    // 添加新玩家
    // 添加成功，返回添加后总玩家个数
    // 添加失败，返回-1
    public void AddNewPlayer(string name)
    {
        if (playerList.Count == maxPlayerNum) return;

        // 寻找没被占位的位置
        int posId = 0;
        for (int i = 0; i < posOccupied.Length; i++)
            if (posOccupied[i] == false)
            {
                posId = i;
                break;
            }

        AddNewPlayer(name, posId);
    }

    // 返回值：-1人数满，-2位置被占，-3玩家重复加入，否则返回当前玩家总数
    public int AddNewPlayer(string name, int posId)
    {
        if (playerList.Count == maxPlayerNum) return -1;
        if (posOccupied[posId]) return -2;

        // 检查name是否已经加入
        if (playerNameSet.Contains(name)) return -3;

        // Create Prefab
        GameObject playerObject = Instantiate(defaultPlayerPrefab, spawnPos[posId].transform);
        Player player = playerObject.GetComponent<Player>();
        playerList.Add(player);
        playerPosMap.Add(player, posId);
        posOccupied[posId] = true;
        playerNameSet.Add(name);

        OnPlayerOccupyPosEvent.Raise(posId);

        player.changeName(name);
        player.transform.localPosition = new Vector3(0, 0, 0);

        // Random
        player.Randomize();

        RefreshBots();
        return playerList.Count;
    }

    public void PlayerDie(Player player)
    {
        int oldPosId = playerPosMap[player];
        posOccupied[oldPosId] = false;
        OnPlayerFreePosEvent.Raise(oldPosId);
        playerList.Remove(player);
        playerPosMap.Remove(player);
        playerNameSet.Remove(player.playerName);

        RefreshBots();
    }

    public void RandomizePlayer(string name)
    {
        int id = GetIdByName(name);
        if (id >= playerList.Count || id < 0)
        {
            Debug.Log("Random Player: user " + name + " not exist");
            return;
        }

        playerList[id].Randomize();
    }


    //输入名字输出id
    private int GetIdByName(string name)
    {
        for (int i = 0; i < playerList.Count; i++)
        {
            if (playerList[i].playerName == name)
                return i;
        }
        return -1;
    }

    // 更新bot的存在情况。当玩家变动时调用
    // 某一排有玩家时，关闭bot
    // 某一排没玩家时，开启bot
    private void RefreshBots()
    {
        for (int i = 0; i < lineCount; i++)
            if (posOccupied[i] || posOccupied[i + lineCount])
                bots[i].SetActive(false);
            else
                bots[i].SetActive(true);
    }

    // 全员开启rage模式
    public void RageModeForAll(float time)
    {
        foreach (var player in playerList)
        {
            player.StartRage(time);
        }
    }

    public void AddExpFromGift1(string name)
    {
        int id = GetIdByName(name);
        playerList[id].addExp(100);
    }

    // 每隔一定时间进行一次所有玩家CP计算
    IEnumerator GetAllCPCoroutinue()
    {
        while (true)
        {
            yield return new WaitForSeconds(updateTimeInterval);
            playerSumCP.value = GetAllCP();
        }
    }

    // 返回现存所有玩家的CP
    private int GetAllCP()
    {
        int sum = 0;
        foreach(Player player in playerList)
        {
            sum += player.GetCP();
        }
        return sum;
    }
}
