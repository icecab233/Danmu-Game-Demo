using UnityEngine;

[CreateAssetMenu(fileName = "New LevelData", menuName = "ScriptableObjects/LevelData")]
public class LevelData : ScriptableObject
{
    public string levelName;
    public Sprite levelBackground;
    // �Ѷȣ�1Ϊ�򵥣�2Ϊ��ͨ��3Ϊ���ѣ�4Ϊ������5Ϊ������
    // ����������ö�ٴ���
    public int difficulty;

    /// <summary>
    /// ������ÿ���йص����飬����Ӧ���ֵ��ͬ
    /// </summary>
    // ����
    public int waveCount;
    public WaveData[] waves;
    // ÿһ������ʱ��
    public float[] waveTime;
    // ÿһ����ʼǰ��׼��ʱ��
    public float[] wavePrepareTime;
}
