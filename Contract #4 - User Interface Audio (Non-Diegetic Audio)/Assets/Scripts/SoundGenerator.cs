﻿using UnityEngine;
using UnityEngine.UI;
using System;

public class SoundGenerator : MonoBehaviour
{
    /// <summary>
    /// Each changable variable of the sound has it's own Slider, Text and Value which is used on the UI creation page.
    /// All of the variables are public variables as this makes it much easier for designers to be able to use the program as it is simply drag and drop.
    /// Similarly, the implementation of sliders are in the project to enable designers within the team to be able to edit sounds so that they are suitable
    /// for the project. An audio source is also declared here as this will be the main variable to be used during sound creation.
    /// </summary>

    public Slider frequencySlider;
    public Slider volumeSlider;
    public Slider durationSlider;
    public Slider sampleSlider;
    public AudioClip soundEffect;
    public Text frequencyText;
    public Text durationText;
    public Text volumeText;
    public Text sampleText;
    public InputField saveNameInput;
    public Button saveAsPositive;

    public AudioSource source;

    private int frequencyValue;
    private int sampleValue;
    private int positiveFrequencyValue;
    private int positiveSampleValue;
    private int negativeFrequencyValue;
    private int negativeSampleValue;
    private float durationValue;
    private float negativeDurationValue;
    private float positiveDurationValue;
    private AudioClip positiveSound;
    private AudioClip negativeSound;


    /// <summary>
    /// Within the start function, multiple operations are performed. Firstly, the audio source is stored within the source variable. This variable can be used to directly edit the
    /// audio source. This is needed to perform volume changes etcetera. Other values set within the start function are done to set default values for the different editable factors of the sound.
    /// This is done, mostly, to ensure that the UI looks presentable and does not have strange values. 
    /// </summary>

    private void Start()
    {
        source = GetComponent<AudioSource>();
        source.volume = 0f;
        durationValue = 1.00f;
        sampleValue = 44100;
        sampleText.text = "1000";
    }

    /// <summary>
    /// Actual Generation code for the sound, works with Mathf.Sin in order to calculate the samples. Takes in multiple parameters that are recieved via the UI Sliders. Frequency affects the pitch
    /// of the sound, duration affects the length that the sound will play for, and sample affects the sample rate of the sound. Having this combination of interchangability allows the designer
    /// to make sounds suitable to whatever project they are working on.
    /// </summary>
    /// <param name="frequency"></param>
    /// <param name="duration"></param>
    /// <param name="sample"></param>
    /// <returns></returns>

    private AudioClip CreateToneAudioClip(int frequency, float duration, int sample)
    {
        float sampleDurationSecs = duration;
        int sampleRate = sample;
        int sampleLength = Mathf.FloorToInt(sampleRate * sampleDurationSecs);
        float maxValue = 1f / 4f;

        var audioClip = AudioClip.Create("audioClip", sampleLength, 1, sampleRate, false);

        float[] samples = new float[sampleLength];
        for (var i = 0; i < sampleLength; i++)
        {
            float s = Mathf.Sin(2.0f * Mathf.PI * frequency * ((float)i / (float)sampleRate));
            float v = s * maxValue;
            samples[i] = v;
        }
        audioClip.SetData(samples, 0);
        return audioClip;
    }

    /// <summary>
    /// SaveAsPositive() is used to save the current values of the slider in a variable in order to be able to re-load the exact same configuration at a later time.
    /// This allows for the designers to ensure they can test sounds on buttons as well as being able to make small adjustments whilst still having a copy of the previous.
    /// It simply takes in values, for the variables and then creates an audio clip which is stored for later use.
    /// </summary>

    public void SaveAsPositive()
    {
        positiveFrequencyValue = frequencyValue;
        positiveSampleValue = sampleValue;
        positiveDurationValue = durationValue;
        positiveSound = CreateToneAudioClip(frequencyValue, durationValue, sampleValue);
    }

    /// <summary>
    /// PlayPositive() simply plays the positive sound, this function is attached to the button "Play Positive".
    /// </summary>

    public void PlayPositive()
    {
        source.PlayOneShot(positiveSound);
    }

    /// <summary>
    /// LoadPositive() is used to re-configure the values to what they were whenever the Save function was called. It edits the value of the sliders whilst also calling on the TextUpdate functions
    /// to ensure that the designer see's the original values when the save was called.
    /// </summary>

