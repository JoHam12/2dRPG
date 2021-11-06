using UnityEngine;

public class Ammo : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float ammoSpeed = 10f;
    private int damage = -60;
    private float maxTime = 6.0f, t;
    void Start()
    {
        t = 0;
        rb = GetComponent<Rigidbody2D>();
    }
    void FixedUpdate()
    {
        if(t>= maxTime){ Destroy(this.gameObject ); }
        t+=Time.deltaTime;
        rb.AddForce(new Vector3(0, -1) * ammoSpeed * Time.fixedDeltaTime, ForceMode2D.Impulse);
        
    }

    public int GetDamage(){ return damage; }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Walls")){ Destroy(this.gameObject); }
        
    }
    // float power = 2;
    // void Update() {
    //     float horizontal = Input.GetAxisRaw("Horizontal");

    //     int j = 0;
    //     if (Input.GetButton("Jump")){ j = 1; }
        
    //     Vector3 velocity = new Vector3(horizontal, 0, power * j);
        
    //     rigidbody.AddForce(velocity, ForceMode2D.Force);
    // }
}
