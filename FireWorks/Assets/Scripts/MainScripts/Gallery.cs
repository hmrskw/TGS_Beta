using UnityEngine;
using System.Collections;

public class Gallery : MonoBehaviour {
    [SerializeField]
    GameObject[] gallerys;

    [SerializeField]
    Vector3 differenceBeforePos;

    [SerializeField, Tooltip("Easingに使う")]
    private AnimationCurve curve;

    [SerializeField]
    private AnimationCurve cheerCurve;

    [SerializeField, Range(1f, 2f)]
    float cheerTime;

    [SerializeField,Range(1f,5f)]
    float moveTime;

    float totalFireWorksNum;
    float scoreRate;

    bool[] isMoveEndGallery;
    Vector3[] startPosition;
    Vector3[] endPosition;
    float[] startTime;

    AudioSource gallerysCheer;

    bool[] isCheer;
    public bool IsCheer {
        set {
            bool canPlayCheerAudio = false;
            for (int i = 0; i < gallerys.Length; i++)
            {
                if (isMoveEndGallery[i] == true)
                {
                    canPlayCheerAudio = true;
                    isCheer[i] = value;
                }
                else isCheer[i] = false;
            }
            if (gallerysCheer.isPlaying == false && canPlayCheerAudio) gallerysCheer.Play();
        }
    }

    void Start () {
        gallerysCheer = GetComponent<AudioSource>();

        totalFireWorksNum = ScoreManager.TotalFireWorksNum;
		scoreRate = 0;

        isMoveEndGallery = new bool[gallerys.Length];
        isCheer = new bool[gallerys.Length];
        startPosition = new Vector3[gallerys.Length];
        endPosition = new Vector3[gallerys.Length];
        startTime = new float[gallerys.Length];

        for (int i = 0; i < gallerys.Length; i++)
        {
            isMoveEndGallery[i] = false;
            isCheer[i] = false;
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
                if (isMoveEndGallery[i] == false)
                {
                    MoveGallery(gallerys[i], i);
                }
                else if(isCheer[i] == true )
                {
                    StartCoroutine(Cheer(gallerys[i], i));
                    isCheer[i] = false;
                }
            }
        }
    }

    void MoveGallery(GameObject obj,int galleryID)
    {
        var diff = Time.timeSinceLevelLoad - startTime[galleryID];

        if (diff > moveTime)
        {
            obj.transform.localPosition = endPosition[galleryID];
            isMoveEndGallery[galleryID] = true;
        }

        var rate = diff / moveTime;
        var pos = curve.Evaluate(rate);

        obj.transform.localPosition = Vector3.Lerp(startPosition[galleryID], endPosition[galleryID], pos);
    }

    public IEnumerator Cheer(GameObject obj, int galleryID)
    {
        float gap = Random.Range(0f, 0.5f);

        yield return new WaitForSeconds(gap);

        float time = 0; 
        float cheerStartTime = MainGameManager.Instance.GameTime;
        float cheerEndTime = cheerTime;

        Vector3 startPos = obj.transform.localPosition;
        Vector3 endPos = startPos + new Vector3(0f, 0.5f, 0f);

        while (cheerEndTime > time) {
            time = MainGameManager.Instance.GameTime - cheerStartTime;
            float rate = time / cheerEndTime;
            float pos = cheerCurve.Evaluate(rate);

            obj.transform.localPosition = Vector3.Lerp(startPos, endPos, pos);
            yield return null;
        }
    }
}