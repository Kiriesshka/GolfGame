using UnityEngine;
using System.Collections.Generic;
using KiriesshkaData;
using TMPro;
using UnityEngine.UI;
public class SkinChanger : MonoBehaviour
{
    public MenuLevelChange mlc;
    public DataSaver dS;
    public List<SkinData> skindatas;
    public List<GameObject> skins;

    public List<GameObject> instantiatedSkins;

    public float currentSkin;

    public Button playButton;
    public GameObject watchAdButton;

    public GameObject star;
    public TMP_Text starText;

    public void PlusSkin()
    {
        currentSkin++;
        if (currentSkin > skindatas.Count - 1) currentSkin = skindatas.Count - 1;
        SaveSkinData();
    }
    public void MinusSkin()
    {
        currentSkin--;
        if (currentSkin < 0) currentSkin = 0;
        SaveSkinData();
    }
    private void Start()
    {
        instantiatedSkins = new List<GameObject>();
        dS = new DataSaver();
        dS.fileName = "SKINS";
        dS.fileExtension = ".txt";
        if (dS.IsSaveFileExsists())
        {
            LoadSkinData();
        }
        else DefaultSave();
        foreach (GameObject skin in skins)
        {
            GameObject skinInstance = Instantiate(skin, new Vector3(5*skins.IndexOf(skin), -2.08f, -1.82f), Quaternion.identity);
            instantiatedSkins.Add(skinInstance);
            skinInstance.AddComponent<PlatformRotator>();
            skinInstance.GetComponent<PlatformRotator>().rotateSpeed = new Vector3(0, 100, 0);
        }
    }
    private void Update()
    {
        foreach(GameObject skin in instantiatedSkins)
        {
            skin.transform.position = Vector3.Lerp(skin.transform.position, new Vector3(5 * (instantiatedSkins.IndexOf(skin)-currentSkin), -2.08f, -1.82f), Time.deltaTime*5);
        }
        if (skindatas[Mathf.RoundToInt(currentSkin)].isOwned)
        {
            playButton.interactable = true;
            star.SetActive(false);
        }
        else
        {
            if(skindatas[Mathf.RoundToInt(currentSkin)].starsForBuying <= mlc.starCount)
            {
                skindatas[Mathf.RoundToInt(currentSkin)].isOwned = true;
                SaveSkinData();
            }
            else
            {
                star.SetActive(true);
                starText.text = skindatas[Mathf.RoundToInt(currentSkin)].starsForBuying.ToString();
            }
            playButton.interactable = false;
        }
    }
    public void LoadSkinData()
    {
        dS.Clear();
        dS.Load();

        skindatas = new List<SkinData>();
        currentSkin = dS.GetInt("CURRENT");
        skindatas.Add(dS.GetClass<SkinData>("DEFAULT"));
        skindatas.Add(dS.GetClass<SkinData>("RED"));
        skindatas.Add(dS.GetClass<SkinData>("BLUE"));
        skindatas.Add(dS.GetClass<SkinData>("PURPLE"));
        skindatas.Add(dS.GetClass<SkinData>("index0"));
    }
    public void SaveSkinData()
    {
        dS.Clear();
        if (skindatas[Mathf.RoundToInt(currentSkin)].isOwned) dS.Add("CURRENT", Mathf.RoundToInt(currentSkin));
        else
        {
            dS.Add("CURRENT", 0);
        }
        dS.Add("DEFAULT", skindatas[0]);
        dS.Add("RED", skindatas[1]);
        dS.Add("BLUE", skindatas[2]);
        dS.Add("PURPLE", skindatas[3]);
        dS.Add("index0", skindatas[4]);

        dS.Save();
    }
    public void DefaultSave()
    {
        currentSkin = 0;
        skindatas = new List<SkinData>();

        SkinData a = new SkinData();
        a.isOwned = true;
        a.name = "Классика";
        skindatas.Add(a);

        SkinData b = new SkinData();
        b.isOwned = false;
        b.starsForBuying = 1;
        b.name = "Красный";
        skindatas.Add(b);

        SkinData c = new SkinData();
        c.isOwned = false;
        c.starsForBuying = 2;
        c.name = "Синий";
        skindatas.Add(c);

        SkinData d = new SkinData();
        d.isOwned = false;
        d.starsForBuying = 3;
        d.name = "Фиолетовый";
        skindatas.Add(d);

        SkinData e = new SkinData();
        e.isOwned = false;
        e.starsForBuying = 6;
        e.name = "index0";
        skindatas.Add(e);

        SaveSkinData();
    }
}
public class SkinData
{
    public bool isOwned;
    public string name;
    public int starsForBuying;
    public int adsForBuying;
}
