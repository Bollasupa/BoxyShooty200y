using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Selectah : MonoBehaviour {
    
    public GameObject musicPlayer;
    public GameObject soundMaker;

	public void StartLevel01()
    {
        Debug.Log("Clickidy click");
        SceneManager.LoadScene("Level01");
        
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ToggleMusic()
    {
        foreach(AudioSource source in musicPlayer.GetComponents<AudioSource>())
        {
            source.mute = !source.mute;
        }
        
    }

    public void ToggleSound()
    {
        foreach (AudioSource source in soundMaker.GetComponents<AudioSource>())
        {
            source.mute = !source.mute;
        }

    }
}
