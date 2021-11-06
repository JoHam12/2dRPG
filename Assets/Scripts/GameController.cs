using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private Transform spwanPoint;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Camera playerCamera, dungeonCamera;
    private GameObject player;
    private bool hasPlayer;
    void Start(){
        hasPlayer = false;
        dungeonCamera.enabled = true;
    }
    void FixedUpdate(){
        if(playerPrefab == null || spwanPoint == null){ return ;}
        if(Input.GetKeyDown(KeyCode.Space) && !hasPlayer){
            player = Instantiate(playerPrefab, spwanPoint.position, spwanPoint.rotation);
            hasPlayer = true;
            playerCamera = player.GetComponentInChildren<Camera>();
            SwitchToPlayerCamera();
        }
    }

    public void SwitchToPlayerCamera(){
        dungeonCamera.gameObject.SetActive(false);
        playerCamera.gameObject.SetActive(true);
    }
    public void SwitchToDungeonCamera(){
        playerCamera.gameObject.SetActive(false);
        dungeonCamera.gameObject.SetActive(true);
    }

    public void SetHasPlayer(){ hasPlayer = false; }

}
