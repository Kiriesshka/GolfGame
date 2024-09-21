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
    public bool canTryPunch() => rb.linearVelocity.magnitude <= 0.2f ? true : false;
    private void Start()
    {

        txt.text = "Количество ударов: " + punchCount + "\n"+punchesForOneStar+"\n"+punchesForTwoStar+"\n"+punchesForThreeStar;
        startPos = transform.position;
        dS = new DataSaver();
        dS.fileName = "SAVE";
        dS.fileExtension = ".txt";
        Application.targetFrameRate = 120;
        state = "Start->INIT";
        rb = GetComponent<Rigidbody>();
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
        punchCount++;
        rb.AddForce(arrow.transform.right *punchForce, ForceMode.Impulse);
        txt.text = "Количество ударов: " + punchCount + "\n" + punchesForOneStar + "\n" + punchesForTwoStar + "\n" + punchesForThreeStar;
    }
    private void Update()
    {
        if (calculateDirection && !isInHole)
        {
            if (Input.touchCount > 0)
            {
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

        levelInfos[levelID-1].isCompleted = true;
        if (punchCount < punchesForThreeStar && levelInfos[levelID - 1].stars<=3) levelInfos[levelID-1].stars = 3;
        else if (punchCount < punchesForTwoStar && levelInfos[levelID - 1].stars<=2) levelInfos[levelID-1].stars = 2;
        else if (punchCount < punchesForOneStar && levelInfos[levelID - 1].stars <= 1) levelInfos[levelID-1].stars = 1;
        else levelInfos[levelID-1].stars = 0;

        endGameWindow.transform.GetChild(1).GetComponent<TMP_Text>().text = $"Статистика:\nКоличество ударов: {punchCount}\nКоличество звезд: {levelInfos[levelID - 1].stars}";
        dS.Clear();

        dS.Add("FIRST", levelInfos[0]);
        dS.Add("SECOND", levelInfos[1]);
        dS.Add("3rd", levelInfos[2]);

        dS.Save();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.tag == "Hole")
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
