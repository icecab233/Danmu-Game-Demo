public static class PlayerData
{
    // ��ҵȼ�����
    public static int maxLevel = 5;
    // �ﵽÿ���ȼ���Ҫ��exp ���ǲ����������ۼ�exp��
    // Array[x]��ʾ��x�ȼ��������Ǵ�0��ʼ��
    public static int[] expOfLevel = { 0, 100, 300, 600, 1000, 2000 };

    // ÿ���ȼ��Ĺ�����
    public static int[] attackOfLevel = { 5, 6, 7, 8, 9, 10 };
    // ÿ���ȼ��Ĺ����ٶȣ���ʾ��ֵ��
    public static int[] attackSpeedOfLevel = { 10, 11, 12, 13, 14, 15 };
    // ÿ���ȼ��Ĺ������ʱ�䣨���ٵ�ʵ��Ч����
    public static float[] attackTimeOfLevel = { 3.0f, 2.75f, 2.5f, 2.25f, 2f, 1.5f };
}
