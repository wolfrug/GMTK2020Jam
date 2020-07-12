using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectAudio : MonoBehaviour
{

    public List<ObjectSound> sounds = new List<ObjectSound> { };
    public CustomAudioSource source;

    public bool initOnStart = true;
    private Dictionary<SoundType, ObjectSound> soundDict = new Dictionary<SoundType, ObjectSound> { };
    private List<AudioClip> randomSounds = new List<AudioClip> { };
    private Coroutine soundPlayer;
    // Start is called before the first frame update
    void Start()
    {
        if (initOnStart)
        {
            Init();
        }
    }

    public void Init()
    { // This is called from the gridobject script, after the data has been loaded into it (can be used sans data, in other words)
        //soundDict.Clear ();
        foreach (ObjectSound sound in sounds)
        {
            if (!soundDict.ContainsKey(sound.type))
            {
                soundDict.Add(sound.type, sound);
            }
            else
            {
                soundDict[sound.type].sounds.AddRange(sound.sounds);
                Debug.LogWarning("Adding more sounds to dictionary. Hmm. (" + sound.type.ToString() + ")", gameObject);
            }
        }
        if (source == null)
        {
            source = GetComponent<CustomAudioSource>();
        }
    }
    public void AddSounds(ObjectSound[] newsounds)
    {
        sounds.AddRange(newsounds);
        Init();
    }

    public void PlayRandomSoundType(SoundType type)
    { // play one random sound
        ObjectSound soundObj = null;
        soundDict.TryGetValue(type, out soundObj);
        if (soundObj != null)
        {
//            Debug.Log("Audioplayer: playing random sound of type " + type);
            PlayRandomSound(soundObj);
        }
    }
    public void PlayRandomSoundTypeFromArray(SoundType type, ObjectSound[] array)
    {
        if (array.Length > 0)
        {
            foreach (ObjectSound gos in array)
            {
                if (gos.type == type)
                {
                    PlayRandomSound(gos);
                }
            }
        };
    }
    public void PlayRandomSound(ObjectSound soundObj)
    {
//        Debug.Log("Playing a random sound from soundobject with count " + soundObj.sounds.Count);
        if (soundObj.sounds.Count > 0)
        {
            //randomSounds.Clear ();
            randomSounds = soundObj.sounds;
            source.sounds = randomSounds.ToArray();
            source.RandomizeAndPlay();
        }
    }
    public void PlayAllSoundsInOrder(SoundType type)
    { // play all sounds in the order they are in the list
        ObjectSound soundObj = null;
        soundDict.TryGetValue(type, out soundObj);
        randomSounds.Clear();
        if (soundObj != null)
        {
            randomSounds = soundObj.sounds;
            if (soundPlayer != null)
            {
                StopCoroutine(soundPlayer);
            }
            soundPlayer = StartCoroutine(SoundPlayerCoroutine(randomSounds.ToArray()));
        }
    }
    IEnumerator SoundPlayerCoroutine(AudioClip[] sounds)
    {
        foreach (AudioClip clip in sounds)
        {
            source.PlayOneShot(clip);
            yield return new WaitForSeconds(clip.length);
        }
        soundPlayer = null;
    }

    // Update is called once per frame
    void Update()
    {

    }
}