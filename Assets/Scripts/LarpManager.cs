using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

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


    private GameState _state;
    public GameState GetState => _state;


    [SerializeField] private int expectedLarpCount = 3; 
    [SerializeField] private List<ILarp> _larps; 
    [SerializeField] private List<GameObject> _enemyList; 
    private ILarp _currentLarp; 

    [SerializeField] private bool _spawningHasEnded;
    public bool AnyEnemyIsAlive => _enemyList.Count > 0;

    [SerializeField] private float _timerInLevel, _timeToNextLevel;


    [SerializeField] private Text levelTimer, hpTracker; 
    [SerializeField] private GameObject menusCollection; 

    void Awake()
    {
        if (Instance == null) {Instance = this;}
        else if (Instance != this){Destroy(this);}

        _enemyList = new();
        _larps = new();
    }
    public void AddLarp(ILarp larp)
    {
        _larps.Add(larp);
        if(_larps.Count == expectedLarpCount){
            Menus();
        }
    }


    void Update()
    {
        switch (_state)
        {
            case GameState.MENUS:
                levelTimer.text = "";
                if (Input.GetKey(KeyCode.Space))
                {
                    BeginGame();
                }
                break;


            case GameState.GAME:
                levelTimer.text = _timerInLevel + " / " + _timeToNextLevel;
                hpTracker.text = GetLarper.GetHp() + " HP";

                _timerInLevel+=Time.deltaTime;
                if(_timerInLevel > _timeToNextLevel)
                {
                    TransitionMap();
                }
                break;

            case GameState.DEAD:
                levelTimer.text = _timerInLevel.ToString();
                if (Input.GetKey(KeyCode.Space))
                {
                    Menus();
                }
                break;

        }
    }

    void BeginGame()
    {
        _state = GameState.GAME;
        BeginRound();
    }

    void BeginRound()
    {
        _timeToNextLevel = 5 + _level * 2;
        _timerInLevel = 0;
        GetLarper.Revive();
        
        StartCoroutine(_currentLarp.SpawnCoroutine());
        LarpMessageManager.Instance.Announce("Round " + _level);
    }

    public void EndGame()
    {
        _state = GameState.DEAD;
        LarpMessageManager.Instance.AnnouncePerma("UR DEAD\n Press Space to Restart", Color.red);
        StopAllCoroutines();
    }

    void ClearEnemies()
    {
        
        for (int i = _enemyList.Count-1; i >= 0; i--){
            _enemyList[i].GetComponent<ILarpemy>().Kill();
        }
    }

    void Menus()
    {
        _level = 0;
        TransitionMap();
        _state = GameState.MENUS;
        GetLarper.Revive();

        
        LarpMessageManager.Instance.AnnouncePerma("Press SPACE to play.", Color.cyan);

    }

    void TransitionMap()
    {
        StopAllCoroutines();
        ClearEnemies();

        _level++;
        
        _spawningHasEnded = false;
        List<ILarp> choosableLarps = _larps.ToList();
        choosableLarps.Remove(_currentLarp);
        _currentLarp = choosableLarps[Random.Range(0, choosableLarps.Count)];

        _currentLarp.Initialize();
        if(_state == GameState.GAME) BeginRound();
    }

    public void NewEnemy(GameObject prefab, Vector2 position)
    {
        GameObject newEnemy = Instantiate(prefab, position, transform.rotation);
        newEnemy.GetComponent<ILarpemy>()?.Initialize();
        _enemyList.Add(newEnemy);
    }

    public void RemoveEnemy(GameObject enemy)
    {
        // enemy.GetComponent<ILarpemy>()?.Kill();
        if(_enemyList.Contains(enemy)){_enemyList.Remove(enemy);}
        Destroy(enemy);
    }

    public void SignalEndOfSpawning()
    {
        _spawningHasEnded = true;
    }
}
