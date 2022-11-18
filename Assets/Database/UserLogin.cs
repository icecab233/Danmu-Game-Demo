using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UserLogin : MonoBehaviour, IPointerClickHandler {
 
    public InputField userNameInput;
 
    public InputField passwordInput;
    //提示用户登录信息
    private Text loginMessage;
 
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
 
 
    private void Start() {
        loginMessage = GameObject.FindGameObjectWithTag("LoginMessage").GetComponent<Text>();
        mysql = new MySqlAccess(host, port, userName, password, databaseName);
    }
 
    public void OnPointerClick(PointerEventData eventData) {
        if (eventData.pointerPress.name == "loginButton") {     //如果当前按下的按钮是注册按钮 
            OnClickedLoginButton();
        }
    }
 
    /// <summary>
    /// 按下登录按钮
    /// </summary>
    private void OnClickedLoginButton() {
        mysql.OpenSql();
        string loginMsg = "";
        DataSet ds = mysql.Select("usertable", new string[] { "level" }, new string[] { "`" + "account" + "`", "`" + "password" + "`" }, 
        new string[] { "=", "=" }, new string[] { userNameInput.text, passwordInput.text });
        if (ds != null) {
            DataTable table = ds.Tables[0];
            if (table.Rows.Count > 0) {
                loginMsg = "登陆成功！";
                loginMessage.color = Color.green;
                Debug.Log("用户权限等级：" + table.Rows[0][0]);
            } else {
                loginMsg = "用户名或密码错误！";
                loginMessage.color = Color.red;
            }
            loginMessage.text = loginMsg;
        }
        mysql.CloseSql();
    }
 
}

