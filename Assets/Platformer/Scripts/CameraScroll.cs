using UnityEngine;

public class CameraScroll : MonoBehaviour
{
    private Transform mario;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mario = GameObject.FindWithTag("Mario").transform;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 cameraPosition = transform.position;
        cameraPosition.x = Mathf.Max(cameraPosition.x, mario.position.x);
        transform.position = cameraPosition;
    }
}
