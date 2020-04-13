using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public void Play()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
        Debug.Log("Loaded");
    }

    public void Credits()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+2);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
