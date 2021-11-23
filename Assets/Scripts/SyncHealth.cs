using UnityEngine;
using Mirror;


public class SyncHealth : NetworkBehaviour {
    private Player player;
    [SyncVar] private int health; 
    [SyncVar] private int maxHealth;

    private void Start() {
        if(!isLocalPlayer){ return ; }
        health = player.GetHealth().GetHealthValue();
        maxHealth = player.GetHealth().GetMaxHealth();
    }
    private void Update() {
        if(!isLocalPlayer){ return ; }
        player.GetHealth().SetHealthValue(health);
    }

    [Command] private void SetHealth(){
        
    }
}
