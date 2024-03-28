using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    [SerializeField] AudioSource RatRoam;
    [SerializeField] AudioSource RatChase;
    [SerializeField] AudioSource MainMenuAmbience;
    [SerializeField] AudioSource GameOver;
    private void Awake()
    {
        Instance = this;
    }

    public void PlayRatRoam()
    {
        if (!RatRoam.isPlaying)
        {
            RatRoam.Play();
            RatChase.Pause();
            MainMenuAmbience.Stop();
            GameOver.Stop();
        }
    }
    public void PlayRatChase()
    {
        if (!RatChase.isPlaying)
        {
            RatRoam.Pause();
            RatChase.Play();
            MainMenuAmbience.Stop();
            GameOver.Stop();
        }
    }
    public void PlayMainMenu()
    {
        if (!MainMenuAmbience.isPlaying)
        {
            RatRoam.Pause();
            RatChase.Pause();
            MainMenuAmbience.Play();
            GameOver.Stop();
        }
    }
    public void PlayGameOver()
    {
        if (!GameOver.isPlaying)
        {
            RatRoam.Pause();
            RatChase.Pause();
            MainMenuAmbience.Stop();
            GameOver.Play();
        }
    }
}
