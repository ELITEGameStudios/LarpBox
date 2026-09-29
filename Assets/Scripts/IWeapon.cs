using UnityEngine;

public interface IWeapon
{
    public abstract void Initialize();
    public abstract void Use();
    public abstract void CanUse();

}
