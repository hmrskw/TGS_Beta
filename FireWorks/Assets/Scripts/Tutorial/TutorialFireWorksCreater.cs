using UnityEngine;
using System.Collections;

public class TutorialFireWorksCreater : MonoBehaviour
{

    [SerializeField, Tooltip("玉が発射される位置")]
    Vector3[] fireWorksInitPosition;

    [SerializeField, Tooltip("玉")]
    GameObject fireWorksSeed;

    [SerializeField, Tooltip("爆発")]
    GameObject fireWorksImpact;

    //シーン内での経過時間
    float time;

    FireWorks fireWorks;

    int LockOnNumber;

    void Start()
    {
        //各値の初期化
        time = 0;
        LockOnNumber = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //ポインターが当たった時の処理
        RayCast();

        //時間の更新
        time += Time.deltaTime;

        if (!ReceivedZKOO.GetHand(ReceivedZKOO.HAND.RIGHT).isTouching) LockOnNumber = 0;

        if (time > 2)
        {
            //玉の生成
            GameObject seedObj = Instantiate(
                fireWorksSeed,//玉のプレハブ
                fireWorksInitPosition[0],//発射位置
                Quaternion.identity//角度
                ) as GameObject;

            fireWorks = seedObj.GetComponent<FireWorks>();

            if (fireWorks != null)
                fireWorks.FireWorksImpact = fireWorksImpact;

            time = 0;
        }
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
            if (fireWorks != null)
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
