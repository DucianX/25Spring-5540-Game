using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakable : MonoBehaviour
{

    public GameObject cratePieces;
    public bool hasChest;
    public bool hasLoot;
    private GameObject _chest;
    private GameObject _lootItem;
    Breakable[] allBreakables;
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Projectile")) {
            Instantiate(cratePieces, transform.position, transform.rotation);
            var lootSpawnPosition = transform.position;
            //lootSpawnPosition.y += 1;
            
            if (hasChest) {
                Instantiate(_chest, lootSpawnPosition, transform.rotation);
            }

            if (hasLoot) {
                Instantiate(_lootItem, lootSpawnPosition, transform.rotation);
            }

            Destroy(gameObject);
        }
    }

    public void PutItem(GameObject lootItem) {
        hasLoot = true;
        _lootItem = lootItem;
    }

    public void PutChest(GameObject chestItem) {
        hasChest = true;
        _chest = chestItem;
    }

    
}
