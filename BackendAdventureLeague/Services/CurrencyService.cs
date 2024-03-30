using System.Net;
using System.Xml;
using BackendAdventureLeague.Models;

namespace BackendAdventureLeague.Services;

public class CurrencyService : ICurrencyService
{
    public static decimal DyrhamCourse { get; set; }
    
    public static decimal YuanCourse { get; set; }
    
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
                    DyrhamCourse = val;
                    if (curType == CurrencyTypes.Dirham)
                        return val;
                    break;
                case "CNY":
                    YuanCourse = val;
                    if (curType == CurrencyTypes.Yuan)
                        return val;
                    break;
            }
        }

        return 0;
    }
}