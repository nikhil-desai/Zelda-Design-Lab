using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance; // Allows other scripts to find this easily
    private AudioSource _source;

    public AudioClip successChime;
    public AudioClip switchSound;

    void Awake()
    {
        Instance = this;
        _source = GetComponent<AudioSource>();
    }

    public void PlaySuccess() => _source.PlayOneShot(successChime);
    public void PlaySwitch() => _source.PlayOneShot(switchSound);
}