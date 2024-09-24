using UnityEngine;
using System.Collections.Generic;
using TMPro;
using KiriesshkaData;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class MenuLevelChange : MonoBehaviour
{
    public List<LevelInfo> levelInfos;
    public List<GameObject> cards;
    public RectTransform canvas;
    public float currentIndex = 0;
    public RectTransform mover;

    private DataSaver dS;
    public int starCount;
    private void Start()
    {
        Application.targetFrameRate = 120;
        dS = new DataSaver();
        dS.fileName = "SAVE";
        dS.fileExtension = ".txt";
        if (dS.IsSaveFileExsists())
        {
            Load();
        }
        else
        {
            DefaultSave();
            Load();
        }
        starCount = 0;
        for (int i = 0; i < levelInfos.Count; i++)
        {
            starCount += levelInfos[i].stars;
        }
        CreateCards();
    }
    private void Update()
    {
        mover.position = Vector3.Lerp(mover.position, new Vector3(-currentIndex * Screen.width + Screen.width / 2, Screen.height / 2, 0), Time.deltaTime*10) ;
        if (Input.touchCount > 0)
        {
            Touch t = Input.GetTouch(0);
            currentIndex -= t.deltaPosition.x * 0.001f;
        }
        else
        {
            currentIndex = Mathf.RoundToInt(currentIndex);
            if (currentIndex < 0) currentIndex = 0;
            if (currentIndex > cards.Count - 1) currentIndex = cards.Count - 1;
        }
    }
    public void Play()
    {
        SceneManager.LoadScene(Mathf.RoundToInt(currentIndex)+1);
    }
    public void CreateCards()
    {
        foreach (LevelInfo lI in levelInfos)
        {
            cards[levelInfos.IndexOf(lI)].GetComponent<RectTransform>().localPosition = new Vector3(canvas.sizeDelta.x * levelInfos.IndexOf(lI), 0, 0);
            cards[levelInfos.IndexOf(lI)].GetComponent<RectTransform>().sizeDelta = canvas.sizeDelta;
            cards[levelInfos.IndexOf(lI)].transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>().text = lI.levelName; // sprite = Resources.Load<Sprite>("LEVEL_IMG_" + lI.id.ToString());
            //cards[levelInfos.IndexOf(lI)].transform.GetChild(0).GetChild(3).GetComponent<LoadScene>().sceneID = lI.id;
            //cards[levelInfos.IndexOf(lI)].transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<TMP_Text>().text = lI.levelName;
            for (int i = 0; i < lI.stars; i++)
            {
                cards[levelInfos.IndexOf(lI)].transform.GetChild(0).GetChild(1).GetChild(i).GetChild(0).gameObject.SetActive(true);
            }
        }
    }
    public void DefaultSave()
    {
        dS.Clear();

        dS.Add("STARS", 0);

        LevelInfo a = new LevelInfo();
        a.id = 1;
        a.levelName = "Угол";
        dS.Add("FIRST", a);

        LevelInfo b = new LevelInfo();
        b.id = 2;
        b.levelName = "Рампа";
        dS.Add("SECOND", b);

        LevelInfo c = new LevelInfo();
        c.id = 3;
        c.levelName = "Наклон";
        dS.Add("3rd", c);

        LevelInfo d = new LevelInfo();
        d.id = 3;
        d.levelName = "Подъемник";
        dS.Add("4th", d);

        dS.Save();
    }
    public void Save()
    {
        dS.Clear();
        dS.Add("FIRST", levelInfos[0]);
        dS.Add("SECOND", levelInfos[1]);
        dS.Add("3rd", levelInfos[2]);
        dS.Add("4th", levelInfos[3]);

        dS.Add("STARS", starCount);
        dS.Save();
    }
    public void Load()
    {
        dS.Load();
        levelInfos = new List<LevelInfo>();
        levelInfos.Add(dS.GetClass<LevelInfo>("FIRST"));
        levelInfos.Add(dS.GetClass<LevelInfo>("SECOND"));
        levelInfos.Add(dS.GetClass<LevelInfo>("3rd"));
        levelInfos.Add(dS.GetClass<LevelInfo>("4th"));
    }
}
public class LevelInfo
{
    public int id;
    public string levelName;
    public bool isCompleted;
    public int stars;
}
public class SaveData
{
    public List<LevelInfo> lifs;
}
