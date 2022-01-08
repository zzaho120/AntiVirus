using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NonBattleMgr : MonoBehaviour
{
    public GameObject player;
    public Transform bunkerPos;
    public List<GameObject> randomLaboratory;
    public List<GameObject> laboratoryObjs;
    public List<GameObject> virusZones1;
    public WindowManager windowManager;

    //연구소 영역.
    float Zone2Magnifi;
    float Zone3Magnifi;
    string[] virusType = { "E", "B", "P", "I", "T" };

    //마크 관리.
    public List<Vector3> markList;

    PlayerController playerController;
    TimeController timeController;
    float timer;

    // Start is called before the first frame update
    private void Awake()
    {
        Zone2Magnifi = 2f;
        Zone3Magnifi = 4f;

        //처음 시작할때.
        if (!PlayerPrefs.HasKey("MonsterAreaX0"))
        {
            var randomVirusType = RandomVirusType();

            //예비 연구소.
            int randomLaboratoryIndex = UnityEngine.Random.Range(0, randomLaboratory.Count);
            randomLaboratory[randomLaboratoryIndex].SetActive(true);
            string laboratoryIndexStr = "randomLaboratoryIndex";
            PlayerPrefs.SetInt(laboratoryIndexStr, randomLaboratoryIndex);
            laboratoryObjs.Add(randomLaboratory[randomLaboratoryIndex]);

            //연구소 영역.
            int i = 0;
            foreach (var element in laboratoryObjs)
            {
                //맨밑이 제일 큼.
                var virusZone3 = element.transform.GetChild(1).gameObject;
                var virusZone2 = element.transform.GetChild(2).gameObject;
                var virusZone1 = element.transform.GetChild(3).gameObject;

                var script = virusZone1.GetComponent<LaboratoryInfo>();
                int randomNum = (!script.isSpareLab) ? 10 : 8;

                virusZone3.transform.localScale = new Vector3(randomNum, randomNum, randomNum);
                virusZone2.transform.localScale = new Vector3(randomNum * Zone2Magnifi, randomNum * Zone2Magnifi, randomNum * Zone2Magnifi);
                virusZone1.transform.localScale = new Vector3(randomNum * Zone3Magnifi, randomNum * Zone3Magnifi, randomNum * Zone3Magnifi);

                var radius = virusZone3.GetComponent<SphereCollider>().radius;
                script.radiusZone3 = radius * virusZone3.transform.lossyScale.x;
                script.radiusZone2 = radius * virusZone2.transform.lossyScale.x;
                script.radiusZone1 = radius * virusZone1.transform.lossyScale.x;

                string str = $"VirusZone1Scale{i}";
                PlayerPrefs.SetInt(str, randomNum);

                script.isActiveZone2 = (Random.Range(0, 2) == 0) ? true : false;
                if (!script.isActiveZone2) virusZone2.SetActive(false);

                if (script.isActiveZone2) script.isActiveZone3 = (Random.Range(0, 2) == 0) ? true : false;
                if (!script.isActiveZone3) virusZone3.SetActive(false);

                str = $"Laboratory{i}Zone2";
                int num = (script.isActiveZone2 == true) ? 1 : 0;
                PlayerPrefs.SetInt(str, num);
                str = $"Laboratory{i}Zone3";
                num = (script.isActiveZone3 == true) ? 1 : 0;
                PlayerPrefs.SetInt(str, num);

                script.virusType = randomVirusType[i];
                str = $"Laboratory{i}VirusType";
                PlayerPrefs.SetString(str, randomVirusType[i]);

                i++;
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

        playerController = player.GetComponent<PlayerController>();
        playerController.manager = this;

        timeController = GameObject.Find("TimeController").GetComponent<TimeController>();
        playerController.timeController = timeController;


        //마크 관리.
        markList = new List<Vector3>();
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

    public void MoveToBunker()
    {
        SceneManager.LoadScene("Bunker");
    }

    public void OpenBunkerPopup()
    {
        var windowId = (int)Windows.BunkerWindow - 1;
        var nonBattlePopUps = windowManager.Open(windowId, false) as NonBattlePopUps;
    }

    public void CloseBunkerPopup()
    {
        var windowId = (int)Windows.BunkerWindow - 1;
        windowManager.windows[windowId].Close();
    }

    public void OpenLaboratoryPopup()
    {
        var windowId = (int)Windows.LaboratoryWindow - 1;
        var nonBattlePopUps = windowManager.Open(windowId, false) as NonBattlePopUps;
    }

    public void CloseLaboratoryPopup()
    {
        var windowId = (int)Windows.LaboratoryWindow - 1;
        windowManager.windows[windowId].Close();
    }

    public void OpenMonsterPopup()
    {
        var windowId = (int)Windows.MonsterWindow - 1;
        var nonBattlePopUps = windowManager.Open(windowId, false) as NonBattlePopUps;
    }

    public void Restart()
    {
        PlayerPrefs.DeleteAll();
    }
}