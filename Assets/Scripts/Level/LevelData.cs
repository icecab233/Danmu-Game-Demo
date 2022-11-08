using UnityEngine;

[CreateAssetMenu(fileName = "New LevelData", menuName = "ScriptableObjects/LevelData")]
public class LevelData : ScriptableObject
{
    public string levelName;
    public Sprite levelBackground;
    // 难度，1为简单，2为普通，3为困难，4为地狱，5为不可能
    // 后续考虑用枚举代替
    public int difficulty;

    /// <summary>
    /// 下列与每波有关的数组，长度应与此值相同
    /// </summary>
    // 波数
    public int waveCount;
    public WaveData[] waves;
    // 每一波持续时间
    public float[] waveTime;
    // 每一波开始前的准备时间
    public float[] wavePrepareTime;
}
