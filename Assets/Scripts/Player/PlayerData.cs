using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using System;

[XmlType("Credential")]
public class Credential
{
    public Credential()
    { 
    }

    public Credential(string log, string pas)
    {
        login = log;
        pass = pas;
    }

    [XmlAttribute]
    public string login;
    [XmlAttribute]
    public string pass;
}

public class PlayerData
{

    private TextAsset credentialFile;
    private float lastCheckTime;

    //Contains login and hashed pass
    private Dictionary<string, string> loginInfos;
    private bool modified = false;

    public PlayerData()
    {
        credentialFile = (TextAsset)UnityEngine.Resources.Load("xml/liste des eleves");
        loginInfos = XmlHelpers.loadCredentials(credentialFile);

        lastCheckTime = Time.time;
    }

    public void resetPassword(string login)
    {
        Debug.Log(login + " " + loginInfos.ContainsKey(login));
        if (loginInfos.ContainsKey(login))
            loginInfos[login] = "";
    }

    public void resetPassword()
    {
        var keys = new List<string>(loginInfos.Keys);
        foreach (string k in keys)
            loginInfos[k] = "";
    }

    //ncrypt password
    private string encryptMd5(string pass)
    {
        System.Security.Cryptography.MD5CryptoServiceProvider md5Hasher = new System.Security.Cryptography.MD5CryptoServiceProvider();
        byte[] bs = System.Text.Encoding.UTF8.GetBytes(pass);
        bs = md5Hasher.ComputeHash(bs);
        System.Text.StringBuilder s = new System.Text.StringBuilder();
        foreach (byte b in bs)
            s.Append(b.ToString("x2").ToLower());

        return s.ToString();
    }

    //Check password
    private bool verifyMd5(string input, string hash)
    {
        string hashOfInput = encryptMd5(input);

        StringComparer comparer = StringComparer.OrdinalIgnoreCase;

        if (0 == comparer.Compare(hashOfInput, hash))
            return true;
        else
            return false;
    }

    public bool checkAuth(string name, string pass, NetworkPlayer player)
    {
        if (loginInfos.ContainsKey(name))
        {
            if (verifyMd5(pass, loginInfos[name]))
            {
                return true;
            }
            else
                if (loginInfos[name] == "")
                {
                    loginInfos[name] = encryptMd5(pass);
                    XmlHelpers.saveCredentials("Assets/Resources/xml/liste des eleves.xml", loginInfos);
                    return true;
                }
                else
                {
                    C3PONetworkManager.Instance.sendNotifyWrongPassword(player, name);
                    return false;
                }
        }
        else
        {
            C3PONetworkManager.Instance.sendNotifyWrongLogin(player, name);
            loginInfos.Add(name, encryptMd5(pass));
            XmlHelpers.saveCredentials("Assets/Resources/xml/liste des eleves.xml", loginInfos);
            return true;
        }
    }

    public void update()
    {
        if(modified)
        {
            if (Time.time - lastCheckTime > 120)
            {
                modified = false;
            }
        }

    }

}
