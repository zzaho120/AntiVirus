using UnityEngine;
using UnityEditor;
using System.IO;

public class BunkerSO
{
    private static string bunkerCSVPath = "/Resources/Choi/BunkerDataTable.csv";

    [MenuItem("Utilities/Generate Bunkers")]
    public static void GenerateCharacters()
    {
        string[] files = Directory.GetFiles("Assets/Resources/Choi/Datas/Bunkers");
        foreach (string file in files)
        {
            File.Delete(file);
            //Debug.Log($"{file} is deleted.");
        }

        int bunkerNum = 0;
        using (FileStream fs = new FileStream(Application.dataPath + bunkerCSVPath, FileMode.Open, FileAccess.Read))
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

                    Bunker bunker = ScriptableObject.CreateInstance<Bunker>();
                    bunker.id       = splitData[0];
                    bunker.type     = int.Parse(splitData[1]);
                    bunker.name     = splitData[2];

                    bunker.level1   = int.Parse(splitData[3]);
                    bunker.level2   = int.Parse(splitData[4]);
                    bunker.level3   = int.Parse(splitData[5]);
                    bunker.level4   = int.Parse(splitData[6]);
                    bunker.level5   = int.Parse(splitData[7]);

                    bunker.level1Cost = int.Parse(splitData[8]);
                    bunker.level2Cost = int.Parse(splitData[9]);
                    bunker.level3Cost = int.Parse(splitData[10]);
                    bunker.level4Cost = int.Parse(splitData[11]);
                    bunker.level5Cost = int.Parse(splitData[12]);
                    AssetDatabase.CreateAsset(bunker, $"Assets//Resources/Choi/Datas/Bunkers/{bunker.name}.asset");
                    bunkerNum++;
                }
                sr.Close();
            }
            fs.Close();
            AssetDatabase.SaveAssets();
            Debug.Log($"벙커SO가 {bunkerNum}개 생성되었습니다.");
        }
    }
}
