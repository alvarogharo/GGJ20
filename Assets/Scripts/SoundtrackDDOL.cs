using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundtrackDDOL : MonoBehaviour
{
    private static SoundtrackDDOL soundtrack;
    public AudioSource audioChange;
    // Start is called before the first frame update
    void Start ()
    {
        if (!soundtrack)
        {
            soundtrack = this;
            DontDestroyOnLoad(transform.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayClick(){
        audioChange.Play();
    }
}


