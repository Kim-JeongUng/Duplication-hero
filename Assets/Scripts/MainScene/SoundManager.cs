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
    }//싱글톤

    [SerializeField] GameObject BGM;
    [SerializeField] GameObject SFX;
    private AudioSource bgmPlayer;
    private AudioSource sfxPlayer;

    public float masterVolumeBGM = 1f;
    public float masterVolumeSFX = 1f;

    [SerializeField]
    private AudioClip mainBgmAudioClip;   //메인씬 bgm
    [SerializeField]
    private AudioClip gameBgmAudioClip; //게임씬 bgm
    [SerializeField]
    private AudioClip[] sfxAudioClips;    //효과음
    //효과음 딕셔너리
    Dictionary<string, AudioClip> audioClipsDic = new Dictionary<string, AudioClip>();
    
    private void Awake() {
        if(instance != this){
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);    //dontdestroy

        bgmPlayer = BGM.GetComponent<AudioSource>();
        sfxPlayer = SFX.GetComponent<AudioSource>();

        foreach(AudioClip audioClip in sfxAudioClips){
            audioClipsDic.Add(audioClip.name, audioClip);
        }
    }

    //효과음 재생(이름 필수, 볼륨 옵션)
    public void PlaySFXSound(string name, float volume = 1f){
        if(audioClipsDic.ContainsKey(name) == false){
            Debug.Log(name + " 컨테이너에 포함 안됨.");
            return;
        }
        sfxPlayer.PlayOneShot(audioClipsDic[name], volume * masterVolumeSFX);
    }

    //BGM 재생(볼륨 옵션)
    public void PlayBGMSound(float volume = 1f){
        bgmPlayer.loop = true;  //계속 반복
        bgmPlayer.volume = volume * masterVolumeBGM;

        //씬에 맞는 bgm 재생
        if(SceneManager.GetActiveScene().name == "MainScene"){
            bgmPlayer.clip = mainBgmAudioClip;
            bgmPlayer.Play();
        }
        else if(SceneManager.GetActiveScene().name == "GameScene"){
            bgmPlayer.clip = gameBgmAudioClip;
            bgmPlayer.Play();
        }
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
