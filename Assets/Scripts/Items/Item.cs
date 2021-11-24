using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] private GameObject itemPicture;
    [SerializeField] private PlaceHolder objectHolder;
    [SerializeField] protected Player userPlayer;
    public GameObject GetItemPicture(){ return itemPicture; }

    /// <summary> Sets Object's PlaceHolder </summary>
    /// <param name="objectHolder"> placeholder </param>
    public void SetObjectHolder(PlaceHolder objectHolder){
        this.objectHolder = objectHolder;
    }

    ///<summary> Sets item's User </summary>
    /// <param name="player"> player to set player to </param>
    public void SetUser(Player player){
        userPlayer = player;
    }

    /// <summary> Function executed after using item </summary>
    public void FinishUseObject(){
        objectHolder.isEmpty = true;
        Destroy(objectHolder.transform.GetChild(0).gameObject);
        userPlayer = null;
    }
}
