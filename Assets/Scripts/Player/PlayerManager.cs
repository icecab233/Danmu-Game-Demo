using UnityEngine;
using Assets.HeroEditor.Common.CharacterScripts;
using Assets.HeroEditor.Common.CommonScripts;
using HeroEditor.Common.Enums;

public class PlayerManager : MonoBehaviour
{
    public GameObject defaultPlayerPrefab;

    public GameObject[] players;
    public GameObject[] spawnPos;

    [SerializeField]
    private int maxPlayerNum;
    [SerializeField]
    private int playerCount = 0;

    void Start()
    {
        players = new GameObject[maxPlayerNum];
    }

    void Update()
    {
        // FOR TEST
        if (Input.GetKeyDown(KeyCode.A))
        {
            addNewPlayer("test");
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            randomPlayer(1);
        }
    }

    public int addNewPlayer(string name)
    {
        // Create Prefab
        players[playerCount] = Instantiate(defaultPlayerPrefab, spawnPos[playerCount].transform);
        players[playerCount].GetComponent<Player>().changeName(name);
        players[playerCount].transform.localPosition = new Vector3(0, 0, 0);
        players[playerCount].transform.localScale = new Vector3(0.5f, 0.5f, 0);

        // Random
        Character character = players[playerCount].GetComponent<Character>();
        character.Equip(character.SpriteCollection.Helmet.Random(), EquipmentPart.Helmet);
        character.Equip(character.SpriteCollection.Helmet.Random(), EquipmentPart.Boots);


        playerCount++;
        return playerCount;
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

    public int getPlayerCount()
    {
        return playerCount;
    }

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
