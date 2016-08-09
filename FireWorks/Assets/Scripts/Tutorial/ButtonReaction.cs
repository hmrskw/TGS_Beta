using UnityEngine;
using System.Collections;

using UnityEngine.SceneManagement;

public class ButtonReaction : MonoBehaviour
{
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
    private bool canRotation = false;
    //Easingを始めていいかどうか
    private bool canEasing = false;

    //Easingをするために必要な起動時間
    private float startTime;


    void Start()
    {
        //Easingを始めるポジションの初期化
        startPosition[0] = signborad.transform.position;
        startPosition[1] = pageTurnOverButton[0].transform.position;
    }

    void Update()
    {
        //FIX:ZKOOの反応がおかしいので、スペースキーで代用している
        //if (ReceivedZKOO.GetRightHand().isTouching)
        {
            // Debug.Log("押された");
            //TIPS:現在はスペースキーで反応するためEasingが連打可能になっている
            if (Input.GetKeyDown(KeyCode.Space))//RayCast())
            {
                Debug.Log("当たった");
                
                if (pageCount == 0)
                {
                    Debug.Log("入った");

                    //ページを次のページに変更
                    pageCount = 1;
                    //看板の回転を許可
                    canRotation = true;
                    //ボタンを１つ目から２つ目に変える
                    ChangeButtonUi();
                }
                else if (pageCount == 1)
                {
                    //Easing開始の許可
                    canEasing = true;
                    //Easingの開始始時間の保存
                    startTime = Time.timeSinceLevelLoad;
                }
            }
        }

        if (canRotation)
        {
            //看板の回転
            RotationSignboard();
        }

        if(canEasing)
        {
            //看板とボタンをEasingで画面外に出して、
            //シーンを変える処理
            StartEasing(startTime, moveTime);
        }
    }

    private void RotationSignboard()
    {
        //  signborad.transform.Rotate(new Vector3(0, 0, 1), 180);

        if (signborad.transform.rotation.z < 180)
        {
            signborad.transform.rotation = Quaternion.Slerp(signborad.transform.rotation, Quaternion.Euler(0, 0, 180), 0.07f);
        }
        else
        {
            canRotation = false;
            tutorialSceneChanger.SceneChange("main");
        }
    }

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
    }

    bool RayCast()
    {
        //カメラの場所からポインタの場所に向かってレイを飛ばす
        Ray ray = Camera.main.ScreenPointToRay(new Vector2(ReceivedZKOO.GetRightHand().position.x, ReceivedZKOO.GetRightHand().position.y + Screen.height));
        Debug.Log("raycast");
        Debug.DrawRay(ray.origin, ray.direction * 100, Color.red);
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