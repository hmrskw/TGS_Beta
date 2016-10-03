using UnityEngine;
using System.Collections;
//using System;

public class FireWorks : MonoBehaviour
{
    enum FIRE_WORKS_TYPE
    {
        NORMAL, IGNORE
    }

    [SerializeField, Tooltip("花火が不発になる高さ")]
    public float deadLine;

    [SerializeField, Tooltip("これより低い場所では花火は爆発しない")]
    float borderLine;

    //花火が登っていく速さ
    [SerializeField]
    float speed;

    [SerializeField, Tooltip("花火が打ちあがるときの振れ幅")]
    float moveSpeed;

    [SerializeField]
    bool isDeflection;

    [SerializeField, Tooltip("失敗したときに発生させるパーティクル")]
    GameObject smoke;

    [SerializeField]
    GameObject lockOnMarker;

    [SerializeField]
    float delay;

    GameObject fireWorksImpact;
    public GameObject FireWorksImpact
    {
        get { return fireWorksImpact; }
        set { fireWorksImpact = value; }
    }

    float fireWorksRotation;
    public float FireWorksRotation
    {
        get { return fireWorksRotation; }
        set {
            if(fireWorksImpact.tag == "Mark") fireWorksRotation = Random.Range(-value,value);
            else fireWorksRotation = 0f;
        }
    }

    float size;
    public float Size
    {
        set { size = 0.5f + value; }
    }

    //触れて爆発させる花火かどうか（使うかは不明）
    FIRE_WORKS_TYPE fireWorksType;

    float deflection;

    //花火が不発かどうか
    private bool isExploded;
    public bool IsExploded
    {
        get { return isExploded; }
        set { isExploded = value; }
    }

    //何番目にロックオンされたか
    private int exploadOrderNumber;
    public int ExploadOrderNumber
    {
        set { exploadOrderNumber = value; }
        get { return exploadOrderNumber; }
    }

    void Start()
    {
        deflection = 0;
        fireWorksType = FIRE_WORKS_TYPE.NORMAL;
        isExploded = false;
    }

    void Update()
    {
        if(isDeflection)deflection = Mathf.Sin(Time.frameCount*1.2f);
        transform.Translate(deflection* moveSpeed, speed, 0);

        lockOnMarker.transform.Translate(-deflection * moveSpeed, 0, 0);
        lockOnMarker.SetActive(isExploded && Input.GetMouseButton(0));

        if (isExploded && /*ReceivedZKOO.GetHand().isTouching*/Input.GetMouseButton(0) == false)
        {
            StartCoroutine(expload());
        }
        else
        {
            //不発状態のままデッドライン超えたとき
            if (transform.position.y > deadLine)
            {
                if (fireWorksType == FIRE_WORKS_TYPE.NORMAL)
                {
                    Miss();
                }
                else if (fireWorksType == FIRE_WORKS_TYPE.IGNORE)
                {
                    transform.position += new Vector3(0, 1, 0);
                    Explosion(0);
                }
            }
        }
    }

    IEnumerator expload()
    {
        yield return new WaitForSeconds(exploadOrderNumber * delay);

        //不発状態でない玉がボーダーライン超えていたとき
        if (transform.position.y > borderLine)
        {
            if (fireWorksType == FIRE_WORKS_TYPE.NORMAL)
            {
                Explosion(1);
            }
            else if (fireWorksType == FIRE_WORKS_TYPE.IGNORE)
            {
                Miss();
            }
        }
        yield return null;
    }

    //爆発したときの処理
    private void Explosion(int score)
    {
        //FireworksSeedを削除
        Destroy(this.gameObject);
        ScoreManager.AddScore(score + exploadOrderNumber);

        //花火のパーティクルを生成
        GameObject fireworksParticle = Instantiate(fireWorksImpact, transform.position, Quaternion.Euler(0, 0, fireWorksRotation)) as GameObject;

        //大きさを変える
        fireworksParticle.transform.localScale *= size;
    }

    private void Miss()
    {
        //FireworksSeedを削除
        Destroy(this.gameObject);
        //不発用パーティクルの生成
        Instantiate(smoke, this.transform.position, Quaternion.Euler(0, 0, 0));
    }
}