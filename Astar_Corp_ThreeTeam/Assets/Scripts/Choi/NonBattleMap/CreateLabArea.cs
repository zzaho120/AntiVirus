using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateLabArea : MonoBehaviour
{
    // 바이러스 영역
    public List<GameObject> virusZones;

    // 각 연구소별 제일 큰 바이러스 영역
    // 몬스터 영역 생성할 때 사용
    public List<GameObject> maxVirusZone;

    //연구소 영역.
    public List<GameObject> randomLaboratory;
    public List<GameObject> laboratoryObjs;

    public float Zone2Magnifi = 2f;
    public float Zone3Magnifi = 4f;

    // 바이러스들
    string[] virusType = { "E", "B", "P", "I", "T" };

    public void Init()
    {
        //처음 시작할때.
        if (!PlayerPrefs.HasKey("MonsterAreaX0"))
        {
            var randomVirusType = RandomVirusType();

            //예비 연구소.
            int randomLaboratoryIndex = Random.Range(0, randomLaboratory.Count);
            randomLaboratory[randomLaboratoryIndex].SetActive(true);
            string laboratoryIndexStr = "randomLaboratoryIndex";
            PlayerPrefs.SetInt(laboratoryIndexStr, randomLaboratoryIndex);
            laboratoryObjs.Add(randomLaboratory[randomLaboratoryIndex].transform.GetChild(0).gameObject);

            //연구소 영역.
            int i = 0;
            foreach (var element in laboratoryObjs)
            {
                //맨밑이 제일 큼.
                var virusZone1 = element.transform.GetChild(0).gameObject; 
                var virusZone2 = element.transform.GetChild(1).gameObject; 
                var virusZone3 = element.transform.GetChild(2).gameObject; 

                virusZones.Add(virusZone1);
                virusZones.Add(virusZone2);
                virusZones.Add(virusZone3);

                //var labInfo = virusZone3.GetComponent<LaboratoryInfo>();
                //int randomNum = (!labInfo.isSpareLab) ? 10 : 8;
                var labInfo = element.GetComponent<LaboratoryInfo>();
                int randomNum = (!labInfo.isSpareLab) ? 1 : 1;

                //virusZone3.transform.localScale = new Vector3(randomNum, randomNum, randomNum);
                //virusZone2.transform.localScale = new Vector3(randomNum * Zone2Magnifi, randomNum * Zone2Magnifi, randomNum * Zone2Magnifi);
                //virusZone1.transform.localScale = new Vector3(randomNum * Zone3Magnifi, randomNum * Zone3Magnifi, randomNum * Zone3Magnifi);

                // SphereCoiilder 어디서 쓰는지 알아보고 수정
                var radius = virusZone1.GetComponent<SphereCollider>().radius;
                labInfo.radiusZone1 = radius * virusZone1.transform.lossyScale.x; 
                labInfo.radiusZone2 = radius * virusZone2.transform.lossyScale.x; 
                labInfo.radiusZone3 = radius * virusZone3.transform.lossyScale.x;  

                // 저장되는 부분 나중에 수정하기
                string str = $"VirusZone1Scale{i}";
                PlayerPrefs.SetInt(str, randomNum);

                // Zone2 : 60% 확률로 활성화
                labInfo.isActiveZone2 = (Random.Range(0, 3) <= 1) ? true : false;
                if (!labInfo.isActiveZone2) virusZone2.SetActive(false);
                // Zone3 (큰원) : 50% 확률로 활성화
                if (labInfo.isActiveZone2) labInfo.isActiveZone3 = (Random.Range(0, 2) == 0) ? true : false;
                if (!labInfo.isActiveZone3) virusZone3.SetActive(false);

                // 리스트에 바이러스 영역 추가 (제일 큰 애만)
                if (labInfo.isActiveZone3) {
                    maxVirusZone.Add(virusZone3); 
                }
                else if (labInfo.isActiveZone2) {
                    maxVirusZone.Add(virusZone2); 
                }
                else {
                    maxVirusZone.Add(virusZone1);
                }

                str = $"Laboratory{i}Zone2";
                int num = (labInfo.isActiveZone2 == true) ? 1 : 0;
                PlayerPrefs.SetInt(str, num);
                str = $"Laboratory{i}Zone3";
                num = (labInfo.isActiveZone3 == true) ? 1 : 0;
                PlayerPrefs.SetInt(str, num);

                labInfo.virusType = randomVirusType[i];
                str = $"Laboratory{i}VirusType";
                PlayerPrefs.SetString(str, randomVirusType[i]);

                i++;


                //원본코드

                ////맨밑이 제일 큼.
                //var virusZone3 = element.transform.GetChild(1).gameObject;  // 렙3
                //var virusZone2 = element.transform.GetChild(2).gameObject;  // 렙2
                //var virusZone1 = element.transform.GetChild(3).gameObject;  // 렙1
                //
                //var labInfo = virusZone1.GetComponent<LaboratoryInfo>();
                //int randomNum = (!labInfo.isSpareLab) ? 10 : 8;
                //
                //-virusZone3.transform.localScale = new Vector3(randomNum, randomNum, randomNum);
                //-virusZone2.transform.localScale = new Vector3(randomNum * Zone2Magnifi, randomNum * Zone2Magnifi, randomNum * Zone2Magnifi);
                //-virusZone1.transform.localScale = new Vector3(randomNum * Zone3Magnifi, randomNum * Zone3Magnifi, randomNum * Zone3Magnifi);
                //
                //var radius = virusZone3.GetComponent<SphereCollider>().radius;
                //labInfo.radiusZone3 = radius * virusZone3.transform.lossyScale.x;
                //labInfo.radiusZone2 = radius * virusZone2.transform.lossyScale.x;
                //labInfo.radiusZone1 = radius * virusZone1.transform.lossyScale.x;
                //
                //// 저장되는 부분 나중에 수정하기
                //string str = $"VirusZone1Scale{i}";
                //PlayerPrefs.SetInt(str, randomNum);
                //
                //labInfo.isActiveZone2 = (Random.Range(0, 2) == 0) ? true : false;
                //if (!labInfo.isActiveZone2) virusZone2.SetActive(false);
                //
                //if (labInfo.isActiveZone2) labInfo.isActiveZone3 = (Random.Range(0, 2) == 0) ? true : false;
                //if (!labInfo.isActiveZone3) virusZone3.SetActive(false);
                //
                //str = $"Laboratory{i}Zone2";
                //int num = (labInfo.isActiveZone2 == true) ? 1 : 0;
                //PlayerPrefs.SetInt(str, num);
                //str = $"Laboratory{i}Zone3";
                //num = (labInfo.isActiveZone3 == true) ? 1 : 0;
                //PlayerPrefs.SetInt(str, num);
                //
                //labInfo.virusType = randomVirusType[i];
                //str = $"Laboratory{i}VirusType";
                //PlayerPrefs.SetString(str, randomVirusType[i]);
                //
                //i++;
            }
        }
        else//이어하기.
        {
            string laboratoryIndexStr = "randomLaboratoryIndex";
            int randomLaboratoryIndex = PlayerPrefs.GetInt(laboratoryIndexStr);
            randomLaboratory[randomLaboratoryIndex].SetActive(true);
            laboratoryObjs.Add(randomLaboratory[randomLaboratoryIndex]);

            int i = 0;
            foreach (var element in laboratoryObjs)
            {
                var virusZone3 = element.transform.GetChild(1).gameObject;
                var virusZone2 = element.transform.GetChild(2).gameObject;
                var virusZone1 = element.transform.GetChild(3).gameObject;

                string str = $"VirusZone1Scale{i}";
                int randomNum = PlayerPrefs.GetInt(str);

                var script = virusZone1.GetComponent<LaboratoryInfo>();
                var radius = virusZone3.GetComponent<SphereCollider>().radius;

                virusZone3.transform.localScale = new Vector3(randomNum, randomNum, randomNum);
                virusZone2.transform.localScale = new Vector3(randomNum * Zone2Magnifi, randomNum * Zone2Magnifi, randomNum * Zone2Magnifi);
                virusZone1.transform.localScale = new Vector3(randomNum * Zone3Magnifi, randomNum * Zone3Magnifi, randomNum * Zone3Magnifi);

                script.radiusZone3 = radius * virusZone3.transform.lossyScale.x;
                script.radiusZone2 = radius * virusZone2.transform.lossyScale.x;
                script.radiusZone1 = radius * virusZone1.transform.lossyScale.x;

                str = $"Laboratory{i}Zone2";
                int num = PlayerPrefs.GetInt(str);
                if (num == 0) virusZone2.SetActive(false);
                else script.isActiveZone2 = true;

                str = $"Laboratory{i}Zone3";
                num = PlayerPrefs.GetInt(str);
                if (num == 0) virusZone3.SetActive(false);
                else script.isActiveZone3 = true;

                str = $"Laboratory{i}VirusType";
                var virusType = PlayerPrefs.GetString(str);
                script.virusType = virusType;

                i++;
            }
        }
    }

    string[] RandomVirusType()
    {
        var randomVirusType = virusType;
        for (int i = 0; i < 50; i++)
        {
            var random1 = UnityEngine.Random.Range(0, virusType.Length);
            var random2 = UnityEngine.Random.Range(0, virusType.Length);
            var temp = randomVirusType[random1];
            randomVirusType[random1] = randomVirusType[random2];
            randomVirusType[random2] = temp;
        }

        return randomVirusType;
    }
}
