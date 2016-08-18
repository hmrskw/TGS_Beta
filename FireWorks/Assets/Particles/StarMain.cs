using UnityEngine;
using System.Collections;

public class StarMain : MonoBehaviour {

    [SerializeField]
    private GameObject[] effect;

    [SerializeField]
    private Vector3 min, max;

    [SerializeField]
    private float durationTime = 0.1f;

    [SerializeField, Tooltip("爆発する回数")]
    private int num = 0;

    private float time = 0;

    private Vector3 pos;

    void Start () {
        pos = transform.position;

        StartCoroutine(Impact());
    }

    IEnumerator Impact()
    {
        for (int i = 0; i < num; i++)
        {
            Vector3 variation = new Vector3(Random.Range(min.x, max.x), Random.Range(min.y, max.y), Random.Range(min.z, max.z));
            int colorType = Random.Range(0, effect.Length);
            Instantiate(effect[colorType],
                pos + variation,
                Quaternion.identity);
            yield return new WaitForSeconds(durationTime);
        }
        yield return null;
    }
}