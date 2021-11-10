using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Mirror;

public class Player : NetworkBehaviour
{
    /* 
        All player subclasses 
    */
    
    /* 
        Player's health class
        Variable     | Type  | Description
        -------------+-------+--------------
        maxHealth    |  int  | maximumHealth
        healthValue  |  int  | current health value
    */
    [System.Serializable]
    public class Health{
        private int maxHealth;        
        private int healthValue;

        /* 
            /Constructor\
            Parameters | Type | Description
            -----------+------+----------------------
            maxHealth  |  int | player maximum health

            Make instance of Health with maxHealth
        */
        public Health(int _maxHealth){
            maxHealth = _maxHealth;
            healthValue = maxHealth;
        }
        /* 
            /HealthManager\
            Parameters | Type | Description
            -----------+------+---------------------------
            damage     | int  | damage received by player 
                       |      | < 0 : Attack; > 0 : Heal
            playerobj  | GO   | player's gameObject
        */
        public void HealthManager(int damage){
            if(healthValue + damage >= maxHealth){
                healthValue += damage;
                // Heal
                return ;
            }
            if(healthValue + damage <= 0){
                healthValue = 0;
                // Die(playerobj);
                return ;
            }
            healthValue += damage;
        }
        // Get Maximum Health
        public int GetMaxHealth(){ return maxHealth; }
        // Get Health
        public int GetHealthValue(){ return healthValue; }
    }

    /* 
        Player's strength class

        Variable     | Type  | Description
        -------------+-------+--------------
        damageValue  | int   | player attack value
    */ 
    public class Strength{
        private int damageValue;
        /* 
            /Constructor\
            Parameters | Type | Description
            -----------+------+----------------------
            _damage    |  int | player attack value

            Make instance of Strength with damage
        */
        public Strength(int _damage){
            damageValue = _damage;
        }
        // Get Damage Value
        public int GetDamageValue(){ return damageValue; }
    }

    /*
        Player's speed class 
        Variable     | Type  | Description
        -------------+-------+--------------
        runningSpeed | float | running speed
        walkingSpeed | float | walking speed
        speedValue   | float | current speed value
    */
    [System.Serializable]
    public class Speed{
        private float runningSpeed;
        private float speedValue;
        /* 
            /Constructor\
        */
        public Speed(float _runningSpeed){
            runningSpeed = _runningSpeed;
        }
        public float GetSpeedValue(){ return speedValue; }
        public void SetSpeedValue(bool isRunning){ 
            speedValue = runningSpeed; 
        }
        
    }

    /*
        Player's Level informations
        /LevelInfo\
        Variable  | Type  | Description
        ----------+-------+--------------
        level     |  int  | current player level
        exp       |  int  | current player experience
        maxExp    |  int  | maximum experience for level $level         
    */
    public class LevelInfo{
        private int level;
        private int exp;
        private int maxExp;
        public void ChangeLevel(){ level += 1; }
        public int GetLevel(){ return level; }
        public void SetMaxExp(){ maxExp = (int)Mathf.Pow(level - 1, 2)*100; }
    }
    /* 
        Player's Movement class
        /Movement\
        Variable        | Type  | Description
        ----------------+-------+--------------
        isRunning       | bool  | checks if player is running (isRunning => isWalking)
        isWalking       | bool  | checks if player is walking 
        leftOrientation | int   | value for player horizontal orientation
        upOrientation   | int   | value for player vertical orientation
    */
    public class Movement{
        public bool isRunning;
        /*
            leftOrientation==-1=>right
            leftOrientation==1=>left
            leftOrientation==0=>Middle
            upOrientation==-1=>down
            upOrientation==1=>up
            upOrientation==0=>Middle
        */
        private int leftOrientation, upOrientation;
        public Movement(){
            isRunning = false;
            leftOrientation = 0;
            leftOrientation = -1;
        }
        /* 
            Moves transform at speed, assigns orientation and flips player 
        */
        public void Move(Speed speed, Transform transform){
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");

            Vector3 direction = new Vector2(horizontal, vertical);
            direction = direction.normalized * speed.GetSpeedValue() * Time.deltaTime;
            isRunning = direction.magnitude != 0;
            transform.position += new Vector3(direction.x, direction.y, direction.z);
            if (direction.normalized.x > 0){ leftOrientation = 1; }
            else if (direction.normalized.x < 0){ leftOrientation = -1; }
            upOrientation = (int) direction.normalized.y;
            // Here
            
        }
        public bool GetIsRunning(){ return isRunning; }
        public int GetLeftOrientation(){ return leftOrientation; }
        public int GetUpOrientation(){ return upOrientation; }
    }
        [System.Serializable]
    /* 
        /PlayerUI\
        Variable       | Type    | Description
        ---------------+---------+------------------
        healthSlider   | Slider  | heath's slider 
        slideColor     | Color   | slider's color 
        inventoryPanel | GO      | inventory gameobject
        activationKey  | KeyCode | key that de/activates inventory
    */
    public class PlayerUI{
        [SerializeField] private Slider healthSlider;
        private Color sliderColor;
        [SerializeField] private GameObject inventoryPanel;
        private KeyCode activationKey;
        private bool invActivated;
        /* 
            /Constructor\
            Variables    | Type   | Description
            health       | Health | contains all info about player health
            healthSlider | Slider | the slider to show in UI
            invActivated | bool   | checks if inventory is activated
        */
        public PlayerUI(Health health, Slider healthSlider, GameObject inventoryPanel){
            // Health UI
            sliderColor = new Color(0f, 255f, 0f);
            this.healthSlider = healthSlider;
            healthSlider.gameObject.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = sliderColor;
            healthSlider.maxValue = health.GetMaxHealth();
            healthSlider.value = health.GetMaxHealth();

            // Inventory UI
            activationKey = KeyCode.I;
            this.inventoryPanel = inventoryPanel;
            invActivated = false;
            this.inventoryPanel.SetActive(false);
        }
        public void SetHealthSlider(Health health){
            healthSlider.maxValue = health.GetMaxHealth();
            healthSlider.value = health.GetHealthValue();
            if(health.GetHealthValue()*100/health.GetMaxHealth() >= 66){
                sliderColor = Color.green;
            }
            else if (health.GetHealthValue()*100/health.GetMaxHealth() >= 33){
                sliderColor = Color.yellow;
            }
            else{
                sliderColor = Color.red;
            }
            healthSlider.gameObject.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = sliderColor;
        }

