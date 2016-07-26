using UnityEngine;
using System.Collections;

public class FireWorksCreater : MonoBehaviour {

    enum DIRECTION
    {
        LEFT = 1, RIGHT = -1
    }
    [SerializeField, Tooltip("玉が発射される位置")]
    Vector3[] fireWorksInitPosition = new Vector3[5];

    [SerializeField, Tooltip("玉が発射される角度")]
    float[] fireWorksAngle = new float[3];


    [SerializeField, Tooltip("玉が飛ぶ速さ")]
    GameObject fireWorksSeed;

    [SerializeField]
    GameObject CSVReader;

    [SerializeField]
    DataManager dataManager;

    //シーン内での経過時間
    float time;

    FireWorks fireWorks;
    ReadCSV readCSV;

    //現在何番目（CSVの何行目）の花火を飛ばしているか
    int readFireworksNumber;

    void Start() {
        //CSVファイルを読み込む
        readCSV = CSVReader.GetComponent<ReadCSV>();
        readCSV.ReadFile();

        //各値の初期化
        readFireworksNumber = 0;
        time = 0;
    }

    // Update is called once per frame
    void Update() {
        //マウスがドラッグ状態で当たった時の処理
        //TIPS:ZKOO設定後は右手に変更
        if (Input.GetMouseButton(0))
        {
            RayCast();
        }

        if (!dataManager.IsGameEnd)
        {
            //時間の更新
            time += Time.deltaTime;

            //CsvDataの配列長さを超えていないかのチェック
            if (readCSV.CsvData.Length > readFireworksNumber)
            {
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
                        fireWorks.SetDataManager(dataManager);
                        //CSVの色の設定に合わせて色を変更
                        fireWorks.setColor(readCSV.CsvData[readFireworksNumber].fireworksColor);
                        //重力を使用する場合はRigidbodyをつける
                        if (readCSV.CsvData[readFireworksNumber].isApplyGravity)
                        {
                            seedObj.AddComponent<Rigidbody>();
                        }
                    }

                    //飛ばす花火を更新
                    readFireworksNumber++;
                    //飛ばしていたのが最後の花火ならループを抜ける
                    if (readCSV.CsvData.Length <= readFireworksNumber) break;
                }
            }
        }
    }

    void RayCast()
    {
        //カメラの場所からマウスポインタの場所に向かってレイを飛ばす
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit = new RaycastHit();

        //レイが何か当たっているかを調べる
        if (Physics.Raycast(ray, out hit))
        {
            //当たったオブジェクトを格納
            GameObject obj = hit.collider.gameObject;
            //花火の玉のオブジェクトなら爆発するかのフラグを真にする
            fireWorks = obj.GetComponent<FireWorks>();
            if (fireWorks != null)
            {
                fireWorks.IsExploded = true;
            }
        }
    }
}
