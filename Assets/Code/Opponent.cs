using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Opponent : Fighter
{
    private Player player;
    private bool isCollidingWithPlayer = false;

        // Probability values for actions
    [Range(0, 1)] public float attackProbability = 0.6f;   
    [Range(0, 1)] public float blockProbability = 0.10f;     
    [Range(0, 1)] public float parryProbability = 0.15f;    
    [Range(0, 1)] public float specialAttackProbability = 0.10f; 

    protected override void Start()
    {
        base.Start();

        player = FindObjectOfType<Player>();
        StartCoroutine(RandomSpecialAttack());
    }

    protected override void Update()
    {
        base.Update(); 

        if (isSpecialAttacking || isParrying || isAttacking || isStunned || isBlocking) return;

        if (player != null)
        {
            if (ShouldParry() && player.isAttacking)
            {
                StartCoroutine(Parry());
            }
            else if(ShouldBlock() && player.isAttacking){
                StartCoroutine(Block());
            }
            else if (ShouldAttack())
            {
                StartCoroutine(Attack());
            }


            if (player.isAttacking && isParrying && isCollidingWithPlayer)
            {
                player.TriggerStun(); 
            }

            if (isAttacking && player.isParrying && isCollidingWithPlayer)
            {
                TriggerStun(); 
            }
        }
    }

    private bool ShouldParry()
    {
        return Random.value < parryProbability;
    }

    private bool ShouldAttack()
    {
        return Random.value < attackProbability;

    }

    private bool ShouldBlock(){
        return Random.value < blockProbability;
    }

    private IEnumerator Attack()
    {
        isAttacking = true;
        ChangeSpriteDuringAttack();
        yield return new WaitForSeconds(attackDuration);
        if (isCollidingWithPlayer && !player.isBlocking) 
        {
            player.TakeDamage(3, true);
            chargeMeterManager.AdjustPlayerChargeMeter(0.5f);
            chargeMeterManager.AdjustEnemyChargeMeter(1f);
        }
        yield return new WaitForSeconds(attackDuration);
        isAttacking = false;
    }

    private IEnumerator Block(){
        isBlocking = true;
        float blockDuration = Random.Range(0.5f, 3f);
        yield return new WaitForSeconds(blockDuration);
        isBlocking = false;
    }

    private IEnumerator RandomSpecialAttack()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            if (Random.value < specialAttackProbability && !isSpecialAttacking && chargeMeterManager != null && chargeMeterManager.IsFullyCharged(false))
            {
                if (player != null)
                {
                    StartCoroutine(PerformSpecialAttack(false));
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isCollidingWithPlayer = true;
            player = other.GetComponent<Player>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isCollidingWithPlayer = false;
        }
    }

    public void TriggerStun() {
        if (!isStunned) {
            isParrying = false;
            isAttacking = false;
            StartCoroutine(Stunned());
        }
    }


}
