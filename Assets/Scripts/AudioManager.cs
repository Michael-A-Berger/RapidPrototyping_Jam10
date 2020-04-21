using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // Public Properties
    public List<AudioContainer> audioSources;

    // Private Properties
    private Dictionary<string, int> listCipher = new Dictionary<string, int>();

    // Start is called before the first frame update
    void Start()
    {
        // foreach (AudioContainer container in audioSources)
        // {
        //     container.source = gameObject.AddComponent<AudioSource>();
        //     container.source.clip = container.clip;
        //     container.source.volume = container.volume;
        //     container.source.loop = container.repeating;
        //     if (container.startOnSceneLoad)
        //         container.source.Play();
        // }

        for (int num = 0; num < audioSources.Count; num++)
        {
            audioSources[num].source = gameObject.AddComponent<AudioSource>();
            AudioContainer container = audioSources[num];
            container.source.clip = container.clip;
            container.source.volume = container.volume;
            container.source.loop = container.repeating;
            if (container.startOnSceneLoad)
                container.source.Play();
            listCipher[container.name] = num;
        }
    }

    // PlayAudio()
    public void PlayAudio(string audioName)
    {
        // bool audioFound = false;

        if (!listCipher.ContainsKey(audioName))
        {
            Debug.LogError("\t[ AudioManager ] could not find container with name \"" + audioName + "\" !");
        }
        else
        {
            int index = listCipher[audioName];
            if (audioSources[index].source.isPlaying)
                audioSources[index].source.Stop();
            audioSources[index].source.Play();
        }
        // for (int num = 0; num < audioSources.Count; num++)
        // {
        //     if (audioName == audioSources[num].name)
        //     {
        //         if (audioSources[num].source.isPlaying)
        //             audioSources[num].source.Stop();
        //         audioSources[num].source.Play();
        //         num = audioSources.Count;
        //         audioFound = true;
        //     }
        // }
        // 
        // if (!audioFound)
        //     Debug.LogError("\t[ AudioManager ] could not find container with name \"" + audioName + "\" !");
    }

    // StopAudio()
    public void StopAudio(string audioName)
    {
        // bool audioFound = false;

        if (!listCipher.ContainsKey(audioName))
        {
            Debug.LogError("\t[ AudioManager ] could not find container with name \"" + audioName + "\" !");
        }
        else
        {
            int index = listCipher[audioName];
            if (audioSources[index].source.isPlaying)
                audioSources[index].source.Stop();
        }

        // for (int num = 0; num < audioSources.Count; num++)
        // {
        //     if (audioName == audioSources[num].name)
        //     {
        //         if (audioSources[num].source.isPlaying)
        //             audioSources[num].source.Stop();
        //         audioFound = true;
        //         num = audioSources.Count;
        //     }
        // }
        // 
        // if (!audioFound)
        //     Debug.LogError("\t[ AudioManager ] could not find container with name \"" + audioName + "\" !");
    }

    // IsPlaying()
    public bool IsPlaying(string audioName)
    {
        bool isPlaying = false;

        if (!listCipher.ContainsKey(audioName))
        {
            Debug.LogError("\t[ AudioManager ] could not find container with name \"" + audioName + "\" !");
        }
        else
        {
            int index = listCipher[audioName];
            isPlaying = audioSources[index].source.isPlaying;
        }

        // bool audioFound = false;
        // for (int num = 0; num < audioSources.Count; num++)
        // {
        //     if (audioName == audioSources[num].name)
        //     {
        //         isPlaying = audioSources[num].source.isPlaying;
        //         audioFound = true;
        //         num = audioSources.Count;
        //     }
        // }
        // 
        // if (!audioFound)
        //     Debug.LogError("\t[ AudioManager ] could not find container with name \"" + audioName + "\" !");

        return isPlaying;
    }
}

[System.Serializable]
public class AudioContainer
{
    // Public Properties
    public string name = "";
    public AudioClip clip;
    [Range(0, 1)]
    public float volume = 1;
    public bool repeating = false;
    public bool startOnSceneLoad = false;
    [HideInInspector]
    public AudioSource source;
}