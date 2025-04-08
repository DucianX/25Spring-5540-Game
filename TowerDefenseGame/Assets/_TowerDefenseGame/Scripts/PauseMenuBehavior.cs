using UnityEngine;

public class PauseMenuBehavior : MonoBehaviour
{
    public GameObject pauseMenuPanel;
    bool isGamePaused = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
         if(Input.GetKeyDown(KeyCode.Escape)) {
            if (isGamePaused) {
                // resmue
                ResumeGame();
            } else {
                // pause
                PauseGame();
            }
         }
    }

    public void ResumeGame() {
        isGamePaused = false;
        Time.timeScale = 1f;
        pauseMenuPanel.SetActive(false);
    }

    public void PauseGame() {
        isGamePaused = true;
        Time.timeScale = 0f;
        pauseMenuPanel.SetActive(true);
    }

    public void LoadMainMenu() {
        // SceneManager.LoadScene(0);
        Debug.Log("LOADING");
    }

    public void ExitGame() {
        Debug.Log("EXIT");
        Application.Quit();
    }
  }
