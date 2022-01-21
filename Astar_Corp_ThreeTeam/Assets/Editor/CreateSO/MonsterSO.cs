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
                    monster.id              = splitData[0];
                    monster.name            = splitData[1];

                    monster.minHp           = int.Parse(splitData[2]);
                    monster.maxHp           = int.Parse(splitData[3]);
                    
                    monster.atkRange        = int.Parse(splitData[4]);
                    monster.ap              = int.Parse(splitData[5]);
                    monster.mp              = int.Parse(splitData[6]);
                    monster.closeUpAtkAp    = int.Parse(splitData[7]);
                    
                    monster.minDmg          = int.Parse(splitData[8]);
                    monster.maxDmg          = int.Parse(splitData[9]);
                    monster.minCritRate     = int.Parse(splitData[10]);
                    monster.maxCritRate     = int.Parse(splitData[11]);
                    monster.critDmg         = int.Parse(splitData[12]);
                    monster.critResist      = int.Parse(splitData[13]);
                    
                    monster.exp             = int.Parse(splitData[14]);
                    monster.sightRange      = int.Parse(splitData[15]);
                    monster.virusGauge      = int.Parse(splitData[16]);
                    monster.escapeHpDec     = int.Parse(splitData[17]);

                    monster.dropItem1     = splitData[18];
                    monster.item1Rate     = splitData[19];
                    monster.dropItem2     = splitData[20];
                    monster.item2Rate     = splitData[21];
                    monster.dropItem3     = splitData[22];
                    monster.item3Rate     = splitData[23];

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
