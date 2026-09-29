using UnityEngine;
using UnityEngine.UI;


// The main UI element in the game to inform the player of gamestate or what to do. Implements a Singleton Pattern
public class LarpMessageManager : MonoBehaviour
{
    public static LarpMessageManager Instance {get; private set;}

    private float currentTime, targetTime;
    private bool active;
    [SerializeField] private Text text;

    void Awake()
    {
        if(Instance == null){Instance = this;}
        else if(Instance != this){Destroy(this);}
    }

    // Can be called to announce anything for a given period of time. Will override the latest announcement if the previous is still active
    public void Announce(string message, int time = 4, Color? color = null)
    {
        text.text = message;
        currentTime = 0;
        targetTime = time;
        text.color = color ?? Color.white;  
        
        text.enabled = true;
        active = true;
    }
    
    // Can announce things permenantly until StopAnnouncement is called or a new announcement is given.
    public void AnnouncePerma(string message, Color? color = null)
    {
        text.text = message;
        currentTime = 0;
        targetTime = Mathf.Infinity;
        text.color = color ?? Color.white;  
        
        text.enabled = true;
        active = true;
    }

    // Stops the current announcement, can be called by any exterior class
    public void StopAnnouncement()
    {
        Stop();
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

    // Stops the current announcement, 
    void Stop()
    {
        text.enabled = false;
        active = false;
    }


}
