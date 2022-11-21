using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
public static class DatabaseManager
{
    //引用数据库
    private static MySqlAccess mysql;

    static DatabaseManager()
    {
        mysql = new MySqlAccess("81.68.234.49", "3306", "dagu", "Tony6666", "nia");
        mysql.CloseSql();
    }

    // 添加或更新一个玩家在数据库的信息
    public static void UpdatePlayerInDB(int uid, string playerName, int level, int exp, List<string> equip)
    {
        mysql.OpenSql();

        DataSet ds = mysql.QuerySet("SELECT * FROM Player WHERE playerUID=" + uid + ";");
        DataTable table = ds.Tables[0];

        if (table.Rows.Count == 0)
        {
            // 添加玩家

            string formulatedEquip = "";
            foreach (var str in equip)
            {
                formulatedEquip += ",'";
                formulatedEquip += str;
                formulatedEquip += "'";
            }

            string cmd = "INSERT INTO nia.Player(playerUID,playerName,level,exp," +
                "helmet,armor,hair,eyebrows,eyes,mouth) VALUES (" + uid + ",'" + playerName
                + "'," + level + "," + exp + formulatedEquip + ")";
            Debug.Log(cmd);
            mysql.QuerySet(cmd);

        }
        else
        {
            // 更新玩家

            string cmd = "UPDATE nia.Player SET level="+level+", exp="+exp+", helmet='"+equip[0]+
                "', armor='"+equip[1]+"', hair='"+equip[2]+"', eyebrows='"+equip[3]+
                "', eyes='"+equip[4]+"', mouth='"+equip[5]+ "' WHERE playerUID=" + uid+";";
            Debug.Log(cmd);
            mysql.QuerySet(cmd);
        }

        mysql.CloseSql();
    }

    public static bool LoadPlayerInDB(int uid, ref string playerName, out int level, out int exp, out List<string> equip)
    {
        mysql.OpenSql();

        DataSet ds = mysql.QuerySet("SELECT * FROM Player WHERE playerUID=" + uid + ";");
        DataTable table = ds.Tables[0];
        if (table.Rows.Count == 0)
        {
            level = 0;
            exp = 0;
            equip = null;
            mysql.CloseSql();
            return false;
        } else
        {
            DataRow dataRow = table.Rows[0];
            playerName = (string)dataRow["playerName"];
            level = (int)dataRow["level"];
            exp = (int)dataRow["exp"];
            equip = new List<string>();
            equip.Add((string)dataRow["helmet"]);
            equip.Add((string)dataRow["armor"]);
            equip.Add((string)dataRow["hair"]);
            equip.Add((string)dataRow["eyebrows"]);
            equip.Add((string)dataRow["eyes"]);
            equip.Add((string)dataRow["mouth"]);

            mysql.CloseSql();
            return true;
        }
    }
}