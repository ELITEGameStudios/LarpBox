using UnityEngine;

public class FakeLarpemy : MonoBehaviour, ILarpemy
{
    [SerializeField] private Rigidbody2D rb;
    
    [SerializeField] private float _damage;
    [SerializeField] private float _speed;
    [SerializeField] private float _startKb;

    // Implements from ILarpemy, will give itself a starting knockback force away from the player to give them time to prep for its dive
    public void Initialize()
    {
        rb.linearVelocity = (transform.position - LarpManager.Instance.GetLarper.transform.position).normalized * _startKb;
    }

    void FixedUpdate()
    {
        rb.AddForce((LarpManager.Instance.GetLarper.transform.position - transform.position).normalized * _speed * Time.fixedDeltaTime);  
    }

    // Will Damage the player and kill itself. Implements from ILarpemy
    public void Attack()
    {
        LarpManager.Instance.GetLarper.Damage(_damage, transform.position);
        Kill();
    }

    // Implements from ILarpemy
    public void Kill()
    {
        LarpManager.Instance.RemoveEnemy(gameObject);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Larper")
        {
            Debug.Log("Yikes");
            Attack();
        }    
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Larper")
        {
            Debug.Log("Yikes");
            Attack();
        }    
    }

}
