using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Credits : MonoBehaviour
{
    public void back()
    {
        SceneManager.LoadScene("Menu");
    }

    public void resources_DawnLike()
    {
        Application.OpenURL("https://opengameart.org/content/dawnlike-16x16-universal-rogue-like-tileset-v181");
    }
}
