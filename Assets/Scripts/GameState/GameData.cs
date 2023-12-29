using System;
using System.Collections.Generic;
using UnityEngine.Serialization;

/// <summary>
/// In the GameData, all the data that we want to permanently save and load is collected.
/// Anything that is linked here has to be serializable and a data type that is supported by
/// the chosen json API.
/// </summary>
[System.Serializable]
public class GameData
{
    /// <summary>
    /// The gameplay scene that is currently loaded. Without it, we cannot know which scene to start in when
    /// loading a save game.
    /// </summary>
    public string loadedSceneName = GameStateManager.level1SceneName;
    
    /// <summary>
    /// All data regarding the current state of the player that has to persist from scene to scene and
    /// play session to play session.
    /// </summary>
    [FormerlySerializedAs("playerData")] public PlayerCharacter.Data data;
    
    /// <summary>
    /// All enemy stats saved in a Dictionary, sorted by their unique Guid.
    /// We need them to be able to be referenced by Guid so that we can load the specific enemy instance we need.
    /// </summary>
    public Dictionary<string, Enemy.Data> enemyStatsByGuid = new Dictionary<string, Enemy.Data>();

    /// <summary>
    /// we call this via the enemy, whenever a new enemy should be added to the save game
    /// </summary>
    /// <param name="guid">the guid that is unique for every single enemy</param>
    /// <param name="data">the actual enemy data</param>
    public void AddEnemy(string guid, Enemy.Data data)
    {
        if (!enemyStatsByGuid.ContainsKey(guid))
            enemyStatsByGuid.Add(guid, data); //add new enemy none was yet saved
        else
            enemyStatsByGuid[guid] = data; //update data for existing enemy
    }

    /// <summary>
    /// this is called by the enemies when they initialize 
    /// </summary>
    /// <param name="guid">the guid that is unique for every single enemy</param>
    /// <returns>Returns the data if it already exists. If return value is null, no data was saved yet</returns>
    public Enemy.Data GetEnemyData(string guid)
    {
        if (enemyStatsByGuid.TryGetValue(guid, out var data))
            return data;
        return null; //return null if no value was yet saved
    }
    
    /// <summary>
    /// A list of all unique identifiers for all collected collectibles 
    /// </summary>
    public List<string> collectedCollectiblesIdentifiers = new List<string>();

    /// <summary>
    /// Called whenever a collectible is collected
    /// </summary>
    /// <param name="identifier">The identifier that is unique for every collectible</param>
    public void AddCollectible(string identifier)
    {
        if (collectedCollectiblesIdentifiers.Contains(identifier))
            return;
        collectedCollectiblesIdentifiers.Add(identifier);
    }

    /// <summary>
    /// Called when we try to find out if a collectible was already collected
    /// </summary>
    /// <param name="identifier">The identifier that is unique for every collectible</param>
    /// <returns>Returns true if collectible is collected, otherwise returns false</returns>
    public bool HasCollectible(string identifier)
    {
        return collectedCollectiblesIdentifiers.Contains(identifier);
    }
}