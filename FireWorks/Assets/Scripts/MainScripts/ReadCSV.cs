using UnityEngine;
using System.Collections;

using System;
using System.Collections.Generic;
using System.IO;

public class ReadCSV : MonoBehaviour
{
    public struct CSVData
    {
        public float shotTiming;
        public int fireWorksType;
        public float shotPosition;
    }

    enum ElementsName
    {
        SHOT_TIMING = 0,
        FIREWORKS_TYPE,
        SHOT_POSITION
    }
    //csvデータの要素数
    const int CSVDATA_ELEMENTS = 3;

    //csvから取り出した情報を入れる配列
    private CSVData[] csvData;

    void Start()
    {
        //CSVデータの格納位置のパス
        //TIPS:なにかしらの形でパスを自由に変えられるようにしておく
        string path = Application.dataPath + "/CSVFiles/Sample.csv";

        //CSVデータを読み込んで、行に分割
        string[] lines = ReadCsvData(path);

        //csvデータの初期化
        csvData = new CSVData[lines.Length];

        //カンマ分けされたデータを仮格納する。その初期化
        string[] didCommaSeparationData = new string[lines.Length];

        for (int i = 0; i < lines.Length; i++)
        {
            //カンマ分けされたデータを格納
            didCommaSeparationData = DataCommaSeparation(lines[i]);

            //データをcsvDataに格納
            csvData[i].shotTiming    = Convert.ToSingle(didCommaSeparationData[(int)ElementsName.SHOT_TIMING]);
            csvData[i].fireWorksType = Convert.ToInt16(didCommaSeparationData[(int)ElementsName.FIREWORKS_TYPE]);
            csvData[i].shotPosition  = Convert.ToSingle(didCommaSeparationData[(int)ElementsName.SHOT_POSITION]);
        }
    }

    string[] ReadCsvData(string path_)
    {
        //ファイル読み込み
        StreamReader sr = new StreamReader(path_);
        //stringに変換
        string strStream = sr.ReadToEnd();

        //カンマとカンマの間に何もなかったら格納しないことにする設定
        System.StringSplitOptions option = StringSplitOptions.RemoveEmptyEntries;

        //行に分ける
        string[] lines = strStream.Split(new char[] { '\r', '\n' }, option);

        return lines;
    }

    string[] DataCommaSeparation(string lines_)
    {
        //カンマとカンマの間に何もなかったら格納しないことにする設定
        System.StringSplitOptions option = StringSplitOptions.RemoveEmptyEntries;

        //カンマ分けの準備(区分けする文字を設定する)
        char[] spliter = new char[1] { ',' };

        //リターン値。カンマ分けしたデータを一行分格納する。
        string[] CommaSeparationData = new string[CSVDATA_ELEMENTS];

        for (int i = 0; i < CSVDATA_ELEMENTS; i++)
        {
            //１行にある３つの要素数分準備する
            string[] readStrData = new string[CSVDATA_ELEMENTS];
            //３つの要素をカンマで区切って1つずつ格納
            readStrData = lines_.Split(spliter, option);
            //readStrDataをリターン値に格納
            CommaSeparationData[i] = readStrData[i];
        }

        return CommaSeparationData;
    }
}
