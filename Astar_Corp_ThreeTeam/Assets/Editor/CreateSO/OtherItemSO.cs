using UnityEngine;
using UnityEditor;
using System.IO;

public class OtherItemSO
{
    private static string otherItemCSVPath = "/Resources/Choi/OtherItemDataTable.csv";

    [MenuItem("Utilities/Generate Other Items")]
    public static void GenerateCharacters()
    {
        string[] files = Directory.GetFiles("Assets/Resources/Choi/Datas/OtherItems");
        foreach (string file in files)
        {
            File.Delete(file);
            //Debug.Log($"{file} is deleted.");
        }

        int itemSkillNum = 0;
        using (FileStream fs = new FileStream(Application.dataPath + otherItemCSVPath, FileMode.Open, FileAccess.Read))
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

                    OtherItem otherItems = ScriptableObject.CreateInstance<OtherItem>();
                    otherItems.id           = splitData[0];
                    otherItems.name         = splitData[1];

                    otherItems.grade        = splitData[2];
                    otherItems.price        = splitData[3];
                    otherItems.weight       = splitData[4];
                    otherItems.dropRate     = splitData[5];
                    otherItems.itemQuantity = splitData[6];
                    otherItems.img = Resources.Load($"Choi/Sprites/Images/Items/Consumables/{splitData[7]}", typeof(Sprite)) as Sprite;

                    AssetDatabase.CreateAsset(otherItems, $"Assets//Resources/Choi/Datas/OtherItems/{otherItems.name}.asset");
                    itemSkillNum++;
                }
                sr.Close();
            }
            fs.Close();
            AssetDatabase.SaveAssets();
            Debug.Log($"기타 아이템SO가 {itemSkillNum}개 생성되었습니다.");
        }
    }
}
