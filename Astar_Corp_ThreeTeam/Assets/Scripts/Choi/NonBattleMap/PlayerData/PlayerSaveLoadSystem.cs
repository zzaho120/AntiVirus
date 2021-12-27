using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;

public class PlayerSaveLoadSystem
{
    public enum Modes
    {
        Text,
        Binary
    }

    public static readonly string DefaultFileName = "PlayerData.json";
    private static string filePath = string.Empty;

    public static string FilePath
    {
        get
        {
            return filePath;
        }
        set
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(Application.persistentDataPath);
            sb.Append(@"\");
            sb.Append(value);
            filePath = sb.ToString();
        }
    }
    public static Modes CurrentMode { get; set; } = Modes.Text;

    public static void Save(PlayerSaveData data)
    {
        FilePath = $"PlayerData.json";
        switch (CurrentMode)
        {
            case Modes.Text:
                using (StreamWriter writer = File.CreateText(FilePath))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    serializer.Serialize(writer, data);
                }
                break;
            case Modes.Binary:
                using (BsonWriter writer = new BsonWriter(new FileStream(FilePath, FileMode.Create)))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    serializer.Serialize(writer, data);
                }
                break;
        }
    }

    public static PlayerSaveData Load(string path)
    {
        PlayerSaveData data = new PlayerSaveData();
        if (File.Exists(path))
        {
            switch (CurrentMode)
            {
                case Modes.Text:
                    using (StreamReader reader = File.OpenText(path))
                    {
                        data = GetDataFromFile(reader);
                    }
                    break;
                case Modes.Binary:
                    data = GetDataFromFile(path);
                    break;
            }
        }
        return data;
    }

    public static PlayerSaveData GetDataFromFile(StreamReader reader)
    {
        JsonSerializer serializer = new JsonSerializer();
        var txtTemp = (PlayerSaveData)serializer.Deserialize(reader, typeof(PlayerSaveData));
        reader.BaseStream.Position = 0;
        
        return txtTemp;
    }

    public static PlayerSaveData GetDataFromFile(string path)
    {
        PlayerSaveData binaryTemp = new PlayerSaveData();
        using (BsonReader reader = new BsonReader(File.OpenRead(path)))
        {
            JsonSerializer serializer = new JsonSerializer();
            binaryTemp = (PlayerSaveData)serializer.Deserialize(reader, typeof(PlayerSaveData));
        }
        using (BsonReader reader = new BsonReader(File.OpenRead(path)))
        {
            JsonSerializer serializer = new JsonSerializer();
        }
        return binaryTemp;
    }

    public static void Delete(string path)
    {
        File.Delete(path);
    }
}
