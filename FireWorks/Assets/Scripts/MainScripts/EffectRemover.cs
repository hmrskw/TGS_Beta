using UnityEngine;
using System.Collections;

public class EffectRemover : MonoBehaviour {

    ParticleSystem effect;

    void Start () {
        effect = GetComponent<ParticleSystem>();
	}
	
	void Update () {
        if (effect.isPlaying == false)
        {
            Destroy(this.gameObject);
        }
	}
}
