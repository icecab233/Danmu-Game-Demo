public static class PlayerData
{
    // ------ �������� ------

    // ��ҵȼ�����
    public static int maxLevel = 26;
    // �ﵽÿ���ȼ���Ҫ��exp ���ǲ����������ۼ�exp��
    // Array[x]��ʾ��x�ȼ��������Ǵ�0��ʼ��
    public static int[] expOfLevel = { 0, 30, 100, 200, 300, 1000, 1500, 2000, 2500, 3000,
    3500, 4000, 4500, 5000, 5500, 6000, 7000, 10000, 20000, 30000, 40000, 50000, 60000, 80000, 100000,
    120000, 150000};

    // ÿ���ȼ��Ĺ�����
    public static int[] attackOfLevel = { 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18,
    19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31};
    // ÿ���ȼ��Ĺ����ٶȣ���ʾ��ֵ��
    public static int[] attackSpeedOfLevel = { 10, 11, 12, 13, 14, 15, 15, 15, 15, 15, 15,
    15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15, 15};
    // ÿ���ȼ��Ĺ������ʱ�䣨���ٵ�ʵ��Ч����
    public static float[] attackTimeOfLevel = { 3.0f, 2.75f, 2.5f, 2.25f, 2f, 1.5f, 1.5f,
    1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f,
    1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f};

    // ÿ���ȼ������HP
    public static int[] hpMaxOfLevel = { 100, 120, 140, 160, 180, 200, 220, 240, 260, 280,
    300, 320, 340, 360, 380, 400, 420, 440, 460, 480, 500, 520, 540, 560, 580, 600, 620};

    // ------ ְҵ��� ------

    // �����ֿ�ʼ�ĵȼ���-1����Ĭ��ְҵ
    public static int archerMinLevel = -1;

    // ÿ���ȼ�װ���Ĺ�������
    public static string[] bowNameOfLevel = { "HunterShortBow", "VikingIronBow3", "OrcBow", "BattleBow",
    "SandShooterBow2", "GoldDragon"};

    // ������ʱ�䣬����ΪAttackCoroutine��StartCharge��Endcharge֮��ļ��
    // ʵ��ÿ��������ʱ�䣬����ȼ�Ϊx��attackTimeOfLevel[x] + bowChargeTime
    public static float bowChargeTime = 1.0f;

    // ǹ�ֿ�ʼ�ĵȼ�
    public static int gunnerMinLevel = 6;
    // ǹ��һ���ж��ټ�
    public static int gunnerLevelLength = 11;
    // ǹ�ֿ�ʼʹ��˫�������ĵȼ����տ�ʼĬ��Ϊ���֣�
    public static int gunner2HLevel = 12;
    public static string[] gunNameOfLevel = {"FlintPistolTypeA", "FlintPistolTypeB", "FlintPistolTypeC",
    "RevolverTypeA", "RevolverTypeB", "RevolverTypeC", "OldShotgun", "OldShotgun", "RifleTypeF",
    "RifleTypeD", "RifleTypeA"};

    // ��ʦ��ʼ�ĵȼ�
    public static int wizardMinLevel = 17;
    // ��ʦһ���ж��ټ�
    public static int wizardLevelLength = 10;
    public static string[] mageNameOfLevel = {"HardwoodWand", "DruidWand", "DrownedGoblinHand",
    "WaterAdeptWand1", "SwampMage2Staff", "IceWizardWand", "FireAdeptWand1", "PyromancerWand",
    "CardinalWand", "BishopWand"};
}
