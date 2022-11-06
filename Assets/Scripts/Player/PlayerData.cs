public static class PlayerData
{
    // 玩家等级上限
    public static int maxLevel = 5;
    // 达到每个等级需要的exp （非差量，而是累计exp）
    // Array[x]表示到x等级，并不是从0开始的
    public static int[] expOfLevel = { 0, 100, 300, 600, 1000, 2000 };

    // 每个等级的攻击力
    public static int[] attackOfLevel = { 5, 6, 7, 8, 9, 10 };
    // 每个等级的攻击速度（显示数值）
    public static int[] attackSpeedOfLevel = { 10, 11, 12, 13, 14, 15 };
    // 每个等级的攻击间隔时间（攻速的实际效果）
    public static float[] attackTimeOfLevel = { 3.0f, 2.75f, 2.5f, 2.25f, 2f, 1.5f };
}
