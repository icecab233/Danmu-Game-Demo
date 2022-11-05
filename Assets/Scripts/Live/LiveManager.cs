using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiveManager : MonoBehaviour
{
    public PlayerManager playerManager;

    public void OnDanmu(string userName, string danmu)
    {
        if (danmu == "����")
        {
            playerManager.addNewPlayer(userName);
        } else if (danmu == "���")
        {
            int id = playerManager.getIdByName(userName);
            if (id == -1)
            {
                Debug.Log("��Ļ-������������û�");
            } else
            {
                playerManager.randomPlayer(id);
            }
        }
    }
}
