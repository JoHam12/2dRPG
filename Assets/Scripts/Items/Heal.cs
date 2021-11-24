using UnityEngine;

public class Heal : Item
{
    [SerializeField] private int gainedHealth;
    public int GetGainedHealth(){ return gainedHealth; }
    
    /// <summary> Heals player </summary>
    public void HealManager(){
        if(userPlayer == null){ return ; }
        userPlayer.GetHealth().HealthManager(gainedHealth);
        FinishUseObject();
        Destroy(this.gameObject);
    }
    
}
