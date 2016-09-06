using UnityEngine;
using System.Collections;

public class FireWorksRemover : MonoBehaviour {
    [SerializeField]
    AudioClip[] impactSE = new AudioClip[3];
    
    ParticleSystem impact;
    AudioSource audioSource;

    bool playedAudio;

    void Start () {
        playedAudio = false;
        impact = GetComponent<ParticleSystem>();
        audioSource = GetComponent<AudioSource>();
        for (int i = 0; i < impactSE.Length; i++) {
            if (transform.localScale.x <= (1f/ impactSE.Length)*(i+1))
            {
                audioSource.clip = impactSE[i];
                break;
            }
        }

        StartCoroutine(playAudio());
	}
	
	void Update () {
        //if (impact != null)
        //{
            if (impact.isPlaying == false && audioSource.isPlaying == false)
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
        audioSource.Play();
        playedAudio = true;
    }
}
