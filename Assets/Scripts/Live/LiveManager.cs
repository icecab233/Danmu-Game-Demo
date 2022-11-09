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
                playerManager.addNewPlayer(userName);
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

        switch (giftName)
        {
            case "����":
                if (giftNum == 1)
                {
                    playerManager.players[id].GetComponent<Player>().addExp(100);
                } else if (giftNum == 2)
                {

                }
                break;
        }
    }
}
