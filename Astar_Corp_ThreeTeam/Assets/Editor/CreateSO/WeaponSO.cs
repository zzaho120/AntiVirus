using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;

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

        // 딕셔너리 <string, List<string>> ItemDics
        // 무게 가격이 있는 아이템 테이블을 읽어서 대입
        // 키는 id, 밸류는 실제 값들
        Dictionary<string, List<string>> itemDics = new Dictionary<string, List<string>>();

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

                    equippable.accurRateBase        = int.Parse(splitData[6]);

                    equippable.minDamage            = int.Parse(splitData[7]);
                    equippable.maxDamage            = int.Parse(splitData[8]);
                    equippable.critRate             = int.Parse(splitData[9]);
                    equippable.critDamage           = int.Parse(splitData[10]);
                    
                    equippable.bullet               = int.Parse(splitData[11]);
                    equippable.mpPerAp              = int.Parse(splitData[12]);

                    equippable.firstShotAp          = int.Parse(splitData[13]);
                    equippable.otherShotAp          = int.Parse(splitData[14]);
                    equippable.loadAp               = int.Parse(splitData[15]);

                    equippable.minRange             = int.Parse(splitData[16]);
                    equippable.maxRange             = int.Parse(splitData[17]);
                    equippable.overRange_Penalty    = int.Parse(splitData[18]);
                    equippable.underRange_Penalty   = int.Parse(splitData[19]);

                    equippable.grade                = int.Parse(splitData[20]);
                    equippable.price                = int.Parse(splitData[21]);
                    equippable.weight               = int.Parse(splitData[22]);
                    equippable.value                = int.Parse(splitData[23]);
                    equippable.dropRate             = float.Parse(splitData[24]);
                    equippable.itemQuantity         = int.Parse(splitData[25]);
                    //equippable.img = Resources.Load<Sprite>($"Choi/Sprites/Images/Items/Weapons/{splitData[26]}");
                    equippable.img = Resources.Load($"Choi/Sprites/Images/Items/Equippables/{splitData[26]}", typeof(Sprite)) as Sprite;
                    equippable.bulletType = int.Parse(splitData[27]);
                    equippable.reloadBullet = int.Parse(splitData[28]);
                    equippable.continuousShootingPenalty = int.Parse(splitData[29]);
                    equippable.battleID = int.Parse(splitData[30]);

                    //// Item 정보 담고 있는 List
                    //List<string> itemInfo = new List<string>();
                    //itemInfo.Add(splitData[20]);    // Grade            // 등급
                    //itemInfo.Add(splitData[21]);    // Price            // 가격
                    //itemInfo.Add(splitData[22]);    // Weight           // 무게
                    //itemInfo.Add(splitData[23]);    // Value            // 밸류
                    //itemInfo.Add(splitData[24]);    // DropRate         // 드롭율
                    //itemInfo.Add(splitData[25]);    // itemQuantity     // 상점 내 아이템 개수
                    //
                    //// 테스트 출력
                    ////equippable.testList = itemInfo;
                    //
                    //// 딕셔너리에 추가
                    //var itemId = splitData[0];
                    //itemDics.Add(itemId, itemInfo);
                    //
                    //if (itemDics.ContainsKey(itemId))
                    //{
                    //    equippable.grade = int.Parse(itemDics[itemId][0]);
                    //    equippable.price = int.Parse(itemDics[itemId][1]);
                    //    equippable.weight = int.Parse(itemDics[itemId][2]);
                    //    equippable.value = int.Parse(itemDics[itemId][3]);
                    //    equippable.dropRate = float.Parse(itemDics[itemId][4]);
                    //    equippable.itemQuantity = int.Parse(itemDics[itemId][5]);
                    //}

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
