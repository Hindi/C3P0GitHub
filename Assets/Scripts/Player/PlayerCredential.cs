using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;

/// <summary>Class that contains client's credentials for xml serialization</summary>
[XmlType("Credential")]
public class Credential
{
    /// <summary>Default constructor.</summary>
    public Credential()
    { 
    }

    /// <summary>Default constructor.</summary>
    public Credential(string log, string pas)
    {
        login = log;
        pass = pas;
    }

    /// <summary>The login.</summary>
    [XmlAttribute]
    public string login;

    /// <summary>The password.</summary>
    [XmlAttribute]
    public string pass;
}

/// <summary>Class that contains the clients credentials when they connect.</summary>
public class PlayerCredential
{
    /// <summary>Contains the necessary informations for XML serialization.</summary>
    private TextAsset credentialFile;

    /// <summary>The Dictionnary that contains the credentials.</summary>
    private Dictionary<string, string> loginInfos;

    /// <summary>Default constructor.</summary>
    public PlayerCredential()
    {
        credentialFile = (TextAsset)UnityEngine.Resources.Load("xml/credentials");
        loginInfos = XmlHelpers.loadCredentials(credentialFile);
    }

    /// <summary>Reset the password so that the client will be able to set another one on the next sign in.</summary>
    /// <param name="login">The login of the client</param>
    /// <returns>void</returns>
    public void resetPassword(string login)
    {
        if (loginInfos.ContainsKey(login))
            loginInfos[login] = "";
    }

    /// <summary>Reset the password of all the clients.</summary>
    /// <returns>void</returns>
    public void resetPassword()
    {
        var keys = new List<string>(loginInfos.Keys);
        foreach (string k in keys)
            loginInfos[k] = "";
    }

    /// <summary>Called when someone tris to sign in. The function checks for the login and password.
    /// and assign a new password if no one is assigned.</summary>
    /// <param name="name">The login delivered by the client.</param>
    /// <param name="pass">The password delivered by the client.</param>
    /// <param name="pass">The NetworkPlayer object that identifies the client.</param>
    /// <returns>Bool : True if login and password are correct.</returns>
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
                    XmlHelpers.saveCredentials("Assets/Resources/xml/credentials.xml", loginInfos);
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
            XmlHelpers.saveCredentials(Application.dataPath + "/Resources/xml/credentials.xml", loginInfos);
            return true;
        }
    }
}
