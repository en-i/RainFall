using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Speech.Synthesis;

namespace g1830176Ⅱday9
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        MapData md;
        WeatherData[] wd;
        //RainFallの実体化
        RainFall rf;
        
        private void Form1_Load(object sender, EventArgs e)
        {
            md = new MapData();
            wd = new WeatherData[md.GetNum()];
            //非同期処理とすることで、二つのXMLファイルを同時に読み込む、
            Task task = Task.Run(() =>
            {
                for (int i = 0; i < wd.Length; i++)
                {
                    wd[i] = new WeatherData(md.id[i]);
                }
            });
            
            //rainFallPercentの初期化
            RainFall.rainFallPercent = new Dictionary<string, string>();

            //県の数だけ繰り返す
            for(int i = 0;i<47; i++)
            {
                //rfの初期化、クラスRainFallを47回呼び出す
                rf = new RainFall();
                
                //prefLinqを検索するために、xml2Countを一ずつ増やす
                RainFall.xml2Count = i;
               
                if (RainFall.xml2Count < 46)
                {
                    RainFall.xml2Count += 1;
                }
                
                //県ごとに、降水確率の情報を連想配列として、格納
                RainFall.rainFallPercent.Add(RainFall.prefName[0], RainFall.rainFallChance[0]);              
            }
            CreateChart(0);
            WeatherSearch(0);
            //ボタンがクリックされた際にも定義しているが、ラムダ式なのでもう一度定義する
            Action<int> RainFallSearch = (num) =>
            { 
                string rainfall = md.name[num];
                string percent = RainFall.rainFallPercent[rainfall];
                label2.Text = "降水確率：" + percent + "%";
               
            };
            RainFallSearch(0);
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            for(int i = 0; i < md.GetNum(); i++)
            {
                Rectangle tempLoc = md.loc[i];
                int borderX = tempLoc.X + tempLoc.Width;
                int borderY = tempLoc.Y + tempLoc.Height;

                if(tempLoc.X <= e.X && e.X <= borderX)
                {
                    if (tempLoc.Y <= e.Y && e.Y <= borderY)
                    {
                        CreateChart(i);
                        WeatherSearch(i);
                        //RainFallSearchを定義し、呼び出す
                        //降水確率を表示するための関数
                        Action<int> RainFallSearch = (num) =>
                        {
                            //CSVファイルから、県名を取得する(正しく取得するために、CSVファイルの県名の項目を修正している。また、佐渡は除外する)
                            string rainfall = md.name[num];

                            //Console.WriteLine("[{0}:{1}]", rainfall, RainFall.rainFallPercent[rainfall]);

                            //県名から、降水確率を取得する
                            string percent = RainFall.rainFallPercent[rainfall];

                            //％を付け足す
                            label2.Text =  "降水確率：" + percent + "%";
                           
                        };
                        if(i == 14)
                        {
                            label2.Text = "表示できません";                                                
                        }
                        else
                        {
                            RainFallSearch(i);
                        }
                        SpeechWeatherInfo(i);
                        break;
                    }
                }
            }
        }
        private void CreateChart(int num)
        {
            chart1.Series.Clear();
            chart1.Series.Add("Max");
            chart1.Series.Add("Min");
            chart1.Series["Max"].ChartType = SeriesChartType.Line;
            chart1.Series["Min"].ChartType = SeriesChartType.Line;
            chart1.Series["Max"].MarkerStyle = MarkerStyle.Circle;
            chart1.Series["Min"].MarkerStyle = MarkerStyle.Circle;
            for(int i = 0; i < wd[num].dataValue.Count; i++)
            {
                string tempData = wd[num].dataValue[i].ToString() + "日";
                int temp_max = wd[num].maxValue[i];
                int temp_min = wd[num].minValue[i];
                chart1.Series["Max"].IsValueShownAsLabel = true;
                chart1.Series["Min"].IsValueShownAsLabel = true;
                chart1.Series["Max"].Points.AddXY(tempData, temp_max);
                chart1.Series["Min"].Points.AddXY(tempData, temp_min);
            }
        }

        //天気情報を表示するための関数
        private void WeatherSearch(int num)
        {
            for (int i = 0; i < wd[num].dataValue.Count; i++)
            {
                string weatherData = wd[num].weather[0];
                label1.Text = "今日の天気：" + weatherData;
            }

            //天気を判定し、合わせた画像を表示するためのif文
            if (label1.Text.IndexOf("晴") >= 0) 
            {
                sunny.ImageLocation = "sunny.png";
            }else
            {
                sunny.ImageLocation = "";
            }
            if (label1.Text.IndexOf("曇") >= 0)
            {
                cloudy.ImageLocation = "cloudy.png";
            }
            else
            {
                cloudy.ImageLocation = "";
            }
            if (label1.Text.IndexOf("雨") >= 0)
            {
                rainy.ImageLocation = "rainy.png";
            }
            else
            {
                rainy.ImageLocation = "";
            }
        }       
        //天気情報を読み上げるための関数
        private void SpeechWeatherInfo(int selectedID)
        {
            SpeechSynthesizer ss = new SpeechSynthesizer();
            ss.Volume = 100;
            ss.Rate = -2;
            ss.SpeakAsync("本日の" + md.name[selectedID] + "の" + "天気は" + wd[selectedID].weather[0] + "です");
        }
    }
}
