using UnityEngine;
using UnityEditor;
using System.IO;

public class ConsumableSO 
{
    private static string consumableCSVPath = "/Resources/Choi/ConsumDataTable.csv";

    [MenuItem("Utilities/Generate Consumables")]
    public static void GenerateConsumables()
    {
        string[] files = Directory.GetFiles("Assets/Resources/Choi/Consumables");
        foreach (string file in files)
        {
            File.Delete(file);
            //Debug.Log($"{file} is deleted.");
        }

        using (FileStream fs = new FileStream(Application.dataPath + consumableCSVPath, FileMode.Open, FileAccess.Read))
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

                    Consumable consumable = ScriptableObject.CreateInstance<Consumable>();
                    consumable.id = splitData[0];
                    consumable.iconId = splitData[1];
                    consumable.prefabsId = splitData[2];
                    consumable.name = splitData[3];
                    consumable.description = splitData[4];
                    consumable.type = splitData[5];
                    consumable.weight = int.Parse(splitData[6]);
                    consumable.cost = int.Parse(splitData[7]);
                    consumable.hp = int.Parse(splitData[8]);
                    consumable.mp = int.Parse(splitData[9]);
                    consumable.str = int.Parse(splitData[10]);
                    consumable.duration = int.Parse(splitData[11]);

                    AssetDatabase.CreateAsset(consumable, $"Assets//Resources/Choi/Consumables/{consumable.name}.asset");
                }
                sr.Close();
            }
            fs.Close();
            AssetDatabase.SaveAssets();
        }
    }
}
