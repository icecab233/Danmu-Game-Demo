using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DatabaseOperation : MonoBehaviour
{
    //IP地址
    public string host;
    //端口号
    public string port;
    //用户名
    public string userName;
    //密码
    public string password;
    //数据库名称
    public string databaseName;
    //封装好的数据库类
    MySqlAccess mysql;
    void Start()
    {
        mysql = new MySqlAccess(host, port, userName, password, databaseName);
        addNewPlayerToDB("20301006","大谷子",999);

    }
    void addNewPlayerToDB(string gamerUid,string gamerName,int gamerScore)
    {
        mysql.QuerySet("INSERT INTO `" + databaseName + "`.`GamerInformation`(`id`,`name`,`score`) VALUES ('"+gamerUid+"','" + gamerName + "','"+gamerScore+"')");
        mysql.CloseSql();
    }
}
