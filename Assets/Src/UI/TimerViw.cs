using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UITimerSG : MonoBehaviour
{
    public float duration = 60f;
    float timeLeft;
    float lastSecond;
    public TMP_Text timerText;

    Material mat;

    void Start()
    {
        timeLeft = duration;
        mat = GetComponent<Graphic>().material;
        lastSecond = Mathf.Ceil(timeLeft);
    }

    void Update()
    {
        timeLeft -= Time.deltaTime;
        timeLeft = Mathf.Max(0, timeLeft);

        float progress = 1f - (timeLeft / duration);
        mat.SetFloat("_TimeProgress", progress);

        float sec = Mathf.Ceil(timeLeft);
        if (sec != lastSecond)
        {
            mat.SetFloat("_Pulse", 1);
            lastSecond = sec;
        }
        else
        {
            mat.SetFloat("_Pulse",
                Mathf.Lerp(mat.GetFloat("_Pulse"), 0, Time.deltaTime * 8));
        }
        
        int seconds = Mathf.CeilToInt(timeLeft);
        timerText.text = seconds.ToString();
    }
}