using UnityEngine;
using UnityEngine.UI;
using KiriesshkaData;
using TMPro;
using System.Collections.Generic;
public class Graphics : MonoBehaviour
{
    private DataSaver dS;
    public string tier = "NORMAL";
    public TMP_Dropdown dropdown;
    public List<string> options = new List<string> { "LOW", "NORMAL", "HIGH" };
    private void Start()
    {
        dS = new DataSaver();
        dS.fileExtension = ".txt";
        dS.fileName = "GRAPHICS";
        if (dS.IsSaveFileExsists())
        {
            LoadData();
        }
        else
        {
            SaveData();
        }
    }
    public void LoadData()
    {
        dS.Clear();
        dS.Load();
        tier = dS.GetString("TIER");
        dropdown.value = options.IndexOf(tier);
    }
    public void SaveData()
    {
        dS.Clear();
        dS.Add("TIER", tier);
        dS.Save();
    }
    public void ChangeTier()
    {
        tier = options[dropdown.value];
        SaveData();
    }
}
