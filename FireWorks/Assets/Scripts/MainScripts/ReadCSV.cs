﻿using UnityEngine;
using System.Collections;

using System;
using System.Collections.Generic;
using System.IO;

using UnityEngine.Assertions;

public class ReadCSV
{
    public struct CSVData
    {
        public float shotTiming;     //撃つ時間
        public EnumDefinition.FireｗorksType fireｗorksType; //花火の型
        public int shotPosition;   //撃つ場所   
        public int fireworksColor; //花火の色
        public EnumDefinition.ShotAngle shotAngle;      //撃つ角度
        public float fireworksSize;//花火のサイズ
    }

    enum ElementsName
    {
        SHOT_TIMING = 0,  //撃つタイミング
        FIREWORKS_TYPE,   //花火の型
        SHOT_POSITION,    //撃つ場所
        FIREWORKS_COLOR,  //花火の色
        SHOT_ANGLE,       //撃つ角度
        FIREWORKS_SIZE    //花火のサイズ
    }

    //csvデータの要素数
    const int CSVDATA_ELEMENTS = 6;
    
    //csvから取り出した情報を入れる配列
    private CSVData[] csvData;

    public CSVData[] CsvData
    {
        get { return csvData; }
    }

    //パスの名前
    private string[] pathName =
    {
        "data1.csv", //構成１
        "data2.csv",  //構成２
        "data3.csv",  //構成３
        "data4.csv",  //構成４
    };
    
    /*修正：三澤
    FireWorksCreaterのStart関数が先に動くとまずいので
    Start関数ではなくFireWorksCreaterのStart関数の一番最初で呼び出せるように変更
    */
    public void ReadFile()
    {
        //FireTypeなどのEnumを定義しているクラスの変数
        //var EnumDefiniton = this.GetComponent<EnumDefinition>();

        //CSVデータの格納位置のパス
        //TIPS:なにかしらの形でパスを自由に変えられるようにしておく
        //追記:三澤
        //exeにビルドしたときのApplication.dataPathの参照先は「○○_DATA」フォルダ直下
        //string path = Application.dataPath + "/CSVFiles/data1.csv";
        //string path = Application.dataPath + "/CSVFiles/data2.csv";

        int suffixNumber = UnityEngine.Random.Range(0, pathName.Length);
        string path = Application.dataPath + "/CSVFiles/" + pathName[suffixNumber];

        Debug.Log(pathName[suffixNumber]);
        //CSVデータを読み込んで、行に分割
        string[] lines = ReadCsvData(path);

        //csvデータの初期化
        csvData = new CSVData[lines.Length];

        //カンマ分けされたデータを仮格納する。その初期化
        string[] didCommaSeparationData = new string[lines.Length];

        //CSVデータを区切る文字
        char[] commaSpliter = { ',' };
        //        char[] dotSpliter = { '.' };

        for (int i = 0; i < lines.Length; i++)
        {
            //カンマ分けされたデータを格納
            didCommaSeparationData = DataSeparation(lines[i], commaSpliter, CSVDATA_ELEMENTS);

            //データをcsvDataに格納
            //花火の発射時間を格納
            csvData[i].shotTiming = Convert.ToSingle(didCommaSeparationData[(int)ElementsName.SHOT_TIMING]);

            //花火の型を文字列で仮格納
            string tempFireWorkstype = Convert.ToString(didCommaSeparationData[(int)ElementsName.FIREWORKS_TYPE]);
            //文字列をEnumに変換
            csvData[i].fireｗorksType = FireworksTypeChecker(tempFireWorkstype);

            csvData[i].shotPosition = Convert.ToInt16(didCommaSeparationData[(int)ElementsName.SHOT_POSITION]) - 1;

            //CSVデータは「１」か「２」か「３」だが、このデータが添え字になるため－１している
            csvData[i].fireworksColor = Convert.ToInt16(didCommaSeparationData[(int)ElementsName.FIREWORKS_COLOR]) - 1;
            if (csvData[i].fireworksColor > 2) csvData[i].fireworksColor = 0;

            //文字列を元にEnumに変換して格納
            csvData[i].shotAngle = FireworksAngleChecker(didCommaSeparationData[(int)ElementsName.SHOT_ANGLE]);

            csvData[i].fireworksSize = Convert.ToSingle(didCommaSeparationData[(int)ElementsName.FIREWORKS_SIZE]);
        }
    }

    //第一引数…読み込むCSVデータファイルのパス　
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

    //第一引数…ReadCsvData関数で一行にされたデータ
    //第二引数…渡されたデータを区切る文字
    //第三引数…第一引数のデータの要素数。for文の周回数
    string[] DataSeparation(string lines_, char[] spliter_, int trialNumber_)
    {
        //カンマとカンマの間に何もなかったら格納しないことにする設定
        System.StringSplitOptions option = StringSplitOptions.RemoveEmptyEntries;

        //リターン値。カンマ分けしたデータを一行分格納する。
        string[] CommaSeparationData = new string[trialNumber_];
        for (int i = 0; i < trialNumber_; i++)
        {
            //１行にあるCsvDataの要素数分準備する
            string[] readStrData = new string[trialNumber_];
            //CsvDataを引数の文字で区切って1つずつ格納
            readStrData = lines_.Split(spliter_, option);
            //readStrDataをリターン値に格納
            CommaSeparationData[i] = readStrData[i];
        }

        return CommaSeparationData;
    }

    //第一引数…CSVデータから読み込んだ文字列
    EnumDefinition.FireｗorksType FireworksTypeChecker(string fireWorksName_)
    {
        if (fireWorksName_ == "牡丹")
        {
            return EnumDefinition.FireｗorksType.BOTAN;
        }
        else if (fireWorksName_ == "土星")
        {
            return EnumDefinition.FireｗorksType.DOSEI;
        }
        else if (fireWorksName_ == "花雷")
        {
            return EnumDefinition.FireｗorksType.KARAI;
        }
        else if (fireWorksName_ == "菊")
        {
            return EnumDefinition.FireｗorksType.KIKU;
        }
        else if (fireWorksName_ == "芯入り菊")
        {
            return EnumDefinition.FireｗorksType.SINIRI_KIKU;
        }
        else if (fireWorksName_ == "スターマイン")
        {
            return EnumDefinition.FireｗorksType.STAR_MAIN;
        }
        else if (fireWorksName_ == "小割")
        {
            return EnumDefinition.FireｗorksType.KOWARI;
        }
		else if (fireWorksName_ == "形花火")
		{
			return EnumDefinition.FireｗorksType.SHAPE;
		}
        else
        {
            //どの花火の型にも当てはまらない場合エラーを出す
            Assert.IsTrue(false);
        }

        return EnumDefinition.FireｗorksType.NONE_TYPE;
    }

    //第一引数…CSVデータから読み込んだ文字列
    EnumDefinition.ShotAngle FireworksAngleChecker(string shotAngle_)
    {
        if (shotAngle_ == "R")
        {
            return EnumDefinition.ShotAngle.RIGHT;
        }
        else if (shotAngle_ == "C")
        {
            return EnumDefinition.ShotAngle.CENTER;
        }
        else if (shotAngle_ == "L")
        {
            return EnumDefinition.ShotAngle.LEFT;
        }
        else
        {
            //どの角度にも当てはまらない場合エラーを出す
            Assert.IsTrue(false);
        }

        return EnumDefinition.ShotAngle.NONE_ANGLE;
    }
}