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
        private void Form1_Load(object sender, EventArgs e)
        {
            md = new MapData();
            wd = new WeatherData[md.GetNum()];
            for(int i=0; i < wd.Length; i++)
            {
                wd[i] = new WeatherData(md.id[i]);
            }
            CreateChart(0);
            WeatherSearch(0);
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
                        //Console.Write(label1.Text);
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

        //天気情報を表示するためのコード
        private void WeatherSearch(int numr)
        {
            for (int i = 0; i < wd[numr].dataValue.Count; i++)
            {
                string weatherData = wd[numr].weather[0];
                label1.Text = weatherData;
            }

            //天気を判定し、合わせた画像を表示するためのif文
            if (label1.Text.IndexOf("晴") >= 0) 
            {
                sunny.ImageLocation = "sunny.png";
                //Console.WriteLine(label1.Text.IndexOf("晴"));
            }else
            {
                sunny.ImageLocation = "";
            }
            if (label1.Text.IndexOf("曇") >= 0)
            {
                cloudy.ImageLocation = "cloudy.png";
                //Console.WriteLine(label1.Text.IndexOf("曇"));
            }
            else
            {
                cloudy.ImageLocation = "";
            }
            if (label1.Text.IndexOf("雨") >= 0)
            {
                rainy.ImageLocation = "rainy.png";
                //Console.WriteLine(label1.Text.IndexOf("雨"));
            }
            else
            {
                rainy.ImageLocation = "";
            }
        }

        //天気情報を読み上げるためのコード
        private void SpeechWeatherInfo(int selectedID)
        {
            SpeechSynthesizer ss = new SpeechSynthesizer();
            ss.Volume = 100;
            ss.Rate = -2;
            ss.SpeakAsync("本日の" + md.name[selectedID] + "の" + "天気は" + wd[selectedID].weather[0] + "です");
        }
    }
}
