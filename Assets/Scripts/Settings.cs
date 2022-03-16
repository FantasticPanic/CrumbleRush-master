using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    [SerializeField]
     AudioMixer masterMixer;
    [SerializeField]
    private GameObject soundSlider;
    [SerializeField]
    private GameObject settingsContainer;

    // Start is called before the first frame update
    void Start()
    {
        settingsContainer.SetActive(false);
        float savedVol = PlayerPrefs.GetFloat("savedVol");

        soundSlider.GetComponent<Slider>().value = savedVol; 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetSoundSlider()
    {
        if (soundSlider.GetComponent<Slider>().value == soundSlider.GetComponent<Slider>().minValue)
        {
            masterMixer.SetFloat("soundsVol", -1000);
        }

        else
        {
            masterMixer.SetFloat("soundsVol", soundSlider.GetComponent<Slider>().value);
        }
        PlayerPrefs.SetFloat("savedVol", soundSlider.GetComponent<Slider>().value);
    }

    public void TurnOn()
    {
        UIManager.Instance.TurnOn(transform.GetChild(0).gameObject);
    }

    public void TurnOff()
    {
        UIManager.Instance.TurnOff(transform.GetChild(0).gameObject);
    }
      
}
