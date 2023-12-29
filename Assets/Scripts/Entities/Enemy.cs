using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Enemy : MonoBehaviour
{
    //this is the guid with which we identify the individual enemies.
    //make sure they are all unique, or two enemy data will be saved and loaded as the same enemy!
    //We set this value by code in the OnValidate-Function
    [SerializeField] private string uniqueGuid;
    
    //In this class, we save all variables that we want to persist over play sessions.
    [System.Serializable]
    public class Data
    {
        public SaveableVector3 position;
        public SaveableVector2 velocity;
        public float health = 100;
    }

    [SerializeField] private Data data;
    
    private void Awake()
    {
        //We try to get the enemy data from the GameStateManager.
        Data loadedData = GameStateManager.instance.data.GetEnemyData(uniqueGuid);
        
        if (loadedData != null)
        {
            //if the GameStateManager returned a data object, we set our values from that
            data = loadedData;
            SetupFromData();
        }
        else
        {
            //if nothing was given back, there was no data saved for this enemy, so we add the current values.
            GameStateManager.instance.data.AddEnemy(uniqueGuid, data);
            data.position = transform.position;
        }
    }

    private void SetupFromData()
    {
        transform.position = data.position;
        GetComponent<Rigidbody2D>().velocity = data.velocity;
        if (data.health <= 0)
            Destroy(gameObject);
    }

    private void LateUpdate()
    {
        //we need to make sure all variables are set to the data, so when we save the game state, they will be
        //correctly added to the save game.
        data.position = transform.position;
    }

    //we use this only to move the enemies around randomly.
    //This way, we can see if the enemy position is saved correctly.
    private void FixedUpdate()
    {
        Rigidbody2D rigidBody = GetComponent<Rigidbody2D>();
        if (rigidBody.velocity.magnitude < 1)
            rigidBody.AddForce(Random.insideUnitCircle * 30f);
        data.velocity = rigidBody.velocity;
    }

    //OnValidate is called by unity in the editor only, whenever something within the scene changes
    //we use this to set a new unique guid when we detect that it is empty.
    private void OnValidate()
    {
        if (string.IsNullOrEmpty(gameObject.scene.name))
        {
            //if there scene name saved, it means that the game object is in "prefab mode".
            //it is not yet instantiated (meaning an instance within our scene), so it should not have a
            //unique identifier yet.
            uniqueGuid = "";
        }
        else if (string.IsNullOrEmpty(uniqueGuid))
        {
            //if there is not yet an guid set, lets set a new one.
            uniqueGuid = System.Guid.NewGuid().ToString();
        }
    }

    //this is so we can kill the enemy by touching it with the player character
    public void Kill()
    {
        data.health = 0;
        Destroy(gameObject);
    }
}
