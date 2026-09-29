using UnityEngine;

public class Larper : MonoBehaviour
{

    public float speed;
    public float startingHealth;
    public float forcePerDmg;
    public float forceTimeDmgValue = 20;


    [SerializeField] private float _health;
    [SerializeField] private Vector2 moveInput;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private IWeapon weapon;


    [SerializeField] private float _forceTime, _forceTimer;
    [SerializeField] private Vector2 _incomingForce;
    [SerializeField] private bool UsingForce => _forceTimer > 0;

    // Update is called once per frame
    void Update()
    {
        moveInput = new Vector2(
            Input.GetAxis("Horizontal"),
            Input.GetAxis("Vertical")
        );
    }

    void FixedUpdate()
    {
        Vector2 movement = moveInput * speed * Time.fixedDeltaTime;
        if (UsingForce)
        {
            rb.linearVelocity = Vector2.Lerp(movement, _incomingForce, _forceTimer / _forceTime);
            _forceTimer -= Time.fixedDeltaTime;   
        }
        else
        {
            rb.linearVelocity = movement;
        }
        
    }

    public void Begin()
    {
        _health = startingHealth;
        transform.position = Vector2.zero;
    }

    public void Damage(float damage, Vector2 position)
    {
        _health -= damage;
        if(_health <= 0){Die();}

        Vector2 forceDir = (position - (Vector2)transform.position).normalized;

        ApplyForce(forceDir * forcePerDmg * damage, 0.3f * Mathf.Sqrt(damage/forceTimeDmgValue));

    }

    public void ApplyForce(Vector2 force, float time = 0.3f)
    {
        _incomingForce = force;
        _forceTime = time;
        _forceTimer = time;
    }

    private void Die()
    {
        LarpManager.Instance.EndGame();
    }
}
