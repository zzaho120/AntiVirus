using UnityEngine;
using UnityEditor;
using System.IO;

public class ConsumableSO 
{
    private static string consumableCSVPath = "/Resources/Choi/ConsumDataTable.csv";

    [MenuItem("Utilities/Generate Consumables")]
    public static void GenerateConsumables()
    {
        string[] files = Directory.GetFiles("Assets/Resources/Choi/Datas/Consumables");
        foreach (string file in files)
        {
            File.Delete(file);
            //Debug.Log($"{file} is deleted.");
        }

        int itemNum = 0;
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
                    consumable.id            = splitData[0];
                    consumable.iconId        = splitData[1];
                    consumable.prefabsId     = splitData[2];
                    consumable.name          = splitData[3];
                                             
                    consumable.description   = splitData[4];
                    consumable.type          = splitData[5];
                                             
                    consumable.ap            = int.Parse(splitData[6]);
                    consumable.hpRecovery    = int.Parse(splitData[7]);
                    consumable.virusGaugeDec = int.Parse(splitData[8]);

                    // 나중에 아이템데이터테이블 딕셔너리로 수정
                    consumable.grade         = int.Parse(splitData[9]);
                    consumable.price         = int.Parse(splitData[10]);
                    consumable.weight        = int.Parse(splitData[11]);
                    consumable.value         = int.Parse(splitData[12]);
                    consumable.dropRate      = float.Parse(splitData[13]);
                    consumable.itemQuantity  = int.Parse(splitData[14]);

                    AssetDatabase.CreateAsset(consumable, $"Assets//Resources/Choi/Datas/Consumables/{consumable.name}.asset");
                    itemNum++;
                }
                sr.Close();
            }
            fs.Close();
            AssetDatabase.SaveAssets();
            Debug.Log($"아이템SO가 {itemNum}개 생성되었습니다.");
        }
    }
}
