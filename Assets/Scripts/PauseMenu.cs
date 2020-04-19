using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject menuPanel;
    public PlayerMover player;
    private bool isPaused;

    void Start()
    {
        menuPanel.SetActive(false);
        isPaused = false;
    }
    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(menuPanel.activeSelf)
            {
                UnpauseGame();
            }
            else
            {
                PauseGame();
            }
        }
    }
    public void MainMenu()
    {
        SceneManager.LoadScene("Scenes/Menu", LoadSceneMode.Single);
        UnpauseGame();
    }

    public void Resume()
    {
        menuPanel.SetActive(false);
        UnpauseGame();
    }

    private void UnpauseGame()
    {
        menuPanel.SetActive(false);
        Time.timeScale = 1;
        player.SetInputEnabled(true);
    }

    private void PauseGame()
    {
        menuPanel.SetActive(true);
        Time.timeScale = 0;
        player.SetInputEnabled(false);
    }
}
