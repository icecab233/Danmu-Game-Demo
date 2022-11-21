using UnityEngine;
using DanmuGame.events;
using System.Data;

public class LiveManager : MonoBehaviour
{
    public PlayerManager playerManager;
    public IntStringEvent AddNewPlayerEvent;
    public StringEvent RandomizePlayerEvent;
    public VoidEvent RageModeForAllEvent;
    public StringEvent AddExpToPlayerFromGift1Event;
    //引用数据库
    MySqlAccess mysql;
    void Start()
    {
        mysql = new MySqlAccess("81.68.234.49", "3306", "dagu", "Tony6666", "nia");
        mysql.CloseSql();
    }
    public void OnDanmu(string userName, long uid,string danmu)
    {
        switch (danmu)
        {
            case "加入":
                IntString intString = new IntString();
                intString.IntValue = (int)uid;
                intString.StringValue = userName;
                AddNewPlayerEvent.Raise(intString);
                break;
            case "随机":
                RandomizePlayerEvent.Raise(userName);
                break;
            case "狂暴":
                RageModeForAllEvent.Raise();
                break;
        }
    }

    public void OnGift(string userName, string giftName, int giftNum)
    {

        switch (giftName)
        {
            case "辣条":
                if (giftNum == 1)
                {
                    AddExpToPlayerFromGift1Event.Raise(userName);
                }
                else if (giftNum == 2)
                {
                    RageModeForAllEvent.Raise();
                }
                break;
        }
    }
}
