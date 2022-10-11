using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource music;
    public AudioSource sounds;
    public BossHealth bossHealth;
    public PlayerHealth playerHealth;
    public AudioClip stage1music;
    public AudioClip stage2music;
    public AudioClip winMusic;
    public AudioClip loseMusic;
    void Start()
    {
        bossHealth.StartStageTwo += OnStateChange;
        bossHealth.Killed += OnWin;
        playerHealth.Killed += OnLose;
    }

    public void Play(AudioClip clip){
        sounds.PlayOneShot(clip);
    }

    private void OnStateChange(){
        music.Stop();
        music.clip = stage2music;
        music.Play();
    }
    private void OnWin(){
        music.Stop();
        music.clip = winMusic;
        music.Play();
    }
    private void OnLose(){
        music.Stop();
        music.clip = loseMusic;
        music.Play();
    }
}
