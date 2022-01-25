using UnityEngine;
using UnityEditor;
using System.IO;


public class VirusSO
{
    private static string virusCSVPath = "/Resources/Choi/VirusDataTable.csv";

    [MenuItem("Utilities/Generate Viruses")]
    public static void GenerateCharacters()
    {
        string[] files = Directory.GetFiles("Assets/Resources/Choi/Datas/Viruses");
        foreach (string file in files)
        {
            File.Delete(file);
            //Debug.Log($"{file} is deleted.");
        }

        int virusNum = 0;
        using (FileStream fs = new FileStream(Application.dataPath + virusCSVPath, FileMode.Open, FileAccess.Read))
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

                    Virus virus = ScriptableObject.CreateInstance<Virus>();
                    virus.id           = splitData[0];
                    virus.name         = splitData[1];
                                       
                    virus.penaltyType  = (VirusPenalyType)int.Parse(splitData[2]);
                    virus.hp_Dec       = int.Parse(splitData[3]);
                    virus.stat_Dec     = int.Parse(splitData[4]);
                                       
                    virus.hp           = int.Parse(splitData[5]);
                    virus.ap           = int.Parse(splitData[6]);
                    virus.damage       = int.Parse(splitData[7]);
                    virus.crit_Rate    = int.Parse(splitData[8]);
                    virus.crit_Dmg     = int.Parse(splitData[9]);

                    virus.exp          = int.Parse(splitData[10]);
                    virus.virusGauge  = int.Parse(splitData[11]);

                    AssetDatabase.CreateAsset(virus, $"Assets//Resources/Choi/Datas/Viruses/{virus.name}.asset");
                    virusNum++;
                }
                sr.Close();
            }
            fs.Close();
            AssetDatabase.SaveAssets();
            Debug.Log($"바이러스SO가 {virusNum}개 생성되었습니다.");
        }
    }
}
