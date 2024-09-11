using UnityEngine;

public abstract class Fighter : MonoBehaviour
{
    public int maxHealth = 100;
    protected int currentHealth;
    public int chargeMeter;
    public int maxCharge = 100;
    public int normalAtk = 5;
    public int comboAtk = 7;
    public int counterAtk = 10;
    public int specialAtk = 20;
    protected int comboCount = 0;
    protected bool isStunned = false;
    public bool isBlocking = false;


    public abstract void Attack();

    public abstract void Block();
    
    public abstract void CounterAtk();

    public abstract void SpecialAtk();

    public abstract void TakeDamage(int damage);

    protected virtual void Start(){
        //Code here
    }

    protected void IncreaseCharge(){
        //Code here
    }

    protected void ResetCombo(){
        //Code here
    }

    protected void ComboAttack(){
        //Code here
    }


}