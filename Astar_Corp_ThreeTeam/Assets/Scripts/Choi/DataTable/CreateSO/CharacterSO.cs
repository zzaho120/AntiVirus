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
                    character.id = splitData[0];
                    character.iconId = splitData[1];
                    character.profileId = splitData[2];
                    character.prefabId = splitData[3];
                    character.name = splitData[4];
                    character.description = splitData[5];
                    character.level = int.Parse(splitData[6]);
                    character.damage = int.Parse(splitData[7]);
                    character.range = int.Parse(splitData[8]);
                    character.hp = int.Parse(splitData[9]);
                    character.crit_rate = int.Parse(splitData[10]);
                    character.willpower = int.Parse(splitData[11]);
                    character.stamina = int.Parse(splitData[12]);
                    character.resistnace = int.Parse(splitData[13]);

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
