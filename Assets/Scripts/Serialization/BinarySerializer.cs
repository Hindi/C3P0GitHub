using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;



/// <summary>Class use to serialize data in binary format.</summary>
public class BinarySerializer {


    /// <summary>Serialize the datas contained in the GameList object.</summary>
    /// <param name="data">The GameList object</param>
    /// <returns>void</returns>
    public static void SerializeData(GameList data)
    {
        FileStream fs = new FileStream("Assets/Resources/data.dat", FileMode.Create);

        // Construct a BinaryFormatter and use it to serialize the data to the stream.
        BinaryFormatter formatter = new BinaryFormatter();
        try
        {
            formatter.Serialize(fs, data);
        }
        catch (SerializationException e)
        {
            Console.WriteLine("Failed to serialize. Reason: " + e.Message);
            throw;
        }
        finally
        {
            fs.Close();
        }
    }


    /// <summary>Deserialize the datas contained in data.dat.</summary>
    /// <returns>GameList:  the datas in data.dat</returns>
    public static GameList DeserializeData()
    {
        // Open the file containing the data that you want to deserialize.
        FileStream fs = new FileStream("Assets/Resources/data.dat", FileMode.Open);
        try
        {
            BinaryFormatter formatter = new BinaryFormatter();

            // Deserialize the hashtable from the file and 
            // assign the reference to the local variable.
            return (GameList)formatter.Deserialize(fs);
        }
        catch (SerializationException e)
        {
            Console.WriteLine("Failed to deserialize. Reason: " + e.Message);
            throw;
        }
        finally
        {
            fs.Close();
        }
    }
}
