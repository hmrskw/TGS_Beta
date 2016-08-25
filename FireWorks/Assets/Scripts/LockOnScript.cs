using UnityEngine;
using System.Collections;

public class LockOnScript : MonoBehaviour {

    AudioSource lockOnAudio;

    float scale;

    void Start() {
        lockOnAudio = GetComponent<AudioSource>();
        scale = 0.3f;
        StartCoroutine(changeSize());
    }

    IEnumerator changeSize()
    {
        while (scale > 0) {
            transform.localScale = new Vector3(0.3f+scale, 0.3f + scale, 0.3f + scale);
            scale -= 0.05f;
            yield return null;
        }
        lockOnAudio.Play();
        yield return null; 
    }
}
