using UnityEngine;
using UnityEditor;
using System.IO;

public class MonsterSO
{
    private static string monsterCSVPath = "/Resources/Choi/MonsterDataTable.csv";

    [MenuItem("Utilities/Generate Monsters")]
    public static void GenerateCharacters()
    {
        string[] files = Directory.GetFiles("Assets/Resources/Choi/Datas/Monsters");
        foreach (string file in files)
        {
            File.Delete(file);
            //Debug.Log($"{file} is deleted.");
        }

        int monsterNum = 0;
        using (FileStream fs = new FileStream(Application.dataPath + monsterCSVPath, FileMode.Open, FileAccess.Read))
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

                    Monster monster = ScriptableObject.CreateInstance<Monster>();
                    monster.id      = splitData[0];
                    monster.name    = splitData[1];
                    monster.damage  = int.Parse(splitData[2]);
                    monster.hp      = int.Parse(splitData[3]);
                    monster.stamina = int.Parse(splitData[4]);
                    monster.range   = int.Parse(splitData[5]);

                    AssetDatabase.CreateAsset(monster, $"Assets//Resources/Choi/Datas/Monsters/{monster.name}.asset");
                    monsterNum++;
                }
                sr.Close();
            }
            fs.Close();
            AssetDatabase.SaveAssets();
            Debug.Log($"몬스터SO가 {monsterNum}개 생성되었습니다.");
        }
    }
}
