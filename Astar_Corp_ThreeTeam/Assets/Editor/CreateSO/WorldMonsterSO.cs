using UnityEngine;
using UnityEditor;
using System.IO;

public class WorldMonsterSO
{
    private static string monsterCSVPath = "/Resources/Choi/NBMonsterDataTable.csv";

    [MenuItem("Utilities/Generate WorldMonsters")]
    public static void GenerateCharacters()
    {
        string[] files = Directory.GetFiles("Assets/Resources/Choi/Datas/WorldMonsters");
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

                    WorldMonster worldMonster = ScriptableObject.CreateInstance<WorldMonster>();
                    worldMonster.id             = splitData[0];
                    worldMonster.prefabId       = splitData[1];
                    worldMonster.name           = splitData[2];

                    worldMonster.speed          = int.Parse(splitData[3]);
                    worldMonster.suddenAtkRate  = int.Parse(splitData[4]);
                    worldMonster.sightRange     = int.Parse(splitData[5]);
                    worldMonster.areaRange      = int.Parse(splitData[6]);
                    worldMonster.areaMaxNum     = int.Parse(splitData[7]);
                    worldMonster.createTime     = int.Parse(splitData[8]);

                    worldMonster.battleMinNum   = int.Parse(splitData[9]);
                    worldMonster.battleMaxNum   = int.Parse(splitData[10]);

                    AssetDatabase.CreateAsset(worldMonster, $"Assets//Resources/Choi/Datas/WorldMonsters/{worldMonster.name}.asset");
                    monsterNum++;
                }
                sr.Close();
            }
            fs.Close();
            AssetDatabase.SaveAssets();
            Debug.Log($"비전투씬 몬스터SO가 {monsterNum}개 생성되었습니다.");
        }
    }
}
