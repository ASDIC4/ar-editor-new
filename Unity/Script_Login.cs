// <钟开>
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;


public class UserData
{
    public int code;
    public string msg;
    public UserDataInfo data;

    [System.Serializable]
    public class UserDataInfo
    {
        public string userId;
        public string userName;
        public string password;
    }
}

public class sendUserData
{
    public string userName;
    public string password;
}

public class Script_Login : MonoBehaviour {
    string txt_ID;
    string txt_Psw;
    Text txt_info;
    string filePath;

    InputField iDText;
    InputField pwdText;
    // Use this for initialization
    void Start()
    {
        init_Component();

        // 获取文件路径
        filePath = Application.dataPath + "/" + "users.txt";
    }
	
	// Update is called once per frame
	void Update () 
    {
		
	}

    private void init_Component()
    {
        // 获取 Txt_Info 的 Text 组件
        txt_info = GameObject.Find("Txt_Info").GetComponent<Text>();    
        // 获取 IptF_ID/Text 和 IptF_Psw/Text 的 InputField 组件
        GameObject idTextObject = GameObject.Find("IptF_ID");
        GameObject pwdTextObject = GameObject.Find("IptF_Psw");

        if (idTextObject != null)
        {
            iDText = idTextObject.GetComponent<InputField>();
        }
        else
        {
            Debug.LogWarning("未找到名为 'IptF_ID' 的 GameObject");
        }

        if (pwdTextObject != null)
        {
            pwdText = pwdTextObject.GetComponent<InputField>();
            // 将密码框内容设置为密码模式
            pwdText.contentType = InputField.ContentType.Password;
        }
        else
        {
            Debug.LogWarning("未找到名为 'IptF_Psw' 的 GameObject");
        }
    }
    // ***********************************************************************
    // public delegate void PostRequestCallback(string result);
    public IEnumerator PostRequest(string url, string userId, string passWord)
    {
        bool is_login_succeed = false;
        string url_login = url + userId + "/" + passWord;
        Debug.Log(url_login);
        using (UnityWebRequest webRequest = UnityWebRequest.Post(url_login, ""))
        {
            // webRequest.SetRequestHeader("Content-Type", "application/json");
            // webRequest.SetRequestHeader("Accept-Encoding", "gzip, deflate, br");
            // webRequest.SetRequestHeader("Connection", "keep-alive");
            Debug.Log(webRequest.ToString());
            yield return webRequest.SendWebRequest();

            if (webRequest.isNetworkError || webRequest.isHttpError)
            {
                Debug.Log("Failed");
                txt_info.text = "服务器连接失败";
                is_login_succeed = false;
                pwdText.text = "";  
            }
            else
            {
                string jsonResponse = webRequest.downloadHandler.text;
                Debug.Log("Json part:");
                Debug.Log(jsonResponse);
                // 解析JSON
                UserData userData = JsonUtility.FromJson<UserData>(jsonResponse);

                if (userData != null)
                {
                    Debug.Log("Code: " + userData.code);
                    Debug.Log("Msg: " + userData.msg);

                    if (userData.code == 0) // Assuming 0 means success
                    {
                        Debug.Log("User ID: " + userData.data.userId);
                        Debug.Log("User Name: " + userData.data.userName);
                        Debug.Log("Password: " + userData.data.password);
                        // 存储用户信息
                        PlayerPrefs.SetString("userid", userData.data.userId);
                        PlayerPrefs.SetString("username", userData.data.userName);
                        PlayerPrefs.SetString("password", userData.data.password);
                        txt_info.text = "登录成功";
                        is_login_succeed = true;
                        SceneManager.LoadScene("Menu");
                    }
                    else
                    {
                        Debug.LogError("Login failed: " + userData.msg);
                        txt_info.text = "登录失败: " + userData.msg;
                               
                        is_login_succeed = false;    
                        pwdText.text = "";    
                    }
                }
                else
                {
                    Debug.LogError("Failed to parse JSON");
                    txt_info.text = "登录失败";
                    is_login_succeed = false;
                    pwdText.text = "";  
                }
            }
            // 清空密码
            if(!is_login_succeed)
            {
                pwdText.text = "";   
            }
        }
    }

    public void Login()
    {
        txt_ID = GameObject.Find("IptF_ID/Text").GetComponent<Text>().text;
        txt_Psw = GameObject.Find("IptF_Psw/Text").GetComponent<Text>().text;
        if (txt_ID == "")
        {
            txt_info.text = "请输入账号";
            return;
        }
        else if (txt_Psw == "")
        {
            txt_info.text = "请输入密码";
            return;
        }
        else
        {
            StartCoroutine(PostRequest("http://47.93.242.88:8080/login/", iDText.text, pwdText.text));  // 启动协程
        }
    }

    bool Check_Login(string id, string psw)
    {
        string[] Users = File.ReadAllLines(filePath);
        for (int i = 0; i < Users.Length; i++)
        {
            string user_id = Users[i].Split(' ')[0];
            string user_psw = Users[i].Split(' ')[1];
            if (id == user_id && psw == user_psw)
            {
                return true;
            }
        }
        return false;
    }
}
// </钟开>