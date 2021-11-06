using UnityEngine;

public class Item : MonoBehaviour
{
    [System.Serializable]
    public class AllPositions{
        public Vector3 stand;
        public Vector3 down;
        public Vector3 run;
    }
    public AllPositions allPositions;
    private bool isOn;
    [SerializeField] private SpriteRenderer spriteRenderer;
    public bool GetIsOn(){ return isOn; }
    public void SetIsOn(bool val){ isOn = val; }
    public void FlipItem(bool val){ 
        spriteRenderer.flipX = val;
    }
    public SpriteRenderer GetSpriteRenderer(){ return spriteRenderer; }
    void Start(){
        SetIsOn(false);
    }
}
