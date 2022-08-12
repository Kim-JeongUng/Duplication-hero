using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
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
    [SerializeField] GameObject BTN;
    private AudioSource bgmPlayer;
    private AudioSource sfxPlayer;
    private AudioSource btnPlayer;

    public float masterVolumeBGM = 1f;
    public float masterVolumeSFX = 1f;
    public float masterVolumeBTN = 1f;

    [SerializeField]
    private AudioClip mainBgmAudioClip;   //mainScene bgm
    [SerializeField]
    private AudioClip DesertNormalBgmAudioClip; //gameScene bgm desert normal
    [SerializeField]
    private AudioClip BossBgmAudioClip; //gameScene bgm boss
    
    [SerializeField]
    private AudioClip[] sfxAudioClips;    //soundEffect 
    //soundEffect Dictionary
    Dictionary<string, AudioClip> sfxAudioClipsDic = new Dictionary<string, AudioClip>();
    
    [SerializeField]
    private AudioClip[] btnAudioClips;
    //btn sound Dictionary
    Dictionary<string, AudioClip> btnAudioClipsDic = new Dictionary<string, AudioClip>();

    void Awake() {        
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
            sfxAudioClipsDic.Add(audioClip.name, audioClip);
        }

        btnPlayer = BTN.GetComponent<AudioSource>(); 
        foreach(AudioClip audioClip in btnAudioClips){
            btnAudioClipsDic.Add(audioClip.name, audioClip);
        }
    }

    public void PlayBGMSound(float volume = 0.8f){
        bgmPlayer.loop = true;  //loop
        
        if(SceneManager.GetActiveScene().name == "MainScene"){
            if(bgmPlayer.clip.name != mainBgmAudioClip.name){
                bgmPlayer.clip = mainBgmAudioClip;
                bgmPlayer.Play();
            }
        }
        else if(SceneManager.GetActiveScene().name == "GameScene"){
            volume = 0.2f;
            if(GameManager.instance.gameData.isBossStage){   //boss
                volume = 0.3f;
                bgmPlayer.clip = BossBgmAudioClip;
                bgmPlayer.Play();
            }
            else{
                //desert normal
                if(bgmPlayer.clip.name != DesertNormalBgmAudioClip.name){
                    bgmPlayer.clip = DesertNormalBgmAudioClip;
                    bgmPlayer.Play();
                }
            }
            
        }

        bgmPlayer.volume = volume * masterVolumeBGM;
    }//bgm(name, volume(option))
    
    public void PlaySFXSound(string name, float volume = 1f){
        if(sfxAudioClipsDic.ContainsKey(name) == false){
            Debug.Log(name + " is not contained.");
            return;
        }
        sfxPlayer.PlayOneShot(sfxAudioClipsDic[name], volume * masterVolumeSFX);
    }//soundEffect(name, volume(option))
    
    public void PlayBTNSound(string name, float volume = 1f){
        if(btnAudioClipsDic.ContainsKey(name) == false){
            Debug.Log(name + " is not contained.");
            return;
        }
        btnPlayer.PlayOneShot(btnAudioClipsDic[name], volume * masterVolumeBTN);
    }//btn sound(name, volume(option))

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
