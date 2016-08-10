using UnityEngine;
using System.Collections;
using UnityEngine.UI;

using System;
using System.Net;
using System.Text;

public class ReceivedZKOO : MonoBehaviour {
    const int HAND_NUM = 2;

    public enum HAND
    {
        RIGHT = 0,
        LEFT,
    }
    public struct ZKOOHandData
    {
        public bool isTracking;
        public bool isTouching;
        public Vector2 position;
        public float rotation;
    }

    [SerializeField, Tooltip("true=ZKOO,false=マウス")]
    bool ZKOOMode = true;

    [SerializeField, Tooltip("0=右手のポインター,1=左手のポインター")]
    GameObject[] pointer = new GameObject[HAND_NUM];

    [SerializeField, Tooltip("0=右手の画像,1=左手の画像")]
    Sprite[] holdHandSprite = new Sprite[HAND_NUM];

    [SerializeField, Tooltip("0=右手の画像,1=左手の画像")]
    Sprite[] openHandSprite = new Sprite[HAND_NUM];

    RectTransform[] rt = new RectTransform[HAND_NUM];
    Image[] img = new Image[HAND_NUM];

    public static ReceivedZKOO Instance
    {
        get; private set;
    }

    private static ZKOOHandData[] hand = new ZKOOHandData[HAND_NUM];
    private static bool nowGripped = false; 

    private System.Net.Sockets.UdpClient udpClient = null;

    private string word;
    int portNumber = 2001;

    //シーンまたいでもオブジェクトが破棄されなくする
    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        //ZKOOで受け取るデータの初期化
        for (int i = 0; i < HAND_NUM; i++)
        {
            hand[i].isTracking = false;
            hand[i].isTouching = false;
            hand[i].position = new Vector2(-1000, -1000);
            hand[i].rotation = 0;
        }

        //ポインターの用意
        for (int i = 0; i < HAND_NUM; i++)
        {
            rt[i] = pointer[i].GetComponent<RectTransform>();
            img[i] = pointer[i].GetComponent<Image>();
            rt[i].anchoredPosition = hand[i].position;
        }

        if (udpClient != null)
        {
            return;
        }

        if (ZKOOMode)
        {
            //UdpClientを作成し、指定したポート番号にバインドする
            IPEndPoint localEP = new IPEndPoint(IPAddress.Any, portNumber);
            udpClient = new System.Net.Sockets.UdpClient(localEP);
        }
    }

    void Update()
    {
        if (ZKOOMode)
        {
            //UDP通信で送られたデータを受け取る
            udpClient.BeginReceive(ReceiveCallback, udpClient);

            //受け取ったデータをZKOOHandDataに変換する
            AdjustmentData();

        }
        else
        {
            hand[0].isTracking = true;
            hand[0].isTouching = Input.GetMouseButton(0);
            hand[0].position = new Vector2(Input.mousePosition.x, Input.mousePosition.y-Screen.height);

            if (hand[0].isTouching) { 
                img[0].sprite = holdHandSprite[0];
            }
            else
            {
                img[0].sprite = openHandSprite[0];
            }
            hand[0].rotation = 0.0f;
        }
        drawPointer();
    }

    //アプリが終了されたとき
    void OnApplicationQuit()
    {
        //UdpClientを閉じて通信をやめる
        if (udpClient != null)
        {
            udpClient.Close();
        }
    }

    //データの受信
    private void ReceiveCallback(IAsyncResult ar)
    {
        udpClient = (System.Net.Sockets.UdpClient)ar.AsyncState;

        //エンコード
        Encoding enc = Encoding.UTF8;

        //非同期受信
        try
        {
            IPEndPoint remoteEP = null;
            //データを受け取る
            byte[] rcvBytes = null;
            while (udpClient.Available != 0)
            {
                rcvBytes = udpClient.Receive(ref remoteEP);
            }
            //受け取ったデータをstring型に変換して格納
            word = enc.GetString(rcvBytes);
        }
        catch (System.Net.Sockets.SocketException ex)
        {
            Console.WriteLine("受信エラー({0}/{1})", ex.Message, ex.ErrorCode);
            return;
        }
        //すでに通信用のソケットが閉じられていたとき
        catch (ObjectDisposedException ex)
        {
            //すでに閉じている時は終了
            Console.WriteLine("Socketは閉じられています。" + ex);
            return;
        }
    }

    //受信したデータをZKOOHandDataに変換する
    void AdjustmentData()
    {
        //カンマとカンマの間に何もなかったら格納しないことにする設定
        System.StringSplitOptions option = StringSplitOptions.RemoveEmptyEntries;

        if (word != null)
        {
            //行に分ける
            string[] handDataString = word.Split(new char[] { '\\' }, option);

            char[] separation = { ',' };
            char[] separationDot = { '.' };

            for (int i = 0; i < handDataString.Length; i++)
            {
                string[] data = new string[5];
                data = DataSeparation(handDataString[i], separation, 4);

                hand[i].isTracking = Convert.ToBoolean(data[0]);

                hand[i].isTouching = Convert.ToBoolean(data[1]);

                string[] handPosition = new string[2];
                handPosition = DataSeparation(data[2], separationDot, 2);

                hand[i].position = new Vector2(Convert.ToSingle(handPosition[0])-Screen.width, (-1 * Convert.ToSingle(handPosition[1]))+Screen.height);

                hand[i].rotation = Convert.ToSingle(data[3]);
            }
//            hand[0].rotation *= -1;
        }
    }

    void drawPointer()
    {
        for (int i = 0; i < HAND_NUM; i++)
        {
            rt[i].anchoredPosition = hand[i].position;
            pointer[i].SetActive(hand[i].isTracking);
            if (hand[i].isTouching)
            {
                img[i].sprite = holdHandSprite[i];
            }
            else
            {
                img[i].sprite = openHandSprite[i];
            }
            rt[i].rotation = Quaternion.Euler(new Vector3(0, 0, hand[i].rotation));
        }
    }

    //第一引数…ReadCsvData関数で一行にされたデータ
    //第二引数…渡されたデータを区切る文字
    //第三引数…第一引数のデータの要素数。for文の周回数
    string[] DataSeparation(string lines_, char[] spliter_, int trialNumber_)
    {
        System.StringSplitOptions option = StringSplitOptions.RemoveEmptyEntries;

        string[] separationData = new string[trialNumber_];

        for (int i = 0; i < trialNumber_; i++)
        {
            string[] readStrData = new string[trialNumber_];
            readStrData = lines_.Split(spliter_, option);
            separationData[i] = readStrData[i];
        }

        return separationData;
    }

    public static ZKOOHandData GetHand(HAND handID)
    {
        return hand[(int)handID];
    }

    public static bool isGripped(HAND handID)
    {
        bool gripped = (hand[(int)handID].isTouching == true && nowGripped == false);

        nowGripped = hand[(int)handID].isTouching;

        return gripped;
    }
    //public static ZKOOHandData GetRightHand(HAND handID)
    //{
    //    return hand[(int)handID];
    //}

    //public static ZKOOHandData GetLeftHand()
    //{
    //    return hand[1];
    //}
}