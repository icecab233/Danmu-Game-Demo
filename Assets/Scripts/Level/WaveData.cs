using UnityEngine;

[CreateAssetMenu(fileName = "New WaveData", menuName = "ScriptableObjects/WaveData")]
public class WaveData : ScriptableObject
{
    public GameObject[] monsters;
    // [x]为第x种怪物生成概率为x%，各项和应为100
    public int[] spawnProbability;

    public float spawnIntervalBase;
    public float spawnIntervalRandom;
}
