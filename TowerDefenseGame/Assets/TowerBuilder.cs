using UnityEngine;
using UnityEngine.InputSystem.Utilities;

public class TowerBuilder : MonoBehaviour
{
    public GameObject[] towers;
    int selectedTowerIndex;
    public static TowerBuilder Instance {get; private set;}
    public static int enemyCount;
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

    public void SelectTower() {

    }

    public GameObject GetSelectedTower() {
        return null;
    }
}
