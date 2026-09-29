
// Interface for Enemies, At the moment the differences involving this are most prominent in the Attack() methods. Special events and unique effects can be added to each on spawn or death with this factory based system.
public interface ILarpemy
{
    public abstract void Initialize();
    public abstract void Attack();
    public abstract void Kill();
}
