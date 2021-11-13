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
        private float rotationAngleVar = 5f;
        private float currentRotationAngleUp;
        private float currentRotationAngleDown;
        private Vector3 rayDirectionUp;
        private Vector3 rayDirectionDown;
        private Vector3 direction;
        private bool init; 
        RaycastHit2D hitUp;
        RaycastHit2D hitDown;
        public Movement(Transform[] waypoints, float speed, Transform transform){
            this.waypoints = waypoints;
            this.speed = speed;
            destPoint = 0;
            waypointTarget = waypoints[0];
            currentRotationAngleUp = 0;
            currentRotationAngleDown = 0;
            direction = waypointTarget.position - transform.position;
            rayDirectionUp = RotatedVector(direction, currentRotationAngleUp);
            rayDirectionDown = RotatedVector(direction, currentRotationAngleDown);

            hitUp = Physics2D.Raycast(transform.position, direction.normalized, 1);
            hitDown = Physics2D.Raycast(transform.position, direction.normalized, 1);

            init = true;
        }
        public void Move(Transform transform){
            direction = waypointTarget.position - transform.position;

            hitUp = Physics2D.Raycast(transform.position, rayDirectionUp, 1.5f);
            hitDown = Physics2D.Raycast(transform.position, rayDirectionDown, 1.5f);
            rayDirectionUp = RotatedVector(rayDirectionUp, rotationAngleVar);

            rayDirectionDown = RotatedVector(rayDirectionDown, -rotationAngleVar);
            if(init){
                hitUp = Physics2D.Raycast(transform.position, direction.normalized, 1.5f);
                hitDown = Physics2D.Raycast(transform.position, direction.normalized, 1.5f);
                rayDirectionDown = direction.normalized;
                rayDirectionUp = direction.normalized;
            }

            Debug.DrawRay(transform.position, rayDirectionDown.normalized*1.5f, Color.green, 0f);

            Debug.DrawRay(transform.position, rayDirectionUp.normalized*1.5f, Color.red, 0f);

            if(hitUp.collider != null && hitDown.collider != null){
                init = false;
                return ;
            }
            direction = RotatedVector(rayDirectionUp, rotationAngleVar);
            if(hitDown.collider == null){ direction = RotatedVector(rayDirectionDown, -rotationAngleVar); }
            spriteRenderer.flipX = direction.x > 0;
            transform.position+=direction.normalized * speed * Time.fixedDeltaTime;
            currentRotationAngleDown = currentRotationAngleUp = 0;

            init = true;
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
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction.normalized, 1.5f);
            if(hit.collider != null){
                Debug.DrawRay(transform.position, direction.normalized*1.5f, Color.blue, 0);
            }
            if((target.position - transform.position).magnitude <= 1.5f){ return ; }
            transform.position+=direction.normalized*speed*Time.fixedDeltaTime;
            
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
        if(hasTarget){ return ; }
        this.target = target; 
    }

    void Start(){
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        health = new Health(1000);
        movement = new Movement(ennemyWaypoints, .9f, transform);
        hasTarget = false;
    }
    void FixedUpdate(){
        if(hasTarget && target != null){
            movement.PathFinder(target, transform);
            return ;
        }

        movement.Move(transform);
        
    }

    static private Vector2 RotatedVector(Vector2 vect, float angle){
        return new Vector2(vect.x * Mathf.Cos(angle * Mathf.Deg2Rad)-vect.y * Mathf.Sin(angle * Mathf.Deg2Rad), 
        vect.y * Mathf.Cos(angle * Mathf.Deg2Rad)+vect.x * Mathf.Sin(angle * Mathf.Sin(angle * Mathf.Deg2Rad)));
    }

}
