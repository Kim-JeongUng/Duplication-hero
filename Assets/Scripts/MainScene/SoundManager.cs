using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    private static SoundManager instance;
    public static SoundManager Instance{
        get{
            if(instance == null){
                instance = instance = FindObjectOfType<SoundManager>();
            }
            return instance;
        }
    }//singleTone

    [SerializeField] GameObject BGM;
    [SerializeField] GameObject SFX;
    private AudioSource bgmPlayer;
    private AudioSource sfxPlayer;

    public float masterVolumeBGM = 1f;
    public float masterVolumeSFX = 1f;

    [SerializeField]
    private AudioClip mainBgmAudioClip;   //mainScene bgm
    [SerializeField]
    private AudioClip gameBgmAudioClip; //gameScene bgm
    [SerializeField]
    private AudioClip[] sfxAudioClips;    //soundEffect 
    //soundEffect Dictionary
    Dictionary<string, AudioClip> audioClipsDic = new Dictionary<string, AudioClip>();
    
    private void Awake() {
        if (instance == null)
        {
            instance = this;
            Debug.Log("SoundMgr");
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if (instance != this)
                Destroy(this.gameObject);
        }

        bgmPlayer = BGM.GetComponent<AudioSource>();
        sfxPlayer = SFX.GetComponent<AudioSource>();

        foreach(AudioClip audioClip in sfxAudioClips){
            audioClipsDic.Add(audioClip.name, audioClip);
        }
    }

    //soundEffect(name, volume(option))
    public void PlaySFXSound(string name, float volume = 1f){
        if(audioClipsDic.ContainsKey(name) == false){
            Debug.Log(name + " is not contained.");
            return;
        }
        sfxPlayer.PlayOneShot(audioClipsDic[name], volume * masterVolumeSFX);
    }

    //bgm(volume(option))
    public void PlayBGMSound(float volume = 1f){
        bgmPlayer.loop = true;  //loop
        bgmPlayer.volume = volume * masterVolumeBGM;

        if(SceneManager.GetActiveScene().name == "MainScene"){
            bgmPlayer.clip = mainBgmAudioClip;
            bgmPlayer.Play();
        }
        else if(SceneManager.GetActiveScene().name == "GameScene"){
            bgmPlayer.clip = gameBgmAudioClip;
            bgmPlayer.Play();
        }
    }

    void OnEnable(){ //델리게이트 체인 추가
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode){
        Debug.Log("OnSceneLoaded: " + scene.name);
        //Debug.Log(mode);
        PlayBGMSound();
    }
    void OnDisable(){  //델리게이트 체인 제거
        Debug.Log("OnDisable");
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
