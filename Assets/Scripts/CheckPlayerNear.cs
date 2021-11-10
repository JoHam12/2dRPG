using UnityEngine;
public class CheckPlayerNear : MonoBehaviour
{
    [SerializeField] private Ennemy ennemy;
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Player")){
            Debug.Log("Player seen");
            ennemy.SetHasTarget(true);
            ennemy.SetTarget(other.transform);
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if(other.CompareTag("Player")){
            Debug.Log("Player lost");
            ennemy.SetHasTarget(false);
            ennemy.SetTarget(null);
        }
    }
}
