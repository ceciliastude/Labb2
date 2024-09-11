using UnityEngine;

public class Player : Fighter
{

    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    public Rigidbody2D rb;

    protected override void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update(){

        if (Input.GetKeyDown(KeyCode.A)){

            rb.velocity = new Vector2(-moveSpeed, rb.velocity.y);

        }

        if (Input.GetKeyDown(KeyCode.D)){

            rb.velocity = new Vector2(moveSpeed, rb.velocity.y);

        }

        if  (Input.GetKeyDown(KeyCode.S)){

        }

        if  (Input.GetKeyDown(KeyCode.W)){

        }

        if (Input.GetKeyDown(KeyCode.Space)){
            Attack();

        }
    
        if (Input.GetKeyDown(KeyCode.LeftShift)){
            Block();

        }

        if (Input.GetKeyDown(KeyCode.Space) && Input.GetKeyDown(KeyCode.LeftShift)){
            CounterAtk();

        }

        if (Input.GetKeyDown(KeyCode.Q)){
            SpecialAtk();

        }

}
public override void Attack(){
    Debug.Log("Player Attacks");

}
public override void Block(){
    Debug.Log("Player blocks");
}
public override void CounterAtk(){
    Debug.Log("Player counter attacks");
}
public override void SpecialAtk(){
    Debug.Log("Player uses special");
}
public override void TakeDamage(int damage){
    Debug.Log("Player takes damage");
}

private void Die(){
    Debug.Log("Player dies");
}

}