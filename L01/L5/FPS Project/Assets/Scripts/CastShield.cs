using TMPro;
using UnityEngine;

public class CastShield : MonoBehaviour
{
    public GameObject shield;
    public int castCount;
    public TextMeshProUGUI castText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    
        if (!shield) return;
        if (castCount > 0 && Input.GetKeyDown(KeyCode.Q)) {
            shield.SetActive(true);
            castCount -= 1;
        }
        castText.text = castCount.ToString();
    }

    public void addShield() {
        castCount++;
    }
}
