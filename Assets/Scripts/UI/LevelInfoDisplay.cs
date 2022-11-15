using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LevelInfoDisplay : MonoBehaviour
{
    public TextMeshProUGUI waveNumberText;
    public TextMeshProUGUI statusText;
    public TextMeshProUGUI leftTimeText;
    public TextMeshProUGUI leftProgressBarNum;
    public TextMeshProUGUI rightProgressBarNum;
    public GameObject WaveBroadcasterNormal;
    public Transform mainCanvas;
    public WaveManager waveManager;
    public GameObject progressBar;
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
                //������ͬ������
                progressBar.GetComponent<Slider>().value=1-waveManager.leftTime/waveManager.levelData.waveTime[waveManager.waveNow];
                break;
            case WaveManager.WaveStatus.Stop:
                statusText.text = "";
                leftTimeText.text = "";
                break;
        }
    }
    // ��ʾ��һ������
    public void ShowWaveBroadcaster(int waveNow)
    {
        GameObject gameObject=Instantiate(WaveBroadcasterNormal,transform);
        gameObject.GetComponentInChildren<TextMeshProUGUI>().text="Wave: " + ++waveNow;
        Destroy(gameObject, 5f);
    }
    //�ı�������ϵ�����
    public void ChangeNumberOnProgressBar(int waveNow)
    {
       leftProgressBarNum.text=(++waveNow).ToString();
       rightProgressBarNum.text=(++waveNow).ToString();

    }
}
