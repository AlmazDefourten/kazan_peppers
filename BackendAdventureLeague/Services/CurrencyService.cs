using System.Net;
using System.Xml;
using BackendAdventureLeague.Models;

namespace BackendAdventureLeague.Services;

public class CurrencyService : ICurrencyService
{
    public static decimal RoubleToDyrhamCourse { get; set; }
    
    public static decimal RoubleToYuanCourse { get; set; }
    
    public static decimal YuanToRoubleCourse { get; set; }
    
    public static decimal YuanToDyrhamCourse { get; set; }
    
    public static decimal DyrhamToRoubleCourse { get; set; }
    
    public static decimal DyrhamToYuanCourse { get; set; }
    
    public decimal GetCurrency(CurrencyTypes curType)
    {
        string url = "https://www.cbr.ru/scripts/XML_daily.asp";
        
        WebClient client = new WebClient();
        string xmlString = client.DownloadString(url);
        
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(xmlString);
        
        XmlNodeList valutes = xmlDoc.SelectNodes("//Valute");
        
        foreach (XmlNode valute in valutes)
        {
            var code = valute.SelectSingleNode("CharCode")?.InnerText;
            var val = decimal.Parse(valute.SelectSingleNode("Value")?.InnerText.Replace(",", ".") 
                                    ?? throw new AggregateException("Не получилось получить курс валюты"));

            switch (code)
            {
                case "AED":
                    RoubleToDyrhamCourse = val;
                    DyrhamToRoubleCourse = 1 / val;
                    if (curType == CurrencyTypes.Dirham)
                        return val;
                    break;
                case "CNY":
                    RoubleToYuanCourse = val;
                    YuanToRoubleCourse = 1 / val;
                    if (curType == CurrencyTypes.Yuan)
                        return val;
                    break;
            }
        }
        YuanToDyrhamCourse = RoubleToDyrhamCourse / RoubleToYuanCourse;
        DyrhamToYuanCourse = 1 / YuanToDyrhamCourse;

        return 0;
    }
}