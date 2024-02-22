using DefineHelper;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Source")]
    [SerializeField] AudioSource MusicSource;
    [SerializeField] AudioSource sfxSource;
    [SerializeField] AudioSource uiSource;
    [SerializeField] AudioSource monsterSource;
    [SerializeField] AudioSource HitSource;
    [SerializeField] AudioSource monsterHitSource;

    [Header("Audio Clip")]
    [Header("BGM")]
    [SerializeField] AudioClip LobbyMusic;
    [SerializeField] AudioClip IngameMusic;
    [Header("Player")]
    public AudioClip Attack1;
    public AudioClip Attack2;
    public AudioClip Death;
    public AudioClip Qskill;
    public AudioClip Eskill;
    public AudioClip Rskill;
    public AudioClip Hit;

    [Header("UI")]
    public AudioClip BtnClick;
    public AudioClip buyClick;
    public AudioClip sellClick;
    public AudioClip invenClick;

    [Header("Monster")]
    public AudioClip mush_slimeAttack;
    public AudioClip mush_slimedead;
    public AudioClip GnollAttack;
    public AudioClip Gnoalldead;
    public AudioClip Growling;

    private void Awake()
    {
        // SoundManager �ν��Ͻ��� �̹� �ִ��� Ȯ��, �� ���·� ����
        if (Instance == null)
            Instance = this;

        // �ν��Ͻ��� �̹� �ִ� ��� ������Ʈ ����
        else if (Instance != this)
            Destroy(gameObject);

        // �̷��� �ϸ� ���� scene���� �Ѿ�� ������Ʈ�� ������� �ʽ��ϴ�.
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        SetMusic(DataManager.Instance.SceneState());
        SetSfxMute();
        SetSfxSlider();
    }

    public void SetMusic(SceneType type)
    {
        if (type == SceneType.LobbyScene)
            MusicSource.clip = LobbyMusic;
        else if (type == SceneType.IngameScene)
            MusicSource.clip = IngameMusic;

        MusicSource.volume = PlayerPrefs.GetFloat("bgmVolume");
        if (Convert.ToBoolean(PlayerPrefs.GetInt("bgmMute")))
            MusicSource.mute = true;
        else
        {
            MusicSource.mute = false;
            MusicSource.Play();
        }
    }
    public void SetMusicMute()
    {
        if (Convert.ToBoolean(PlayerPrefs.GetInt("bgmMute")))
            MusicSource.mute = true;
        else
        {
            MusicSource.mute = false;
            MusicSource.Play();
        }
    }
    public void SetMusicSlider()
    {
        MusicSource.volume = PlayerPrefs.GetFloat("bgmVolume");
    }
    public void SetSfxMute()
    {
        if (Convert.ToBoolean(PlayerPrefs.GetInt("sfxMute")))
        {
            sfxSource.mute = true;
            uiSource.mute = true;
            monsterSource.mute = true;
        }
        else
        {
            sfxSource.mute = false;
            uiSource.mute = false;
            monsterSource.mute = false;
        }
    }

    public void SetSfxSlider()
    {
        sfxSource.volume = PlayerPrefs.GetFloat("sfxVolume");
        uiSource.volume = PlayerPrefs.GetFloat("sfxVolume");
        monsterSource.volume = PlayerPrefs.GetFloat("sfxVolume");
    }

    public void SfxPlay(AudioClip clip)
    {
        sfxSource.clip = clip;
        sfxSource.Play();
    }

    public void UiPlay(AudioClip clip)
    {
        uiSource.clip = clip;
        uiSource.Play();
    }

    public void monsterPlay(AudioClip clip)
    {
        monsterSource.clip = clip;
        monsterSource.Play();
    }

    public void HitPlay(AudioClip clip)
    {
        HitSource.clip = clip;
        HitSource.Play();
    }

    public void monsterHitPlay(AudioClip clip)
    {
        monsterHitSource.clip = clip;
        monsterHitSource.Play();
    }
}
