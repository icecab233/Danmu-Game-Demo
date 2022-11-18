using UnityEngine;
using DanmuGame.events;
using System.Data;

public class LiveManager : MonoBehaviour
{
    public PlayerManager playerManager;
    public StringEvent AddNewPlayerEvent;
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
                AddNewPlayerEvent.Raise(userName);
                //将新玩家信息存入数据库，默认积分为0
                addNewPlayerToDB(userName,uid,0);
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
                } else if (giftNum == 2)
                {
                    RageModeForAllEvent.Raise();
                }
                break;
        }
    }
     void addNewPlayerToDB(string gamerName,long gamerUid,int gamerScore)
    {
        mysql.OpenSql();
        DataSet ds=mysql.QuerySet("select * from GamerInformation where name=\""+gamerName+"\";");
        DataTable table=ds.Tables[0];
        if(table.Rows.Count==0)
        {
            Debug.Log("欢迎新玩家");
            mysql.QuerySet("INSERT INTO `nia`.`GamerInformation`(`name`,`uid`,`score`) VALUES ('"+gamerName+"','" + gamerUid + "','"+gamerScore+"')");
            
        }
        else
        {
            Debug.Log("欢迎老玩家");
        }
        mysql.CloseSql();
       
    }
}
