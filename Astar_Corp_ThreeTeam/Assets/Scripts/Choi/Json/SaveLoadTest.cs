using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SaveLoadTest : MonoBehaviour
{
    public SaveDataV2 saveData = new SaveDataV2();
    public TMP_Text text;
    public int idx = 1;

    private void Start()
    {
        //saveData.Version = 0;
        SaveLoadSystem.Init();
        PrintValue();

        SaveValue();
    }

    public void SaveValue()
    {
        SaveLoadSystem.Save(saveData, idx);
        PrintValue();
    }

    public void LoadValue()
    {
        var save = SaveLoadSystem.filePathList[idx];
        saveData = SaveLoadSystem.Load(save) as SaveDataV2;
        Debug.Log(save);
        PrintValue();
    }

    public void RandomValue()
    {
        saveData.IntData = Random.Range(0, 100);
        saveData.stringData = Random.Range(0, 100).ToString();
        PrintValue();
    }

    public void ChangeSaveLoadMode()
    {
        SaveLoadSystem.ChangeMode();

        PrintValue();
    }

    public void PrintValue()
    {
        text.text = $"Mode : {SaveLoadSystem.CurrentMode}\n" +
        $"filePath : {SaveLoadSystem.FilePath}\n" +
        $"Version : {saveData.Version}\n" +
        $"int : {saveData.IntData}\n" +
        $"string : {saveData.stringData}\n";

        for (int idx = 0; idx < SaveLoadSystem.filePathList.Count; idx++)
        {
            text.text += $"{SaveLoadSystem.filePathList[idx]}\n";
        }
    }

    public void UpIdx()
    {
        idx = Mathf.Clamp(++idx, 0, SaveLoadSystem.filePathList.Count);
    }

    public void DownIdx()
    {
        idx = Mathf.Clamp(--idx, 0, SaveLoadSystem.filePathList.Count);
    }
}