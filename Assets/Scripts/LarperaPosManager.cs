using UnityEngine;

public class LarperaPosManager : MonoBehaviour
{

    public static LarperaPosManager Instance {get; private set;}
    public float targetFactor = 0.2f;
    public float proportionalFactor = 0.05f;
    
    void Awake()
    {
        if(Instance == null){Instance = this;}
        else if(Instance != this){Destroy(this);}
    }

    void Update()
    {
        Vector2 targetPos = LarpManager.Instance.GetLarper.transform.position * targetFactor;
        transform.position = (Vector3)Vector2.Lerp(transform.position, targetPos, proportionalFactor) + Vector3.forward * -10;
    }
}
