using UnityEngine;

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
    public bool canTryPunch() => rb.linearVelocity.magnitude <= 0.1f ? true : false;
    private void Start()
    {
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
        rb.AddForce(arrow.transform.right *punchForce, ForceMode.Impulse);
    }
    private void Update()
    {
        if (calculateDirection)
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
                arrow.GetChild(0).transform.localScale = new Vector3(3* (0.5f+direction.magnitude / Screen.height), 0.2f, 0.2f);
                arrow.GetChild(0).transform.localPosition = new Vector3(-arrow.GetChild(0).transform.localScale.x / 2, 0, 0);
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
            calculateDirection = true;
            ShowArrow();
        }
    }
}
