using UnityEngine.Audio;
using UnityEngine;
using UnityEditor;

public class AudioManager : MonoBehaviour
{
    public AudioMixer audioMixer;

    public Sound[] sounds;

    [SerializeField] private PlayerSettings playerSettings;

    public static AudioManager Instance;

    void Awake()
    {
        Instance = this;
        InitSoundList(sounds);
    }

    public void PlayNormalBGM()
    {
        Play("NormalBGM");
        PlayAfterDelay(FindSound("NormalBGM").clip.length, "NormalBGM1");
    }

    public void PlayBossBGM()
    {
        Play("BossBGM");
        PlayAfterDelay(FindSound("BossBGM").clip.length, "BossBGM1");
    }

    private void InitSoundList(Sound[] sounds)
    {
        foreach (Sound s in sounds)
        {
            if (!s.createSource)
                continue;

            s.source = gameObject.AddComponent<AudioSource>();
            InitAudioSource(s.source, s);

            if (s.playOnAwake)
                s.source.Play();
        }
    }

    public void InitAudioSource(AudioSource source, Sound sound)
    {
        source.clip = sound.clip;
        source.outputAudioMixerGroup = sound.mixerGroup;
        source.pitch = sound.pitch;
        source.volume = sound.volume;
        source.loop = sound.loop;
    }

    public Sound FindSound(string name)
    {
        foreach (Sound s in sounds)
        {
            if (s.name.Equals(name))
                return s;
        }
        return null;
    }

    public void Play(string sound)
    {
        Sound s = FindSound(sound);
        ResetVolumeOfSound(s);
        s.source.Play();
    }

    public void PlayOneShot(string sound)
    {
        Sound s = FindSound(sound);
        ResetVolumeOfSound(s);
        s.source.PlayOneShot(s.clip);
    }

    public void PlayOneShot(AudioSource source, string sound)
    {
        Sound s = FindSound(sound);
        InitAudioSource(source, s);
        source.PlayOneShot(s.clip);
    }

    public void OnlyPlayAfterSoundEnds(string sound)
    {
        Sound s = FindSound(sound);
        if (s.name.Equals(sound) && !s.source.isPlaying)
            s.source.Play();
    }

    public void Stop(string sound)
    {
        FindSound(sound).source.Stop();
    }

    public void StopAllSounds()
    {
        foreach (Sound s in sounds)
            s.source.Stop();
    }

    public void Pause(string sound)
    {
        FindSound(sound).source.Pause();
    }

    public void Unpause(string sound)
    {
        FindSound(sound).source.UnPause();
    }

    public void PauseAllSounds()
    {
        foreach (Sound s in sounds)
        {
            if (s.source != null)
                s.source.Pause();
        }
    }

    public void UnpauseAllSounds()
    {
        foreach (Sound s in sounds)
        {
            if (s.source != null)
                s.source.UnPause();
        }
    }

    public bool CheckIfSoundPlaying(string sound)
    {
        return FindSound(sound).source.isPlaying;
    }

    public void FadeAllSound(bool fadeIn, float duration, float targetVolume)
    {
        foreach (Sound s in sounds)
        {
            if (s.doNotFade)
                continue;

            StopFadeRoutine(s.name);
            s.fadeRoutine = StartCoroutine(s.FadeSoundRoutine(fadeIn, duration, targetVolume));
        }
    }

    public void FadeSound(bool fadeIn, string sound, float duration, float targetVolume)
    {
        Sound s = FindSound(sound);
        StopFadeRoutine(sound);
        s.fadeRoutine = StartCoroutine(s.FadeSoundRoutine(fadeIn, duration, targetVolume));
    }

    public void StopFadeRoutine(string sound)
    {
        Sound s = FindSound(sound);

        if (s.fadeRoutine != null)
        {
            StopCoroutine(s.fadeRoutine);
            s.fadeRoutine = null;
            ResetVolumeOfSound(s);
        }
    }

    private void ResetVolumeOfSound(Sound sound)
    {
        if (sound.source == null)
            return;

        FindSound(sound.name).source.volume = sound.volume;
    }

    public void PlayAfterDelay(float delay, string sound)
    {
        FindSound(sound).source.PlayDelayed(delay);
    }

    public void SetPitch(string sound, float newPitch)
    {
        Sound s = FindSound(sound);
        s.source.pitch = newPitch;
    }

    private void OnApplicationQuit()
    {
        playerSettings.ResetVolume();
    }
}