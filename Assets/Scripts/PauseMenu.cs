using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject menuPanel;

    void Start()
    {
        menuPanel.SetActive(false);
    }
    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            menuPanel.SetActive(!menuPanel.activeSelf);
            Time.timeScale = menuPanel.activeSelf ? 0 : 1;
        }
    }
    public void MainMenu()
    {
        SceneManager.LoadScene("Scenes/Menu", LoadSceneMode.Single);
    }

    public void Reseme()
    {
        menuPanel.SetActive(false);
        Time.timeScale = 1;
    }
}
