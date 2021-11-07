using UnityEngine;
using Mirror;

public class AnimationController : NetworkBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private Animator animator;
    void Start(){
        if(!isLocalPlayer){ return ; }
        player = GetComponent<Player>();
        animator = GetComponent<Animator>();

        animator.SetBool("isRunning", false);
    }

    // Update is called once per frame
    void Update()
    {
        if(!isLocalPlayer){ return ; }
        animator.SetBool("isRunning", player.GetMovement().GetIsRunning());
    }
}
