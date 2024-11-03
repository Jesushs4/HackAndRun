using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] levelClips;

    private string currentScene = "";


    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            //assign the static instance to the object
            Instance = this;
            //maintain this instance to the next scene
            DontDestroyOnLoad(this.gameObject);
        }

    }

    private void Update()
    {
        
        if (currentScene != "")
        {
            if (currentScene != SceneManager.GetActiveScene().name) {
                

                PlayMusicLevel(levelClips[SceneManager.GetActiveScene().buildIndex-1]);
            }
        }
        currentScene = SceneManager.GetActiveScene().name;
    }


    private void PlayMusicLevel(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.Play();
    }



}