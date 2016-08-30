using UnityEngine;
using System.Collections;

using UnityEngine.SceneManagement;

using UnityEngine.Assertions;

public class ButtonReaction : MonoBehaviour
{
    [SerializeField]
    TutorialSceneChanger tutorialSceneChanger;

    [SerializeField]
    TutorialFireWorksCreater tutorialFireworksCreater;

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

    [SerializeField]
    Material textColorChanger;

    //Easingのスタートポジション。0が看板、1がボタン用
    private Vector3[] startPosition = new Vector3[2];

    //説明が何ページ目かのカウント
    private int pageCount = 0;

    //看板を回転させていいかどうか
    //private bool canRotation = false;
    
    //Easingを始めていいかどうか
    private bool canEasing = false;
	public bool CanEasin{
		get{return canEasing;}
	}

    //Easingをするために必要な起動時間
    private float startTime;

    //「次へ」アイコンのマテリアル変更用
    private Material pageTurnOverButtonIconMaterial;

    //「次へ」ボタンを押されたときに、花火のポイントをリセットする
    //bool isPointReset = false;
    //lockOnNumberを貰うために使う
    int lockOnNumber = 0;

    void Start()
    {
        lockOnNumber = tutorialFireworksCreater.LockOnNumber;

        //Easingを始めるポジションの初期化
        startPosition[0] = signborad.transform.position;
        startPosition[1] = pageTurnOverButton[0].transform.position;

        //「次へ」アイコンのマテリアルを取得       
        pageTurnOverButtonIconMaterial = textColorChanger;

        pageTurnOverButton[0].SetActive(false);
    }

    void Update()
    {
        lockOnNumber = tutorialFireworksCreater.LockOnNumber;

        if (RayCast("UI"))
        {
            //レイが当たったら次へアイコンの色を変更
            //TIPS：色は現在適当なのでのちのち変更する
            pageTurnOverButtonIconMaterial.color = new Color(255f, 0f, 0f);

            if (Input.GetMouseButtonDown(0))
            {
                if (pageCount == 0)
                {

                    //ページを次のページに変更
                    pageCount++;

                    pageTurnOverButton[0].SetActive(false);

                    //看板の回転を許可
                    //canRotation = true;
                }
                else if (pageCount == 1)
                {
                    //ページを次のページに変更
                    pageCount++;
                    //看板の回転を許可
                    //canRotation = true;
                    //ボタンを2つ目から3つ目に変える
                    //ChangeButtonUi();
					pageTurnOverButton[0].SetActive(false);
                }
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
            //光が当たっていないときは真っ黒にする
            pageTurnOverButtonIconMaterial.color = new Color(0f,0f,0f);
        }

        signborad.transform.rotation = Quaternion.Slerp(signborad.transform.rotation, Quaternion.Euler(-30, 0, 120 * pageCount), 0.07f);

        CheckFireworksHowExplosion();

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

    void CheckFireworksHowExplosion()
    {
        //単発で爆発させているか
        if (pageCount == 0)
        {
            if (RayCast("FireWorksSeed") && !Input.GetMouseButton(0))
            {
                pageTurnOverButton[0].SetActive(true);
            }
        }
		if (pageCount == 1)
		{
			if (lockOnNumber > 1)
			{
				pageTurnOverButton[0].SetActive(true);
			}
		}
        //させていたら複数爆発させたかのチェックへ
        else if (pageCount == 2)
        {
			if (RayCast("FireWorksSeed") && !Input.GetMouseButton(0))
			{
				ChangeButtonUi();
			}
        }
    }

    bool RayCast(string tagName_)
    {
        //カメラの場所からポインタの場所に向かってレイを飛ばす
        Ray ray = Camera.main.ScreenPointToRay(/*new Vector2(ReceivedZKOO.GetHand().position.x, ReceivedZKOO.GetHand().position.y + Screen.height)*/Input.mousePosition);
        RaycastHit hit = new RaycastHit();

        //レイが何か当たっているかを調べる
        if (Physics.Raycast(ray, out hit))
        {
            //当たったオブジェクトを格納
            GameObject obj = hit.collider.gameObject;

            if (obj.CompareTag(tagName_))
            {
                return true;
            }
        }
        return false;
    }
}