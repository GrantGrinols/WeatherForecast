using Microsoft.AspNetCore.Mvc;

public class CardModel
{
 



    public string Date { get; set; }
    public string Description { get; set; }
    public string TempMax { get; set; }
    public string TempMin { get; set; }
    public string Humidity { get; set; }

        public CardModel(string Date, string Description, string TempMax, string TempMin, string Humidity)
    {
        this.Date = Date;
        this.Description = Description;
        this.TempMax = TempMax;
        this.TempMin = TempMin;
        this.Humidity = Humidity;
}
}