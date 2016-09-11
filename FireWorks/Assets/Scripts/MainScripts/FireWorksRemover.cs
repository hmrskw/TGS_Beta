using UnityEngine;
using System.Collections;

public class FireWorksRemover : MonoBehaviour {
    [SerializeField]
    AudioClip[] impactSE = new AudioClip[3];
    
    ParticleSystem impact;
    AudioSource impactAudioSource;

    //bool playedAudio;

    void Start () {
        //playedAudio = false;
        impact = GetComponent<ParticleSystem>();
        impactAudioSource = GetComponent<AudioSource>();
        for (int i = 0; i < impactSE.Length; i++) {
            if (transform.localScale.x <= (1f/ impactSE.Length)*(i+1))
            {
                impactAudioSource.clip = impactSE[i];
                break;
            }
        }

        StartCoroutine(playAudio());
	}
	
	void Update () {
        //if (impact != null)
        //{
            if (impact.isPlaying == false && impactAudioSource.isPlaying == false)
            {
                Destroy(this.gameObject);
            }
        /*}
        else
        {
            if (audioSource.isPlaying == false && playedAudio == true)
            {
                Destroy(this.gameObject);
            }
        }*/
	}

    IEnumerator playAudio()
    {
        yield return new WaitForSeconds(0.5f);
        impactAudioSource.Play();
        //playedAudio = true;
    }
}
