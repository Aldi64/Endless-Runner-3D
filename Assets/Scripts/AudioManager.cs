using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    [Header("Clips")]
    [SerializeField] private AudioClip coinClip;
    [SerializeField] private AudioClip gameOverClip;

    [Header("Volume")]
    [SerializeField, Range(0f, 1f)] private float coinVolume = 0.7f;
    [SerializeField, Range(0f, 1f)] private float gameOverVolume = 1f;

    private void Awake() => Instance = this;

    public void PlayCoin() => Play(coinClip, coinVolume);
    public void PlayGameOver() => Play(gameOverClip, gameOverVolume);
    public void StopMusic() => musicSource.Stop();

    private void Play(AudioClip clip, float volume)
    {
        if (clip != null) sfxSource.PlayOneShot(clip, volume);
    }
}