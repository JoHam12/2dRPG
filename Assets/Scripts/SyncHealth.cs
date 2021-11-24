using UnityEngine;
using Mirror;
using UnityEngine.UI;


public class SyncHealth : NetworkBehaviour {
    [SerializeField] private Player player;
    [SerializeField] private Slider syncHealthSlider;
    [SerializeField] private int health; 
    private int maxHealth;

    private void Start() {
        if(!isLocalPlayer){ return ; }
        player = GetComponent<Player>();
        health = player.GetHealth().GetHealthValue();
        maxHealth = player.GetHealth().GetMaxHealth();
    }
    private void Update() {
        if(!isLocalPlayer){ 
            syncHealthSlider.gameObject.SetActive(false);
            return ; 
        }
        health = player.GetHealth().GetHealthValue();
        maxHealth =  player.GetHealth().GetMaxHealth();
        SetHealth(health, maxHealth);
    }

    [Command] private void SetHealth(int newHealth, int newMaxHealth){
        syncHealthSlider.maxValue = newMaxHealth;
        syncHealthSlider.value = newHealth;
    }
}