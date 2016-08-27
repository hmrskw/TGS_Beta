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
    GameObject bord;

    [SerializeField]
    GameObject[] evalutionImpact;

    [SerializeField]
	GameObject impact;

    [SerializeField]
    GameObject scoreText;

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

    //[SerializeField]
    //Vector3 impactPosition;

    float scoreRate;

    Easing[] gallerysEasing;
    Easing stageEasing;
    Easing bordEasing;

    ResultSceneChanger resultSceneChanger;

    float[] delay;

    void Start () {
        resultSceneChanger = new ResultSceneChanger();

        scoreRate = (float)ScoreManager.ExplosionNum / (float)ScoreManager.TotalFireWorksNum;

        gallerysEasing = new Easing[gallerys.Length];
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

        bordEasing.endPosition = bord.transform.position;
        bordEasing.startPosition = bordEasing.endPosition - easingData[1].DifferenceBeforePos;
        bordEasing.startTime = Time.timeSinceLevelLoad;

        bord.SetActive(false);
        stage.SetActive(false);
        scoreText.SetActive(false);

        StartCoroutine(ResultUpdate());
	}

    IEnumerator ResultUpdate()
    {
            yield return StartCoroutine(MoveGallery());
            yield return StartCoroutine(PopStage());
            yield return new WaitForSeconds(0.5f);
            yield return StartCoroutine(ImpactResultFireworks());
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
        bord.SetActive(true);
        while (true)
        {
            var diff = Time.timeSinceLevelLoad - stageEasing.startTime;

            if (diff > easingData[1].MoveTime)break;

            var rate = diff / easingData[1].MoveTime;
            var pos = easingData[1].Curve.Evaluate(rate);

            stage.transform.position = Vector3.Lerp(stageEasing.startPosition, stageEasing.endPosition, pos);
            bord.transform.position = Vector3.Lerp(bordEasing.startPosition, bordEasing.endPosition, pos);
            yield return null;
        }
    }

    IEnumerator ImpactResultFireworks()
    {
        scoreText.SetActive(true);
        for (int i = evalutionImpact.Length-1; i >= 0; i--)
        {
            if (ScoreManager.Score* ((float)ScoreManager.ExplosionNum / (float)ScoreManager.TotalFireWorksNum) >= 
				(float)(ScoreManager.TotalFireWorksNum*2)/3*i*i) {
				Instantiate(impact);
				Instantiate(evalutionImpact[i]);//, impactPosition, Quaternion.Euler(0, 180, 0));
                break;
            }
        }
        while (true)
        {
            if (Input.GetMouseButtonDown(0))
            {
                resultSceneChanger.SceneChange("Title");
            }
            yield return null;
        }
    }
}
