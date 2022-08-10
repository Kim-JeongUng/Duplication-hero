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
    }//�̱���

    [SerializeField] GameObject BGM;
    [SerializeField] GameObject SFX;
    private AudioSource bgmPlayer;
    private AudioSource sfxPlayer;

    public float masterVolumeBGM = 1f;
    public float masterVolumeSFX = 1f;

    [SerializeField]
    private AudioClip mainBgmAudioClip;   //���ξ� bgm
    [SerializeField]
    private AudioClip gameBgmAudioClip; //���Ӿ� bgm
    [SerializeField]
    private AudioClip[] sfxAudioClips;    //ȿ����
    //ȿ���� ��ųʸ�
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

    //ȿ���� ���(�̸� �ʼ�, ���� �ɼ�)
    public void PlaySFXSound(string name, float volume = 1f){
        if(audioClipsDic.ContainsKey(name) == false){
            Debug.Log(name + " �����̳ʿ� ���� �ȵ�.");
            return;
        }
        sfxPlayer.PlayOneShot(audioClipsDic[name], volume * masterVolumeSFX);
    }

    //BGM ���(���� �ɼ�)
    public void PlayBGMSound(float volume = 1f){
        bgmPlayer.loop = true;  //��� �ݺ�
        bgmPlayer.volume = volume * masterVolumeBGM;

        //���� �´� bgm ���
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
