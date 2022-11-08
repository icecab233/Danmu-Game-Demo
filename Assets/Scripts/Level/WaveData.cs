using UnityEngine;

[CreateAssetMenu(fileName = "New WaveData", menuName = "ScriptableObjects/WaveData")]
public class WaveData : ScriptableObject
{
    public GameObject[] monsters;
    // [x]Ϊ��x�ֹ������ɸ���Ϊx%�������ӦΪ100
    public int[] spawnProbability;

    public float spawnIntervalBase;
    public float spawnIntervalRandom;
}
