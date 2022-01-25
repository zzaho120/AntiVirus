using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class CharacterExpSO
{
    private static string characterExpCSVPath = "/Resources/Choi/CharacterExpDataTable.csv";

    [MenuItem("Utilities/Generate Character Exp")]
    public static void GenerateCharacterExp()
    {
        string[] files = Directory.GetFiles("Assets/Resources/Choi/Datas/CharacterExp");
        foreach (string file in files)
        {
            File.Delete(file);
        }

        int monsterNum = 0;
        using (FileStream fs = new FileStream(Application.dataPath + characterExpCSVPath, FileMode.Open, FileAccess.Read))
        {
            using (StreamReader sr = new StreamReader(fs))
            {
                bool isFirst = true;

                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    if (isFirst)
                    {
                        isFirst = false;
                        continue;
                    }
                    string[] splitData = line.Split(',');

                    CharacterExp characterExp = ScriptableObject.CreateInstance<CharacterExp>();

                    characterExp.id = splitData[0];
                    characterExp.level = int.Parse(splitData[1]);
                    characterExp.totalExp = int.Parse(splitData[2]);

                    AssetDatabase.CreateAsset(characterExp, $"Assets//Resources/Choi/Datas/CharacterExp/{characterExp.id}.asset");
                    monsterNum++;
                }
                sr.Close();
            }
            fs.Close();
            AssetDatabase.SaveAssets();
            Debug.Log($"경험치SO가 {monsterNum}개 생성되었습니다.");
        }
    }
}