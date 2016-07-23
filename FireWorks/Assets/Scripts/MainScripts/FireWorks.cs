using UnityEngine;
using System.Collections;
using System;

public class FireWorks : MonoBehaviour {
    enum FIRE_WORKS_TYPE
    {
        NORMAL, IGNORE
    }

    [SerializeField,Tooltip("花火が不発になる高さ")]
    float deadLine;

    [SerializeField,Tooltip("これより低い場所では花火は爆発しない")]
    float borderLine;

    //花火が登っていく速さ
    [SerializeField]
    float speed;

    [SerializeField]
    Material[] mat = new Material[Enum.GetNames(typeof(FIRE_WORKS_TYPE)).Length];

    [SerializeField, Tooltip("成功したときに発生させるパーティクル")]
    GameObject fireWorks;

    [SerializeField,Tooltip("失敗したときに発生させるパーティクル")]
    GameObject smoke;

    FIRE_WORKS_TYPE fireWorksType;

    DataManager dataManager;

//    GameObject instantiateObj;

    //花火が不発かどうか
    private bool isExploded;
    public bool IsExploded
    {
        get { return isExploded; }
        set {isExploded = value; }
    }

    // Use this for initialization
    void Start () {
        int rand = UnityEngine.Random.Range(0, 3) % 2;
        fireWorksType = (FIRE_WORKS_TYPE)rand;
        this.GetComponent<Renderer>().material = mat[(int)fireWorksType];
        isExploded = false;
    }

    // Update is called once per frame
    void Update () {
        transform.Translate(0, speed, 0);

        if (isExploded)
        {
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
                    Explosion(5);
                }
            }
        }
    }

    public void SetDataManager(DataManager dataManager)
    {
        this.dataManager = dataManager;
    }

    private void Explosion(int score)
    {
        Destroy(this.gameObject);
        dataManager.AddScore(score);
        Instantiate(fireWorks, this.transform.position, Quaternion.Euler(0, 0, 0));
    }

    private void Miss()
    {
        Destroy(this.gameObject);
        Instantiate(smoke, this.transform.position, Quaternion.Euler(0, 0, 0));
    }
}
