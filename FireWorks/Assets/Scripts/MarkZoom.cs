using UnityEngine;
using System.Collections;

public class MarkZoom : MonoBehaviour {

    float maxScale;

    float scale;

	void Start () {
        maxScale = transform.localScale.x;
        scale = 0f;
        transform.localScale = new Vector3(scale, scale, 1f);
    }

    void Update() {
        if (scale < maxScale) {
            transform.localScale = new Vector3(scale, scale, 1f);
            scale += maxScale/60;
        }
    }
}
