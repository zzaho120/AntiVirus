using System.IO;
using UnityEditor;
using UnityEngine;

public class ResistExpSO
{
    private static string resistExpCSVPath = "/Resources/Choi/ResistExpDataTable.csv";

    [MenuItem("Utilities/Generate Resist Exp")]
    public static void GenerateCharacterExp()
    {
        string[] files = Directory.GetFiles("Assets/Resources/Choi/Datas/ResistExp");
        foreach (string file in files)
        {
            File.Delete(file);
        }

        int resistExpNum = 0;
        using (FileStream fs = new FileStream(Application.dataPath + resistExpCSVPath, FileMode.Open, FileAccess.Read))
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

                    ResistExp resistExp = ScriptableObject.CreateInstance<ResistExp>();

                    resistExp.id = splitData[0];
                    resistExp.level = int.Parse(splitData[1]);
                    resistExp.exp = int.Parse(splitData[2]);

                    AssetDatabase.CreateAsset(resistExp, $"Assets//Resources/Choi/Datas/ResistExp/{resistExp.id}.asset");
                    resistExpNum++;
                }
                sr.Close();
            }
            fs.Close();
            AssetDatabase.SaveAssets();
            Debug.Log($"내성경험치SO가 {resistExpNum}개 생성되었습니다.");
        }
    }
}
