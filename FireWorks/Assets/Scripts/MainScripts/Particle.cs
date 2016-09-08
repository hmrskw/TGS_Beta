using UnityEngine;
using System.Collections;

public class Particle : MonoBehaviour {
    
    ParticleSystem particle;

    //花火の色
    Color fireColor;

    // Use this for initialization
    void Start () {
        particle = GetComponent<ParticleSystem>();
        //花火の色をfireworksの色に変更
        //particle.GetComponent<Renderer>().material.SetColor("_TintColor", fireColor);
    }
	
	// Update is called once per frame
	void Update () {
        if (!particle.isPlaying)
        {
            if (particle.subEmitters.birth0 == null || !particle.subEmitters.birth0.isPlaying)
            {
                Destroy(this.gameObject);
            }
        }
	}

    public void setColor(Color color)
    {
        //正規化
        this.fireColor = color/255f;
    }
}