        public KeyCode GetInventoryActivationKey(){ return activationKey; }
        public bool GetInvActivated(){ return invActivated; }
        public void ActivateInventory(){ 
            inventoryPanel.SetActive(true);
            invActivated = true;
        }
        public void DeActivateInventory(){ 
            invActivated = false;
            inventoryPanel.SetActive(false); 
        }
    }

    /* 
        /Resource class\
        Variable         | Type       | Description
        -----------------+------------+--------------
        gameObjectPrefab | GameObject | GameObject's prefab
        num              | int        | number of instances of the resource
        resId            | int        | Id of the resource (Id must be different for all resources) 
    */
    public class Resource{
        private GameObject gameObjectPrefab;
        private int num;
        private int resId;
        public Resource(GameObject gameObject, int num, int resId){
            this.gameObjectPrefab = gameObject;
            this.resId = resId;
            this.num = num;
        }
        public GameObject GetGameObjectPrefab(){ return gameObjectPrefab; }
        public int GetNum(){ return num; }
        public void AddNum(int n){ num += n; }
        public int GetResId(){ return resId; }
        public bool Equ(Resource otherResource){
            return resId == otherResource.GetResId();
        }
    }
    /*  /Inventory Class\ 
        Variable          | Type           | Description
        ------------------+----------------+--------------
        resourcesList     | List<Resource> | List of resources in inventory
        numOfResources    | int            | number of resources 
        numOfAllResources | int            | sum of number of all resources
    */
    public class Inventory{
        private List<Resource> resourcesList;
        private int numOfResources;
        private int numOfAllResources;
        public Inventory(){
            resourcesList = new List<Resource>();
            numOfResources = 0;
            numOfAllResources = 0;
        }
        public Inventory(Resource[] resources, int num){
            resourcesList = new List<Resource>();
            numOfResources = num;
            numOfAllResources = 0;
            for(int i = 0; i < num; i++){
                resourcesList.Add(resources[i]);
                numOfAllResources += resources[i].GetNum();
            }
        }
        public int Contains(Resource resource){
            for(int i = 0; i < numOfResources; i++){
                if(resourcesList[i].Equ(resource) && resourcesList[i].GetNum() > 0){
                    return i;
                }
            }
            return -1;
        }
        public void Add(Resource resource){
            int ind = this.Contains(resource);
            if (ind!=-1){
                resourcesList[ind].AddNum(resource.GetNum());
                numOfResources += 1;
                numOfAllResources += resource.GetNum();
                return ;
            }
            resourcesList.Add(resource);
            numOfResources += 1;
            numOfAllResources += resource.GetNum();
        }
        public void Remove(Resource resource){
            for(int i = 0; i < numOfResources; i++){
                if(resourcesList[i].Equ(resource)){
                    if(resourcesList[i].GetNum() - resource.GetNum() > 0){
                        resourcesList[i].AddNum(-resource.GetNum());
                        numOfResources -= 1; 
                        numOfAllResources -= resource.GetNum();
                        return ;
                    }
                    resourcesList.Remove(resource);          
                    numOfResources -= 1; 
                    numOfAllResources -= resource.GetNum();
                    return ;
                }
            }
        }

