using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using NativeWebSocket;
using Newtonsoft.Json;
using OpenBLive.Runtime;
using OpenBLive.Runtime.Data;
using OpenBLive.Runtime.Utilities;
using UnityEngine;

public class ConnectLive : MonoBehaviour
{
    public static ConnectLive Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<ConnectLive>();
            }
            return instance;
        }
    }
    private static ConnectLive instance;
    // Start is called before the first frame update
    private WebSocketBLiveClient m_WebSocketBLiveClient;
    private InteractivePlayHeartBeat m_PlayHeartBeat;
    private string gameId;
    private const string accessKeySecret = "o5kBVXQYiYLbjQIvqX7rEJ4wmCdKHm";
    private const string accessKeyId = "eLlAG7R9VRVmBeGPA5UmlUeH";
    public string appId;

    public Action ConnectSuccess;
    public Action ConnectFailure;

    public LiveManager liveManager;

    public async void LinkStart(string code)
    {
        //���Ե���Կ
        SignUtility.accessKeySecret = accessKeySecret;
        //���Ե�ID
        SignUtility.accessKeyId = accessKeyId;
        var ret = await BApi.StartInteractivePlay(code, appId);
        //��ӡ������̨��־
        var gameIdResObj = JsonConvert.DeserializeObject<AppStartInfo>(ret);
        if (gameIdResObj.Code != 0)
        {
            Debug.LogError(gameIdResObj.Message);
            ConnectFailure?.Invoke();
            return;
        }

        m_WebSocketBLiveClient = new WebSocketBLiveClient(gameIdResObj.GetWssLink(), gameIdResObj.GetAuthBody());
        m_WebSocketBLiveClient.OnDanmaku += WebSocketBLiveClientOnDanmaku;
        m_WebSocketBLiveClient.OnGift += WebSocketBLiveClientOnGift;
        m_WebSocketBLiveClient.OnGuardBuy += WebSocketBLiveClientOnGuardBuy;
        m_WebSocketBLiveClient.OnSuperChat += WebSocketBLiveClientOnSuperChat;

        try
        {
            m_WebSocketBLiveClient.Connect(TimeSpan.FromSeconds(1), 1000000);
            ConnectSuccess?.Invoke();
            Debug.Log("���ӳɹ�");
        }
        catch (Exception ex)
        {
            ConnectFailure?.Invoke();
            Debug.Log("����ʧ�� " + ex.Message);
            throw;
        }

        gameId = gameIdResObj.GetGameId();
        m_PlayHeartBeat = new InteractivePlayHeartBeat(gameId);
        m_PlayHeartBeat.HeartBeatError += M_PlayHeartBeat_HeartBeatError;
        m_PlayHeartBeat.HeartBeatSucceed += M_PlayHeartBeat_HeartBeatSucceed;
        m_PlayHeartBeat.Start();


    }


    public async Task LinkEnd()
    {
        m_WebSocketBLiveClient.Dispose();
        m_PlayHeartBeat.Dispose();
        await BApi.EndInteractivePlay(appId, gameId);
        Debug.Log("��Ϸ�ر�");
    }

    private void WebSocketBLiveClientOnSuperChat(SuperChat superChat)
    {
        StringBuilder sb = new StringBuilder("�յ�SC!");
        sb.AppendLine();
        sb.Append("�����û���");
        sb.AppendLine(superChat.userName);
        sb.Append("�������ݣ�");
        sb.AppendLine(superChat.message);
        sb.Append("��");
        sb.Append(superChat.rmb);
        sb.Append("Ԫ");
        Debug.Log(sb);
    }

    private void WebSocketBLiveClientOnGuardBuy(Guard guard)
    {
        StringBuilder sb = new StringBuilder("�յ��󺽺�!");
        sb.AppendLine();
        sb.Append("�����û���");
        sb.AppendLine(guard.userInfo.userName);
        sb.Append("������");
        sb.Append(guard.guardUnit);
        Debug.Log(sb);
    }

    private void WebSocketBLiveClientOnGift(SendGift sendGift)
    {
        StringBuilder sb = new StringBuilder("�յ�����!");
        sb.AppendLine();
        sb.Append("�����û���");
        sb.AppendLine(sendGift.userName);
        sb.Append("������");
        sb.Append(sendGift.giftNum);
        sb.Append("��");
        sb.Append(sendGift.giftName);
        Debug.Log(sb);

        liveManager.OnGift(sendGift.userName, sendGift.giftName, (int)sendGift.giftNum);
    }

    private void WebSocketBLiveClientOnDanmaku(Dm dm)
    {
        StringBuilder sb = new StringBuilder("�յ���Ļ!");
        sb.AppendLine();
        sb.Append("�û���");
        sb.AppendLine(dm.userName);
        sb.Append("��Ļ���ݣ�");
        sb.Append(dm.msg);
        Debug.Log(sb);

        liveManager.OnDanmu(dm.userName, dm.msg);
    }


    private static void M_PlayHeartBeat_HeartBeatSucceed()
    {
        Debug.Log("�����ɹ�");
    }

    private static void M_PlayHeartBeat_HeartBeatError(string json)
    {
        Debug.Log("����ʧ��" + json);
    }


    private void Update()
    {
#if !UNITY_WEBGL || UNITY_EDITOR
        if (m_WebSocketBLiveClient is { ws: { State: WebSocketState.Open } })
        {
            m_WebSocketBLiveClient.ws.DispatchMessageQueue();
        }
#endif
    }

    private void OnDestroy()
    {
        if (m_WebSocketBLiveClient == null)
            return;

        m_PlayHeartBeat.Dispose();
        m_WebSocketBLiveClient.Dispose();
    }
}
