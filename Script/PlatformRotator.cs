using UnityEngine;

public class PlatformRotator : MonoBehaviour
{
    public Vector3 rotateSpeed;
    private void Update()
    {
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles+rotateSpeed*Time.deltaTime);
    }
}
