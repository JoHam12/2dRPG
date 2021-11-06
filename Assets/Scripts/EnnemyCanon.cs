using UnityEngine;

public class EnnemyCanon : MonoBehaviour
{

    [SerializeField] private GameObject ammoPrefab;
    private GameObject currentAmmoClone;
    
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space)){
            currentAmmoClone = Instantiate(ammoPrefab, transform.position, transform.rotation);
        }
    }
}
