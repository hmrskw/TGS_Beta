using UnityEngine;
using System.Collections;

public class ButtonScalingChanger : MonoBehaviour
{
    [SerializeField]
    private AnimationCurve curve;

    [SerializeField, Tooltip("目的サイズまでの間隔")]
    private Vector3 endSizeDistance;

    [SerializeField, Tooltip("最大値になるまでの時間"), Range(0.1f, 5.0f)]
    private float moveTime;

    [SerializeField]
    Material textColorChanger;

    //Easingのスタートポジション
    private Vector3 startSize;
    //Easingのエンドポジション
    private Vector3 endSize;

    //Easingの起動し始める時間
    private float startTime;
    //Easingを行っていいかどうか
    private bool canEasing = true;

    //シーンの変更が入っていないかどうか
    private bool isChangeScene = false;

    private Material startIconMaterial;

    void Start()
    {
        //初期位置設定
        startSize = transform.localScale;
        //目的地の設定
        //※あえてendPositionDistanceを作ったのは、
        //子オブジェクトのローカル座標を基準にEasingさせたかったため
        endSize += new Vector3(this.transform.localScale.x + endSizeDistance.x,
                               this.transform.localScale.y + endSizeDistance.y,
                               this.transform.localScale.z + endSizeDistance.z);
        //起動する時間の設定
        startTime = Time.timeSinceLevelLoad;

        textColorChanger.color = new Color(0f, 0f, 0f);

        startIconMaterial = textColorChanger;
    }

    void Update()
    {
        //Rayが当たっている場合は、Easingをストップし、
        //当たっていない場合は常にEasingを行う
        if (!RayCast() && !isChangeScene)
        {
            startIconMaterial.color = new Color(0f, 0f, 0f);

            if (!canEasing)
            {
                canEasing = true;
                startTime = Time.timeSinceLevelLoad;
            }
            else if (canEasing)
            {
                RoundTripEasing(startTime);
            }
        }

        if (RayCast())
        {
            canEasing = false;

            startIconMaterial.color = new Color(255f, 0f, 0f);

            if(Input.GetMouseButtonDown(0))
            {
                isChangeScene = true;
            }

        }
    }
    //第一引数……動かし始める時間
    //private IEnumerator StartEasing(float startTime_,int delayTime_)
    void RoundTripEasing(float startTime_)
    {
        var diff = Time.timeSinceLevelLoad - startTime_;

        if (diff > moveTime)
        {
            transform.localScale = endSize;

            canEasing = false;
        }

        var rate = diff / moveTime;
        var pos = curve.Evaluate(rate);

        transform.localScale = Vector3.Lerp(startSize, endSize, pos);

        if (rate >= 1)
        {
            canEasing = true;
            rate = 0;

            startTime = Time.timeSinceLevelLoad;

            RoundTripEasing(startTime);
        }
    }

    bool RayCast()
    {
        //カメラの場所からポインタの場所に向かってレイを飛ばす
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
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
