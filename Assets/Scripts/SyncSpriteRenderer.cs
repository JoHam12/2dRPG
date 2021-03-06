using UnityEngine;
using Mirror;
public class SyncSpriteRenderer : NetworkBehaviour
{
    
    [SerializeField] private SpriteRenderer syncSpriteRenderer;
    [SerializeField] private Player player;
    [SyncVar] [SerializeField] private bool isFlipped;
    void Start(){
        if(!isLocalPlayer){ return ; }
        isFlipped = false;
        syncSpriteRenderer = GetComponent<SpriteRenderer>();
    }
    
    void Update(){
        if(syncSpriteRenderer != null){ syncSpriteRenderer.flipX = isFlipped; }
        if(!isLocalPlayer){ return ; }

        // Needs correction
        if(Input.GetKeyDown(KeyCode.A)){
            SetFlipState(false);
        }
        if(Input.GetKeyDown(KeyCode.D)){
            SetFlipState(true);
        }
        syncSpriteRenderer.flipX = isFlipped;
    }
    
    /// <summary> Set Flip state across network </summary>
    [Command] private void SetFlipState(bool newState){
        isFlipped = newState;
    }

}
