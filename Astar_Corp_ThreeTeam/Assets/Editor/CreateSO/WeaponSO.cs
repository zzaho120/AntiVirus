using UnityEngine;
using UnityEditor;
using System.IO;

public class WeaponSO 
{
    private static string equippableCSVPath = "/Resources/Choi/EquipDataTable.csv";

    [MenuItem("Utilities/Generate Weapons")]
    public static void GenerateEquippables()
    {
        string[] files = Directory.GetFiles("Assets/Resources/Choi/Datas/Weapons");
        foreach (string file in files)
        {
            File.Delete(file);
            //Debug.Log($"{file} is deleted.");
        }

        int equippableNum = 0;
        using (FileStream fs = new FileStream(Application.dataPath + equippableCSVPath, FileMode.Open, FileAccess.Read))
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

                    Weapon equippable = ScriptableObject.CreateInstance<Weapon>();
                    equippable.id                   = splitData[0];
                    equippable.iconId               = splitData[1];
                    equippable.prefabId             = splitData[2];

                    equippable.type                 = splitData[3];
                    equippable.kind                 = splitData[4];
                    equippable.name                 = splitData[5];

                    equippable.accur_Rate_Base      = int.Parse(splitData[6]);

                    equippable.min_damage           = int.Parse(splitData[7]);
                    equippable.max_damage           = int.Parse(splitData[8]);
                    equippable.crit_Damage          = int.Parse(splitData[9]);

                    equippable.bullet               = int.Parse(splitData[10]);
                    equippable.accur_Rate_Dec       = int.Parse(splitData[11]);
                    equippable.weight               = int.Parse(splitData[12]);

                    equippable.firstShot_Ap         = int.Parse(splitData[13]);
                    equippable.alertShot_Ap         = int.Parse(splitData[14]);
                    equippable.aimShot_Ap           = int.Parse(splitData[15]);
                    equippable.load_Ap              = int.Parse(splitData[16]);

                    equippable.range                = int.Parse(splitData[17]);
                    equippable.overRange_Penalty    = int.Parse(splitData[18]);
                    equippable.underRange_Penalty   = int.Parse(splitData[19]);

                    AssetDatabase.CreateAsset(equippable, $"Assets//Resources/Choi/Datas/Weapons/{equippable.name}.asset");
                    equippableNum++;
                }
                sr.Close();
            }
            fs.Close();
            AssetDatabase.SaveAssets();
            Debug.Log($"무기SO가 {equippableNum}개 생성되었습니다.");

        }
    }
}
