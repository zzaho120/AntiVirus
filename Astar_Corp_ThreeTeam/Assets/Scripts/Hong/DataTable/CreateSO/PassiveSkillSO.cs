using UnityEngine;
using UnityEditor;
using System.IO;

public class PassiveSkillSO
{
    private static string passiveSkillCSVPath = "/Resources/Choi/PassiveSkillDataTable.csv";

    [MenuItem("Utilities/Generate Passive Skills")]
    public static void GenerateCharacters()
    {
        string[] files = Directory.GetFiles("Assets/Resources/Choi/Datas/Skills/PassiveSkills");
        foreach (string file in files)
        {
            File.Delete(file);
            //Debug.Log($"{file} is deleted.");
        }

        int passiveSkillNum = 0;
        using (FileStream fs = new FileStream(Application.dataPath + passiveSkillCSVPath, FileMode.Open, FileAccess.Read))
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

                    PassiveSkill passiveSkill = ScriptableObject.CreateInstance<PassiveSkill>();
                    passiveSkill.id         = splitData[0];
                    passiveSkill.name       = splitData[1];
                    passiveSkill.hp         = int.Parse(splitData[2]);
                    passiveSkill.willpower  = int.Parse(splitData[3]);
                    passiveSkill.stamina    = float.Parse(splitData[4]);

                    AssetDatabase.CreateAsset(passiveSkill, $"Assets//Resources/Choi/Datas/Skills/PassiveSkills/{passiveSkill.name}.asset");
                    passiveSkillNum++;
                }
                sr.Close();
            }
            fs.Close();
            AssetDatabase.SaveAssets();
            Debug.Log($"스킬(Passive)SO가 {passiveSkillNum}개 생성되었습니다.");
        }
    }
}
