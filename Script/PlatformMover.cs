using UnityEngine;
using System.Collections.Generic;
public class PlatformMover : MonoBehaviour
{
    public float speed;
    public List<Vector3> beacons;
    public int current;
    private Vector3 direction;
    private float waitTime;
    private void Start()
    {
        current = 1;
        CalculateDirection();
    }
    private void Update()
    {
        if (waitTime <= 0)
        {
            transform.position += direction * speed * Time.deltaTime;
            if (transform.position.x > beacons[current].x - 0.2f && transform.position.x < beacons[current].x + 0.2f)
            {
                if (transform.position.y > beacons[current].y - 0.2f && transform.position.y < beacons[current].y + 0.2f)
                {
                    current++;
                    waitTime = 1;
                    if (current > beacons.Count - 1) current = 0;
                    CalculateDirection();
                }
            }
        }
        else waitTime -= Time.deltaTime;
        
    }
    public void CalculateDirection()
    {
        direction = (beacons[current] - transform.position).normalized;
    }
}
