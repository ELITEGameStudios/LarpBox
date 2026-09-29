using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

// The alias for GameManager, handles the core game state and logic.
public class LarpManager : MonoBehaviour
{
    // Declaring singleton
    public static LarpManager Instance {get; private set;}
    
    // Player reference (Known as Larper)
    [SerializeField] private Larper _larper;
    public Larper GetLarper => _larper; 

    private int _level;
    public int GetLevel => _level;

    // Basic game states
    public enum GameState
    {
        MENUS,
        GAME,
        TRANSITION,
        DEAD
    }


    private GameState _state;
    public GameState GetState => _state;


    // Map management factory implementation (Denoted Larp management)
    private ILarp _currentLarp; 
    [SerializeField] private List<ILarp> _larps; 
    [SerializeField] private List<GameObject> _enemyList; 
    [SerializeField] private int expectedLarpCount = 3; 


    // Game timer management
    [SerializeField] private float _timerInLevel, _timeToNextLevel;

    // Ui and Aesthetics references

    [SerializeField] private Text levelTimer, hpTracker; 
    [SerializeField] private GameObject menusCollection; 
    [SerializeField] private Light2D globalLight; 

    void Awake()
    {
        // Singleton implementation
        if (Instance == null) {Instance = this;}
        else if (Instance != this){Destroy(this);}

        _enemyList = new();
        _larps = new();
    }
    public void AddLarp(ILarp larp)
    {
        // Maps add themselves to the pool before proceeding with game logic. Expected larp count should be hit
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
                
                // Space input to play in menus
                levelTimer.text = "";
                if (Input.GetKey(KeyCode.Space))
                {
                    BeginGame();
                }
                break;


            case GameState.GAME:
                // Updating UI
                levelTimer.text = _timerInLevel + " / " + _timeToNextLevel;
                hpTracker.text = GetLarper.GetHp() + " HP";

                // Updating timer and transitioning if target time is hit
                _timerInLevel+=Time.deltaTime;
                if(_timerInLevel > _timeToNextLevel)
                {
                    TransitionMap();
                }
                break;

            case GameState.DEAD:

                // Input to return to menus
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

        // Using _currentLarp (Map)'s interface implementation to begin customizable spawn patterns
        StartCoroutine(_currentLarp.SpawnCoroutine());
        LarpMessageManager.Instance.Announce("Round " + _level);
        globalLight.color = Color.HSVToRGB(Random.Range(0f, 1f), 0.6f, 1);
    }

    public void EndGame()
    {
        _state = GameState.DEAD;
        // Using LarpMessageManager (Just the big UI element)'s singleton implementation to show dead message
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

        
        // Using LarpMessageManager (Just the big UI element)'s singleton implementation to show play button
        LarpMessageManager.Instance.AnnouncePerma("Press SPACE to play.", Color.cyan);

    }

    void TransitionMap()
    {
        StopAllCoroutines();
        ClearEnemies();

        _level++;
        
        _currentLarp?.End();
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
}
