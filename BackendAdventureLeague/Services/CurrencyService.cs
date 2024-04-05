using System.Net;
using System.Xml;
using BackendAdventureLeague.Models;

namespace BackendAdventureLeague.Services;

public class CurrencyService : ICurrencyService
{
    public static decimal RoubleToDyrhamCourse { get; set; }
    
    public static decimal RoubleToYuanCourse { get; set; }

    public static decimal RoubleToDollar { get; set; }

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
                case "USD":
                    RoubleToDollar = val;
                    break;
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

    public void TransferMoney(Account from, Account to, decimal sum)
    {
        decimal toSum = 0;
        decimal toMinus = 0;

        switch (from.CurrencyType)
        {
            case(CurrencyTypes.Ruble):
                switch (to.CurrencyType)
                {
                    case CurrencyTypes.Ruble:
                        toSum = sum;
                        toMinus = sum;
                        break;
                    case CurrencyTypes.Dirham:
                        toSum = sum;
                        toMinus = sum * RoubleToDyrhamCourse;
                        break;
                    case CurrencyTypes.Yuan:
                        toSum = sum;
                        toMinus = sum * RoubleToYuanCourse;
                        break;
                }
                break;
            case CurrencyTypes.Yuan:
                switch (to.CurrencyType)
                {
                    case CurrencyTypes.Yuan:
                        toSum = sum;
                        toMinus = sum;
                        break;
                    case CurrencyTypes.Dirham:
                        toSum = sum;
                        toMinus = sum * YuanToDyrhamCourse;
                        break;
                    case CurrencyTypes.Ruble:
                        toSum = sum;
                        toMinus = sum * YuanToRoubleCourse;
                        break;
                }
                break;
            case CurrencyTypes.Dirham:
                switch (to.CurrencyType)
                {
                    case CurrencyTypes.Dirham:
                        toSum = sum;
                        toMinus = sum;
                        break;
                    case CurrencyTypes.Ruble:
                        toSum = sum;
                        toMinus = sum * DyrhamToRoubleCourse;
                        break;
                    case CurrencyTypes.Yuan:
                        toSum = sum;
                        toMinus = sum * DyrhamToYuanCourse;
                        break;
                }
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        if (from.Sum < sum)
        {
            return;
        }

        if (from.Sum - Math.Round(toMinus, 2) <= 0)
        {
            return;
        }

        to.Sum += Math.Round(toSum, 2);
        from.Sum -= Math.Round(toMinus, 2);
    }
}