using UnityEngine;
using UnityEditor;
using System.IO;

public class NameSO
{
    private static string nameCSVPath = "/Resources/Choi/NameDataTable.csv";

    [MenuItem("Utilities/Generate Names")]
    public static void GenerateCharacters()
    {
        string[] files = Directory.GetFiles("Assets/Resources/Choi/Datas/Names");
        foreach (string file in files)
        {
            File.Delete(file);
            //Debug.Log($"{file} is deleted.");
        }

        int nameNum = 0;
        using (FileStream fs = new FileStream(Application.dataPath + nameCSVPath, FileMode.Open, FileAccess.Read))
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

                    Name name = ScriptableObject.CreateInstance<Name>();
                    name.id     = splitData[0];
                    name.name   = splitData[1];

                    AssetDatabase.CreateAsset(name, $"Assets//Resources/Choi/Datas/Names/{name.name}.asset");
                    nameNum++;
                }
                sr.Close();
            }
            fs.Close();
            AssetDatabase.SaveAssets();
            Debug.Log($"이름SO가 {nameNum}개 생성되었습니다.");
        }
    }
}
