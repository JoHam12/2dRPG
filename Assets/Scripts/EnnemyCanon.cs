using UnityEngine;
using Mirror;

public class EnnemyCanon : NetworkBehaviour
{

    [SerializeField] private GameObject ammoPrefab;
    private GameObject currentAmmoClone;
    [Command]
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space)){
            currentAmmoClone = Instantiate(ammoPrefab, transform.position, transform.rotation);
        }
    }
}
