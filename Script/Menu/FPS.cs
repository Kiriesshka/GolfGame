using UnityEngine;
using TMPro;
public class FPS : MonoBehaviour
{
    public int targetFrameRate = 120;
    private float localTime;
    private int frameCount;
    public TMP_Text txt;
    void Start()
    {
        Application.targetFrameRate = 120;
    }

    void Update()
    {
        frameCount += 1;
        localTime += Time.deltaTime;
        if(localTime >= 1)
        {
            localTime = 0;
            txt.text = "FPS: " + frameCount;
            frameCount = 0;
        }
    }
}
