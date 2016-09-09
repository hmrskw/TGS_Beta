using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class FireWorksCreater : MonoBehaviour
{
    //[SerializeField, Tooltip("時間制限")]
    //int timeLimit;

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

    [SerializeField]
    GameObject lockonRankObj;

    [SerializeField]
    Sprite[] lockonRankSprite;

    //シーン内での経過時間
    //float time;

    ReadCSV readCSV;

    //Image lockonRankImage;
    //LockonRank lockonRank;
    SpriteRenderer lockonRankImage;
    LockonRank1 lockonRank;

    bool isEnd;

    //現在何番目（CSVの何行目）の花火を飛ばしているか
    int readFireworksNumber;

    List<GameObject> lockOnSeedObjects = new List<GameObject>();
    List<FireWorks> lockOnObjFireWorks = new List<FireWorks>();
    
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
        //各値の初期化
        readFireworksNumber = 0;
        //time = 0;

        //lockonRankImage = lockonRankObj.GetComponent<Image>();
        //lockonRank = lockonRankObj.GetComponent<LockonRank>();
        lockonRankImage = lockonRankObj.GetComponent<SpriteRenderer>();
        lockonRank = lockonRankObj.GetComponent<LockonRank1>();

        lockonRankObj.SetActive(false);
    }

    void hitCheck()
    {
        //ポインターが当たった時の処理
        GameObject hitObj = MainGameManager.Instance.RayCast();
        if (hitObj != null && hitObj.name == "Utiage(Clone)")
        {
            FireWorks fireWorks = hitObj.GetComponent<FireWorks>();
            if (fireWorks.IsExploded == false)
            {
                //ロックオンされた弾のリストに追加
                lockOnSeedObjects.Add(hitObj);
                lockOnObjFireWorks.Add(fireWorks);

                fireWorks.ExploadOrderNumber = lockOnSeedObjects.Count - 1;
                fireWorks.IsExploded = true;
                Instantiate(hitEffect, hitObj.transform.position, Quaternion.Euler(0, 0, 0));

                if (lockOnSeedObjects.Count >= 3)
                {
                    if (lockOnSeedObjects.Count >= 5)
                    {
                        lockonRankImage.sprite = lockonRankSprite[1];
                        lockonRankImage.color = new Color(0.9f, 0.9f, 0.9f, 0f);
                    }
                    else
                    {
                        lockonRankImage.sprite = lockonRankSprite[0];
                        lockonRankImage.color = new Color(0.7f, 0.7f, 0.7f, 0f);
                    }

                    if (lockonRankObj.activeInHierarchy == false) lockonRankObj.SetActive(true);
                    else lockonRank.StartTime = Time.timeSinceLevelLoad;
                }
            }
        };
    }

    void Update()
    {
        hitCheck();

        //timeBarSlider.value = 1-time/timeLimit;
        timeBarSlider.value = 1 - MainGameManager.Instance.GameTime / MainGameManager.Instance.TimeLimit;

        if (MainGameManager.Instance.TimeLimit >= MainGameManager.Instance.GameTime/*MainGameManager.Instance.IsEnd*/)
        {
            //時間の更新
            //time += Time.deltaTime;

            //入力が無ければロックオンした数を０に戻す
            if (Input.GetMouseButton(0) == false)
            {
                if (lockOnSeedObjects.Count >= 5 && gallerys.isPlaying == false) StartCoroutine(PlayAudio());
                lockOnObjFireWorks.Clear();
                lockOnSeedObjects.Clear();
            }

            //CsvDataの配列長さを超えていないかのチェック
            if (readCSV.CsvData.Length > readFireworksNumber)
            {
                //発射時間が現在の経過時間と同じときの処理
                while (readCSV.CsvData[readFireworksNumber].shotTiming <= /*time*/MainGameManager.Instance.GameTime)
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

                    FireWorks fireWorks = seedObj.GetComponent<FireWorks>();

                    if (fireWorks != null)
                    {
                        fireWorks.FireWorksImpact = fireWorksImpact
                            [(int)readCSV.CsvData[readFireworksNumber].fireｗorksType]
                            .Color[readCSV.CsvData[readFireworksNumber].fireworksColor];
                        fireWorks.Size = readCSV.CsvData[readFireworksNumber].fireworksSize;
                    }

                    //飛ばす花火を更新
                    readFireworksNumber++;
                    //飛ばしていたのが最後の花火ならループを抜ける
                    if (readCSV.CsvData.Length <= readFireworksNumber) break;
                }

                for (int i = 0; i < lockOnSeedObjects.Count; i++)
                {
                    lockOnObjFireWorks[i].ExploadOrderNumber = i;
                    if (lockOnSeedObjects[i] == null || lockOnSeedObjects[i].transform.position.y >= 40)
                    {
                        lockOnObjFireWorks.RemoveAt(i);
                        lockOnSeedObjects.RemoveAt(i);
                    }
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

    /*void RayCast()
    {
        //カメラの場所からポインタの場所に向かってレイを飛ばす
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit = new RaycastHit();

        //レイが何か当たっているかを調べる
        if (Physics.Raycast(ray, out hit))
        {
            //当たったオブジェクトを格納
            GameObject obj = hit.collider.gameObject;
            //花火の玉のオブジェクトなら爆発するかのフラグを真にする
            FireWorks fireWorks = obj.GetComponent<FireWorks>();

            if (fireWorks != null && fireWorks.IsExploded == false)
            {
                //ロックオンされた弾のリストに追加
                lockOnSeedObjects.Add(obj);
                lockOnObjFireWorks.Add(fireWorks);

                fireWorks.ExploadOrderNumber = lockOnSeedObjects.Count-1;
                fireWorks.IsExploded = true;
                Instantiate(hitEffect, obj.transform.position, Quaternion.Euler(0, 0, 0));

                if (lockOnSeedObjects.Count >= 3)
                {
                    ///TODO:マウスカーソルの左上に表示するのに使う
                    //lockonRankImage.transform.position = obj.transform.position + new Vector3(-5, 5, 0);

                    if (lockOnSeedObjects.Count >= 5)
                    {
                        lockonRankImage.sprite = lockonRankSprite[1];
                        lockonRankImage.color = new Color(0.9f, 0.9f, 0.9f,0f);
                    }
                    else
                    {
                        lockonRankImage.sprite = lockonRankSprite[0];
                        lockonRankImage.color = new Color(0.7f,0.7f,0.7f, 0f);
                    }

                    if (lockonRankObj.activeInHierarchy == false) lockonRankObj.SetActive(true);
                    else
                    {
                        ///TODO:画面の左上に表示するのに使う
                        lockonRank.StartTime = Time.timeSinceLevelLoad;

                        ///TODO:マウスカーソルの左上に表示するのに使う
                        //lockonRankObj.SetActive(false);
                        //lockonRankObj.SetActive(true);
                    }
                }
            }
        }
    }*/
}