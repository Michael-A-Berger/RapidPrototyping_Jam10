using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public void Play()
    {
        SceneManager.LoadScene("test_platforming", LoadSceneMode.Single);
    }

    public void Credits()
    {
        SceneManager.LoadScene("Credits", LoadSceneMode.Single);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
