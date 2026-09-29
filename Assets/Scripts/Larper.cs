using UnityEngine;

public class Larper : MonoBehaviour
{

    public float speed;
    public float startingHealth;
    public float forcePerDmg;
    public float forceTimeDmgValue = 20;
    public float _currentHitCooldown;


    [SerializeField] private SpriteRenderer spriteRenderer;
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
        if(LarpManager.Instance.GetState != LarpManager.GameState.GAME){return;}

        if(_currentHitCooldown > 0){_currentHitCooldown -= Time.deltaTime;}

        Vector2 movement = moveInput * speed;
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

    public void Revive()
    {
        spriteRenderer.enabled = true;
        _health = startingHealth;
        transform.position = Vector2.zero;
    }


    public bool Damage(float damage, Vector2 position)
    {  
        if(_currentHitCooldown > 0){return false;}

        _health -= damage;
        if(_health <= 0){Die();}

        Vector2 forceDir = ((Vector2)transform.position - position).normalized;
        float forceTime = 0.3f * Mathf.Sqrt(damage/forceTimeDmgValue);

        ApplyForce(damage * forcePerDmg * forceDir, forceTime);
        _currentHitCooldown = forceTime + 0.2f;
        return true;
    }

    public void ApplyForce(Vector2 force, float time = 0.3f)
    {
        _incomingForce = force;
        _forceTime = time;
        _forceTimer = time;
    }

    private void Die()
    {
        spriteRenderer.enabled = false;
        LarpManager.Instance.EndGame();
    }
}
