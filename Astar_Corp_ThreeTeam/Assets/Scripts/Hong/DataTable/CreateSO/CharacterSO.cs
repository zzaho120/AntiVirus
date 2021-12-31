using UnityEngine;
using UnityEditor;
using System.IO;


public class CharacterSO 
{
    private static string characterCSVPath = "/Resources/Choi/CharacterDataTable.csv";

    [MenuItem("Utilities/Generate Characters")]
    public static void GenerateCharacters()
    {
        string[] files = Directory.GetFiles("Assets/Resources/Choi/Datas/Characters");
        foreach (string file in files)
        {
            File.Delete(file);
            //Debug.Log($"{file} is deleted.");
        }

        int characterNum = 0;
        using (FileStream fs = new FileStream(Application.dataPath + characterCSVPath, FileMode.Open, FileAccess.Read))
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

                    Character character = ScriptableObject.CreateInstance<Character>();
                    character.id                    = splitData[0];
                    character.iconId                = splitData[1];
                    character.profileId             = splitData[2];
                    character.prefabId              = splitData[3];

                    character.name                  = splitData[4];
                    character.type                  = splitData[5];
                    character.description           = splitData[6];

                    character.min_Hp                = int.Parse(splitData[7]);
                    character.max_Hp                = int.Parse(splitData[8]);
                    character.min_Hp_Rise           = int.Parse(splitData[9]);
                    character.max_Hp_Rise           = int.Parse(splitData[10]);

                    character.min_Sensitivity       = int.Parse(splitData[11]);
                    character.max_Sensitivity       = int.Parse(splitData[12]);
                    character.min_Sen_Rise          = int.Parse(splitData[13]);
                    character.max_Sen_Rise          = int.Parse(splitData[14]);

                    character.min_Avoid_Rate        = int.Parse(splitData[15]);
                    character.max_Avoid_Rate        = int.Parse(splitData[16]);

                    character.min_Concentration     = int.Parse(splitData[17]);
                    character.max_Concentration     = int.Parse(splitData[18]);
                    character.min_Con_Rise          = int.Parse(splitData[19]);
                    character.max_Con_Rise          = int.Parse(splitData[20]);

                    character.min_Willpower         = int.Parse(splitData[21]);
                    character.max_Willpower         = int.Parse(splitData[22]);
                    character.min_Will_Rise         = int.Parse(splitData[23]);
                    character.max_Will_Rise         = int.Parse(splitData[24]);

                    character.min_Crit_Rate         = int.Parse(splitData[25]);
                    character.max_Crit_Rate         = int.Parse(splitData[26]);

                    character.min_Char_Cost         = int.Parse(splitData[27]);
                    character.max_Char_Cost         = int.Parse(splitData[28]);

                    AssetDatabase.CreateAsset(character, $"Assets//Resources/Choi/Datas/Characters/{character.name}.asset");
                    characterNum++;
                }
                sr.Close();
            }
            fs.Close();
            AssetDatabase.SaveAssets();
            Debug.Log($"캐릭터SO가 {characterNum}개 생성되었습니다.");
        }
    }
}
