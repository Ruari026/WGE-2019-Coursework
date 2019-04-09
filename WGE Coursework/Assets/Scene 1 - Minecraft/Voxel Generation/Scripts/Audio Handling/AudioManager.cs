using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    public AudioClip destroyBlockSound;
    public AudioClip placeBlockSound;

    // Use this for initialization
    void Start ()
    {
		
	}
	// Update is called once per frame
	void Update ()
    {
		
	}

    // When game object is enabled
    void OnEnable()
    {
        VoxelChunk.OnEventBlockChanged += PlayBlockSound;
    }
    // When game object is disabled
    void OnDisable()
    {
        VoxelChunk.OnEventBlockChanged -= PlayBlockSound;
    }
    
    // play the destroy block sound
    void PlayDestroyBlockSound()
    {
        GetComponent<AudioSource>().PlayOneShot(destroyBlockSound);
    }

    // play the place block sound
    void PlayPlaceBlockSound()
    {
        GetComponent <AudioSource>().PlayOneShot(placeBlockSound);
    }

    void PlayBlockSound(int blockType)
    {
        switch (blockType)
        {
            case (0):
                {
                    GetComponent<AudioSource>().PlayOneShot(destroyBlockSound);
                }
                break;

            default:
                {
                    GetComponent<AudioSource>().PlayOneShot(placeBlockSound);
                }
                break;
        }
    }
}
