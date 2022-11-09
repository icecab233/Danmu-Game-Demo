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
            case "加入":
                playerManager.addNewPlayer(userName);
                break;
            case "随机":
                int id = playerManager.getIdByName(userName);
                if (id == -1)
                {
                    Debug.Log("弹幕-随机：不存在用户");
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
            Debug.Log("接受礼物：不存在用户");
            return;
        }

        switch (giftName)
        {
            case "辣条":
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
