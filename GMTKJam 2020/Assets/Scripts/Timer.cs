using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[System.Serializable]
public class TimerEndEvent : UnityEvent<Timer> { }

public class Timer : MonoBehaviour
{
    public bool active = false;
    public Image fillImage;
    public GameObject visibleTimerObj;
    public float timeInSeconds = 60f;
    public float timeLeft = 60f;

    public TimerEndEvent timerEnded;
    // Start is called before the first frame update
    void Start()
    {
        visibleTimerObj.SetActive(active);
    }

    public void InitTimer(float targetTime, bool activate = true)
    {
        timeInSeconds = targetTime;
        timeLeft = targetTime;
        visibleTimerObj.SetActive(activate);
        active = activate;
    }
    public void StartTimer()
    {
        visibleTimerObj.SetActive(true);
        active = true;
    }
    public void PauseTimer()
    {
        active = false;
    }
    public void StopTimer()
    {
        active = false;
        visibleTimerObj.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (active)
        {
            timeLeft -= Time.deltaTime;
            if (timeLeft <= 0)
            {
                timerEnded.Invoke(this);
                StopTimer();
            }
        }
        if (fillImage != null) { fillImage.fillAmount = timeLeft / timeInSeconds; };
    }
}
