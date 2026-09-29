using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LarpManager : MonoBehaviour
{
    public static LarpManager Instance {get; private set;}
    [SerializeField] private Larper _larper;
    public Larper GetLarper => _larper; 

    private int _level;
    public int GetLevel => _level;

    public enum GameState
    {
        MENUS,
        GAME,
        TRANSITION,
        DEAD
    }


    private GameState state;
    public GameState GetState => state;


    [SerializeField] private ILarp[] larps; 
    [SerializeField] private ILarp currentLarp; 
    [SerializeField] private List<GameObject> enemyList; 

    [SerializeField] private bool spawningHasEnded;
    [SerializeField] private bool anyEnemyIsAlive => enemyList.Count > 0;

    void Awake()
    {
        if (Instance == null) {Instance = this;}
        else if (Instance != this){Destroy(this);}

        enemyList = new();
    }

    void Update()
    {
        switch (state)
        {
            case GameState.GAME:
                if(spawningHasEnded && !anyEnemyIsAlive)
                {
                    TransitionMap();
                }
                break;
        }
    }

    void BeginRound()
    {
        StartCoroutine(currentLarp.SpawnCoroutine());
        LarpMessageManager.Instance.Announce("Round " + _level);
    }

    public void EndGame()
    {
        
    }

    void Menus()
    {
        
    }

    void TransitionMap()
    {
        _level++;

        List<ILarp> choosableLarps = larps.ToList();
        choosableLarps.Remove(currentLarp);
        currentLarp = choosableLarps[Random.Range(0, choosableLarps.Count)];

        currentLarp.Initialize();
        BeginRound();
    }

    public void NewEnemy(GameObject prefab, Vector2 position)
    {
        GameObject newEnemy = Instantiate(prefab, position, transform.rotation);
        newEnemy.GetComponent<ILarpemy>()?.Initialize();
        enemyList.Add(newEnemy);
    }

    public void RemoveEnemy(GameObject enemy)
    {
        // enemy.GetComponent<ILarpemy>()?.Kill();
        if(enemyList.Contains(enemy)){enemyList.Remove(enemy);}
        Destroy(enemy);
    }

    public void SignalEndOfSpawning()
    {
        spawningHasEnded = true;
    }
}
