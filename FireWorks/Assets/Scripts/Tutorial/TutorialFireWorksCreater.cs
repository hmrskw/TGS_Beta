using UnityEngine;
using System.Collections;

public class TutorialFireWorksCreater : MonoBehaviour
{
	[SerializeField]
	ButtonReaction reaction;

    [SerializeField, Tooltip("玉が発射される位置")]
    Vector3[] fireWorksInitPosition;

    [SerializeField, Tooltip("玉")]
    GameObject fireWorksSeed;

    [SerializeField, Tooltip("爆発")]
    GameObject fireWorksImpact;

    [SerializeField]
    GameObject hitEffect;

    [SerializeField, Tooltip("何秒に一度発射するか")]
    float frequency;
     
    //シーン内での経過時間
    float time;

    FireWorks fireWorks;

    //int lockOnNumber;
    private int lockOnNumber;

    public int LockOnNumber
    {
        get { return lockOnNumber; }
    }

    /*private bool isExplode;

    public bool  IsExplode
    {
        get { return isExplode; }
    }*/

    void Awake()
    {
        //各値の初期化
        time = 0;
        lockOnNumber = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //ポインターが当たった時の処理
        RayCast();

        //時間の更新
        time += Time.deltaTime;

        if (!Input.GetMouseButton(0)) lockOnNumber = 0;

		if (time > frequency && reaction.CanEasin == false)
        {
            for (int i = 0; i < fireWorksInitPosition.Length; i++)
            {
                //玉の生成
                GameObject seedObj = Instantiate(
                    fireWorksSeed,//玉のプレハブ
                    fireWorksInitPosition[i],//発射位置
                    Quaternion.Euler(-15, 0, 0)
                    //Quaternion.identity//角度
                    ) as GameObject;

                fireWorks = seedObj.GetComponent<FireWorks>();

                if (fireWorks != null)
                {
                    fireWorks.FireWorksImpact = fireWorksImpact;
                    fireWorks.Size = 0.5f;
                }
            }
            time = 0;
        }
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
                fireWorks.ExploadOrderNumber = lockOnNumber++;
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
        Debug.Log("raycast");
        s.DrawRay(ray.origin,ray.direction*100,Color.red);
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
