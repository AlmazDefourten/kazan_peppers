using System.Net;
using System.Xml;
using Microsoft.ML;
using Microsoft.ML.Data;

namespace BackendAdventureLeague.Endpoints.Prediction;

public interface IPredictionService
{
    void GeneratePredictions();
}

public class PredictionService : IPredictionService
{
    public static string DyrhamPrediction = "";
    public static string YuanPrediction = "";

    public static string PredictionCalculated => DyrhamPrediction + "/" + YuanPrediction;

    public void GeneratePredictions()
    {
        GenerateDyrhamPredictions();
        GenerateYuanPredictions();
    }

    private void GenerateYuanPredictions()
    {
        var now = DateTime.Now.ToString("dd/MM/yyyy");
        string url = $@"http://www.cbr.ru/scripts/XML_dynamic.asp?date_req1=02/01/2023&date_req2={now}&VAL_NM_RQ=R01375";
        
        WebClient client = new WebClient();
        string xmlString = client.DownloadString(url);
        
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(xmlString);

        XmlNodeList valueNodes = xmlDoc.SelectNodes("//Value");
        string[] valuesArray = new string[valueNodes.Count];

        for (int i = 0; i < valueNodes.Count; i++)
        {
            valuesArray[i] = valueNodes[i].InnerText;
        }

        foreach (string value in valuesArray)
        {
            Console.WriteLine(value);
        }

        MLContext mlContext = new MLContext();

        List<DataPoint> trainingDataView = new List<DataPoint>();
        for (int i = 0; i < valuesArray.Length; i++)
        {
            trainingDataView.Add(new DataPoint { Value = float.Parse(valuesArray[i].Replace(",", ".")) });
        }

        IDataView trainingData = mlContext.Data.LoadFromEnumerable(trainingDataView);

        var pipeline = mlContext.Transforms.Concatenate("Features", new[] { "Value" })
            .Append(mlContext.Regression.Trainers.Sdca(labelColumnName: "Value", maximumNumberOfIterations: 100));

        var model = pipeline.Fit(trainingData);

        var predictions = model.Transform(trainingData);
        var predictedValues = mlContext.Data.CreateEnumerable<Prediction>(predictions, reuseRowObject: false).ToList();
        var bestOutcome = Math.Round(predictedValues.Max(p => p.PredictedValue), 2);
        var worstOutcome = Math.Round(predictedValues.Min(p => p.PredictedValue), 2);
        var averageOutcome = Math.Round(predictedValues.Average(p => p.PredictedValue), 2);
        YuanPrediction = bestOutcome.ToString() + " " + averageOutcome.ToString() + " " + worstOutcome.ToString();
        Console.WriteLine($"best: {bestOutcome}, avg: {averageOutcome}, worst: {worstOutcome}");
    }

    private void GenerateDyrhamPredictions()
    {
        var now = DateTime.Now.ToString("dd/MM/yyyy");
        string url = $@"http://www.cbr.ru/scripts/XML_dynamic.asp?date_req1=02/01/2023&date_req2={now}&VAL_NM_RQ=R01230";
        
        WebClient client = new WebClient();
        string xmlString = client.DownloadString(url);
        
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(xmlString);

        XmlNodeList valueNodes = xmlDoc.SelectNodes("//Value");
        string[] valuesArray = new string[valueNodes.Count];

        for (int i = 0; i < valueNodes.Count; i++)
        {
            valuesArray[i] = valueNodes[i].InnerText;
        }

        foreach (string value in valuesArray)
        {
            Console.WriteLine(value);
        }

        MLContext mlContext = new MLContext();

        List<DataPoint> trainingDataView = new List<DataPoint>();
        for (int i = 0; i < valuesArray.Length; i++)
        {
            trainingDataView.Add(new DataPoint { Value = float.Parse(valuesArray[i].Replace(",", ".")) });
        }

        IDataView trainingData = mlContext.Data.LoadFromEnumerable(trainingDataView);

        var pipeline = mlContext.Transforms.Concatenate("Features", new[] { "Value" })
            .Append(mlContext.Regression.Trainers.Sdca(labelColumnName: "Value", maximumNumberOfIterations: 100));

        var model = pipeline.Fit(trainingData);

        var predictions = model.Transform(trainingData);
        var predictedValues = mlContext.Data.CreateEnumerable<Prediction>(predictions, reuseRowObject: false).ToList();
        var bestOutcome = Math.Round(predictedValues.Max(p => p.PredictedValue), 2);
        var worstOutcome = Math.Round(predictedValues.Min(p => p.PredictedValue), 2);
        var averageOutcome = Math.Round(predictedValues.Average(p => p.PredictedValue), 2);
        DyrhamPrediction = bestOutcome.ToString() + " " + averageOutcome.ToString() + " " + worstOutcome.ToString();
        Console.WriteLine($"best: {bestOutcome}, avg: {averageOutcome}, worst: {worstOutcome}");
    }
}

public class DataPoint
{
    [LoadColumn(0)]
    public float Value;
}

public class Prediction
{
    [ColumnName("Score")]
    public float PredictedValue;
}