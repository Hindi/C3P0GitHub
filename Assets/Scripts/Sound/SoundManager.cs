using UnityEngine;
using System.Collections;
using System.Collections.Generic;


/**
 * This class plays sounds at will.
 * 
 */


public class SoundManager : MonoBehaviour {

    [SerializeField]
    private List<EnumSound> names;
    [SerializeField]
    private List<AudioClip> sounds;

    private Dictionary<EnumSound, AudioClip> soundList;

	// Use this for initialization
	void Start () {

        soundList = new Dictionary<EnumSound, AudioClip>();

        for (int i = 0; i < names.Count; ++i)
        {
            soundList.Add(names[i], sounds[i]);
        }

        EventManager<EnumSound, Vector3>.AddListener(EnumEvent.PLAYSOUND, onPlaySound);
        EventManager<EnumSound, Vector3>.Raise(EnumEvent.PLAYSOUND, EnumSound.PORTAL1, Vector3.zero);
	}
	
	// Update is called once per frame
	void Update () {
	
	}


    /**
     * This class manage the states in the whole game (menus and minigame)
     * 
     */
    public void onPlaySound(EnumSound name, Vector3 pos)
    {
        AudioSource.PlayClipAtPoint(soundList[name], pos);
    }
}
