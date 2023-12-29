using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerCharacter : MonoBehaviour
{
    [System.Serializable]
    public class Data
    {
        public string lastScene;
        public Dictionary<string, SaveableVector3> positionsBySceneName = new Dictionary<string, SaveableVector3>();
        public float health = 100;
        public int strength = 1;
        public int constitution = 1;
        public string name = "player";
    }

   [SerializeField] private Data data;
   [SerializeField] private Color fullHealthColor = Color.green;
   [SerializeField] private Color noHealthColor = Color.red;

    private Vector2 input;
    
    private void Awake()
    {
        var currentStats = GameStateManager.instance.data.data;
        if (currentStats != null)
        {
            //if a set of exists, that means we loaded a save and can take over those values.
            data = currentStats;
            SetupFromData();
        }
        
        GameStateManager.instance.data.data = data;
    }

    private void SetupFromData()
    {
        //because the player can move from scene to scene, we want to load the position for the scene we are currently in.
        //if the player was never in this scene, we keep the default position the prefab is at.
        if (data.positionsBySceneName.TryGetValue(gameObject.scene.name, out var position))
            transform.position = position;
    }

    private void LateUpdate()
    {
        //we have to save the current position dependant on the scene the player is in.
        //this way, the position can be retained across multiple scenes, and we can switch back and forth.
        var sceneName = gameObject.scene.name;
        if (!data.positionsBySceneName.ContainsKey(sceneName))
            data.positionsBySceneName.Add(sceneName, transform.position);
        else
            data.positionsBySceneName[sceneName] = transform.position;
        
        //for test purposes, we fade the player color from a full health color to a no health color,
        //depending on how much health they have left. This way, we can test if the health is correctly
        //saved and retained across scenes.
        GetComponentInChildren<SpriteRenderer>().color = Color.Lerp(noHealthColor, fullHealthColor, data.health / 100);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //with this, we test the loss of health, by removing health whenever the player collides with an enemy
        Enemy enemy = collision.gameObject.GetComponent<Enemy>();
        if (enemy == null)
            return;
        data.health -= 25;
        enemy.Kill(); //we kill all enemies we touch
    }

    //the following code is just so we have a quick and dirty code that can move the player around
    private void Update()
    {
        input.x = Input.GetAxis("Horizontal");
        input.y = Input.GetAxis("Vertical");
        input.Normalize();
    }

    private void FixedUpdate()
    {
        Rigidbody2D rigidBody = GetComponent<Rigidbody2D>();
        rigidBody.velocity = input * 15;
    }
}
