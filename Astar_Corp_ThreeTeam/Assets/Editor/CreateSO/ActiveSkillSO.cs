using UnityEngine;
using UnityEditor;
using System.IO;

public class ActiveSkillSO
{
    private static string activeSkillCSVPath = "/Resources/Choi/ActiveSkillDataTable.csv";

    [MenuItem("Utilities/Generate Active Skills")]
    public static void GenerateCharacters()
    {
        string[] files = Directory.GetFiles("Assets/Resources/Choi/Datas/Skills/ActiveSkills");
        foreach (string file in files)
        {
            File.Delete(file);
            //Debug.Log($"{file} is deleted.");
        }

        int activeSkillNum = 0;
        using (FileStream fs = new FileStream(Application.dataPath + activeSkillCSVPath, FileMode.Open, FileAccess.Read))
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

                    ActiveSkill activeSkill = ScriptableObject.CreateInstance<ActiveSkill>();
                    activeSkill.id      = splitData[0];
                    activeSkill.name    = splitData[1];
                    activeSkill.damage  = int.Parse(splitData[2]);

                    AssetDatabase.CreateAsset(activeSkill, $"Assets//Resources/Choi/Datas/Skills/ActiveSkills/{activeSkill.name}.asset");
                    activeSkillNum++;
                }
                sr.Close();
            }
            fs.Close();
            AssetDatabase.SaveAssets();
            Debug.Log($"스킬(Active)SO가 {activeSkillNum}개 생성되었습니다.");
        }
    }
}
