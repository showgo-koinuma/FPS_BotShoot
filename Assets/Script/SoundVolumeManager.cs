using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundVolumeManager : MonoBehaviour
{
    [SerializeField] AudioMixer _audioMixer;
    [SerializeField] Slider _masterSlider;
    [SerializeField] Slider _SESlider;
    [SerializeField] TextMeshProUGUI _masterValueText;
    [SerializeField] TextMeshProUGUI _SEValueText;

    public void SetAudioMixerMaster()
    {
        _audioMixer.SetFloat("Master", _masterSlider.value * 100 - 80);
        _masterValueText.text = ((int)(_masterSlider.value * 100)).ToString();
    }

    //SE
    public void SetAudioMixerSE()
    {
        _audioMixer.SetFloat("SE", _SESlider.value * 100 - 80);
        _SEValueText.text = ((int)(_SESlider.value * 100)).ToString();
    }

}
