using UnityEngine;
using Assets.HeroEditor.Common.CommonScripts;
using System.Collections.Generic;
using System.Collections;
using DanmuGame.events;

/// <summary>
/// TO DO:
///     1. ��refreshBot��Ϊ�¼���������
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
     * ά��������ҵı���
     */
    // Player��Ӧ��intΪ��ҵ�posID
    public Dictionary<Player, int> playerPosMap;
    public List<Player> playerList;
    public bool[] posOccupied;
    // �洢������ֵ�set���ϣ����ڲ��أ���ֹһ����Ҽ�������
    public HashSet<string> playerNameSet;

    public Player[] bots;
    [SerializeField]
    private int lineCount;
    private const float updateTimeInterval = 5f;
    private const float playerDBUpdateInternal = 60f;

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
        StartCoroutine(UpdateAllPlayerToDB(playerDBUpdateInternal));
        RefreshBots();
    }

    void Update()
    {
        // ��������
        if (Input.GetKeyDown(KeyCode.A))
        {
            IntString intString = new IntString();
            intString.IntValue = 0;
            intString.StringValue = "BOT" + Random.Range(10, 100);
            AddNewPlayer(intString);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            RageModeForAll(20.0f);
        }
    }

    // ��������
    // ��ӳɹ���������Ӻ�����Ҹ���
    // ���ʧ�ܣ�����-1
    public void AddNewPlayer(IntString intString)
    {
        if (playerList.Count == maxPlayerNum) return;

        // Ѱ��û��ռλ��λ��
        int posId = 0;
        for (int i = 0; i < posOccupied.Length; i++)
            if (posOccupied[i] == false)
            {
                posId = i;
                break;
            }

        AddNewPlayer(intString.IntValue, intString.StringValue, posId);
    }

    // ����ֵ��-1��������-2λ�ñ�ռ��-3����ظ����룬���򷵻ص�ǰ�������
    public int AddNewPlayer(int uid, string name, int posId)
    {
        if (playerList.Count == maxPlayerNum) return -1;
        if (posOccupied[posId]) return -2;

        // ���name�Ƿ��Ѿ�����
        if (playerNameSet.Contains(name)) return -3;

        // Create Prefab
        GameObject playerObject = Instantiate(defaultPlayerPrefab, spawnPos[posId].transform);
        Player player = playerObject.GetComponent<Player>();
        playerList.Add(player);
        playerPosMap.Add(player, posId);
        posOccupied[posId] = true;
        playerNameSet.Add(name);
        player.uid = uid;

        OnPlayerOccupyPosEvent.Raise(posId);

        // �������ݿ⣬�������Ѿ��������ȡ֮ǰ���������
        int level, exp;
        List<string> equip;
        bool exist = DatabaseManager.LoadPlayerInDB(uid,ref name, out level, out exp, out equip);

        if (exist)
        {
            player.LoadPlayer(level, exp, equip);
        } else
        {
            player.Randomize();
        }

        player.changeName(name);
        player.transform.localPosition = new Vector3(0, 0, 0);

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

        // ����������������ݿ�
        int level, exp;
        List<string> equip;
        player.SavePlayer(out level, out exp, out equip);
        DatabaseManager.UpdatePlayerInDB(player.uid, player.playerName, level, exp, equip);

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


    //�����������id
    private int GetIdByName(string name)
    {
        for (int i = 0; i < playerList.Count; i++)
        {
            if (playerList[i].playerName == name)
                return i;
        }
        return -1;
    }

    // ����bot�Ĵ������������ұ䶯ʱ����
    // ĳһ�������ʱ���ر�bot
    // ĳһ��û���ʱ������bot
    private void RefreshBots()
    {
        for (int i = 0; i < lineCount; i++)
            if (posOccupied[i] || posOccupied[i + lineCount])
                bots[i].SetActive(false);
            else
                bots[i].SetActive(true);
    }

    // ȫԱ����rageģʽ
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
        playerList[id].addExp(1000);
    }

    // ÿ��һ��ʱ�����һ���������CP����
    IEnumerator GetAllCPCoroutinue()
    {
        while (true)
        {
            yield return new WaitForSeconds(updateTimeInterval);
            playerSumCP.value = GetAllCP();
        }
    }

    // �����ִ�������ҵ�CP
    private int GetAllCP()
    {
        int sum = 0;
        foreach(Player player in playerList)
        {
            sum += player.GetCP();
        }
        return sum;
    }

    // ÿ��һ��ʱ�佫���������Ϣ�ϴ������ݿ�
    IEnumerator UpdateAllPlayerToDB(float time)
    {
        while (true)
        {
            yield return new WaitForSeconds(time);
            foreach(var player in playerList)
            {
                int level, exp;
                List<string> equip;
                player.SavePlayer(out level, out exp, out equip);
                DatabaseManager.UpdatePlayerInDB(player.uid, player.playerName, level, exp, equip);
            }
        }
    }
}
