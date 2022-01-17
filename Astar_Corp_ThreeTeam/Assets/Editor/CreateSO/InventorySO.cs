using UnityEngine;
using UnityEditor;
using System.IO;

public class InventorySO
{
    private static string inventoryCSVPath = "/Resources/Choi/InventoryDataTable.csv";

    [MenuItem("Utilities/Generate Inventorys")]
    public static void GenerateCharacters()
    {
        string[] files = Directory.GetFiles("Assets/Resources/Choi/Datas/Inventorys");
        foreach (string file in files)
        {
            File.Delete(file);
            //Debug.Log($"{file} is deleted.");
        }

        int inventoryNum = 0;
        using (FileStream fs = new FileStream(Application.dataPath + inventoryCSVPath, FileMode.Open, FileAccess.Read))
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

                    Inventory inventory = ScriptableObject.CreateInstance<Inventory>();
                    inventory.id        = splitData[0];
                    inventory.name      = splitData[1];
                    inventory.level     = int.Parse(splitData[2]);
                    inventory.type      = int.Parse(splitData[3]);
                    inventory.weight    = int.Parse(splitData[4]);

                    AssetDatabase.CreateAsset(inventory, $"Assets//Resources/Choi/Datas/Inventorys/{inventory.name}.asset");
                    inventoryNum++;
                }
                sr.Close();
            }
            fs.Close();
            AssetDatabase.SaveAssets();
            Debug.Log($"인벤토리SO가 {inventoryNum}개 생성되었습니다.");
        }
    }
}