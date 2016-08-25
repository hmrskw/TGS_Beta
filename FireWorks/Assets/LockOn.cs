using UnityEngine;
using System.Collections;

public class LockOn : MonoBehaviour {
    //[SerializeField]
    //AudioClip lockOn;

    AudioSource audioSource;

	void Start () {
        audioSource = GetComponent<AudioSource>();
        //audioSource.clip = lockOn;
        //audioSource.Play();
    }

    void Update () {
	}
}
