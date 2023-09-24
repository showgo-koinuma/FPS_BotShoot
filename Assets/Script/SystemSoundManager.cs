using UnityEngine;

public class SystemSoundManager : MonoBehaviour
{
    public static SystemSoundManager instance = null;
    AudioSource _audioSouce;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(this.gameObject);
    }

    private void Start()
    {
        _audioSouce = GetComponent<AudioSource>();
    }

    public void PlayOneShotClip(AudioClip audioClip)
    {
        _audioSouce.PlayOneShot(audioClip);
        Debug.Log("colled mesod");
    }
}
