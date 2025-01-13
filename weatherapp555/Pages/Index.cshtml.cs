
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;


namespace weatherapp555.Pages;

public class IndexModel : PageModel
{
    public List<CardModel> Cards { get; set; } = new List<CardModel>();

    [BindProperty]
    public required string Location {get; set;}
    public required string ResponseMessage {get; set;}
    [BindProperty]
    public bool hasAddress {get; set;}
    [BindProperty]
    public required string userAddress {get; set;}
    private string XCoords = "";
    private string YCoords = "";
    public required string APIkey;
    public required string GoogleKey;
    

    

    public async Task OnGet()
    {
        StartUp();
        Location = "New Baltimore";
        await GetWeather();





    }
    public async Task OnPost(){
        StartUp();
        if(!hasAddress){
        if(!string.IsNullOrEmpty(Location)){
        await GetWeather();
            
        }
        }else{
            Location = userAddress;
            await CoordsGivenAddressFinder(userAddress);
            await GetWeather();
        }
    }
    private void StartUp(){
         try{
            using (StreamReader r = new StreamReader("urlcode.json")){
                string json = r.ReadToEnd();

#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
                dynamic key = JsonConvert.DeserializeObject(json);


#pragma warning disable CS8602 // Dereference of a possibly null reference.
                APIkey =key["urlcode"];
                GoogleKey = key["googleapi"];
#pragma warning restore CS8602 // Dereference of a possibly null reference.
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.                

            }
            
        }
        catch(Exception ex){
            Console.WriteLine($"Something went wrong: {ex.Message}");
        }   
    }
    private async Task GetWeather(){
        
        CoordinateFinder();
        var client = new HttpClient();
        Console.WriteLine($"This is a test for {XCoords} and {YCoords}");
        if(XCoords.Equals("0")&&YCoords.Equals("0")){
            Location = "Invalid";
            return;
        }

        
        var request = new HttpRequestMessage(HttpMethod.Get, "https://weather.visualcrossing.com/VisualCrossingWebServices/rest/services/timeline/"+XCoords+","+YCoords+"?key="+APIkey);
        Console.WriteLine("https://weather.visualcrossing.com/VisualCrossingWebServices/rest/services/timeline/"+XCoords+","+YCoords+"?key="+APIkey);
        var response = await client.SendAsync(request);
        
        response.EnsureSuccessStatusCode();

        var body = await response.Content.ReadAsStringAsync();


        dynamic weather = JsonConvert.DeserializeObject(body);

        
        foreach (var day in weather.days){
        
        string Date = day.datetime;
        string Description = day.description;
        string TempMin = day.tempmax;
        string TempMax = day.tempmin;
        string Humidity = day.humidity;
        
        CardModel card = new CardModel(Date, Description, TempMin, TempMax, Humidity);

        Cards.Add(card);
        };
    }
    private void CoordinateFinder(){
        if(Location=="New Baltimore"){
            XCoords = "42.6825808";
            YCoords = "-82.7357693";
        }
        if(Location=="Detroit"){
            XCoords="42.3305211";
            YCoords="-83.0454411";
        }
        if(Location=="Royal Oak"){
            XCoords="42.4887329";
            YCoords="-83.1415354";
        }
        if(Location=="Ferndale"){
            XCoords="42.4603147";
            YCoords="-83.1309949";
        }
        if(Location=="Ann Arbor"){
            XCoords="42.2816101";
            YCoords="-83.745644";
        }
        if(Location=="Lansing"){
            XCoords="42.7341099";
            YCoords="-84.5533317";
        }
        if(Location=="Houghton"){
            XCoords="47.1221194";
            YCoords="-88.5660664";
        }
    }
    public async Task CoordsGivenAddressFinder(string address){

        var locationService = new GoogleLocationServiceWrapper(GoogleKey);
        var latLong = new AddressFetcher(locationService);

        var Coordinates = await latLong.FetchAddressAsync(address);

#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
    
        XCoords = Coordinates[0].ToString();
        YCoords = Coordinates[1].ToString();
        
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
    
    } 
}
