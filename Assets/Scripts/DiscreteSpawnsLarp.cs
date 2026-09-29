using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DiscreteSpawnsLarp : MonoBehaviour, ILarp
{
    [SerializeField] GameObject mapRootObject;
    
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
        LarpManager.Instance.AddLarp(this);
        mapRootObject.SetActive(false);
    }

    public void Initialize(){
        mapRootObject.SetActive(true);
    }


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

        LarpManager.Instance.SignalEndOfSpawning();
    }

    public void End()
    {
        mapRootObject.SetActive(false);
    }
}
