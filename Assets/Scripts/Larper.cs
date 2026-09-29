using UnityEngine;

// Alias for Player, handles much of the player logic.
public class Larper : MonoBehaviour
{

    public float startingSpeed, speedAdditiveFactor;
    private float _speed;
    public float startingHealth;
    public float forcePerDmg;
    public float forceTimeDmgValue = 20;
    public float _currentHitCooldown;


    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private float _health;
    [SerializeField] private Vector2 moveInput;
    [SerializeField] private Rigidbody2D rb;

    // For handling custom Kb taken from enemies
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

        Vector2 movement = moveInput * _speed;
        if (UsingForce)
        {
            // Will Apply Kb to the movement for a very short time frame, temporarily taking movement control
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
        // Called to both bring back the player from death on reset as well as to reset the player in between maps.
        spriteRenderer.enabled = true;
        _health = startingHealth;
        transform.position = Vector2.zero;
        _speed = startingSpeed + speedAdditiveFactor * (LarpManager.Instance.GetLevel-1);
    }


    public bool Damage(float damage, Vector2 position)
    {  
        // Will damage the player if hit cooldown is a non-factor
        if(_currentHitCooldown > 0){return false;}

        _health -= damage;
        if(_health <= 0){Die();}

        Vector2 forceDir = ((Vector2)transform.position - position).normalized;
        float forceTime = 0.3f * Mathf.Sqrt(damage/forceTimeDmgValue);

        ApplyForce(damage * forcePerDmg * forceDir, forceTime);
        _currentHitCooldown = forceTime + 0.2f;
        return true;
    }

    public float GetHp()
    {
        return _health;
    }

    public void ApplyForce(Vector2 force, float time = 0.3f)
    {
        // Sets a new custom force
        _incomingForce = force;
        _forceTime = time;
        _forceTimer = time;
    }

    private void Die()
    {
        // Ends the game
        spriteRenderer.enabled = false;
        LarpManager.Instance.EndGame();
    }
}
