using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelInfoDisplay : MonoBehaviour
{
    public TextMeshProUGUI waveNumberText;
    public TextMeshProUGUI statusText;
    public TextMeshProUGUI leftTimeText;

    public WaveManager waveManager;

    // To Do: ��ÿ֡ˢ�¸�Ϊ�¼�����ʽˢ��
    void Update()
    {
        waveNumberText.text = "Wave: " + (waveManager.waveNow+1) + "/" + (waveManager.levelData.waveCount);
        switch (waveManager.currentWaveStatus)
        {
            case WaveManager.WaveStatus.Preparing:
                statusText.text = "Prepare Time";
                leftTimeText.text = waveManager.leftTime.ToString("0.0");
                break;
            case WaveManager.WaveStatus.Battle:
            case WaveManager.WaveStatus.Waiting:
                statusText.text = "Battle Time";
                leftTimeText.text = waveManager.leftTime.ToString("0.0");
                break;
            case WaveManager.WaveStatus.Stop:
                statusText.text = "";
                leftTimeText.text = "";
                break;
        }
    }
}
