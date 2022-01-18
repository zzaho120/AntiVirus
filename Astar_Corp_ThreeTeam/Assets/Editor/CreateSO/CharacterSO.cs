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

                    character.minHp                = int.Parse(splitData[7]);
                    character.maxHp                = int.Parse(splitData[8]);
                    character.hpChance              = int.Parse(splitData[9]);
                    character.hpRise                = int.Parse(splitData[10]);

                    character.weight                = int.Parse(splitData[11]);
                    character.weight_Rise           = int.Parse(splitData[12]);

                    character.mpPenalty1            = int.Parse(splitData[13]);
                    character.mpPenalty2            = int.Parse(splitData[14]);
                    character.mpPenalty3            = int.Parse(splitData[15]);

                    character.minSensitivity        = int.Parse(splitData[16]);
                    character.maxSensitivity        = int.Parse(splitData[17]);
                    character.senChance             = int.Parse(splitData[18]);
                    character.senRise               = int.Parse(splitData[19]);

                    character.minAvoidRate          = int.Parse(splitData[20]);
                    character.maxAvoidRate          = int.Parse(splitData[21]);
                    character.avoidRateRisePerSen   = int.Parse(splitData[22]);

                    character.minConcentration      = int.Parse(splitData[23]);
                    character.maxConcentration      = int.Parse(splitData[24]);
                    character.concentrationChance   = int.Parse(splitData[25]);
                    character.concentrationRise     = int.Parse(splitData[26]);
                    character.accurRatePerCon       = int.Parse(splitData[27]);

                    character.minWillpower          = int.Parse(splitData[28]);
                    character.maxWillpower          = int.Parse(splitData[29]);
                    character.willChance            = int.Parse(splitData[30]);
                    character.willRise              = int.Parse(splitData[31]);
                    character.alertAccurRateRise    = int.Parse(splitData[32]);

                    character.critRateRise          = int.Parse(splitData[33]);
                    character.critResistRateRise    = int.Parse(splitData[34]);

                    character.minCharCost           = int.Parse(splitData[35]);
                    character.maxCharCost           = int.Parse(splitData[36]);

                    character.exp                   = int.Parse(splitData[37]);
                    character.resistGauge           = int.Parse(splitData[38]);
                    character.virusDec_Lev0         = int.Parse(splitData[39]);
                    character.virusDec_Lev1         = int.Parse(splitData[40]);


                    // 무기 리스트
                    string[] weapon = splitData[41].Split('*');
                    for (int i = 0; i < weapon.Length; i++)
                    {
                        character.weapons.Add(weapon[i]);
                    }

                    // 스킬 리스트
                    string[] skillA = splitData[42].Split('*');
                    for (int i = 0; i < skillA.Length; i++)
                    {
                        character.skillA.Add(skillA[i]);
                    }
                    string[] skillB = splitData[43].Split('*');
                    for (int i = 0; i < skillB.Length; i++)
                    {
                        character.skillB.Add(skillB[i]);
                    }
                    string[] skillC = splitData[44].Split('*');
                    for (int i = 0; i < skillC.Length; i++)
                    {
                        character.skillC.Add(skillC[i]);
                    }
                    string[] skillD = splitData[45].Split('*');
                    for (int i = 0; i < skillD.Length; i++)
                    {
                        character.skillD.Add(skillD[i]);
                    }

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
