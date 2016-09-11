using UnityEngine;
using System.Collections;

public class LockOnScript : MonoBehaviour {
    //[SerializeField]
    //FireWorks fireworks;

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
        //TODO:ロックオンしたときに音のピッチを上げる
        //lockOnAudio.pitch += 0.2f* fireworks.ExploadOrderNumber;
        lockOnAudio.Play();
        yield return null; 
    }
}