        public List<Resource> GetResourcesList(){ return resourcesList; }
        public int GetNumOfResources(){ return numOfResources; }

    }
    /* 
        Player's attributes
    */
    // Player's class (Archer, Assassin, ...)
    private Category category;
    // [SerializeField] static private SpriteRenderer spriteRenderer;
    private Health health;
    private Strength strength;
    private Speed speed;
    private LevelInfo levelInfo;
    private Movement movement;
    [SerializeField] private Slider healthSlider;
    [SerializeField] private GameObject invPanel;
    private PlayerUI playerUI;
    private string cat = "Archer";
    [SerializeField] private GameObject itemPrefab; 
    [SerializeField] private Transform itemsLeftParent, itemsRightParent;
    [SerializeField] private Item currentEquippedItem;
    [SerializeField] private GameObject currentItemGameObj;
    [SerializeField] private Transform cam;

    /* Getters */
    public Category GetCategory(){ return category; }
    public Health GetHealth(){ return health; }
    public Strength GetStrength(){ return strength; }
    public Speed GetSpeed(){ return speed; }
    public LevelInfo GetLevelInfo(){ return levelInfo; }
    public Movement GetMovement(){ return movement; }
    
    void Start()
    {
        if(!isLocalPlayer){ return ; }
        if(cat.Equals("Archer")){ category = new Archer(); }
        if(cat.Equals("Assassin")){ category = new Assassin(); }
        if(cat.Equals("Nomad")){ category = new Nomad(); }
        if(cat.Equals("Necromancian")){ category = new Necromancian(); }
        if(cat.Equals("Warrior")){ category = new Warrior(); }
        if(cat.Equals("Wizard")){ category = new Wizard(); }

        healthSlider = GetComponentInChildren<Slider>();

        // spriteRenderer = GetComponent<SpriteRenderer>();

        health = new Health(100*category.GetConstitution());
        strength = new Strength(20*category.GetEnergy());
        speed = new Speed(2.5f);

        cam = transform.Find("/Main Camera");

        movement = new Movement();
        speed.SetSpeedValue(movement.isRunning);

        playerUI = new PlayerUI(health, healthSlider, invPanel);
        
    }
    void FixedUpdate(){     
        if(!isLocalPlayer){ return ; }
        movement.Move(speed, transform);
        // spriteRenderer.flipX = movement.GetLeftOrientation() > 0;
        cam.position = new Vector3(transform.position.x, transform.position.y, -10);
        playerUI.SetHealthSlider(health);
        if(Input.GetKeyDown(KeyCode.H)){ health.HealthManager(-20); }
        /* Debug */
        // Debug.Log("health : " + health.GetHealthValue());
        // Debug.Log("Energy : " + category.GetEnergy());
        // Debug.Log("Dexterity : " + category.GetDexterity());
        // Debug.Log("Constitution : " + category.GetConstitution());
        // Debug.Log("Wisdom : " + category.GetWisdom());
    }

    private void Update() {
        if(!isLocalPlayer){ return ; }
        
        if(Input.GetKeyDown(KeyCode.E)){
            EquipItem(itemPrefab);
        }
        if(Input.GetKeyDown(playerUI.GetInventoryActivationKey())){
            if(!playerUI.GetInvActivated()){ playerUI.ActivateInventory(); }
            else{ playerUI.DeActivateInventory(); }
        }
        if(currentEquippedItem == null){ return ;}
        // currentEquippedItem.FlipItem(spriteRenderer.flipX);
        if(GetComponent<SpriteRenderer>().flipX){ // hhheh
            currentEquippedItem.transform.SetParent(itemsRightParent);
            currentEquippedItem.transform.localPosition = Vector3.zero;
            return ;
        }
        currentEquippedItem.transform.SetParent(itemsLeftParent);
        currentEquippedItem.transform.localPosition = Vector3.zero;  
              
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(!isLocalPlayer){ return ; }
        Debug.Log(other.gameObject.name + " : " + gameObject.name + " : " + Time.time);
        if(other.CompareTag("Ammo")){
            Ammo ammo = other.GetComponent<Ammo>();
            health.HealthManager(ammo.GetDamage());
        }
        Destroy(other.gameObject);
    }
    /* 
        Function to equip an item to player
        (takes item's prefab)
    */
    private void EquipItem(GameObject item){
        if(!item.CompareTag("Item")){ return ;}
        currentItemGameObj = Instantiate(item, itemsLeftParent.transform.position, itemsLeftParent.rotation, itemsLeftParent);
        currentEquippedItem = currentItemGameObj.GetComponent<Item>();
        currentEquippedItem.SetIsOn(true);

    }
    // private static void Die(GameObject player){
    //     gameController.SwitchToDungeonCamera();
    //     // Death Animation
    //     gameController.SetHasPlayer();
    //     Destroy(player);
    // }

}
