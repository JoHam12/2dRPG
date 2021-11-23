using UnityEngine;
using System.Collections.Generic;


public class GameController : MonoBehaviour
{
    public Transform playerTransform;
    private void Update() {
        playerTransform = transform.Find("Player(Clone)");
    }
    

    public Transform GetTarget(){ return playerTransform; }
}
