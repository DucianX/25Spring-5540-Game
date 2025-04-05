using System.Collections.Generic;
using UnityEngine;

public class ChestManagerPolished : MonoBehaviour
{

    public GameObject ballChestPrefab;
    public int ammoPickupTotal;
    public GameObject ammoPickupPrefab;
    public int shieldPickupTotal;
    public GameObject shieldPickupPrefab;

    void Start()
    {
        // find all breakables in the scene
        var allBreakables = FindObjectsByType<Breakable>(FindObjectsInactive.Include, FindObjectsSortMode.None);

        // shuffle the array
        for (int i = allBreakables.Length - 1; i > 0; i--)
        {
            int j = UnityEngine.Random.Range(0, i + 1);
            (allBreakables[j], allBreakables[i]) = (allBreakables[i], allBreakables[j]);
        }

        int set = 0;
        int setAmmo = 0;
        int setShield = 0;
        
        if (allBreakables.Length > 0)
        {
            allBreakables[0].PutChest(ballChestPrefab);
            set++;
        }

        while (set < allBreakables.Length)
        {
            if (setShield < shieldPickupTotal)
            {
                allBreakables[set].PutItem(shieldPickupPrefab);
                setShield++;
                set++;
                continue;
            }

            if (setAmmo < ammoPickupTotal)
            {
                allBreakables[set].PutItem(ammoPickupPrefab);
                setAmmo++;
                set++;
                continue;
            }

            break;
        }
    }
    
}
