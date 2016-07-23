using UnityEngine;
using System.Collections;

public class Particle : MonoBehaviour {

    ParticleSystem particle;

    // Use this for initialization
    void Start () {
        particle = GetComponent<ParticleSystem>();
        if (transform.tag == "FireWorks")
        {
            if (transform.position.y < 30)
            {
                particle.startColor = new Color(1, transform.position.y / 29f, 0);
            }
            else
            {
                particle.startColor = Color.white;
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (!particle.isPlaying)
        {
            Destroy(this.gameObject);
        }
	}
}
