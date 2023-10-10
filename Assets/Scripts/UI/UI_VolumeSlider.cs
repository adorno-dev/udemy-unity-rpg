using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class UI_VolumeSlider : MonoBehaviour
{
    public Slider slider;
    public string parameter;
    
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private float multiplier;

    public void SliderValue(float value) => audioMixer.SetFloat(parameter, Mathf.Log10(value) * multiplier);

    public void LoadSlider(float value)
    {
        if (value >= 0.0001f)
            slider.value = value;
    }
}
