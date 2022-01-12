using UnityEngine.Audio;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    public static AudioManager instance;
    public int chipCollisionsThisTurn;
    public int lastPinCollisionSampleNumber;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            References.audioManager = this;
            chipCollisionsThisTurn = 0;
            lastPinCollisionSampleNumber = 1;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.source.spatialBlend = s.spatialBlend;
        }
    }
    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            return;
        }
        else
        {
            s.source.Play();
        }
    }

    public void SetLevelAndPlay(string name, float level)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            return;
        }
        else
        {
            s.source.volume = level;
            s.source.Play();
        }
    }

    public void SetLevelAndPlayRandomPitch(string name, float level)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            return;
        }
        else
        {
            s.source.pitch = UnityEngine.Random.Range(0.8f, 1.2f);
            s.source.volume = level;
            s.source.Play();
        }
    }


    void Start()
    {
        Play("menu-boot");
    }

    public void HandleChipCollision(float level)
    {
        int sampleIndexToPlay = chipCollisionsThisTurn + 1;
        string sampleSelect = "chip-collision-" + sampleIndexToPlay.ToString();
        SetLevelAndPlay(sampleSelect, level);
        chipCollisionsThisTurn++;
        if (chipCollisionsThisTurn > 15)
        {
            chipCollisionsThisTurn = 0;
        }
    }

    public void HandleMenuChipCollision(float level)
    {
        string sampleSelect = "chip-collision-" + UnityEngine.Random.Range(1, 16).ToString();
        SetLevelAndPlayRandomPitch(sampleSelect, level);
    }

    public void RandomChipCollision(float level)
    {
        int sampleIndexToPlay = UnityEngine.Random.Range(1, 16);
        string sampleSelect = "chip-collision-" + sampleIndexToPlay.ToString();
        SetLevelAndPlay(sampleSelect, level);
    }

    public void HandlePinCollision(float level)
    {
        if (lastPinCollisionSampleNumber == 1)
        {
            SetLevelAndPlay("pin-collision-2", level);
            lastPinCollisionSampleNumber = 2;
        }
        else if (lastPinCollisionSampleNumber == 2)
        {
            SetLevelAndPlay("pin-collision-1", level);
            lastPinCollisionSampleNumber = 1;
        }
    }

    public void PlayTurnIndicator()
    {
        if (References.isPlayerOnesTurn)
        {
            Play("player-1");
        }
        else
        {
            Play("player-2");
        }
    }
}
