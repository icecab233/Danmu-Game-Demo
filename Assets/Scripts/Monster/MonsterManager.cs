using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI.Extensions;

public class MonsterManager : MonoBehaviour
{
    public GameObject[] spawnPos;
    public GameObject[] monstersPrefab;
    public float spwanInternalBase = 3.0f;
    public float spawnInternalRandom = 1.5f;

    private void Start()
    {
        StartCoroutine(SpawnMonsterCoroutine());
    }

    private void Update()
    {
        
    }

    IEnumerator SpawnMonsterCoroutine()
    {
        while (true)
        {
            int spawnPosId = Random.Range(0, spawnPos.Length);
            int spawnMonsterId = Random.Range(0, monstersPrefab.Length);
            GameObject newMonster = Instantiate(monstersPrefab[spawnMonsterId], spawnPos[spawnPosId].transform);
            newMonster.transform.localScale = new Vector3(0.4f, 0.4f, 1);

            yield return new WaitForSeconds(Random.Range(spwanInternalBase - spawnInternalRandom, spwanInternalBase + spawnInternalRandom));
        }
    }
}
