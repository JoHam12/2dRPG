using UnityEngine;
using System.Collections.Generic;


public class GameController : MonoBehaviour
{
    [SerializeField] private Transform spwanPoint;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Camera playerCamera, dungeonCamera;
    private List<GameObject> players;
    private int numberOfPlayers, maxNumOfPlayers = 2;
    private bool hasPlayers;
    void Start(){
        hasPlayers = false;
        dungeonCamera.enabled = true;
        numberOfPlayers = 0;
    }
    void FixedUpdate(){
        
        if(playerPrefab == null || spwanPoint == null){ return ;}
        hasPlayers = numberOfPlayers >= maxNumOfPlayers;
        if(Input.GetKeyDown(KeyCode.Space) && !hasPlayers){
            players[numberOfPlayers] = Instantiate(playerPrefab, spwanPoint.position, spwanPoint.rotation);
            numberOfPlayers += 1;
            playerCamera = players[numberOfPlayers].GetComponentInChildren<Camera>();
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


}
