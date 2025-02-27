using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI gameOverText;
    /*
    public GameObject marioPrefab;

    void Start()
    {
        Vector3 pos = new Vector3(16f, 2f, 0f);
        GameObject newObj = Instantiate(marioPrefab);
        newObj.transform.position = pos;
    }
*/
    // Update is called once per frame
    void Update()
    {
        int timeLeft = 250 - (int)(Time.time / 0.4);
        timerText.text = $"Time {timeLeft.ToString("000")}";

        //MAKE TIMER NOT GO NEGATIVE
        if (timeLeft == 0)
        {
            gameOverText.text = "Game Over\nTime Ran Out";
        }
    }
}
