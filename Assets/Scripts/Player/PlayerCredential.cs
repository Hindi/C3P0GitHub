using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;

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

public class PlayerCredential
{

    private TextAsset credentialFile;
    private float lastCheckTime;

    //Contains login and hashed pass
    private Dictionary<string, string> loginInfos;
    private bool modified = false;

    public PlayerCredential()
    {
        credentialFile = (TextAsset)UnityEngine.Resources.Load("xml/liste des eleves");
        loginInfos = XmlHelpers.loadCredentials(credentialFile);

        lastCheckTime = Time.time;
    }

    public void resetPassword(string login)
    {
        if (loginInfos.ContainsKey(login))
            loginInfos[login] = "";
    }

    public void resetPassword()
    {
        var keys = new List<string>(loginInfos.Keys);
        foreach (string k in keys)
            loginInfos[k] = "";
    }

    public bool checkAuth(string name, string pass, NetworkPlayer player)
    {
        if (name.Length > 0 && loginInfos.ContainsKey(name))
        {
            if (Crypto.verifyMd5(pass, loginInfos[name]))
            {
                return true;
            }
            else
                if (loginInfos[name] == "")
                {
                    loginInfos[name] = Crypto.encryptMd5(pass);
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
            loginInfos.Add(name, Crypto.encryptMd5(pass));
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
