using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float moveSpeed = 5f;    
    public float jumpForce = 5f;
    private Vector3 input;   
    private bool isCollidingWithEnemy = false; // Track collision with enemy dummy
    private bool isBlocking = false; // Track if the player is blocking
    private bool isSpecialAttacking = false; // Track if the player is performing a special attack

    private Opponent enemy; // Reference to the enemy (Opponent)
    public HealthManager healthManager;
    public ChargeMeterManager chargeMeterManager;

    private SpriteRenderer spriteRenderer; // Reference to the SpriteRenderer
    public Sprite normalSprite;            
    public Sprite blockSprite;
    public List<Sprite> attackSprites;     
    public float attackDuration = 0.5f;    
    private int currentAttackIndex = 0;    

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = normalSprite; // Set the initial sprite
    }

    void Update()
    {
        if (isSpecialAttacking) return; // Disable all inputs during special attack

        input.x = Input.GetAxisRaw("Horizontal"); // A = -1, D = 1
        input.y = 0;

        // Calculate potential movement
        Vector3 targetPos = transform.position;
        targetPos.x += input.x * moveSpeed * Time.deltaTime;

        // Check for potential collisions with the enemy dummy
        if (!isCollidingWithEnemy || (input.x < 0 && !IsColliding()))
        {
            transform.position = targetPos; // Move if not colliding or moving away
        }

        // Block input
        if (Input.GetKey(KeyCode.LeftShift))
        {
            isBlocking = true;
            spriteRenderer.sprite = blockSprite; 
        }
        else if (!IsAttacking()) // If not attacking, revert to normal sprite
        {
            isBlocking = false;
            spriteRenderer.sprite = normalSprite; 
        }

        // Check for attack inputs
        if (!isBlocking) // Only allow attacking if not blocking
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                ChangeSpriteDuringAttack();
                if (isCollidingWithEnemy && enemy != null)
                {
                    enemy.TakeDamage(3); // Normal attack
                    chargeMeterManager.AdjustPlayerChargeMeter(1);
                    chargeMeterManager.AdjustEnemyChargeMeter(0.5f);
                    Debug.Log("Normal ATK, charging 1");
                }
            }
            
            if (Input.GetKeyDown(KeyCode.Q))
            {
                if (chargeMeterManager.IsFullyCharged())
                {
                    StartCoroutine(PerformSpecialAttack()); // Start the special attack sequence
                }
                else
                {
                    Debug.Log("Charge meter not full! Cannot perform special attack.");
                }
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                ChangeSpriteDuringAttack();
                if (isCollidingWithEnemy && enemy != null)
                {
                    enemy.TakeDamage(5); // Counter attack
                    chargeMeterManager.AdjustPlayerChargeMeter(2);
                    chargeMeterManager.AdjustEnemyChargeMeter(1f);
                    Debug.Log("Counter ATK, charging 2");
                }
            }
        }
    }

    private IEnumerator PerformSpecialAttack()
    {
        isSpecialAttacking = true; // Disable inputs
        chargeMeterManager.ActivateSpecialAttack(); // Activate special attack logic
        yield return new WaitForSeconds(3f); // Wait for the duration of the special attack
        isSpecialAttacking = false; // Re-enable inputs
    }

    private void ChangeSpriteDuringAttack()
    {
        if (attackSprites.Count > 0)
        {
            spriteRenderer.sprite = attackSprites[currentAttackIndex];
            StartCoroutine(ResetSpriteAfterDelay(attackDuration));
        }
    }

    private IEnumerator ResetSpriteAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (!isBlocking) // Only reset to normal sprite if not blocking
        {
            spriteRenderer.sprite = normalSprite; 
        }
        currentAttackIndex = (currentAttackIndex + 1) % attackSprites.Count; 
    }

    private bool IsAttacking()
    {
        // Check if the player is in the attack animation
        return spriteRenderer.sprite != normalSprite && spriteRenderer.sprite != blockSprite;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EnemyDummy"))
        {
            isCollidingWithEnemy = true; 
            enemy = other.GetComponent<Opponent>(); 

            if (enemy == null)
            {
                Debug.LogError("Enemy Dummy does not have an Opponent component!");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("EnemyDummy"))
        {
            isCollidingWithEnemy = false; 
            enemy = null; 
        }
    }

    private bool IsColliding()
    {
        // Check for a collider to the left of the player
        return Physics.Raycast(transform.position, Vector3.left, 0.5f); 
    }
}
