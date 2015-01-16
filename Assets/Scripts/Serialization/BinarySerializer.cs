using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;


public class BinarySerializer {

    public static void SerializeData(PlayerData data)
    {
        FileStream fs = new FileStream("Assets/Ressources/data.dat", FileMode.Create);

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

    public static PlayerData DeserializeData()
    {
        // Open the file containing the data that you want to deserialize.
        FileStream fs = new FileStream("Assets/Ressources/data.dat", FileMode.Open);
        try
        {
            BinaryFormatter formatter = new BinaryFormatter();

            // Deserialize the hashtable from the file and 
            // assign the reference to the local variable.
            return (PlayerData)formatter.Deserialize(fs);
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
