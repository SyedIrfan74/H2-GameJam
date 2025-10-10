using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    #region Singleton
    public static AudioManager instance;

    private void Awake()
    {
        if (instance != null && instance != this) Destroy(instance);
        else instance = this;
    }
    #endregion

    [SerializeField] private AudioSource audioSourcePrefab; // Reference to the AudioSource prefab
    [SerializeField] private List<AudioData> audioDatas;    // List of audio data objects

    private Dictionary<string, AudioData> audioDictionary;  // Dictionary for quick lookup of audio data
    [SerializeField] private List<AudioSource> audioSources = new List<AudioSource>(); // Pool of audio sources

    private void InitializeAudioDictionary()
    {
        audioDictionary = new Dictionary<string, AudioData>(); // creates the dictionary here
        foreach (var audioData in audioDatas) // checks every audio data in the audiodatalist
        {
            if (audioData != null && audioData.clip != null) // checks if the audio data scriptable object contains any audio clip
            {
                audioDictionary[audioData.name] = audioData;// adds it into the audio data dictionary by the name of the audio data

            }
            else
            {
                Debug.LogWarning("AudioData or AudioClip is null in the audioDatas list.");
            }
        }
    }

    public void PlayAudio(AudioData audioData) // the functions to actually play the audio data, called by the buttons
    {
        if (audioData == null || audioData.clip == null)
        {
            Debug.LogWarning("AudioData or AudioClip is null.");
            return;
        }

        AudioSource audioSource = GetAudioSource(audioData); // tries to get an audio source for the audio data to play the audio from
        if (audioSource != null) // sets all the info for the audio source in here
        {
            audioSource.clip = audioData.clip;
            audioSource.loop = audioData.loop;
            audioSource.volume = audioData.volume;
            audioSource.outputAudioMixerGroup = audioData.mixergroup;
            audioSource.pitch = audioData.pitch;
            audioSource.Play();
        }
        //Debug.Log($"Playing audio: {audioData.name}");

    }

    public void PlayAudio(AudioData audioData, float minPitch, float maxPitch) // the functions to actually play the audio data, called by the buttons
    {
        if (audioData == null || audioData.clip == null)
        {
            Debug.LogWarning("AudioData or AudioClip is null.");
            return;
        }

        AudioSource audioSource = GetAudioSource(audioData); // tries to get an audio source for the audio data to play the audio from
        if (audioSource != null) // sets all the info for the audio source in here
        {
            audioSource.clip = audioData.clip;
            audioSource.loop = audioData.loop;
            audioSource.volume = audioData.volume;
            audioSource.outputAudioMixerGroup = audioData.mixergroup;

            audioSource.pitch = Random.Range(minPitch, maxPitch);

            audioSource.Play();
        }
        //Debug.Log($"Playing audio: {audioData.name}");

    }



    public void StopAudio(AudioData audioData) // stops a specific audio by the audio data scriptable object
    {
        if (audioData == null || audioData.clip == null)
        {
            Debug.LogWarning("AudioData or AudioClip is null.");
            return;
        }

        foreach (var audioSource in audioSources)
        {
            if (audioSource.clip != null && audioSource.clip == audioData.clip)
            {
                audioSource.Stop();
                Debug.Log($"Stopped audio: {audioData.name}");
            }
        }
    }

    public void PlayAudioByName(string audioName) // plays the audio by searching the name, checks if the name is in the dictionary and act accordingly, called inside scripts
    {
        if (audioDictionary.TryGetValue(audioName, out AudioData audioData))
        {
            PlayAudio(audioData);
        }
        else
        {
            Debug.LogWarning($"Audio clip with name {audioName} not found.");
        }
    }
    public void StopAudioByName(string audioName) // stops audio by name
    {
        if (audioDictionary.TryGetValue(audioName, out AudioData audioData))
        {
            StopAudio(audioData);
        }
        else
        {
            Debug.LogWarning($"Audio clip with name {audioName} not found.");
        }
    }

    public void StopAllAudio()
    {
        foreach (var audioSource in audioSources)
        {
            audioSource.Stop();
        }
    }

    private AudioSource GetAudioSource(AudioData audioData)
    {
        foreach (var source in audioSources) // checks all audiosources already created and stored in the audiosourcelist
        {
            if (source.outputAudioMixerGroup == audioData.mixergroup && !source.isPlaying || (source.outputAudioMixerGroup == audioData.mixergroup && audioData.mixergroup.name == "BGM")) // returns if any of the sources are not playing or if the sources is playing the bgm
            {
                return source;
            }
        }

        if (audioSourcePrefab != null)// if unable to meet requirement, create a new one for the audio data to take
        {
            AudioSource newSource = Instantiate(audioSourcePrefab, transform);
            audioSources.Add(newSource);
            return newSource;
        }
        else
        {
            Debug.LogError("AudioSource prefab is not set.");
            return null;
        }
    }
}


