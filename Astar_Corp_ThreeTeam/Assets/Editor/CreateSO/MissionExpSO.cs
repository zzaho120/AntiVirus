using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class MissionExpSO
{
    private static string missionExpCSVPath = "/Resources/Choi/MissionExpDataTable.csv";

    [MenuItem("Utilities/Generate Mission Exp")]
    public static void GenerateMissionExp()
    {
        string[] files = Directory.GetFiles("Assets/Resources/Choi/Datas/MissionExp");
        foreach (string file in files)
        {
            File.Delete(file);
        }

        int missionNum = 0;
        using (FileStream fs = new FileStream(Application.dataPath + missionExpCSVPath, FileMode.Open, FileAccess.Read))
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

                    MissionExp missionExp = ScriptableObject.CreateInstance<MissionExp>();

                    missionExp.id = splitData[0];
                    missionExp.classType = int.Parse(splitData[1]);
                    missionExp.missionId = int.Parse(splitData[2]);
                    missionExp.battleLocation = int.Parse(splitData[3]);
                    missionExp.exp = int.Parse(splitData[4]);

                    AssetDatabase.CreateAsset(missionExp, $"Assets//Resources/Choi/Datas/MissionExp/{missionExp.id}.asset");
                    missionNum++;
                }
                sr.Close();
            }
            fs.Close();
            AssetDatabase.SaveAssets();
            Debug.Log($"임무 경험치 SO가 {missionNum}개 생성되었습니다.");
        }
    }
}