using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public static class SaveLoadSystem
{
    public enum Modes
    {
        Text,
        Binary
    }

    public static readonly string DefaultFileName = "SaveData.json";
    public static readonly string SaveFileListPath = "SaveFileList.json";

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

    public static int saveListIndex = 0;


    public static List<string> filePathList = new List<string>();
    private static SaveDatabase currentVersion = new SaveDataV2();

    public static void Init()
    {
        filePathList = LoadList();
    }

    public static void Save(SaveDatabase data, int idx = 0)
    {
        if (!(filePathList.Count > idx))
        {
            idx = filePathList.Count;
        }

        FilePath = $"SaveData{idx}.json";
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

        if (!filePathList.Contains(FilePath))
        {
            filePathList.Add(FilePath);
            SaveList();
        }
    }

    public static SaveDatabase Load(string path)
    {
        SaveDatabase data = new SaveDatabase();
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

    public static SaveDatabase GetDataFromFile(StreamReader reader)
    {
        JsonSerializer serializer = new JsonSerializer();
        var txtTemp = (SaveDatabase)serializer.Deserialize(reader, typeof(SaveDatabase));
        reader.BaseStream.Position = 0;
        switch (txtTemp.Version)
        {
            case 1:
                txtTemp = (SaveDatabase)serializer.Deserialize(reader, typeof(SaveDataV1));
                break;
            case 2:
                txtTemp = (SaveDatabase)serializer.Deserialize(reader, typeof(SaveDataV2));
                break;
        }

        while (txtTemp.Version != currentVersion.Version)
            txtTemp = txtTemp.VersionUp();

        return txtTemp;
    }

    public static SaveDatabase GetDataFromFile(string path)
    {
        SaveDatabase binaryTemp = new SaveDatabase();
        using (BsonReader reader = new BsonReader(File.OpenRead(path)))
        {
            JsonSerializer serializer = new JsonSerializer();
            binaryTemp = (SaveDatabase)serializer.Deserialize(reader, typeof(SaveDatabase));
        }
        using (BsonReader reader = new BsonReader(File.OpenRead(path)))
        {
            JsonSerializer serializer = new JsonSerializer();

            switch (binaryTemp.Version)
            {
                case 1:
                    binaryTemp = (SaveDatabase)serializer.Deserialize(reader, typeof(SaveDataV1));
                    break;
                case 2:
                    binaryTemp = (SaveDatabase)serializer.Deserialize(reader, typeof(SaveDataV2));
                    break;
            }
            while (binaryTemp.Version != currentVersion.Version)
                binaryTemp = binaryTemp.VersionUp();
        }
        return binaryTemp;
    }
    public static void ChangeMode()
    {
        switch (CurrentMode)
        {
            case Modes.Text:
                CurrentMode = Modes.Binary;
                break;
            case Modes.Binary:
                CurrentMode = Modes.Text;
                break;
        }
    }
    public static void SaveList()
    {
        string json = JsonConvert.SerializeObject(filePathList);
        File.WriteAllText(SaveFileListPath, json);
    }

    public static List<string> LoadList()
    {
        List<string> list = new List<string>();
        if (File.Exists(SaveFileListPath))
        {
            list = JsonConvert.DeserializeObject<List<string>>(File.ReadAllText(SaveFileListPath));
        }
        return list;
    }
}