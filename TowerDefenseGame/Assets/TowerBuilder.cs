using UnityEngine;
using UnityEngine.InputSystem.Utilities;

public class TowerBuilder : MonoBehaviour
{
    public GameObject[] towers;
    int selectedTowerIndex;
    public static TowerBuilder Instance {get; private set;}
    public static int enemyCount;
    bool selectedTower = false;
    void Awake()
    {
        // Make sure there is only one instance
        if (Instance != null & Instance != this) {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        Debug.Log(transform.name);
        // Dont destory whe nswitching scences
        DontDestroyOnLoad(gameObject);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SelectTower(int index) {
        if(index < towers.Length && index >= 0) {
            selectedTowerIndex = index;
            selectedTower = true;
        }
        else
        {
            selectedTower = false;
            Debug.LogWarning("Invalid tower index...");
        }
    }

    public GameObject GetSelectedTower() {
        return towers[selectedTowerIndex];
    }

    public bool HasSelectedTower() {
        return selectedTower;
    }
}
