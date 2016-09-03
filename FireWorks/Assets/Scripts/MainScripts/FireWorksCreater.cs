using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class FireWorksCreater : MonoBehaviour
{
    enum DIRECTION
    {
        LEFT = 1, RIGHT = -1
    }
    [SerializeField, Tooltip("時間制限")]
    int timeLimit;

    [SerializeField]
    Slider timeBarSlider;

	[SerializeField]
	GameObject timeBarFire;

	[SerializeField, Tooltip("制限時間が来たら飛ぶ花火")]
	GameObject endFireworksSeed;

    [SerializeField, Tooltip("玉が発射される位置")]
    Vector3[] fireWorksInitPosition = new Vector3[5];

    [SerializeField, Tooltip("玉が発射される角度")]
    float[] fireWorksAngle = new float[3];
    
    [SerializeField, Tooltip("玉")]
    GameObject fireWorksSeed;

    [SerializeField]
    GameObject hitEffect;

    //Inspectorに複数データを表示するためのクラス
    [System.SerializableAttribute]
    public class FireWorksImpact
    {
        public GameObject[] Color = new GameObject[2];

        public FireWorksImpact(GameObject[] color)
        {
            Color = color;
        }
    }

    //Inspectorに表示される
    [SerializeField, Tooltip("爆発")]
    private FireWorksImpact[] fireWorksImpact;

    [SerializeField]
    AudioSource gallerys;

    ReadCSV CSVReader;

    //シーン内での経過時間
    float time;

    FireWorks fireWorks;

    ReadCSV readCSV;

    int LockOnNumber;

	bool isEnd;
	//GameObject endFireworks;

    //現在何番目（CSVの何行目）の花火を飛ばしているか
    int readFireworksNumber;

    //MainSceneChanger mSceneChanger;

    void Awake()
    {
        //CSVファイルを読み込む
        readCSV = new ReadCSV();
        readCSV.ReadFile();

        ScoreManager.init();
        ScoreManager.TotalFireWorksNum = readCSV.CsvData.Length;
    }
    void Start()
    {
        //mSceneChanger = new MainSceneChanger();
        //各値の初期化
        readFireworksNumber = 0;
        time = 0;
        LockOnNumber = 0;
    }

    void Update()
    {
        //ポインターが当たった時の処理
        RayCast();

		timeBarSlider.value = 1-time/timeLimit;

		if (timeLimit >= time)
        {
            //時間の更新
            time += Time.deltaTime;
            //CsvDataの配列長さを超えていないかのチェック
            if (readCSV.CsvData.Length > readFireworksNumber)
            {
                if (/*ReceivedZKOO.GetHand().isTouching == false*/Input.GetMouseButton(0) == false)
                {
					if (LockOnNumber >= 5&& gallerys.isPlaying == false) StartCoroutine(PlayAudio());
                    LockOnNumber = 0;
                }

                //発射時間が現在の経過時間と同じときの処理
                while (readCSV.CsvData[readFireworksNumber].shotTiming <= time)
                {
                    //発射する玉の角度
                    Quaternion angle = Quaternion.identity;

                    //発射方向が設定されていれば玉を飛ばす角度その方向に変える
                    if (readCSV.CsvData[readFireworksNumber].shotAngle != EnumDefinition.ShotAngle.NONE_ANGLE)
                    {
                        angle = Quaternion.Euler(0, 0, fireWorksAngle[(int)readCSV.CsvData[readFireworksNumber].shotAngle]);
                    }

                    //玉の生成
                    GameObject seedObj = Instantiate(
                        fireWorksSeed,//玉のプレハブ
                        fireWorksInitPosition[readCSV.CsvData[readFireworksNumber].shotPosition],//発射位置
                        angle//角度
                        ) as GameObject;

                    fireWorks = seedObj.GetComponent<FireWorks>();

                    if (fireWorks != null)
                    {
                        fireWorks.FireWorksImpact = fireWorksImpact
                            [(int)readCSV.CsvData[readFireworksNumber].fireｗorksType]
                            .Color[readCSV.CsvData[readFireworksNumber].fireworksColor/* % fireWorksImpact[(int)readCSV.CsvData[readFireworksNumber].fireｗorksType].Color.Length*/];
                        fireWorks.Size = readCSV.CsvData[readFireworksNumber].fireworksSize;
                    }

                    //飛ばす花火を更新
                    readFireworksNumber++;
                    //飛ばしていたのが最後の花火ならループを抜ける
                    if (readCSV.CsvData.Length <= readFireworksNumber) break;
                }
            }
        }
        else
        {
            GameObject[] fireWorksSeeds;
            fireWorksSeeds = GameObject.FindGameObjectsWithTag("FireWorksSeed");

			if (fireWorksSeeds.Length == 0 && isEnd == false){
				isEnd = true;
				timeBarFire.SetActive(false);
				StartCoroutine(SceneChanger());
			}
		}
    }

    IEnumerator PlayAudio()
    {
        yield return new WaitForSeconds(1f);
        gallerys.Play();
    }

    IEnumerator SceneChanger()
    {
    	yield return new WaitForSeconds(2f);
		Instantiate(endFireworksSeed,new Vector3(16.9f,4.25f,2f),Quaternion.Euler(-30,0,0));
    }

    void RayCast()
    {
        //カメラの場所からポインタの場所に向かってレイを飛ばす
        Ray ray = Camera.main.ScreenPointToRay(/*new Vector2(ReceivedZKOO.GetHand().position.x, ReceivedZKOO.GetHand().position.y + Screen.height)*/Input.mousePosition);
        RaycastHit hit = new RaycastHit();

        //レイが何か当たっているかを調べる
        if (Physics.Raycast(ray, out hit))
        {
            //当たったオブジェクトを格納
            GameObject obj = hit.collider.gameObject;
            //花火の玉のオブジェクトなら爆発するかのフラグを真にする
            fireWorks = obj.GetComponent<FireWorks>();
            if (fireWorks != null && fireWorks.IsExploded == false)
            {
                fireWorks.ExploadOrderNumber = LockOnNumber++;
                fireWorks.IsExploded = true;
                Instantiate(hitEffect, obj.transform.position, Quaternion.Euler(0, 0, 0));
            }
        }
    }
}
/*
RayCast(当たった時に実行したい関数(), string "判定したいオブジェクトの名前")
{
        //カメラの場所からポインタの場所に向かってレイを飛ばす
        Ray ray = Camera.main.ScreenPointToRay(new Vector2( ReceivedZKOO.GetRightHand().position.x, ReceivedZKOO.GetRightHand().position.y));
        RaycastHit hit = new RaycastHit();

        //レイが何か当たっているかを調べる
        if (Physics.Raycast(ray, out hit))
        {
            //当たったオブジェクトを格納
            string objName = hit.collider.gameObject.name;
            if(objName == "判定したいオブジェクトの名前"){
                当たった時に実行したい関数();
            }
        }    
}
*/
