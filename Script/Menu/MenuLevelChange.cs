using UnityEngine;
using System.Collections.Generic;
using TMPro;
using KiriesshkaData;
using UnityEngine.UI;
public class MenuLevelChange : MonoBehaviour
{
    public List<LevelInfo> levelInfos;
    public GameObject levelCardPrefab;
    public RectTransform cardsAnchor;
    public List<GameObject> instantiated;
    public RectTransform canvas;
    public float currentIndex = 0;
    public RectTransform mover;

    private DataSaver dS;
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
        instantiated = new List<GameObject>();
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
            if (currentIndex > instantiated.Count - 1) currentIndex = instantiated.Count - 1;
        }
    }
    public void CreateCards()
    {
        foreach(GameObject a in instantiated)
        {
            Destroy(a);
        }
        instantiated = new List<GameObject>();
        foreach(LevelInfo lI in levelInfos)
        {
            GameObject card = Instantiate(levelCardPrefab, cardsAnchor);
            card.GetComponent<RectTransform>().localPosition = new Vector3(canvas.sizeDelta.x*levelInfos.IndexOf(lI), 0, 0);
            card.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("LEVEL_IMG_" +lI.id.ToString());
            card.transform.GetChild(0).GetChild(3).GetComponent<LoadScene>().sceneID = lI.id;
            card.transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<TMP_Text>().text = lI.levelName;
            for(int i = 0; i < lI.stars; i++)
            {
                if (i > 3) break;
                card.transform.GetChild(0).GetChild(2).GetChild(i).gameObject.SetActive(true);
            }
            instantiated.Add(card);
        }
    }
    public void DefaultSave()
    {
        dS.Clear();

        LevelInfo a = new LevelInfo();
        a.id = 1;
        a.levelName = "first";
        dS.Add("FIRST", a);

        LevelInfo b = new LevelInfo();
        b.id = 2;
        b.levelName = "second";
        dS.Add("SECOND", b);

        dS.Save();
    }
    public void Save()
    {
        dS.Clear();
        dS.Add("FIRST", levelInfos[0]);
        dS.Add("SECOND", levelInfos[1]);
        dS.Save();
    }
    public void Load()
    {
        dS.Load();
        levelInfos = new List<LevelInfo>();
        levelInfos.Add(dS.GetClass<LevelInfo>("FIRST"));
        levelInfos.Add(dS.GetClass<LevelInfo>("SECOND"));
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
