using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace g1830176Ⅱday9
{
    public class WeatherData
    {
        public List<int> dataValue { get; set; }
        public List<int> maxValue { get; set; }
        public List<int> minValue { get; set; }
        public List<string> weather { get; set; }
       
        public WeatherData(int id)
        {
           
            dataValue = new List<int>();
            maxValue = new List<int>();
            minValue = new List<int>();
            weather = new List<string>();

            var xml = XDocument.Load("http://rss.weather.yahoo.co.jp/rss/days/" + id + ".xml");
           



            var xmlElement = xml.Element("rss").Element("channel").Elements("item");
            var data = new List<string>();
            var description = new List<string>();

            //Console.WriteLine(xmlElement.Count());
            foreach(var x in xmlElement)
            {
                data.Add(x.Element("title").Value);
                description.Add(x.Element("description").Value);
                //Console.WriteLine(description[0]);
               
            }

           

            foreach (string value in data)
            {
                //Console.WriteLine(value);
                int v;
                if(int.TryParse(value.Substring(2, 1), out v))
                {
                    //Console.WriteLine(v);
                    string temp_v = v.ToString();
                    if(int.TryParse(value.Substring(3, 1), out v))
                    temp_v += v.ToString();
                    dataValue.Add(int.Parse(temp_v));
                    //Console.WriteLine(dataValue[0]);
                }
              
            }

            

            foreach (string value in description)
            {
                int numLog = -1;
                for(int i=0; i<value.Length; i++)
                {
                    if(value[i].ToString() =="-" && numLog < 0)
                    {
                        numLog = i;
                        maxValue.Add(int.Parse(getTemperature(value, i+1)));
                    }
                    if (value[i].ToString() == "/")
                    {
                        
                        minValue.Add(int.Parse(getTemperature(value, i)));
                    }
                }
                if (value[value.Length - 1].ToString() == "℃")
                    weather.Add(value.Substring(0, numLog));
               
            }
        }
        private string getTemperature(string data, int num)
        {
            string str = "";
            if(data[num + 1].ToString() == "-")
            {
                str += data[num + 1].ToString();
                str += data[num + 2].ToString();
                if (data[num + 3].ToString() != "℃")
                    str += data[num + 3].ToString();
            }else
            {
                str += data[num + 1].ToString();
                if (data[num + 2].ToString() != "℃")
                    str += data[num + 2].ToString();
            }
            return str;
        }
    }
}
