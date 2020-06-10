using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace g1830176Ⅱday9
{
    public class RainFall
    {
        //県別の天気情報すべてを取得するためのList。LINQで連番を追加。
        List<int> prefLinq = Enumerable.Range(1, 47).ToList();
        
        //県ごとの情報すべてを取得する。Form1で使用、1から47まで入ったprefLinqを検索するための数値
        public static int xml2Count = 0;

        //県名を格納するList
        public static List<string> prefName { get; set; }

        //一時的に降水確率を格納するList(6時間ごと)
        List<string> rain1 { get; set; }
        List<string> rain2 { get; set; }
        List<string> rain3 { get; set; }

        //1日の降水確率の平均を求めるためのList
        List<int> rainPercent { get; set; }

        //降水確率を格納するList(1日の平均)
        public static List<string> rainFallChance { get; set; }

        //prefNameをKey、rainFallChanceをValueとして格納する連想配列
        public static Dictionary<string, string> rainFallPercent = new Dictionary<string, string>();

        public RainFall()
        {
            //Listの初期化
            prefName = new List<string>();
            rainFallChance = new List<string>();
            rain1 = new List<string>();
            rain2 = new List<string>();
            rain3 = new List<string>();
            rainPercent = new List<int>();

            //URLに0を付け足すための文字
            string zero = "0";

            //URLが01であるため、1から9に0を付け足す
            if (xml2Count < 9)
            {
                zero = "0";
            }
            else
            {
                zero = "";
            }

            //降水確率を参照するためのxmlファイル
            var xml2 = XDocument.Load("https://www.drk7.jp/weather/xml/" + zero + prefLinq[xml2Count] + ".xml");

            //県名の取得
            var xmlElementp2 = xml2.Element("weatherforecast").Elements("pref");

            //降水確率の取得(6時間ごと(0時から6時は除く))
            IEnumerable<XElement> hour1 = from xr1 in xml2.Element("weatherforecast").Element("pref").Elements("area").Elements("info").Elements("rainfallchance").Elements("period") where (string)xr1.Attribute("hour") == "06-12" select xr1;
            IEnumerable<XElement> hour2 = from xr2 in xml2.Element("weatherforecast").Element("pref").Elements("area").Elements("info").Elements("rainfallchance").Elements("period") where (string)xr2.Attribute("hour") == "12-18" select xr2;
            IEnumerable<XElement> hour3 = from xr3 in xml2.Element("weatherforecast").Element("pref").Elements("area").Elements("info").Elements("rainfallchance").Elements("period") where (string)xr3.Attribute("hour") == "18-24" select xr3;

            //prefの属性から県名の情報を取得、prefNameに格納(非同期処理)
            Parallel.ForEach(xmlElementp2, xd2 =>
            {
                prefName.Add(xd2.Attribute("id").Value);
                
            });

            //6時から12時までの降水確率の取得
            foreach (XElement xr1 in hour1)
            {
                rain1.Add(xr1.ToString());
            }
            string r1;

            //文字列から、降水確率だけを抜き取る
            r1 = rain1[0].Substring(21, 2);

            //0%の時は1文字だけ抜き取る
            if (r1.IndexOf("<") >= 0)
            {
                r1 = r1.Substring(0, 1);
            }

            //12時から18時までの降水確率の取得
            foreach (XElement xr2 in hour2)
            {               
                rain2.Add(xr2.ToString());
            }
            string r2;
            r2 = rain2[0].Substring(21, 2);
            if (r2.IndexOf("<") >= 0)
            {
                r2 = r2.Substring(0, 1);
            }

            //18時から24時までの降水確率の取得
            foreach (XElement xr3 in hour3)
            {
                rain3.Add(xr3.ToString());
            }
            string r3;
            r3 = rain3[0].Substring(21, 2);
            if (r3.IndexOf("<") >= 0)
            {
                r3 = r3.Substring(0, 1);
            }

            //降水確率をstring型から、int型に変更し、Listに格納する
            rainPercent.Add(int.Parse(r1));
            rainPercent.Add(int.Parse(r2));
            rainPercent.Add(int.Parse(r3));
            
            //rainPercentから、降水確率の平均を求め、10の位で切り上げている(LINQ機能)
            var dayPercent = Math.Ceiling(rainPercent.Average() / 10) * 10;

            //rainFallChanceに降水確率をstring型に置き換え、格納する
            rainFallChance.Add(dayPercent.ToString());
        }
    }
}
