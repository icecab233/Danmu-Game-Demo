public static class PlayerData
{
    // ------ 基础属性 ------

    // 玩家等级上限
    public static int maxLevel = 26;
    // 达到每个等级需要的exp （非差量，而是累计exp）
    // Array[x]表示到x等级，并不是从0开始的
    public static int[] expOfLevel = { 0, 30, 100, 200, 300, 1000, 1500, 2000, 2500, 3000,
    3500, 4000, 4500, 5000, 5500, 6000, 7000, 10000, 20000, 30000, 40000, 50000, 60000, 80000, 100000,
    120000, 150000};

    // 每个等级的攻击力
    public static int[] attackOfLevel = { 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18,
    19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31};
    // 每个等级的攻击速度（显示数值）
    public static int[] attackSpeedOfLevel = { 10, 11, 12, 13, 14, 15, 15, 15, 15, 15, 15,
    15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15};
    // 每个等级的攻击间隔时间（攻速的实际效果）
    public static float[] attackTimeOfLevel = { 3.0f, 2.75f, 2.5f, 2.25f, 2f, 1.5f, 1.5f,
    1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f,
    1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f};

    // 每个等级的最大HP
    public static int[] hpMaxOfLevel = { 100, 120, 140, 160, 180, 200, 220, 240, 260, 280,
    300, 320, 340, 360, 380, 400, 420, 440, 460, 480, 500, 520, 540, 560, 580, 600, 620};

    // ------ 职业相关 ------

    // 弓箭手开始的等级，-1代表默认职业
    public static int archerMinLevel = -1;

    // 每个等级装备的弓的名称
    public static string[] bowNameOfLevel = { "HunterShortBow", "VikingIronBow3", "OrcBow", "BattleBow",
    "SandShooterBow2", "GoldDragon"};

    // 弓拉弓时间，具体为AttackCoroutine中StartCharge和Endcharge之间的间隔
    // 实际每次射箭间隔时间，假设等级为x，attackTimeOfLevel[x] + bowChargeTime
    public static float bowChargeTime = 1.0f;

    // 枪手开始的等级
    public static int gunnerMinLevel = 6;
    // 枪手一共有多少级
    public static int gunnerLevelLength = 11;
    // 枪手开始使用双手武器的等级（刚开始默认为单手）
    public static int gunner2HLevel = 12;
    public static string[] gunNameOfLevel = {"FlintPistolTypeA", "FlintPistolTypeB", "FlintPistolTypeC",
    "RevolverTypeA", "RevolverTypeB", "RevolverTypeC", "OldShotgun", "OldShotgun", "RifleTypeF",
    "RifleTypeD", "RifleTypeA"};

    // 法师开始的等级
    public static int wizardMinLevel = 17;
    // 法师一共有多少级
    public static int wizardLevelLength = 10;
    public static string[] mageNameOfLevel = {"HardwoodWand", "DruidWand", "DrownedGoblinHand",
    "WaterAdeptWand1", "SwampMage2Staff", "IceWizardWand", "FireAdeptWand1", "PyromancerWand",
    "CardinalWand", "BishopWand"};
}
