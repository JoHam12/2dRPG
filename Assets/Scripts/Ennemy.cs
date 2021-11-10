using UnityEngine;
using Mirror;

/* 
    Ennemy class
    
*/
public class Ennemy : NetworkBehaviour
{
    public class Health{
        private int maxHealth;        
        private int healthValue;

        /* 
            /Constructor\
            Parameters | Type | Description
            -----------+------+----------------------
            maxHealth  |  int | player maximum health

            Make instance of Health with maxHealth
        */
        public Health(int _maxHealth){
            maxHealth = _maxHealth;
            healthValue = maxHealth;
        }
        /* 
            /HealthManager\
            Parameters | Type | Description
            -----------+------+---------------------------
            damage     | int  | damage received by player 
                       |      | < 0 : Attack; > 0 : Heal
        */
        public void HealthManager(int damage){
            if(healthValue + damage >= maxHealth){
                healthValue += damage;
                // Heal
                return ;
            }
            if(healthValue + damage <= 0){
                healthValue = 0;
                // Die
                return ;
            }
            healthValue += damage;
        }
        // Get Maximum Health
        public int GetMaxHealth(){ return maxHealth; }
        // Get Health
        public int GetHealthValue(){ return healthValue; }
    }

    

    // Strength and Attack class
    public class Strength{

    }


    // Movement class (AI)
    public class Movement{
        private Transform[] waypoints;
        private Transform waypointTarget;
        private int destPoint;
        private float speed;
        public Movement(Transform[] waypoints, float speed){
            this.waypoints = waypoints;
            this.speed = speed;
            destPoint = 0;
            waypointTarget = waypoints[0];
        }
        public void Move(Transform transform){
            Vector3 direction = waypointTarget.position - transform.position;
            direction = direction.normalized * speed * Time.fixedDeltaTime;
            spriteRenderer.flipX = direction.x > 0;
            transform.position += direction;
            if(Vector3.Distance(waypointTarget.position, transform.position) < .3f){
                destPoint = (destPoint + 1) % waypoints.Length;
                waypointTarget = waypoints[destPoint];
            }
        }
        public Transform GetWaypointTarget(){ return waypointTarget; }
        public int GetDestPoint(){ return destPoint; }
        public float GetSpeed(){ return speed; }
        public void PathFinder(Transform target, Transform transform){
            Vector3 direction = target.position - transform.position;
            spriteRenderer.flipX = direction.x > 0;
            if((target.position - transform.position).magnitude <= 1f){ return ; }
            transform.position += (direction.normalized * speed * Time.fixedDeltaTime);
            
        }
    }

    private Health health;
    [SerializeField] private bool hasTarget;
    [SerializeField] private Transform target;
    private Movement movement;
    [SerializeField] private Transform[] ennemyWaypoints;
    static private Rigidbody2D rb;
    static private SpriteRenderer spriteRenderer;
    public Health GetHealth(){ return health; }
    public Movement GetMovement(){ return movement; }
    public void SetHasTarget(bool state){ hasTarget = state; }
    public void SetTarget(Transform target){ 
        if(this.target != null){ return ; }
        this.target = target; 
    }

    void Start(){
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        health = new Health(1000);
        movement = new Movement(ennemyWaypoints, .7f);
        hasTarget = false;
    }
    void FixedUpdate(){
        if(hasTarget && target != null){
            movement.PathFinder(target, transform);
            return ;
        }

        movement.Move(transform);
        Debug.Log(movement.GetWaypointTarget());
        
    }


}
