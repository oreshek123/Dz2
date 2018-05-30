using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace XML
{
    [Serializable]
    public class HabrNews
    {
        public string Title { get; set; }
        public string Link { get; set; }
        public string Description { get; set; }
        public DateTime PubDate;
    }
    class Program
    {
        static void Main(string[] args)
        {
            List<HabrNews> habrNews = new List<HabrNews>();
            XmlDocument doc = new XmlDocument();
            doc.Load("https://habrahabr.ru/rss/interesting/");
            //doc.Save(Console.Out);


            var rootElement = doc.DocumentElement;
            foreach (XmlNode item in rootElement.ChildNodes)
            {
                Console.WriteLine(item.Name);
                foreach (XmlElement xmlElement in item.ChildNodes)
                {
                    Console.WriteLine(xmlElement.Name);
                    if (xmlElement.Name == "item")
                    {
                        HabrNews hnews = new HabrNews();
                        foreach (XmlNode it in xmlElement.ChildNodes)
                        {
                            if (it.Name == "title")
                            {
                                hnews.Title = it.InnerText;
                            }
                            else if (it.Name == "link")
                            {
                                hnews.Link = it.InnerText;
                            }
                            else if (it.Name == "description")
                            {
                                hnews.Description = it.InnerText;
                            }
                            else if (it.Name == "pubDate")
                            {
                                DateTime.TryParse(it.InnerText, out hnews.PubDate);
                            }
                            habrNews.Add(hnews);
                            Console.WriteLine("-->" + it.Name);
                            Console.WriteLine("-->" + it.InnerText);
                        }
                    }
                }
            }

            foreach (HabrNews item in habrNews)
            {
                Console.WriteLine(item.Title);
                Console.WriteLine(item.Link);
            }

            XmlSerialize(habrNews);

            Console.ReadLine();
        }

        public static void XmlSerialize(List<HabrNews> hnews)
        {
            XmlSerializer formatter = new XmlSerializer(typeof(List<HabrNews>));

            using (FileStream fs = new FileStream("HabrNews.xml", FileMode.OpenOrCreate))
            {
                formatter.Serialize(fs, hnews);
            }
        }
    }

}

