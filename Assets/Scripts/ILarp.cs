using System.Collections;
using UnityEngine;

public interface ILarp
{
    public abstract void Initialize();
    public abstract IEnumerator SpawnCoroutine();
    public abstract void End();

}
