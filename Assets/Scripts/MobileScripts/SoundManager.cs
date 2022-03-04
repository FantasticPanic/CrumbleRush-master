using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{

    //public AudioSource _as;

    public Camera playerCam;

    public bool isSoundPlaying;
    private bool isDeathSoundPlayed;

    public GameObject audioOnIcon;
    public GameObject audioOffIcon;

    public AudioClip[] audioClips;
    public AudioClip gameOverClip;
    public AudioClip pickUpClip;

    private static AudioSource _audioSource = null;

    private static SoundManager soundInstance;

    //allows us to use SoundManager wherever
    public static SoundManager SoundInstance
    {
        get
        {
            //if instance does not exist
            if (soundInstance == null)
            {
                // find the TileManager and made a reference to it
                //we can access this in the PlayerDetection.cs script
                soundInstance = FindObjectOfType<SoundManager>();
            }
            return soundInstance;
        }
    }
    // Start is called before the first frame update

    void Awake()
    {
        
        if (_audioSource != null && _audioSource != this)
        {
            Destroy(this.gameObject);
            return;
        }

        _audioSource = gameObject.GetComponent<AudioSource>();

        
       
    }

    void Start()
    {
         
        PlayerPrefs.GetInt("Muted");

        if (PlayerPrefs.GetInt("Muted",0) == 0)
        {
            isSoundPlaying = true;
            AudioListener.volume = 1;
            //audioOnIcon.SetActive(true);
            //audioOffIcon.SetActive(false);
        }
        else
        {

            isSoundPlaying = false;
            AudioListener.volume = 0;
            //audioOnIcon.SetActive(false);
            //audioOffIcon.SetActive(true);

        }
    

}

    // Update is called once per frame
    void Update()
    {
        
    }

   public void MuteSound()
    {
        if (PlayerPrefs.GetInt("Muted", 0) == 0)
        {
            PlayerPrefs.SetInt("Muted", 1);
            isSoundPlaying = false;
        }
        else
        {
            PlayerPrefs.SetInt("Muted", 0);
            isSoundPlaying = true;
        }
        SetSoundState();
    }


    //turns the "Sound On" and "Sound Off" icon on and off
    private void SetSoundState()
    {
        AudioListener _al = playerCam.GetComponent<AudioListener>();

        if (PlayerPrefs.GetInt("Muted", 0) == 0)
        {

            AudioListener.volume = 1;
            

            audioOnIcon.SetActive(true);
            audioOffIcon.SetActive(false);
        }
        else
        {


            AudioListener.volume = 0;

            audioOnIcon.SetActive(false);
            audioOffIcon.SetActive(true);
            
        }
    }

    public void PlayRandomMoveSound()
    {
        int moveSound = Random.Range(0, audioClips.Length);
        _audioSource.PlayOneShot(audioClips[moveSound]);
    }

    public void PlayGameOverSound()
    {
        if (isDeathSoundPlayed == false)
        {
            _audioSource.PlayOneShot(gameOverClip);
            isDeathSoundPlayed = true;
        }

        else
        {
            isDeathSoundPlayed = true;
        }
    }

    public void PlayPickupSound()
    {
        _audioSource.PlayOneShot(pickUpClip);
    }
}
