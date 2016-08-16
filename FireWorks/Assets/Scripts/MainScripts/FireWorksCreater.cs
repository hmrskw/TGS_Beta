using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FireWorksCreater : MonoBehaviour
{

    enum DIRECTION
    {
        LEFT = 1, RIGHT = -1
    }
    [SerializeField, Tooltip("時間制限")]
    int timeLimit;

    [SerializeField, Tooltip("玉が発射される位置")]
    Vector3[] fireWorksInitPosition = new Vector3[5];

    [SerializeField, Tooltip("玉が発射される角度")]
    float[] fireWorksAngle = new float[3];


    [SerializeField, Tooltip("玉")]
    GameObject fireWorksSeed;

//    [SerializeField, Tooltip("爆発")]
//    GameObject[] fireWorksImpact;

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

    ReadCSV CSVReader;

    //[SerializeField]
    //DataManager dataManager;

    //シーン内での経過時間
    float time;

    FireWorks fireWorks;
    ReadCSV readCSV;

    int LockOnNumber;

    //現在何番目（CSVの何行目）の花火を飛ばしているか
    int readFireworksNumber;

    MainSceneChanger mSceneChanger;
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
        mSceneChanger = new MainSceneChanger();
        //各値の初期化
        readFireworksNumber = 0;
        time = 0;
        LockOnNumber = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //ポインターが当たった時の処理
        RayCast();

        if (timeLimit > time)
        {
            //時間の更新
            time += Time.deltaTime;

            //CsvDataの配列長さを超えていないかのチェック
            if (readCSV.CsvData.Length > readFireworksNumber)
            {
                if (!ReceivedZKOO.GetHand(ReceivedZKOO.HAND.RIGHT).isTouching) LockOnNumber = 0;

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
                        //スコアを計算をfireworksが行うためにパスを渡す
                        //fireWorks.SetDataManager(dataManager);

                        fireWorks.FireWorksImpact = fireWorksImpact
                            [(int)readCSV.CsvData[readFireworksNumber].fireｗorksType]
                            .Color[readCSV.CsvData[readFireworksNumber].fireworksColor];

/*                        switch (readCSV.CsvData[readFireworksNumber].fireｗorksType)
                        {
                            case EnumDefinition.FireｗorksType.BOTAN:
                                fireWorks.FireWorksImpact = fireWorksImpact[0,readCSV.CsvData[readFireworksNumber].fireworksColor];
                                break;
                            case EnumDefinition.FireｗorksType.DOSEI:
                                fireWorks.FireWorksImpact = fireWorksImpact[1,readCSV.CsvData[readFireworksNumber].fireworksColor];
                                break;
                            case EnumDefinition.FireｗorksType.KARAI:
                                fireWorks.FireWorksImpact = fireWorksImpact[2,readCSV.CsvData[readFireworksNumber].fireworksColor];
                                break;
                            case EnumDefinition.FireｗorksType.KIKU:
                                fireWorks.FireWorksImpact = fireWorksImpact[3,readCSV.CsvData[readFireworksNumber].fireworksColor];
                                break;
                            case EnumDefinition.FireｗorksType.SINIRI_KIKU:
                                fireWorks.FireWorksImpact = fireWorksImpact[4,readCSV.CsvData[readFireworksNumber].fireworksColor];
                                break;
                            case EnumDefinition.FireｗorksType.MANGEKYOU:
                                fireWorks.FireWorksImpact = fireWorksImpact[5,readCSV.CsvData[readFireworksNumber].fireworksColor];
                                break;
                            default:
                                fireWorks.FireWorksImpact = fireWorksImpact[0,readCSV.CsvData[readFireworksNumber].fireworksColor];
                                break;
                        }
*/
                        //CSVの色の設定に合わせて色を変更
                        //fireWorks.setColor(readCSV.CsvData[readFireworksNumber].fireworksColor);
                        //重力を使用する場合はRigidbodyをつける
                        //if (readCSV.CsvData[readFireworksNumber].isApplyGravity)
                        //{
                            //seedObj.AddComponent<Rigidbody>();
                        //}
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

            if (fireWorksSeeds.Length == 0)
                StartCoroutine(SceneChanger());
        }
    }

    IEnumerator SceneChanger()
    {
        yield return new WaitForSeconds(5f);
        mSceneChanger.SceneChange("Result");
        yield return null;
    }

    void RayCast()
    {
        //カメラの場所からポインタの場所に向かってレイを飛ばす
        Ray ray = Camera.main.ScreenPointToRay(new Vector2(ReceivedZKOO.GetHand(ReceivedZKOO.HAND.RIGHT).position.x, ReceivedZKOO.GetHand(ReceivedZKOO.HAND.RIGHT).position.y + Screen.height));
        RaycastHit hit = new RaycastHit();
        Debug.DrawRay(ray.origin, ray.direction * 100, Color.red);

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
            }
        }
    }
}
/*
RayCast(当たった時に実行したい関数(), string "判定したいオブジェクトの名前")
{
        //カメラの場所からポインタの場所に向かってレイを飛ばす
        Ray ray = Camera.main.ScreenPointToRay(new Vector2( ReceivedZKOO.GetRightHand().position.x, ReceivedZKOO.GetRightHand().position.y));
        Debug.Log("raycast");
        Debug.DrawRay(ray.origin,ray.direction*100,Color.red);
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
