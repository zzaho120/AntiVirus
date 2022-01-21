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
                    passiveSkill.skillName  = splitData[1];
                    passiveSkill.character  = splitData[2];
                    passiveSkill.type       = splitData[3];
                    passiveSkill.level      = int.Parse(splitData[4]);
                    passiveSkill.skillCase  = splitData[5];
                    switch (splitData[6])
                    {
                        case "Sight":
                            passiveSkill.stat = Stat.Sight;
                            break;
                        case "Reduction":
                            passiveSkill.stat = Stat.Reduction;
                            break;
                    }
                    passiveSkill.increase   = float.Parse(splitData[7]);
                    passiveSkill.decrease   = float.Parse(splitData[8]);
                    passiveSkill.lifeTurn   = int.Parse(splitData[9]);

                    AssetDatabase.CreateAsset(passiveSkill, $"Assets//Resources/Choi/Datas/Skills/PassiveSkills/{passiveSkill.skillName}.asset");
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
