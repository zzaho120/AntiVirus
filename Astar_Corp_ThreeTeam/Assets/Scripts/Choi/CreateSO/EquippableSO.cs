using UnityEngine;
using UnityEditor;
using System.IO;

public class EquippableSO 
{
    private static string equippableCSVPath = "/Resources/Choi/EquipDataTable.csv";

    [MenuItem("Utilities/Generate Equippables")]
    public static void GenerateEquippables()
    {
        string[] files = Directory.GetFiles("Assets/Resources/Choi/Equippables");
        foreach (string file in files)
        {
            File.Delete(file);
            //Debug.Log($"{file} is deleted.");
        }

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

                    Equippable equippable = ScriptableObject.CreateInstance<Equippable>();
                    equippable.id = splitData[0];
                    equippable.iconId = splitData[1];
                    equippable.prefabId = splitData[2];
                    equippable.name = splitData[3];
                    equippable.description = splitData[4];
                    equippable.type = splitData[5];
                    equippable.damage = int.Parse(splitData[6]);
                    equippable.cost = int.Parse(splitData[7]);
                    equippable.critRate = int.Parse(splitData[8]);
                    equippable.critDamage = int.Parse(splitData[9]);
                    equippable.weight = int.Parse(splitData[10]);
                    equippable.str = int.Parse(splitData[11]);
                    equippable.con = int.Parse(splitData[12]);
                    equippable.intellet = int.Parse(splitData[13]);
                    equippable.luck = int.Parse(splitData[14]);

                    AssetDatabase.CreateAsset(equippable, $"Assets//Resources/Choi/Equippables/{equippable.name}.asset");
                }
                sr.Close();
            }
            fs.Close();
            AssetDatabase.SaveAssets();
        }
    }
}
