using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI timerText;

    // Update is called once per frame
    void Update()
    {
        int timeLeft = 300 - (int)(Time.time / 0.4);
        timerText.text = $"Time {timeLeft}";
    }
}
