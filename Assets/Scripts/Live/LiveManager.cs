using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiveManager : MonoBehaviour
{
    public PlayerManager playerManager;

    public void OnDanmu(string userName, string danmu)
    {
        if (danmu == "加入")
        {
            playerManager.addNewPlayer(userName);
        } else if (danmu == "随机")
        {
            int id = playerManager.getIdByName(userName);
            if (id == -1)
            {
                Debug.Log("弹幕-随机：不存在用户");
            } else
            {
                playerManager.randomPlayer(id);
            }
        }
    }
}
