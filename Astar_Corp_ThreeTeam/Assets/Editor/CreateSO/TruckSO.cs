using UnityEngine;
using UnityEditor;
using System.IO;

public class TruckSO
{
    private static string truckCSVPath = "/Resources/Choi/TruckDataTable.csv";

    [MenuItem("Utilities/Generate Trucks")]
    public static void GenerateTrucks()
    {
        string[] files = Directory.GetFiles("Assets/Resources/Choi/Datas/Trucks");
        foreach (string file in files)
        {
            File.Delete(file);
            //Debug.Log($"{file} is deleted.");
        }

        int truckNum = 0;
        using (FileStream fs = new FileStream(Application.dataPath + truckCSVPath, FileMode.Open, FileAccess.Read))
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

                    Truck truck = ScriptableObject.CreateInstance<Truck>();
                    truck.id            = splitData[0];
                    truck.name          = splitData[1];

                    truck.capacity      = int.Parse(splitData[2]);

                    truck.speed         = int.Parse(splitData[3]);
                    truck.speed_Rise    = int.Parse(splitData[4]);
                    truck.speedUp_Cost  = int.Parse(splitData[5]);

                    truck.sight         = int.Parse(splitData[6]);
                    truck.sight_Rise    = int.Parse(splitData[7]);
                    truck.sightUp_Cost  = int.Parse(splitData[8]);

                    truck.weight         = int.Parse(splitData[9]);
                    truck.weight_Rise    = int.Parse(splitData[10]);
                    truck.weightUp_Cost  = int.Parse(splitData[11]);
                    
                    truck.price         = int.Parse(splitData[12]);

                    AssetDatabase.CreateAsset(truck, $"Assets//Resources/Choi/Datas/Trucks/{truck.name}.asset");
                    truckNum++;
                }
                sr.Close();
            }
            fs.Close();
            AssetDatabase.SaveAssets();
            Debug.Log($"트럭SO가 {truckNum}개 생성되었습니다.");
        }
    }
}
