using System.Collections.Generic;
using Entities;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    [Tooltip("List of possible enemies which can be spawned at this spot")]
    public List<GameObject> spawnedEnemies = new List<GameObject>();

    public void spawn(RoomController owner)
    {
        if (Instantiate(
                spawnedEnemies[Random.Range(0, spawnedEnemies.Count)],
                transform.position,
                transform.rotation,
                transform
            )
            .TryGetComponent(out EntityStatsBase enemy))
        {
            owner.addInhabitant(enemy);
            
            enemy.onDeath += () =>
            {
                owner.onInhabitantDeath(enemy);
            };
        }
    }
}
