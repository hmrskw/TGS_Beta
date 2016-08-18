using UnityEngine;
using System.Collections;
using System;

public class FireWorks : MonoBehaviour
{
    enum FIRE_WORKS_TYPE
    {
        NORMAL, IGNORE
    }

    [SerializeField, Tooltip("花火が不発になる高さ")]
    float deadLine;

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

    GameObject fireWorksImpact;
    public GameObject FireWorksImpact
    {
        set { fireWorksImpact = value; }
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
    }

    void Start()
    {
        deflection = 0;
        fireWorksType = FIRE_WORKS_TYPE.NORMAL;
        isExploded = false;
        //size = 1;
    }

    void Update()
    {
        if(isDeflection)deflection = Mathf.Sin(Time.frameCount * moveSpeed);
        transform.Translate(deflection, speed, 0);

        lockOnMarker.SetActive(isExploded);

        if (isExploded && ReceivedZKOO.GetHand(ReceivedZKOO.HAND.RIGHT).isTouching == false)
        {
            /*
            //不発状態でない玉がボーダーライン超えていたとき
            if (transform.position.y > borderLine)
            {
                if (fireWorksType == FIRE_WORKS_TYPE.NORMAL)
                {
                    Explosion((int)this.transform.position.y);
                }
                else if (fireWorksType == FIRE_WORKS_TYPE.IGNORE)
                {
                    Miss();
                }
            }
            */
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
        yield return new WaitForSeconds(exploadOrderNumber * 0.1f);

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

    //データマネージャ－のパスを受け取る
    //public void SetDataManager(DataManager dataManager)
    //{
        //this.dataManager = dataManager;
    //}

    //色の変更
    //public void setColor(Vector4 color)
    //{
        //this.GetComponent<Renderer>().material.color = color;
    //}

    //爆発したときの処理
    private void Explosion(int score)
    {
        //FireworksSeedを削除
        Destroy(this.gameObject);
        //if (dataManager != null)
        //{
        //スコアを加算
        //dataManager.AddScore(score);
        //}
        ScoreManager.AddScore(score + exploadOrderNumber);

        //花火のパーティクルを生成
        GameObject fireworksParticle = Instantiate(fireWorksImpact, transform.position, Quaternion.Euler(0, 0, 0)) as GameObject;

        //大きさを変える
        fireworksParticle.transform.localScale *= size;
        //花火の色を設定
        //fireworksParticle.GetComponent<Particle>().setColor(this.GetComponent<Renderer>().material.color);
    }

    private void Miss()
    {
        //FireworksSeedを削除
        Destroy(this.gameObject);
        //不発用パーティクルの生成
        Instantiate(smoke, this.transform.position, Quaternion.Euler(0, 0, 0));
    }
}