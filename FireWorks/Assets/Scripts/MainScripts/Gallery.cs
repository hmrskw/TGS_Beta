using UnityEngine;
using System.Collections;

public class Gallery : MonoBehaviour {
    [SerializeField]
    GameObject[] gallerys;

    [SerializeField]
    Vector3 differenceBeforePos;

    [SerializeField, Tooltip("Easingに使う")]
    private AnimationCurve curve;

    [SerializeField,Range(1f,5f)]
    float moveTime;

    float totalFireWorksNum;
    float scoreRate;

    Vector3[] startPosition;
    Vector3[] endPosition;
    float[] startTime;

    void Start () {
		totalFireWorksNum = ScoreManager.TotalFireWorksNum;
		scoreRate = 0;

        startPosition = new Vector3[gallerys.Length];
        endPosition = new Vector3[gallerys.Length];
        startTime = new float[gallerys.Length];

        for (int i = 0; i < gallerys.Length; i++)
        {
            endPosition[i] = gallerys[i].transform.localPosition;
            startPosition[i] = endPosition[i] + differenceBeforePos;
            gallerys[i].transform.localPosition = startPosition[i];
            gallerys[i].SetActive(false);
        }
    }

    void Update () {
		scoreRate = (float)ScoreManager.ExplosionNum / totalFireWorksNum;
		for (int i = 0; i < gallerys.Length; i++)
        {
            if (gallerys[i].activeInHierarchy == false)
            {
                if (scoreRate > (1f / gallerys.Length) * i)
                {
                    startTime[i] = Time.timeSinceLevelLoad;
                    gallerys[i].SetActive(true);
                }
            }
            else
            {
                MoveGallery(gallerys[i], i);
            }
        }
    }

    void MoveGallery(GameObject obj,int galleryID)
    {
        var diff = Time.timeSinceLevelLoad - startTime[galleryID];

        if (diff > moveTime)
        {
            obj.transform.localPosition = endPosition[galleryID];
        }

        var rate = diff / moveTime;
        var pos = curve.Evaluate(rate);

        obj.transform.localPosition = Vector3.Lerp(startPosition[galleryID], endPosition[galleryID], pos);
    }
}
