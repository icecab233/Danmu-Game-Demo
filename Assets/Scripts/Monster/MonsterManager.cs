using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : MonoBehaviour
{
    public GameObject[] spawnPos;
    public GameObject monsterPrefab;
    public float spwanInternalBase = 3.0f;
    public float spawnInternalRandom = 1.5f;

    private void Update()
    {
        
    }

    IEnumerator SpawnMonsterCoroutine()
    {
        while (true)
        {
            int spawnPosId = Random.Range(0, spawnPos.Length);
            GameObject newMonster = Instantiate(monsterPrefab, spawnPos[spawnPosId].transform);
            newMonster.transform.localScale = new Vector3(0.7f, 0.7f, 1);
        }
    }
}
