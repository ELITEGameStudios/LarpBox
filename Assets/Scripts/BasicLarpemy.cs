using UnityEngine;

// One of the two enemy variants implementing ILarpemy
public class BasicLarpemy : MonoBehaviour, ILarpemy
{
    [SerializeField] private Rigidbody2D rb;
    
    public float speedBase, speedVariance;

    [SerializeField] private float _damage;
    [SerializeField] private float _speed;

    // Implemented from ILarpemy
    public void Initialize()
    {
        _speed = speedBase + Random.Range(-speedVariance, speedVariance);
    }

    void FixedUpdate()
    {
        rb.linearVelocity = (LarpManager.Instance.GetLarper.transform.position - transform.position).normalized * _speed;  
    }


    // Implemented from ILarpemy
    public void Attack()
    {
        LarpManager.Instance.GetLarper.Damage(_damage, transform.position);
    }

    // Implemented from ILarpemy
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
