using UnityEngine;
using Assets.HeroEditor.Common.CharacterScripts;
using Assets.HeroEditor.Common.CommonScripts;
using HeroEditor.Common.Enums;
using System.Collections.Generic;

public class PlayerManager : MonoBehaviour
{
    public GameObject defaultPlayerPrefab;

    public static List<GameObject> players;
    public GameObject[] spawnPos;

    public static bool[] posOccupied;
    public static Dictionary<GameObject, int> posOfPlayer;

    [SerializeField]
    private int maxPlayerNum;
    public static int playerCount = 0;

    void Start()
    {
        posOccupied = new bool[maxPlayerNum];
        posOfPlayer = new Dictionary<GameObject, int>();

        players = new List<GameObject>();
    }

    void Update()
    {
        // FOR TEST
        if (Input.GetKeyDown(KeyCode.A))
        {
            addNewPlayer("test");
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
        character.Equip(character.SpriteCollection.Helmet.Random(), EquipmentPart.Boots);


        playerCount++;
        return playerCount;
    }

    public static void playerDie(GameObject playerObject)
    {
        int oldPosId = posOfPlayer[playerObject];
        posOccupied[oldPosId] = false;
        playerCount--;
    }

    public void randomPlayer(int id)
    {
        if (id >= playerCount)
        {
            Debug.Log("Random Player: ID out of index");
            return;
        }

        players[id].GetComponent<Character>().Randomize();
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
}
