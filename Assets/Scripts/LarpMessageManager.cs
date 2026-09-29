using UnityEngine;
using UnityEngine.UI;

public class LarpMessageManager : MonoBehaviour
{
    public static LarpMessageManager Instance {get; private set;}

    private float currentTime, targetTime;
    private bool active;
    [SerializeField] private Text text;

    public void Announce(string message, int time = 4, Color? color = null)
    {
        text.text = message;
        currentTime = 0;
        targetTime = time;
        text.color = color ?? Color.white;  
        
        text.enabled = true;
        active = true;
    }

    void Update()
    {
        if(!active) return;

        if(currentTime >= targetTime)
        {
            Stop();
        }
        else
        {
            currentTime+= Time.deltaTime;
        }

    }

    void Stop()
    {
        text.enabled = false;
        active = false;
    }


}
