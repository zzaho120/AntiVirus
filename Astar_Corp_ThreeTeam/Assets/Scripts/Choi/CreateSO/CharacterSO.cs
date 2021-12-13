using UnityEngine;
using UnityEditor;
using System.IO;


public class CharacterSO 
{
    private static string characterCSVPath = "/Resources/Choi/CharacterDataTable.csv";

    [MenuItem("Utilities/Generate Characters")]
    public static void GenerateCharacters()
    {
        string[] files = Directory.GetFiles("Assets/Resources/Choi/Characters");
        foreach (string file in files)
        {
            File.Delete(file);
            //Debug.Log($"{file} is deleted.");
        }

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
                    character.ad = int.Parse(splitData[6]);
                    character.ap = int.Parse(splitData[7]);
                    character.df = int.Parse(splitData[8]);
                    character.hp = int.Parse(splitData[9]);
                    character.mp = int.Parse(splitData[10]);
                    character.statStr = int.Parse(splitData[11]);
                    character.statDex = int.Parse(splitData[12]);
                    character.statInt = int.Parse(splitData[13]);
                    character.statLuk = int.Parse(splitData[14]);

                    AssetDatabase.CreateAsset(character, $"Assets//Resources/Choi/Characters/{character.name}.asset");
                }
                sr.Close();
            }
            fs.Close();
            AssetDatabase.SaveAssets();
        }
    }
}
