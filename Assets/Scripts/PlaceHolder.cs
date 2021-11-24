using UnityEngine;
using UnityEngine.UI;

public class PlaceHolder : MonoBehaviour
{
    [SerializeField] private Player player;
    public Button button;
    public bool isEmpty;
    private void Awake() {
        isEmpty = true;  
    }

    /* Setter functions */
    public void SetPlayer(Player player){
        this.player = player;
    }
    public void SetButton(Button button){ this.button = button; }
    
}
