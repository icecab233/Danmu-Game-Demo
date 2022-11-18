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
    //�������ݿ�
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
            case "����":
                AddNewPlayerEvent.Raise(userName);
                //���������Ϣ�������ݿ⣬Ĭ�ϻ���Ϊ0
                addNewPlayerToDB(userName,uid,0);
                break;
            case "���":
                RandomizePlayerEvent.Raise(userName);
                break;
            case "��":
                RageModeForAllEvent.Raise();
                break;
        }
    }

    public void OnGift(string userName, string giftName, int giftNum)
    {

        switch (giftName)
        {
            case "����":
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
            Debug.Log("��ӭ�����");
            mysql.QuerySet("INSERT INTO `nia`.`GamerInformation`(`name`,`uid`,`score`) VALUES ('"+gamerName+"','" + gamerUid + "','"+gamerScore+"')");
            
        }
        else
        {
            Debug.Log("��ӭ�����");
        }
        mysql.CloseSql();
       
    }
}
