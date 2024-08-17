using UnityEngine.Audio;
using UnityEngine;
using System.Collections;

[System.Serializable]
public class Sound
{
    public enum SoundName
    {
    }
    public SoundName name;

    public AudioClip clip;

    public AudioMixerGroup mixerGroup;

    [Range(0.1f, 3f)]
    public float pitch = 1;

    [Range(0f, 10f)]
    public float volume = 1;

    [HideInInspector]
    public AudioSource source;

    public bool loop;
    public bool createSource = true;
    public bool playOnAwake = false;
    public bool doNotFade = false;

    public Coroutine fadeRoutine;

    public IEnumerator FadeSoundRoutine(bool fadeIn, float duration, float targetVolume)
    {
        if (source == null)
        {
            fadeRoutine = null;
            yield break;
        }

        float time = 0f;
        float startVolume = source.volume;

        while (time < duration)
        {
            time += Time.unscaledDeltaTime;
            if (!fadeIn)
                source.volume = Mathf.Lerp(startVolume, targetVolume, time / duration);
            else
                source.volume = Mathf.Lerp(0, targetVolume, time / duration);
            yield return null;
        }

        if (!fadeIn)
        {
            source.Stop();
            source.volume = 0;
        }
        else
        {
            source.Play();
            source.volume = targetVolume;
        }

        fadeRoutine = null;

        yield break;
    }
}