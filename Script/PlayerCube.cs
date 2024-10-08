using UnityEngine;
using KiriesshkaData;
using System.Collections.Generic;
using TMPro;
public class PlayerCube : MonoBehaviour
{
    public string state;
    public Camera camForWTS;
    public Transform arrow;
    public Vector3 direction;
    public bool calculateDirection;
    private Rigidbody rb;
    public float punchForce;
    public OrbitCamera Oc;
    public Color startColor;
    public Color endColor;
    public bool isInHole;
    public int levelID;
    public int punchCount;
    public int punchesForOneStar;
    public int punchesForTwoStar;
    public int punchesForThreeStar;
    public TMP_Text txt;
    public GameObject endGameWindow;
    private List<LevelInfo> levelInfos;
    private DataSaver dS;
    private Vector3 startPos;
    private GameSound gS;

    public DataSaver dSkins;
    public List<GameObject> skins;
    public bool canTryPunch() => rb.linearVelocity.magnitude <= 0.2f ? true : false;
    private void Start()
    {
        gS = GetComponent<GameSound>();
        txt.text = "���������� ������: " + punchCount + "\n"+punchesForOneStar+"\n"+punchesForTwoStar+"\n"+punchesForThreeStar;
        startPos = transform.position;
        dS = new DataSaver();
        dS.fileName = "SAVE";
        dS.fileExtension = ".txt";
        Application.targetFrameRate = 120;
        state = "Start->INIT";
        rb = GetComponent<Rigidbody>();
        dSkins = new DataSaver();
        dSkins.fileExtension = ".txt";
        dSkins.fileName = "SKINS";
        dSkins.Load();
        GameObject tmp = Instantiate(skins[dSkins.GetInt("CURRENT")], transform);
        tmp.transform.localScale = new Vector3(1, 1, 1);
    }
    public void ShowArrow()
    {
        arrow.gameObject.SetActive(true);
        Oc.enabled = false;
    }
    public void CloseArrow()
    {
        arrow.gameObject.SetActive(false);
        Oc.enabled = true;
    }
    public void Punch()
    {
        gS.channelsVolumeSettings[1] = punchForce / 10;
        gS.MakeSound("HIT", "FX");
        punchCount++;
        rb.AddForce(arrow.transform.right *punchForce, ForceMode.Impulse);
        txt.text = "���������� ������: " + punchCount + "\n" + punchesForOneStar + "\n" + punchesForTwoStar + "\n" + punchesForThreeStar;
    }
    private void Update()
    {
        if (calculateDirection && !isInHole)
        {
            if (Input.touchCount > 0)
            {
                rb.angularVelocity = Vector3.zero;
                Touch t = Input.GetTouch(0);
                Vector2 pointOnScreen = camForWTS.WorldToScreenPoint(transform.position);
                Vector2 pointerPosition = t.position;
                direction = (pointOnScreen - pointerPosition);
                Vector3 rotation = new Vector3(0, -Mathf.Rad2Deg*(Mathf.Atan2(direction.y, direction.x)) + camForWTS.transform.eulerAngles.y, 0);
                arrow.transform.rotation = Quaternion.Euler(rotation);
                punchForce = direction.magnitude/Screen.height*30;
                arrow.GetChild(0).GetComponent<Renderer>().material.color = Color.Lerp(startColor, endColor, direction.magnitude/Screen.height*2);
                arrow.GetChild(0).transform.localScale = new Vector3(45, 45, 220 *(0.2f+ direction.magnitude / Screen.height));
                arrow.GetChild(0).transform.localPosition = new Vector3(-arrow.GetChild(0).transform.localScale.z/40 / 2, 0, 0);
            }
            else
            {
                Punch();
                calculateDirection = false;
                CloseArrow();
            }
        }
    }
    private void OnMouseDown()
    {
        if (canTryPunch())
        {
            startPos = transform.position;
            calculateDirection = true;
            ShowArrow();
        }
    }
    public void EndGame()
    {
        dS = new DataSaver();
        dS.fileName = "SAVE";
        dS.fileExtension = ".txt";
        endGameWindow.SetActive(true);
      
        txt.gameObject.SetActive(false);
        dS.Load();
        levelInfos = new List<LevelInfo>();

        levelInfos.Add(dS.GetClass<LevelInfo>("FIRST"));
        levelInfos.Add(dS.GetClass<LevelInfo>("SECOND"));
        levelInfos.Add(dS.GetClass<LevelInfo>("3rd"));
        levelInfos.Add(dS.GetClass<LevelInfo>("4th"));


        levelInfos[levelID-1].isCompleted = true;
        int newStars = 0;
        if (punchCount <= punchesForThreeStar)
        {
            newStars = 3;
        }
        else if (punchCount <= punchesForTwoStar)
        {
            newStars = 2;
        }
        else if (punchCount <= punchesForOneStar)
        {
            newStars = 1;
        }

        //endGameWindow.transform.GetChild(1).GetComponent<TMP_Text>().text = $"����������:\n���������� ������: {punchCount}\n���������� �����: {newStars}";
        if(newStars >= 1)
        {
            endGameWindow.transform.GetChild(0).GetChild(0).GetChild(0).gameObject.AddComponent<Grower>().timeBeforeGrow = 0f;
        }
        if(newStars >= 2)
        {
            endGameWindow.transform.GetChild(0).GetChild(1).GetChild(0).gameObject.AddComponent<Grower>().timeBeforeGrow = 0.5f;
        }
        if(newStars >= 3)
        {
            endGameWindow.transform.GetChild(0).GetChild(2).GetChild(0).gameObject.AddComponent<Grower>().timeBeforeGrow = 1f;
        }
        if (newStars < levelInfos[levelID - 1].stars) newStars = levelInfos[levelID - 1].stars;
        levelInfos[levelID - 1].stars = newStars;

        int stars = dS.GetInt("STARS");

        dS.Clear();

        dS.Add("STARS", stars);
        dS.Add("FIRST", levelInfos[0]);
        dS.Add("SECOND", levelInfos[1]);
        dS.Add("3rd", levelInfos[2]);
        dS.Add("4th", levelInfos[3]);

        dS.Save();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.tag == "GRASS" || collision.transform.tag == "WOOD" || collision.transform.tag == "STONE" || collision.transform.tag == "LEAVES")
        {
            float soundVolume = rb.linearVelocity.magnitude / 10;
            if (soundVolume > 1) soundVolume = 1;
            gS.channelsVolumeSettings[1] = soundVolume;
            gS.MakeSound(collision.transform.tag, "FX");
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "GRASS" || other.transform.tag == "WOOD" || other.transform.tag == "STONE" || other.transform.tag == "LEAVES")
        {
            float soundVolume = rb.linearVelocity.magnitude / 10;
            if (soundVolume > 1) soundVolume = 1;
            gS.channelsVolumeSettings[1] = soundVolume;
            gS.MakeSound(other.transform.tag, "FX");
        }
        if (other.transform.tag == "Hole")
        {
            EndGame();
            isInHole = true;
        }
        if (other.transform.tag == "OOB")
        {
            transform.position = startPos;
            rb.angularVelocity = Vector3.zero;
            rb.linearVelocity = Vector3.zero;
        }
    }
}
public class Grower : MonoBehaviour
{
    public float timeBeforeGrow = 0;
    private void Update()
    {
        if(timeBeforeGrow > 0)
        {
            timeBeforeGrow -= Time.deltaTime;
        }
        else
        {
            GetComponent<RectTransform>().sizeDelta = Vector2.Lerp(GetComponent<RectTransform>().sizeDelta, transform.parent.GetComponent<RectTransform>().sizeDelta, Time.deltaTime * 5);
            if (GetComponent<RectTransform>().sizeDelta == transform.parent.GetComponent<RectTransform>().sizeDelta) Destroy(this);
        }
    }
}
