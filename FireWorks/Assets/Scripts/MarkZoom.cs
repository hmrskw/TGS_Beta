using UnityEngine;
using System.Collections;

public class MarkZoom : MonoBehaviour {

    float maxScale;

    float scale;

	void Start () {
        maxScale = transform.localScale.x;
        scale = 0f;
        transform.localScale = new Vector3(scale, scale, scale);
    }

    void Update() {
        if (scale < maxScale) {
            transform.localScale = new Vector3(scale, scale, scale);
            scale += maxScale/60;
        }
    }
}
