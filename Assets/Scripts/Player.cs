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
    /// <summary> Health class </summary>
    [System.Serializable]
    public class Health{
        [SyncVar] private int maxHealth;        
        [SyncVar] private int healthValue;

        /* 
            /Constructor\
            Parameters | Type | Description
            -----------+------+----------------------
            maxHealth  |  int | player maximum health

            Make instance of Health with maxHealth
        */
        /// <summary> Health constructor </summary>
        /// <param name="_maxHealth"> Maximum player health </param>
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

        /// <summary> Manage player health </summary>
        /// <param name="damage"> damage received by player < 0:Attack > 0 : Heal </param>
        [Command] public void HealthManager(int damage){
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
        /* Getter function */
        public int GetMaxHealth(){ return maxHealth; }
        public int GetHealthValue(){ return healthValue; }
        /* Setter functions */
        public void SetHealthValue(int health){ healthValue = health; }
    }

    /* 
        Player's strength class

        Variable     | Type  | Description
        -------------+-------+--------------
        damageValue  | int   | player attack value
    */ 
    ///<summary> Strength class </summary>
    public class Strength{
        private int damageValue;
        /* 
            /Constructor\
            Parameters | Type | Description
            -----------+------+----------------------
            _damage    |  int | player attack value

            Make instance of Strength with damage
        */
        /// <summary> Strength constructor </summary>
        /// <param name="damage"> player damage </param> 
        public Strength(int _damage){
            damageValue = _damage;
        }
        /* Getter functions */
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
    /// <summary> Speed class </summary>
    [System.Serializable]
    public class Speed{
        private float runningSpeed;
        private float speedValue;
        
        /// <summary> Speed constructor </summary>
        /// <param name="_runningSpeed"> player speed </param>
        public Speed(float _runningSpeed){
            runningSpeed = _runningSpeed;
        }
        /* Getter functions */
        public float GetSpeedValue(){ return speedValue; }
        /* Setter functions */
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
    /// <summary> Level info </summary>
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
    /// <summary> Player movement class </summary>
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
        /// <summary> Movement constructor </summary>
        public Movement(){
            isRunning = false;
            leftOrientation = 0;
            leftOrientation = -1;
        }
        /* 
            Moves transform at speed, assigns orientation and flips player 
        */
        /// <summary> Move player transform at speed </summary>
        /// <param name="speed"> player speed </param>
        /// <param name="transform"> player transform </param>
        public void Move(Speed speed, Transform transform){
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");

            Vector3 direction = new Vector2(horizontal, vertical);
            direction = direction.normalized * speed.GetSpeedValue() * Time.fixedDeltaTime;
            isRunning = direction.magnitude != 0;
            transform.position += direction;
            if (direction.normalized.x > 0){ leftOrientation = 1; }
            else if (direction.normalized.x < 0){ leftOrientation = -1; }
            upOrientation = (int) direction.normalized.y;
            
        }
        /* Getter functions */
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
    /// <summary> playerUI class </summary>
    public class PlayerUI{
        [SyncVar] [SerializeField] private Slider healthSlider;
        [SyncVar] private Color sliderColor;
        [SerializeField] private GameObject inventoryPanel;
        private KeyCode activationKey;
        private bool invActivated;
        [SerializeField] private PlaceHolder[] placeHolders;
        /* 
            /Constructor\
            Variables    | Type   | Description
            health       | Health | contains all info about player health
            healthSlider | Slider | the slider to show in UI
            invActivated | bool   | checks if inventory is activated
        */
        /// <summary> PlayerUI constructor </summary>
        /// <param name="health"> player health </param>
        /// <param name="healthSlider"> health slider UI </param>
        /// <param name="inventoryPanel"> inventory gameobject </param>
        /// <param name="player"> current player instance </param>
        public PlayerUI(Health health, Slider healthSlider, GameObject inventoryPanel, Player player){
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
            placeHolders = new PlaceHolder[24];
            placeHolders = inventoryPanel.GetComponentsInChildren<PlaceHolder>();
            foreach(PlaceHolder ph in placeHolders){ ph.SetPlayer(player); }
        }

        /// <summary> Sets health slider value </summary> 
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

        /* Getter functions */
        public KeyCode GetInventoryActivationKey(){ return activationKey; }
        public bool GetInvActivated(){ return invActivated; }
        /// <summary> Adds item to inventory UI </summary>
        /// <param name="item"> item to add to inventory </param>
        /// <param name="player"> current player instance </param>
        public void AddItemToInventUI(Item item, Player player){
            foreach(PlaceHolder ph in placeHolders){
                if(ph.isEmpty){
                    GameObject itemInstance = Instantiate(item.GetItemPicture(), ph.transform);
                    item.SetUser(player);
                    ph.SetPlayer(player);
                    ph.SetButton(itemInstance.GetComponentInChildren<Button>());
                    // To Change
                    if(item.TryGetComponent<Heal>(out Heal heal)){
                        ph.button.onClick.AddListener(heal.HealManager);
                        heal.SetObjectHolder(ph);
                    }
                    
                    ph.isEmpty = false;
                    return ; 
                }
            }
            
        }
        /// <summary> Show Inventory </summary>
        public void ActivateInventory(){ 
            inventoryPanel.SetActive(true);
            invActivated = true;
        }

        /// <summary> Hide Inventory </summary>
        public void DeActivateInventory(){ 
            invActivated = false;
            inventoryPanel.SetActive(false); 
        }
    }

    /// <summary> Inventory class </summary>
    [System.Serializable]
    public class Inventory{
        public List<Item> itemsList;
        public int maxNumItems;
        
        /// <summary> Inventory constructor </summary>
        /// <param name="maxNumItems"> Max Number of items in inventory </param>
        public Inventory(int maxNumItems){
            itemsList = new List<Item>();
            this.maxNumItems = maxNumItems;
        }

        /// <summary> Adds item to player's inventory </summary>
        /// <param name="player"> current player instance </param>
        public void Add(Item item, Player player){
            if(itemsList.Count >= maxNumItems){
                Debug.LogWarning("Cant add full inventory");
                return ;
            }
            itemsList.Add(item);
            item.SetUser(player);
        }

        /// <summary> Remove item from player's inventory </summary>
        /// <param name="item"> item to remove </param>
        public void Remove(Item item){
            foreach(Item i in itemsList){
                if(i == item){
                    itemsList.Remove(item);
                    return ;
                }
            }
        }
        
        /// <summary> Checks if player has item in inventory </summary>
        /// <param name="item"> item to look for </param>
        public bool Contains(Item item){
            foreach(Item i in itemsList){
                if(i == item){ return true; }
            }
            
            return false;
        }
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
    [SerializeField] private PlayerUI playerUI;
    private string cat = "Archer";
    [SerializeField] private GameObject itemPrefab; 
    [SerializeField] private Transform itemsLeftParent, itemsRightParent;
    [SerializeField] private Item currentEquippedItem;
    [SerializeField] private GameObject currentItemGameObj;
    [SerializeField] private Transform cam;
    [SerializeField] private Inventory inventory;
    [SerializeField] private Item pickableItem;
    private bool canPickItem;
    /* Getter functions */
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

        inventory = new Inventory(24);
        canPickItem = false;

        playerUI = new PlayerUI(health, healthSlider, invPanel, this);
        
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
        
        if(canPickItem && Input.GetKeyDown(KeyCode.E)){
            inventory.Add(pickableItem, this);
            playerUI.AddItemToInventUI(pickableItem, this);
            pickableItem.gameObject.SetActive(false);
        }

        if(Input.GetKeyDown(KeyCode.H)){
            health.HealthManager(-200);
            Debug.Log(health.GetHealthValue());
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
        if(other.CompareTag("Item")){
            
            pickableItem = other.GetComponent<Item>();
            canPickItem = true;
        
        }
        Debug.Log(other.gameObject.name + " : " + gameObject.name + " : " + Time.time);
        if(!other.CompareTag("Ammo")){ return ; }
    
        Ammo ammo = other.GetComponent<Ammo>();
        health.HealthManager(ammo.GetDamage());
        Destroy(other.gameObject);
    }
    private void OnTriggerExit2D(Collider2D other) {
        if(!isLocalPlayer){ return ; }
        if(other.CompareTag("Item")){
            pickableItem = null;
            canPickItem = false;
        }
    }
    // /* 
    //     Function to equip an item to player
    //     (takes item's prefab)
    // */

    // /// <summary> Equip  </summary>
    // private void EquipItem(GameObject item){
    //     if(!item.CompareTag("Item")){ return ;}
    //     currentItemGameObj = Instantiate(item, itemsLeftParent.transform.position, itemsLeftParent.rotation, itemsLeftParent);
    //     currentEquippedItem = currentItemGameObj.GetComponent<Item>();

    // }
    // private static void Die(GameObject player){
    //     gameController.SwitchToDungeonCamera();
    //     // Death Animation
    //     gameController.SetHasPlayer();
    //     Destroy(player);
    // }

}
