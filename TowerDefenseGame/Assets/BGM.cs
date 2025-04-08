using UnityEngine;

public class BackgroundMusicManager : MonoBehaviour
{
    [Header("Audio Settings")]
    public AudioClip backgroundMusic;      // 背景音乐剪辑
    [Range(0f, 1f)]
    public float musicVolume = 0.5f;       // 音量控制
    public bool playOnAwake = true;        // 是否在游戏开始时自动播放
    public bool loop = true;               // 是否循环播放
    
    private AudioSource audioSource;
    
    // 单例模式，确保只有一个BGM管理器存在
    private static BackgroundMusicManager instance;
    public static BackgroundMusicManager Instance
    {
        get { return instance; }
    }
    
    void Awake()
    {
        // 检查是否已存在实例，实现单例模式
        if (instance != null && instance != this)
        {
            // 如果已经存在，销毁这个新创建的实例
            Destroy(gameObject);
            return;
        }
        
        // 设置单例实例并在场景加载时不销毁
        instance = this;
        DontDestroyOnLoad(gameObject);
        
        // 获取或添加AudioSource组件
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        
        // 配置AudioSource
        audioSource.clip = backgroundMusic;
        audioSource.volume = musicVolume;
        audioSource.loop = loop;
        
        // 如果设置为游戏开始时播放，则播放音乐
        if (playOnAwake && backgroundMusic != null)
        {
            PlayMusic();
        }
    }
    
    // 开始播放音乐
    public void PlayMusic()
    {
        if (audioSource != null && backgroundMusic != null && !audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }
    
    // 停止播放音乐
    public void StopMusic()
    {
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }
    
    // 暂停播放音乐
    public void PauseMusic()
    {
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Pause();
        }
    }
    
    // 调整音量
    public void SetVolume(float volume)
    {
        if (audioSource != null)
        {
            musicVolume = Mathf.Clamp01(volume);
            audioSource.volume = musicVolume;
        }
    }
}