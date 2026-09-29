using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadialSpawnsLarp : MonoBehaviour, ILarp
{
    // Uses ILarp (For Maps) to customize mostly the spawn process. "Map" and spawn behaviours are linked
    [SerializeField] GameObject mapRootObject;
    
    [System.Serializable]
    public struct RadialSpawnInfo
    {
        public float radius;
        public int count;
        public float interval;
        public float startAngle;
        public float endAngle;
        public GameObject[] prefabs;
    }


    public List<RadialSpawnInfo> spawnsList;
    public int stepFactor;
    public float timeBetweenSteps;


    void Start()
    {
        LarpManager.Instance.AddLarp(this);
        mapRootObject.SetActive(false);
    }
    
    public void Initialize(){
        mapRootObject.SetActive(true);
    }

    public Vector2 GetVectorByAngle(float angle) // In degrees
    {
        return new Vector2(
            Mathf.Cos(angle * Mathf.Deg2Rad),
            Mathf.Sin(angle * Mathf.Deg2Rad)
        );
    }


    public IEnumerator SpawnCoroutine()
    {
        // Spawns a variable number of enemies along a circle arc based on the RadialSpawnInfo parameters given. This will loop through the spawn info list if nessecary to continue its assigned spawn steps.

        int steps = LarpManager.Instance.GetLevel+1 / stepFactor;
        for (int i = 0; i < steps; i++)
        {
            int index = i % spawnsList.Count;
            RadialSpawnInfo spawn = spawnsList[index];


            for (int j = 0; j < spawn.count; j++)
            {

                float angle = spawn.startAngle + (spawn.startAngle - spawn.endAngle)/spawn.count * j;
                Vector2 position = GetVectorByAngle(angle) * spawn.radius;
                
                LarpManager.Instance.NewEnemy(spawn.prefabs[j % spawn.prefabs.Length], position);
                yield return new WaitForSeconds(spawn.interval);
                
            }

            yield return new WaitForSeconds(timeBetweenSteps);
        }
    }

    public void End()
    {
        mapRootObject.SetActive(false);
    }
}
