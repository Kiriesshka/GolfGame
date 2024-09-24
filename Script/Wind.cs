using UnityEngine;
using System.Collections.Generic;

public class Wind : MonoBehaviour
{
    public List<GameObject> windObjects;
    public Vector3 windDirection;
    public float timeForChange;
    private float localTime;
    private void Start()
    {
        windDirection = (windDirection + new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f))).normalized * Random.Range(-5f, 5f);
    }
    private void Update()
    {
        foreach(GameObject o in windObjects)
        {
            o.GetComponent<Rigidbody>().AddForce(windDirection,ForceMode.Force);
        }
        if(localTime >= timeForChange)
        {
            windDirection = (windDirection+ new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f))).normalized * Random.Range(-5f, 5f);
            localTime = 0;
        }
        localTime+=Time.deltaTime;
    }
}
