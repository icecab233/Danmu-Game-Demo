public static class PlayerData
{
    // ��ҵȼ�����
    public static int maxLevel = 5;
    // �ﵽÿ���ȼ���Ҫ��exp ���ǲ����������ۼ�exp��
    // Array[x]��ʾ��x�ȼ��������Ǵ�0��ʼ��
    public static int[] expOfLevel = { 0, 30, 100, 200, 300, 1000 };

    // ÿ���ȼ��Ĺ�����
    public static int[] attackOfLevel = { 5, 6, 7, 8, 9, 10 };
    // ÿ���ȼ��Ĺ����ٶȣ���ʾ��ֵ��
    public static int[] attackSpeedOfLevel = { 10, 11, 12, 13, 14, 15 };
    // ÿ���ȼ��Ĺ������ʱ�䣨���ٵ�ʵ��Ч����
    public static float[] attackTimeOfLevel = { 3.0f, 2.75f, 2.5f, 2.25f, 2f, 1.5f };

    // ÿ���ȼ������HP
    public static int[] hpMaxOfLevel = { 100, 120, 140, 160, 180, 200 };

    // ÿ���ȼ�װ���Ĺ��ı��
    public static int[] bowIdOfLevel = { 7, 25, 0, 4, 3, 1};

    // ������ʱ�䣬����ΪAttackCoroutine��StartCharge��Endcharge֮��ļ��
    // ʵ��ÿ��������ʱ�䣬����ȼ�Ϊx��attackTimeOfLevel[x] + bowChargeTime
    public static float bowChargeTime = 1.0f;
}
