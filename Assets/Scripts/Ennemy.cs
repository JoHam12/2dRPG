using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 
    Ennemy class
    
*/
public class Ennemy : MonoBehaviour
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
    [SerializeField] private Health health;
    public Health GetHealth(){ return health; }
    

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
        }
        public void Move(Transform transform){
            Vector3 dir = waypointTarget.position - transform.position;
            transform.Translate(dir.normalized * speed * Time.deltaTime, Space.World);

            if(Vector3.Distance(transform.position, waypointTarget.position) < 0.3f){
                destPoint =(destPoint + 1) % waypoints.Length;
                waypointTarget = waypoints[destPoint];
            }
        }
        // Getters
        public Transform GetWaypointTarget(){ return waypointTarget; }
        public int GetDestPoint(){ return destPoint; }
        public float GetSpeed(){ return speed; }
        // Fonction Suivre Joueur
    }

}
