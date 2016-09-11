using UnityEngine;
using System.Collections;

public class Stamp : MonoBehaviour {
    [SerializeField]
    float initRate;

    [SerializeField]
    float initRotation;

    [SerializeField]
    Vector3 defaultScale;

    //Quaternion defaultRotation;

    float rate;
    float scale;
    float rotation;

    AudioSource stampSE;

    void OnEnable()
    {
        //defaultRotation = transform.localRotation;

        stampSE = GetComponent<AudioSource>();

        rate = initRate;
        rotation = initRotation;

        //scale = initScale;
        //rotation = initRotation;

        transform.localScale *= initRate;
        transform.localRotation = Quaternion.Euler(new Vector3(0,0, initRotation));

        StartCoroutine(Pressing());
    }

    IEnumerator Pressing()
    {
        while (/*transform.localScale.x > defaultScale.x*/ rate> 1f)
        {
            transform.localScale = defaultScale * rate;
            transform.localRotation = Quaternion.Euler(new Vector3(0, 0, rotation));

            rotation = initRotation * (rate-1.25f);
            rate -= 0.1f;
            yield return null;
        }
        stampSE.Play();
        //TODO:スタンプを押す音をここで鳴らす
    }
}
