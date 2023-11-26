//<钟开>
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Threading;

public class Script_Regist : MonoBehaviour {
    string txt_ID;
    string txt_Psw;
    string txt_Psw_2;
    bool b_IfLoginSuccess;
    Text txt_info;
    string filePath;

    InputField idInput;
    InputField pwdInput;
    InputField comfirmInput;
    StreamWriter sw;

    // Use this for initialization
    void Start () 
    {
        filePath = Application.dataPath + "/" + "users.txt";
        init_Component();
    }
	
	// Update is called once per frame
	void Update () 
    {
		
	}

    private void init_Component()
    {
        // 获取提示文本
        txt_info = GameObject.Find("Txt_Info").GetComponent<Text>();
        GameObject idObject = GameObject.Find("IptF_ID");
        GameObject pwdObject = GameObject.Find("IptF_Psw");
        GameObject pwd2Object = GameObject.Find("comfirm");

        if(idObject != null)
        {
            idInput = idObject.GetComponent<InputField>();
        }
        else
        {
            Debug.LogWarning("未找到名为 'IptF_ID' 的 GameObject");
        }

        if(pwdObject != null)
        {
            pwdInput = pwdObject.GetComponent<InputField>();
            pwdInput.contentType = InputField.ContentType.Password;
        }
        else
        {
            Debug.LogWarning("未找到名为 'IptF_Psw' 的 GameObject");
        }

        if(pwd2Object != null)
        {
            comfirmInput = pwd2Object.GetComponent<InputField>();
            comfirmInput.contentType = InputField.ContentType.Password;
        }
        else
        {
            Debug.LogWarning("未找到名为 'comfirm' 的 GameObject");
        }
    }

    public bool IsPasswordValid(string password)
    {
        // 检查密码长度是否超过8
        if (password.Length < 8)
        {
            Debug.Log("密码长度必须至少为8个字符");
            txt_info.text = "密码长度必须至少为8个字符";
            return false;
        }

        // 检查密码是否包含数字、大写字母、小写字母、符号中的至少两种
        int categoriesCount = 0;

        if (ContainsDigit(password))
        {
            categoriesCount++;
        }

        if (ContainsUppercaseLetter(password))
        {
            categoriesCount++;
        }

        if (ContainsLowercaseLetter(password))
        {
            categoriesCount++;
        }

        if (ContainsSymbol(password))
        {
            categoriesCount++;
        }

        if (categoriesCount < 2)
        {
            Debug.Log("密码必须包含数字、大写字母、小写字母、符号中的至少两种");
            txt_info.text = "密码必须包含数字、大写字母、小写字母、符号中的至少两种";
            return false;
        }

        // 如果所有条件都满足，密码有效
        Debug.Log("密码有效");
        return true;
    }

    bool ContainsDigit(string password)
    {
        return System.Text.RegularExpressions.Regex.IsMatch(password, @"\d");
    }

    bool ContainsUppercaseLetter(string password)
    {
        return System.Text.RegularExpressions.Regex.IsMatch(password, @"[A-Z]");
    }

    bool ContainsLowercaseLetter(string password)
    {
        return System.Text.RegularExpressions.Regex.IsMatch(password, @"[a-z]");
    }

    bool ContainsSymbol(string password)
    {
        return System.Text.RegularExpressions.Regex.IsMatch(password, @"[!@#\$%^&*\(\),.?"":{}|<>]");
    }

    public IEnumerator WaitSecond()
    {
        yield return new WaitForSeconds(3);
    }

    public void Regist()
    {
        txt_ID = GameObject.Find("IptF_ID/Text").GetComponent<Text>().text;
        txt_Psw = GameObject.Find("IptF_Psw/Text").GetComponent<Text>().text;
        txt_Psw_2 = GameObject.Find("comfirm/Text").GetComponent<Text>().text;
        if(txt_ID == "")
        {
            txt_info.text = "请输入账号";
            return;
        }
        if (txt_Psw == "")
        {
            txt_info.text = "请输入密码";
            return;
        }
        if (txt_Psw_2 == "")
        {
            txt_info.text = "请输入密码";
            return;
        }
        if(!IsPasswordValid(pwdInput.text))
        {
            return ;
        }
        if (pwdInput.text != comfirmInput.text)
        {
            txt_info.text = "两次输入的密码不一致";
        }
        else
        {
            // if (CheckID(txt_ID) == false)
            // {
            //     txt_info.text = "已存在相同账号";
            // }
            // else
            // {

            txt_info.text = "注册成功";
            // System.Threading.Thread.Sleep(1000);
            SceneManager.LoadScene("UI_Login");
            //     //
            //     WriteUserInfo(txt_ID, txt_Psw);
            // }
        }
    }

    void WriteUserInfo(string id, string psw)
    {
        if (!File.Exists(filePath))
        {
            sw = File.CreateText(filePath);
        }

        sw = File.AppendText(filePath);
        sw.WriteLine(id + " " + psw);
        sw.Close();
    }

    bool CheckID(string id)
    {
        string[] Users = File.ReadAllLines(filePath);
        for (int i = 0; i < Users.Length; i++)
        {
            string user_id = Users[i].Split(' ')[0];
            //string user_psw = Users[i - 1].Split( )[1];
            if (id == user_id)
            {
                return false;
            }
        }
        return true;
    }
}
//</钟开>