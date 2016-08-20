using UnityEngine;
using System.Collections;

using UnityEngine.SceneManagement;

public class ButtonReaction : MonoBehaviour
{
    [SerializeField]
    TutorialSceneChanger tutorialSceneChanger;

    [SerializeField, Tooltip("画面左下に描画する看板")]
    private GameObject signborad;

    [SerializeField, Tooltip("画面右下に描画するボタン。0番が一枚目、１番が二枚目")]
    private GameObject[] pageTurnOverButton = new GameObject[2];

    [SerializeField, Tooltip("看板とボタンのEasingに使う")]
    private AnimationCurve curve;

    [SerializeField, Tooltip("Easingの目的地。０番が看板用、1番がボタン用")]
    private Vector3[] endPositon = new Vector3[2];

    [SerializeField, Tooltip("目的地にたどり着くまでの時間"), Range(1, 5)]
    private int moveTime;

    //Easingのスタートポジション。0が看板、1がボタン用
    private Vector3[] startPosition = new Vector3[2];

    //説明が何ページ目かのカウント
    private int pageCount = 0;

    //看板を回転させていいかどうか
    //private bool canRotation = false;
    
    //Easingを始めていいかどうか
    private bool canEasing = false;

    //Easingをするために必要な起動時間
    private float startTime;

    //「次へ」アイコンのマテリアル変更用
    private Material pageTurnOverButtonIconMaterial;

    void Start()
    {   
        //Easingを始めるポジションの初期化
        startPosition[0] = signborad.transform.position;
        startPosition[1] = pageTurnOverButton[0].transform.position;

        //「次へ」アイコンのマテリアルを取得
        pageTurnOverButtonIconMaterial = pageTurnOverButton[0].GetComponent<Renderer>().material;
    }

    void Update()
    {
        if (/*ReceivedZKOO.GrippedHand()*/RayCast())
        {
            //レイが当たったら次へアイコンの色を変更
            //TIPS：色は現在適当なのでのちのち変更する
            pageTurnOverButtonIconMaterial.color = new Color(255f, 0f, 0f);


            //TIPS:現在はスペースキーで反応するためEasingが連打可能になっている
            if (Input.GetMouseButtonDown(0))
            {
                if (pageCount == 0)
                {

                    //ページを次のページに変更
                    pageCount++;
                    //看板の回転を許可
                    //canRotation = true;
                    //ボタンを１つ目から２つ目に変える
                    ChangeButtonUi();
                }
                /*else if (pageCount == 1)
                {
                    //ページを次のページに変更
                    pageCount++;
                    //看板の回転を許可
                    //canRotation = true;
                    //ボタンを１つ目から２つ目に変える
                    ChangeButtonUi();
                }*/
                else
                {
                    if (!canEasing)
                    {
                        //Easing開始の許可
                        canEasing = true;
                        //Easingの開始始時間の保存
                        startTime = Time.timeSinceLevelLoad;
                    }
                }
            }
        }
        else
        {
            //光が当たっていないときは真っ白にする
            pageTurnOverButtonIconMaterial.color = new Color(255f, 255f, 255f);
        }

        signborad.transform.rotation = Quaternion.Slerp(signborad.transform.rotation, Quaternion.Euler(0, 0, 120 * pageCount), 0.07f);

/*        if (canRotation)
        {
            //看板の回転
            RotationSignboard();
        }
        */
        if(canEasing)
        {
            //看板とボタンをEasingで画面外に出して、
            //シーンを変える処理
            StartEasing(startTime, moveTime);
        }
    }

    /*private void RotationSignboard()
    {
        //  signborad.transform.Rotate(new Vector3(0, 0, 1), 180);

        if (pageCount == 1)
        {
            signborad.transform.rotation = Quaternion.Slerp(signborad.transform.rotation, Quaternion.Euler(0, 0, 120), 0.07f);
        }
        else if (pageCount == 2)
        {
            signborad.transform.rotation = Quaternion.Slerp(signborad.transform.rotation, Quaternion.Euler(0, 0, 240), 0.07f);
        }
        else
        {
            canRotation = false;
            //tutorialSceneChanger.SceneChange("Main");
        }
    }*/

    private void ChangeButtonUi()
    {
        pageTurnOverButton[0].SetActive(false);
        pageTurnOverButton[1].SetActive(true);
    }

    //看板とボタンのイージング
    //第一引数……動かし始める時間
    //第二引数……シーンを遷移させるまでの時間。兼、Easingで目的地に到着させる時間
    //private IEnumerator StartEasing(float startTime_,int delayTime_)
    void StartEasing(float startTime_, int delayTime_)
    {   
        var diff = Time.timeSinceLevelLoad - startTime_;

        if (diff > moveTime)
        {
            signborad.transform.position = endPositon[0];
            pageTurnOverButton[1].transform.position = endPositon[1];
            canEasing = false;
        }

        var rate = diff / moveTime;
        var pos = curve.Evaluate(rate);

        signborad.transform.position = Vector3.Lerp(startPosition[0], endPositon[0], pos);
        pageTurnOverButton[1].transform.position = Vector3.Lerp(startPosition[1], endPositon[1], pos);

        if(rate >= 1)tutorialSceneChanger.SceneChange("Main");
    }

    bool RayCast()
    {
        //カメラの場所からポインタの場所に向かってレイを飛ばす
        Ray ray = Camera.main.ScreenPointToRay(/*new Vector2(ReceivedZKOO.GetHand().position.x, ReceivedZKOO.GetHand().position.y + Screen.height)*/Input.mousePosition);
        RaycastHit hit = new RaycastHit();

        //レイが何か当たっているかを調べる
        if (Physics.Raycast(ray, out hit))
        {
            //当たったオブジェクトを格納
            GameObject obj = hit.collider.gameObject;

            if (obj.CompareTag("UI"))
            {
                return true;
            }
        }

        return false;
    }
}