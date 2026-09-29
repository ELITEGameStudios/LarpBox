using System.Collections;
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


    public void Initialize(){
        mapRootObject.SetActive(true);
    }


    public IEnumerator SpawnCoroutine()
    {
        int loops = LarpManager.Instance.GetLevel / loopFactor;

        for (int i = 0; i < loops; i++)
        {
            foreach (DiscreteSpawnInfo spawn in spawnsList)
            {
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
