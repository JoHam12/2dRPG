using UnityEngine;

public class AnimationController : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private Animator animator;
    void Start(){
        player = GetComponent<Player>();
        animator = GetComponent<Animator>();

        animator.SetBool("isRunning", false);
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetBool("isRunning", player.GetMovement().GetIsRunning());
    }
}
