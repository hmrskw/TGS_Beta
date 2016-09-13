using UnityEngine;
using System.Collections;
using UnityEngine.UI;

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
    Slider evaluationSlider;

    [SerializeField]
    bool isDelay;

    [SerializeField]
    GameObject endBoard;

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
	private AudioSource SE_result;

    [SerializeField]
    private AudioSource SE_gage;

    float scoreRate;
    float score;
    float gageValue;

    float evaluationStandard;

    Easing[] gallerysEasing;
    Easing stageEasing;
    Easing bordEasing;

    ResultSceneChanger resultSceneChanger;

    float[] delay;

    void Start () {
        resultSceneChanger = new ResultSceneChanger();

        scoreRate = (float)ScoreManager.ExplosionNum / (float)ScoreManager.TotalFireWorksNum;
        score = ScoreManager.Score * ((float)ScoreManager.ExplosionNum / (float)ScoreManager.TotalFireWorksNum);
        gageValue = 0f;

        evaluationStandard = (float)(ScoreManager.TotalFireWorksNum * 2f) / 3f;

        gallerysEasing = new Easing[gallerys.Length];

        delay = new float[gallerys.Length];

        evaluationSlider.value = 0;
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
        yield return StartCoroutine(Gage());
        yield return new WaitForSeconds(0.5f);
        yield return StartCoroutine(ImpactResultFireworks());
        yield return new WaitForSeconds(5f);
        yield return StartCoroutine(FallEndBoard());
        yield return new WaitForSeconds(5f);
        resultSceneChanger.SceneChange("Title");
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
		SE_result.Play();
    }

    IEnumerator Gage()
    {
        scoreText.SetActive(true);
        SE_gage.time = 4.3f - System.Math.Min((score / (evaluationStandard * 2f)), 1f)*2f;
        SE_gage.Play();
        while (gageValue <= System.Math.Min((score / (evaluationStandard * 2f)*0.8f)+0.2f, 1f)) {
            evaluationSlider.value = gageValue;
            gageValue += 0.5f*Time.deltaTime;
            yield return null;
        }
        SE_gage.Stop();
    }

    IEnumerator ImpactResultFireworks()
    {
        for (int i = evalutionImpact.Length-1; i >= 0; i--)
        {
            if (score >=
                evaluationStandard * i) {
				GameObject impactObj = Instantiate(impact);
                impactObj.transform.localScale *= 1.5f;
                Instantiate(evalutionImpact[i]);
                break;
            }
        }
        yield return null;
    }

    IEnumerator FallEndBoard()
    {
        while(endBoard.transform.position.y > 3.75)
        {
            endBoard.transform.Translate(new Vector3(0f,-0.1f,0f));
            yield return null;
        }
    }
}