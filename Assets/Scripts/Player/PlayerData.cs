using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Runtime.Serialization;

[XmlType("Credential")]
public class Credential
{
    [XmlAttribute]
    public string login;
    [XmlAttribute]
    public string pass;
}

public class PlayerData
{

    private TextAsset credentialFile;
    private float lastCheckTime;

    private Dictionary<string, string> loginInfos;
    private bool modified = false;

    public PlayerData()
    {
        credentialFile = (TextAsset)UnityEngine.Resources.Load("xml/liste des eleves");
        loginInfos = XmlHelpers.loadCredentials(credentialFile);

        lastCheckTime = Time.time;
    }

    public bool checkAuth(string name, string pass, NetworkPlayer player)
    {
        if (loginInfos.ContainsKey(name))
        {
            if (loginInfos[name] == pass)
            {
                return true;
            }
            else
                if (loginInfos[name] == "")
                {
                    loginInfos[name] = pass;
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
            return false;
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
