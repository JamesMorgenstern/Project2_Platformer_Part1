using UnityEngine;
using TMPro;

public class Raycast : MonoBehaviour
{
    public GameObject brick;
    public GameObject question;
    public TextMeshProUGUI coinAmount;
    Camera cam;
    private int count;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cam = Camera.main;
        count = 00;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 11f;
        mousePos = cam.ScreenToWorldPoint(mousePos);
        //Debug.DrawRay(transform.position, mousePos - transform.position, Color.blue);

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100))
            {
                //Debug.Log(hit.transform.name);
                if (hit.transform.gameObject.tag == "Brick")
                {
                    Destroy(hit.transform.gameObject);
                }
                else if (hit.transform.gameObject.tag == "Question")
                {
                    count++;
                    coinAmount.text = $"x{count.ToString("00")}";
                }
            }
        }
    }
}
