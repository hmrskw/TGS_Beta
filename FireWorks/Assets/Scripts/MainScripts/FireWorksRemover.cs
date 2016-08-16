using UnityEngine;
using System.Collections;

public class FireWorksRemover : MonoBehaviour {
    [SerializeField]
    AudioClip[] impactSE = new AudioClip[3];

    ParticleSystem impact;
    AudioSource audioSource;

    void Start () {
        impact = GetComponent<ParticleSystem>();
        audioSource = GetComponent<AudioSource>();
        for (int i = 0; i < impactSE.Length; i++) {
            if (transform.localScale.x <= (1f/ impactSE.Length)*(i+1))
            {
                audioSource.clip = impactSE[i];
                break;
            }
        }
        audioSource.Play();
	}
	
	void Update () {
        if (impact.isPlaying == false)
        {
            Destroy(this.gameObject);
        }
	}
}
