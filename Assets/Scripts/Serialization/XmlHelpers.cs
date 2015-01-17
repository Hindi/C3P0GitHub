using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;

using UnityEngine;

public class XmlHelpers
{
    public static List<T> LoadFromTextAsset<T>(TextAsset textAsset, System.Type[] extraTypes = null)
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
                ElementName = "Datatable"
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

    public static void SaveToXML<T>(string path, T objectToSerialize) where T : class
    {
        if (string.IsNullOrEmpty(path))
            return;
        XmlSerializer serializer = new XmlSerializer(typeof(T));
        using (StreamWriter stream = new StreamWriter(path, false, new UTF8Encoding(false)))
        {
            Debug.Log(stream.ToString());

            serializer.Serialize(stream, objectToSerialize);
            stream.Close();
        }
    }

    public static Dictionary<string, string> loadCredentials(TextAsset asset)
    {
        Dictionary<string, string> d = new Dictionary<string, string>();
        List<Credential> list = LoadFromTextAsset<Credential>(asset);

        foreach (Credential c in list)
            d.Add(c.login, c.pass);

        return d;
    }

    public static void saveCredentials(string path, Dictionary<string, string> dict)
    {
        List<Credential> list = new List<Credential>();
        foreach (KeyValuePair<string, string> e in dict)
            list.Add(new Credential( e.Key, e.Value));

        SaveToXML<Dictionary<string, string> >(path, dict);
    }
}
