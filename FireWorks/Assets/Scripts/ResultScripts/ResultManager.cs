using UnityEngine;
using System.Collections;

public class ResultManager : MonoBehaviour {
    struct Easing
    {
        public Vector3 startPosition;
        public Vector3 endPosition;
        public float startTime;
    }

    [SerializeField]
    GameObject[] gallerys;

    [SerializeField]
    GameObject stage;

    [SerializeField]
    GameObject[] evalutionImpact;
    //[SerializeField]
    //Vector3 differenceBeforePos;

    //[SerializeField, Tooltip("Easingに使う")]
    //private AnimationCurve curve;

    //[SerializeField, Range(1f, 5f)]
    //float moveTime;

    [SerializeField]
    bool isDelay;

    //Inspectorに複数データを表示するためのクラス
    [System.SerializableAttribute]
    public class EasingData
    {
        public float MoveTime;

        public Vector3 DifferenceBeforePos;

        public AnimationCurve Curve;

        public EasingData(float moveTime, Vector3 differenceBeforePos, AnimationCurve curve)
        {
            MoveTime = moveTime;
            DifferenceBeforePos = differenceBeforePos;
            Curve = curve;
        }
    }

    //Inspectorに表示される
    [SerializeField]
    private EasingData[] easingData;

    [SerializeField]
    Vector3 impactPosition;

    //float totalFireWorksNum;
    float scoreRate;

    Easing[] gallerysEasing;
    Easing stageEasing;

    float[] delay;


    // Use this for initialization
    void Start () {
        scoreRate = (float)ScoreManager.ExplosionNum / (float)ScoreManager.TotalFireWorksNum;

        gallerysEasing = new Easing[gallerys.Length];
        //startPosition = new Vector3[gallerys.Length];
        //endPosition = new Vector3[gallerys.Length];
        //startTime = new float[gallerys.Length];
        delay = new float[gallerys.Length];

        for (int i = 0; i < gallerys.Length; i++)
        {
            gallerysEasing[i].endPosition = gallerys[i].transform.localPosition;
            gallerysEasing[i].startPosition = gallerysEasing[i].endPosition + easingData[0].DifferenceBeforePos;
            gallerys[i].transform.localPosition = gallerysEasing[i].startPosition;
            gallerys[i].SetActive(false);
            delay[i] = 0;
            if (isDelay) delay[i] = Random.Range(-1f, 0);

            if (scoreRate > (1f / gallerys.Length) * i)
            {
                gallerysEasing[i].startTime = Time.timeSinceLevelLoad;
                if (isDelay) gallerysEasing[i].startTime -= delay[i];
                gallerys[i].SetActive(true);
            }
        }
        stageEasing.endPosition = stage.transform.position;
        stageEasing.startPosition = stageEasing.endPosition + easingData[1].DifferenceBeforePos;
        stageEasing.startTime = Time.timeSinceLevelLoad;
        stage.SetActive(false);

        StartCoroutine(ResultUpdate());
	}

    IEnumerator ResultUpdate()
    {
//        while (true)
//        {
            yield return StartCoroutine(MoveGallery());
            yield return StartCoroutine(PopStage());
            yield return new WaitForSeconds(0.5f);
            yield return StartCoroutine(ImpactResultFireworks());
//        }
    }

    IEnumerator MoveGallery()
    {
        bool isMoving = true;
        while (true)
        {
            for (int i = 0; i < gallerys.Length; i++)
            {

                var diff = Time.timeSinceLevelLoad - gallerysEasing[i].startTime;

                if (diff > easingData[0].MoveTime)
                {
                    isMoving = false;
                }

                var rate = diff / easingData[0].MoveTime;
                var pos = easingData[0].Curve.Evaluate(rate);
                gallerys[i].transform.localPosition = Vector3.Lerp(gallerysEasing[i].startPosition, gallerysEasing[i].endPosition, pos);
            }
            if(isMoving == false) break;
            yield return null;
        }
        yield return null;
    }

    IEnumerator PopStage()
    {
        stage.SetActive(true);
        while (true)
        {
            var diff = Time.timeSinceLevelLoad - stageEasing.startTime;

            if (diff > easingData[1].MoveTime)
            {
                break;
            }

            var rate = diff / easingData[1].MoveTime;
            var pos = easingData[1].Curve.Evaluate(rate);

            stage.transform.position = Vector3.Lerp(stageEasing.startPosition, stageEasing.endPosition, pos);
            yield return null;
        }
        yield return null;
    }

    IEnumerator ImpactResultFireworks()
    {
        var rate = (float)ScoreManager.ExplosionNum / (float)ScoreManager.TotalFireWorksNum;
        Debug.Log(ScoreManager.Score * ((float)ScoreManager.ExplosionNum / (float)ScoreManager.TotalFireWorksNum) +"\n"+ (float)(ScoreManager.TotalFireWorksNum * 2) / 3);
        for (int i = evalutionImpact.Length-1; i >= 0; i--)
        {
            if (ScoreManager.Score* ((float)ScoreManager.ExplosionNum / (float)ScoreManager.TotalFireWorksNum) >= (float)(ScoreManager.TotalFireWorksNum*2)/3*i) {
                Instantiate(evalutionImpact[i], impactPosition, Quaternion.Euler(-30, 0, 0));
                break;
            }
        }
        yield return null;
    }

/*
    void Start()
    {
        totalFireWorksNum = ScoreManager.TotalFireWorksNum;
        scoreRate = 0;

        startPosition = new Vector3[gallerys.Length];
        endPosition = new Vector3[gallerys.Length];
        startTime = new float[gallerys.Length];
        delay = new float[gallerys.Length];

        for (int i = 0; i < gallerys.Length; i++)
        {
            endPosition[i] = gallerys[i].transform.localPosition;
            startPosition[i] = endPosition[i] + differenceBeforePos;
            gallerys[i].transform.localPosition = startPosition[i];
            gallerys[i].SetActive(false);
            delay[i] = 0;
            if (isDelay) delay[i] = Random.Range(-1f, 0);
        }
    }

    void Update()
    {
        scoreRate = (float)ScoreManager.ExplosionNum / totalFireWorksNum;
        for (int i = 0; i < gallerys.Length; i++)
        {
            if (gallerys[i].activeInHierarchy == false)
            {
                if (scoreRate > (1f / gallerys.Length) * i)
                {
                    startTime[i] = Time.timeSinceLevelLoad;
                    if (isDelay) startTime[i] -= delay[i];
                    gallerys[i].SetActive(true);
                }
            }
            else
            {
                MoveGallery(gallerys[i], i);
            }
        }
    }

    void MoveGallery(GameObject obj, int galleryID)
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
    */
}
