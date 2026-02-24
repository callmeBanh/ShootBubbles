using UnityEngine;

public class musicBackground : MonoBehaviour
{
    public static musicBackground instance;    
    private AudioSource audioSource;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            audioSource = GetComponent<AudioSource>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void toggleMusic()
    {
        if(audioSource != null)        {    
        audioSource.mute = !audioSource.mute;
        }
    }

    public bool isMute()
    {
        return audioSource != null && audioSource.mute;
    }



    
}