    public void LoadPositive()
    {
        CreateToneAudioClip(positiveFrequencyValue, positiveDurationValue, positiveSampleValue);
        sampleSlider.value = positiveSampleValue;
        frequencySlider.value = positiveFrequencyValue;
        durationSlider.value = positiveDurationValue;
        SampleRateTextUpdate();
        DurationTextUpdate();
        FrequencyTextUpdate();
    }

    /// <summary>
    /// Values are saved into the negative variables so that they can be loaded whenever requested by the designer. This makes it easier for small adjustments to be made as they can test multiple
    /// sounds whilst still keeping values that have been stored.
    /// </summary>

    public void SaveAsNegative()
    {
        negativeFrequencyValue = frequencyValue;
        negativeSampleValue = sampleValue;
        negativeDurationValue = durationValue;
        negativeSound = CreateToneAudioClip(frequencyValue, durationValue, sampleValue);
    }

    /// <summary>
    /// Simply just plays the saved Negative Sound.
    /// </summary>
    public void PlayNegative()
    {
        source.PlayOneShot(negativeSound);
    }

    /// <summary>
    /// Loads the negative sound from the variables into the actual configuration of the sliders. This will allow for small changes to be made whilst still keeping the original config for the sliders.
    /// It also updates all the UI elements to show the exact values.
    /// </summary>

    public void LoadNegative()
    {
        CreateToneAudioClip(negativeFrequencyValue, negativeDurationValue, negativeSampleValue);
        sampleSlider.value = negativeSampleValue;
        frequencySlider.value = negativeFrequencyValue;
        durationSlider.value = negativeDurationValue;
        SampleRateTextUpdate();
        DurationTextUpdate();
        FrequencyTextUpdate();
    }

    /// <summary>
    /// Changes the volume of the audio source depending on how high or low the slider is.
    /// </summary>

    public void ChangeVolume()
    {
        source.volume = volumeSlider.value;
        VolumeTextUpdate();
    }

    /// <summary>
    /// Function that updates the text whilst also limiting the value to 2dp.
    /// </summary>

    private void VolumeTextUpdate()
    {
        volumeText.text = source.volume.ToString("F2");
    }

    /// <summary>
    /// Changes the sample rate according to how far along the slider is. It then updates the sample text as to match the value on the UI Slider.
    /// </summary>

    public void ChangeSampleRate()
    {
        sampleValue = Mathf.RoundToInt(sampleSlider.value);
        SampleRateTextUpdate();
    }

    /// <summary>
    /// Updates the sample rate text to match the slider.
    /// </summary>

    private void SampleRateTextUpdate()
    {
        sampleText.text = sampleSlider.value.ToString();
    }

    /// <summary>
    /// Changes the duration of the sound, it also calls the function that will update the text underneath the slider.
    /// </summary>

    public void ChangeDuration()
    {
        durationValue = (durationSlider.value);
        DurationTextUpdate();
    }

    /// <summary>
    /// Function that updates the text underneath the slider. Again, limits it to 2dp.
    /// </summary>

    private void DurationTextUpdate()
    {
        durationText.text = durationValue.ToString("F2") + " seconds";
    }

    /// <summary>
    /// Function to change the frequency value of the sound, this value is taken from a slider again. The text update function is then called.
    /// </summary>

    public void ChangeFrequency()
    {
        frequencyValue = Mathf.RoundToInt(frequencySlider.value);
        FrequencyTextUpdate();
    }

    /// <summary>
    /// Function that updates the text value to that of the slider.
    /// </summary>

    private void FrequencyTextUpdate()
    {
        frequencyText.text = frequencyValue.ToString() + "Hz";
    }

    /// <summary>
    /// Function that actually creates the tone, it takes in the frequency value, duration value and sample value which is then used within the CreateToneAudioClip() function.
    /// It then plays a one shot of the created sound effect.
    /// </summary>

    public void CreateSound()
    {
        soundEffect = CreateToneAudioClip(frequencyValue, durationValue, sampleValue);
        source.PlayOneShot(soundEffect);
    }

    /// <summary>
    /// Saves the sound as a .wav file.
    /// </summary>

    public void SaveToWav()
    {
        if (soundEffect != null)
        {
            SavWav.Save(saveNameInput.text, soundEffect);
        }
        else
        {
            Debug.Log("Press play first");
        }
    }
}