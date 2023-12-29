using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is a meta-list of all collectibles that can be found within our game.
/// We need this so that we can get a collectible by its identifier, and use it at
/// different locations. For example when displaying all collectibles that where
/// already found.
/// </summary>
[CreateAssetMenu(fileName = "new Collectibles List", menuName = "Our Game/Create new Collectibles List", order = 0)]
public class CollectiblesList : ScriptableObject
{
    public List<CollectibleData> allCollectibles = new List<CollectibleData>();

    public CollectibleData GetDataByIdentifier(string identifier)
    {
        for (int index = 0; index < allCollectibles.Count; index++)
        {
            if (allCollectibles[index].identifier == identifier)
                return allCollectibles[index];
        }

        return null;
    }
}
