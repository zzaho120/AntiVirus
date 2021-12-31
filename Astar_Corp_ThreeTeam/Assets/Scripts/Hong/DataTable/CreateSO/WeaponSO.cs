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
                    equippable.accur_Rate_Alert     = int.Parse(splitData[7]);

                    equippable.unstability          = int.Parse(splitData[8]);
                    equippable.min_damage           = int.Parse(splitData[9]);
                    equippable.max_damage           = int.Parse(splitData[10]);
                    equippable.crit_Damage          = int.Parse(splitData[11]);

                    equippable.bullet               = int.Parse(splitData[12]);
                    equippable.recoil               = int.Parse(splitData[13]);
                    equippable.accur_Rate_Dec       = int.Parse(splitData[14]);
                    equippable.weight               = int.Parse(splitData[15]);

                    equippable.firstAp              = int.Parse(splitData[16]);
                    equippable.nextAp               = int.Parse(splitData[17]);
                    equippable.loadAp               = int.Parse(splitData[18]);

                    equippable.range                = int.Parse(splitData[19]);
                    equippable.overRange_Penalty    = int.Parse(splitData[20]);
                    equippable.underRange_Penalty   = int.Parse(splitData[21]);

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
