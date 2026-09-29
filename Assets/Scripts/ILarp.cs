using System.Collections;
using UnityEngine;

// Interface for Map variants. Although Initialize and End are identical in the two existing ones, SpawnCoroutine demonstrates different functionality
// If custom events wanted to be added for their initialization or end states, it can be done with this.
public interface ILarp
{
    public abstract void Initialize();
    public abstract IEnumerator SpawnCoroutine();
    public abstract void End();

}
