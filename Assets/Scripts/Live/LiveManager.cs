using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiveManager : MonoBehaviour
{
    public PlayerManager playerManager;

    public void OnDanmu(string userName, string danmu)
    {
        switch (danmu)
        {
            case "����":
                int log =playerManager.addNewPlayer(userName);
                if (log < 0)
                {
                    Debug.Log("����ʧ�ܣ�����" + log);
                }
                break;
            case "���":
                int id = playerManager.getIdByName(userName);
                if (id == -1)
                {
                    Debug.Log("��Ļ-������������û�");
                }
                else
                {
                    playerManager.randomPlayer(id);
                }
                break;
            case "��":
                playerManager.rageModeForAll(15f);
                break;
        }
    }

    public void OnGift(string userName, string giftName, int giftNum)
    {
        int id = playerManager.getIdByName(userName);
        if (id == -1)
        {
            Debug.Log("��������������û�");
            return;
        }
        Debug.Log("GIFTNAME: " + giftName);

        switch (giftName)
        {
            case "����":
                if (giftNum == 1)
                {
                    playerManager.playerList[id].addExp(100);
                } else if (giftNum == 2)
                {
                    playerManager.rageModeForAll(20f);
                }
                break;
        }
    }
}
