using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;

[XmlType("PlayerData")]
public class PlayerData {

    PlayerData()
    {
        loginInfos = new Dictionary<string, string>();
        studentNames = new List<string>();

        TextAsset studentFile;
        studentFile = (TextAsset)UnityEngine.Resources.Load("xml/liste des eleves");
        studentNames = XmlHelpers.LoadFromTextAsset<string>(studentFile);

        lastCheckTime = Time.time;
    }

    private float lastCheckTime;
    private Dictionary<string, string> loginInfos;
    private List<string> studentNames;
    private bool modified = false;

    public void modifyEntry(string name, string pass, NetworkPlayer player)
    {
        if(loginInfos.ContainsKey(name))
        {
            modified = true;
            loginInfos[name] = pass;
        }
        else 
        {
            if(studentNames.Contains(name))
                loginInfos.Add(name, pass);
            else
            {
                C3PONetworkManager.Instance.sendNotifyWrongLogin(player, name);
            }
        }
        BinarySerializer.SerializeData(this);
    }

    public bool checkAuth(string name, string pass, NetworkPlayer player)
    {
        if (loginInfos.ContainsKey(name) && loginInfos[name] == pass)
            return true;
        else if (loginInfos.ContainsKey(name) && loginInfos[name] != pass)
            return false;
        else
        {
            Debug.Log("Adding password to base");
            modifyEntry(name, pass, player);
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
                BinarySerializer.SerializeData(this);
            }
        }

    }

}
