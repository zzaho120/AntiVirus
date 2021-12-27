using UnityEngine;
using UnityEditor;
using System.IO;

public class AntibodySO
{
    private static string antibodyCSVPath = "/Resources/Choi/AntibodyDataTable.csv";

    [MenuItem("Utilities/Generate Antibodys")]
    public static void GenerateCharacters()
    {
        string[] files = Directory.GetFiles("Assets/Resources/Choi/Datas/Antibodys");
        foreach (string file in files)
        {
            File.Delete(file);
            //Debug.Log($"{file} is deleted.");
        }

        int antibodyNum = 0;
        using (FileStream fs = new FileStream(Application.dataPath + antibodyCSVPath, FileMode.Open, FileAccess.Read))
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

                    Antibody antibody = ScriptableObject.CreateInstance<Antibody>();
                    antibody.id               = splitData[0];
                    antibody.name             = splitData[1];
                    antibody.level            = int.Parse(splitData[2]);
                    antibody.demandedExp      = int.Parse(splitData[3]);
                    antibody.hitDmgDecRate    = float.Parse(splitData[4]);
                    antibody.virusSkillResist = float.Parse(splitData[5]);
                    antibody.virusDmgDecRate  = float.Parse(splitData[6]);
                    antibody.suddenDmgDecRate = float.Parse(splitData[7]);

                    AssetDatabase.CreateAsset(antibody, $"Assets//Resources/Choi/Datas/Antibodys/{antibody.name}항체 {antibody.level}.asset");
                    antibodyNum++;
                }
                sr.Close();
            }
            fs.Close();
            AssetDatabase.SaveAssets();
            Debug.Log($"항체SO가 {antibodyNum}개 생성되었습니다.");
        }
    }
}
