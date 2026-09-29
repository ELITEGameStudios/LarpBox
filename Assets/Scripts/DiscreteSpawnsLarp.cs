using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DiscreteSpawnsLarp : MonoBehaviour, ILarp
{
    // Uses ILarp (For Maps) to customize mostly the spawn process. "Map" and spawn behaviours are linked
    [SerializeField] GameObject mapRootObject;
    
    // Spawn data serialized into a struct for the unity editor
    [System.Serializable]
    public struct DiscreteSpawnInfo
    {
        public GameObject prefab;
        public Transform tf;
    }

    public DiscreteSpawnInfo[] spawnsList;
    public int loopFactor;
    public float timeBetweenLoops;
    public float timeBetweenSpawns;

    public void Start()
    {
        // Adding to LarpManager
        LarpManager.Instance.AddLarp(this);
        mapRootObject.SetActive(false);
    }

    public void Initialize(){
        mapRootObject.SetActive(true);
    }

    // Spawn Coroutine shared from interface
    // Spawns the exact position and enemy pairs in a random order, and loops if the round is high enough.
    public IEnumerator SpawnCoroutine()
    {
        int loops = LarpManager.Instance.GetLevel+1 / loopFactor;

        for (int i = 0; i < loops; i++)
        {
            List<DiscreteSpawnInfo> spawnListPool = spawnsList.ToList();

            for(int j = spawnListPool.Count-1; j >= 0; j--)
            {
                DiscreteSpawnInfo spawn = spawnListPool[Random.Range(0, spawnListPool.Count-1)];
                spawnListPool.Remove(spawn);
                
                LarpManager.Instance.NewEnemy(spawn.prefab, spawn.tf.position);
                yield return new WaitForSeconds(timeBetweenSpawns);
            }

            yield return new WaitForSeconds(timeBetweenLoops);
        }

    }

    public void End()
    {
        mapRootObject.SetActive(false);
    }
}
