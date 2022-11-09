using UnityEngine;
using Assets.HeroEditor.Common.CharacterScripts;
using Assets.HeroEditor.Common.CommonScripts;
using HeroEditor.Common.Enums;
using System.Collections.Generic;

// Singleton Class
public class PlayerManager : MonoBehaviour
{
    // Singleton
    private static PlayerManager _instance;
    public static PlayerManager Instance { get { return _instance; } }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }
    }


    public GameObject defaultPlayerPrefab;

    public List<GameObject> players;
    public GameObject[] spawnPos;

    public bool[] posOccupied;
    public Dictionary<GameObject, int> posOfPlayer;

    public GameObject[] bots;
    private const int lineCount = 5;

    [SerializeField]
    private int maxPlayerNum;
    public int playerCount = 0;

    void Start()
    {
        posOccupied = new bool[maxPlayerNum];
        posOfPlayer = new Dictionary<GameObject, int>();

        players = new List<GameObject>();
        refreshBots();
    }

    void Update()
    {
        // FOR TEST
        if (Input.GetKeyDown(KeyCode.A))
        {
            addNewPlayer("NIA-AIN");
        }
    }

    public int addNewPlayer(string name)
    {
        if (playerCount == posOccupied.Length) return -1;

        // 寻找没被占位的位置
        int posId = 0;
        for (int i = 0; i < posOccupied.Length; i++)
            if (posOccupied[i] == false)
            {
                posId = i;
                break;
            }

        // Create Prefab
        GameObject player = Instantiate(defaultPlayerPrefab, spawnPos[posId].transform);
        players.Add(player);
        posOccupied[posId] = true;
        posOfPlayer[player] = posId;
        player.GetComponent<Player>().changeName(name);
        player.transform.localPosition = new Vector3(0, 0, 0);
        player.transform.localScale = new Vector3(0.5f, 0.5f, 0);

        // Random
        Character character = player.GetComponent<Character>();
        character.Equip(character.SpriteCollection.Helmet.Random(), EquipmentPart.Helmet);
        character.Equip(character.SpriteCollection.Armor.Random(), EquipmentPart.Armor);
        character.SetBody(character.SpriteCollection.Hair.Random(), BodyPart.Hair, CharacterExtensions.RandomColor);
        character.SetBody(character.SpriteCollection.Eyebrows.Random(), BodyPart.Eyebrows);
        character.SetBody(character.SpriteCollection.Eyes.Random(), BodyPart.Eyes, CharacterExtensions.RandomColor);
        character.SetBody(character.SpriteCollection.Mouth.Random(), BodyPart.Mouth);

        playerCount++;
        refreshBots();
        return playerCount;
    }

    public void playerDie(GameObject playerObject)
    {
        int oldPosId = posOfPlayer[playerObject];
        posOccupied[oldPosId] = false;
        playerCount--;

        refreshBots();
    }

    public void randomPlayer(int id)
    {
        if (id >= playerCount)
        {
            Debug.Log("Random Player: ID out of index");
            return;
        }

        Character character = players[id].GetComponent<Character>();
        character.Equip(character.SpriteCollection.Helmet.Random(), EquipmentPart.Helmet);
        character.Equip(character.SpriteCollection.Armor.Random(), EquipmentPart.Armor);
        character.SetBody(character.SpriteCollection.Hair.Random(), BodyPart.Hair, CharacterExtensions.RandomColor);
        character.SetBody(character.SpriteCollection.Eyebrows.Random(), BodyPart.Eyebrows);
        character.SetBody(character.SpriteCollection.Eyes.Random(), BodyPart.Eyes, CharacterExtensions.RandomColor);
        character.SetBody(character.SpriteCollection.Mouth.Random(), BodyPart.Mouth);
    }


    // 供外部调用，输入名字输出id
    public int getIdByName(string name)
    {
        for (int i = 0; i<playerCount; i++)
        {
            if (players[i].GetComponent<Player>().getName() == name)
                return i;
        }
        return -1;
    }

    // 更新bot的存在情况。当玩家变动时调用
    // 某一排有玩家时，关闭bot
    // 某一排没玩家时，开启bot
    private void refreshBots()
    {
        for (int i = 0; i < lineCount; i++)
            if (posOccupied[i] || posOccupied[i + 5])
                bots[i].SetActive(false);
            else
                bots[i].SetActive(true);
    }
}
