using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;

using UnityEngine;


/// <summary>Serialize and deserialize the datas from and to xml.</summary>
public class XmlHelpers
{
    /// <summary>Use a textAsset to load datas.</summary>
    /// <param name="textAsset">The textAsset</param>
    /// <param name="root">The root node in the xml. Default is "Datatable"</param>
    /// <returns>List<typeparam name="T"></typeparam></returns>
    public static List<T> LoadFromTextAsset<T>(TextAsset textAsset, string root = "Datatable")
    {
        if (textAsset == null)
        {
            throw new ArgumentNullException("textAsset");
        }

        System.IO.TextReader textStream = null;

        try
        {
            textStream = new System.IO.StringReader(textAsset.text);

            XmlRootAttribute xRoot = new XmlRootAttribute
            {
                ElementName = root
            };

            XmlSerializer serializer = new XmlSerializer(typeof(List<T>), xRoot);
            List<T> data = serializer.Deserialize(textStream) as List<T>;

            textStream.Close();

            return data;
        }
        catch (System.Exception exception)
        {
            Debug.LogError("The database of type '" + typeof(T) + "' failed to load the asset. The following exception was raised:\n " + exception.Message);
        }
        finally
        {
            if (textStream != null)
            {
                textStream.Close();
            }
        }

        return null;
    }

    /// <summary>Save datas contained on the serializabe object in an xml file.</summary>
    /// <param name="path">The path (including the name) of the xml file.</param>
    /// <param name="objectToSerialize">The object to be saved.</param>
    /// <returns>void</returns>
    public static void SaveToXML<T>(string path, T objectToSerialize) where T : class
    {
        if (string.IsNullOrEmpty(path))
            return;
        XmlSerializer serializer = new XmlSerializer(typeof(T));
        using (StreamWriter stream = new StreamWriter(path, false, new UTF8Encoding(false)))
        {
            serializer.Serialize(stream, objectToSerialize);
            stream.Close();
        }
    }

    public static T loadFromXML<T>(string path, string root = "Datatable")
    {
        XmlRootAttribute xRoot = new XmlRootAttribute();
        xRoot.ElementName = root;
        xRoot.IsNullable = true;
        XmlSerializer serializer = new XmlSerializer(typeof(T),xRoot);
        using (TextReader reader = new StreamReader(path, new UTF8Encoding(false)))
        {
            T datas = (T)serializer.Deserialize(reader);
            return datas;
        }
    }

    /// <summary>Load the credential file.</summary>
    /// <param name="asset">The textAsset where the credential file is.</param>
    /// <returns>A dictionnary containing the credentials.</returns>
    public static Dictionary<string, string> loadCredentials(TextAsset asset)
    {
        Dictionary<string, string> d = new Dictionary<string, string>();
        List<Credential> list = LoadFromTextAsset<Credential>(asset, "ArrayOfCredential");

        foreach (Credential c in list)
            d.Add(c.login, c.pass);

        return d;
    }

    /// <summary>Save the credential in a file.</summary>
    /// <param name="path">The path (including the name) of the xml file.</param>
    /// <param name="dict">The dictionnary containing the credentials.</param>
    /// <returns>void</returns>
    public static void saveCredentials(string path, Dictionary<string, string> dict)
    {
        List<Credential> list = new List<Credential>();
        foreach (KeyValuePair<string, string> e in dict)
            list.Add(new Credential( e.Key, e.Value));

        SaveToXML<List<Credential>>(path, list);
    }
}
