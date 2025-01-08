using System.Text.Json;
using System.IO;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using System.Linq.Expressions;

namespace weatherapp555.Pages;

public class IndexModel : PageModel
{
    public List<CardModel> Cards { get; set; } = new List<CardModel>();

    [BindProperty]
    public required string Location {get; set;}
    public required string ResponseMessage {get; set;}
    private string XCoords = "";
    private string YCoords = "";
    public required string APIkey;


    

    public async Task OnGet()
    {

        try{
            using (StreamReader r = new StreamReader("urlcode.json")){
                string json = r.ReadToEnd();

                dynamic key = JsonConvert.DeserializeObject(json);
                
                APIkey=key["urlcode"];
                
            
            }
            
        }
        catch(Exception ex){
            Console.WriteLine($"Something went wrong: {ex.Message}");
        }
        Location = "New Baltimore";
        await GetWeather(Location);




    }
    public async Task OnPost(){

        if(!string.IsNullOrEmpty(Location)){
        await GetWeather(Location);
            
        }
    }

    private async Task GetWeather(string location){
        CoordinateFinder();
        var client = new HttpClient();

        var request = new HttpRequestMessage(HttpMethod.Get, "https://weather.visualcrossing.com/VisualCrossingWebServices/rest/services/timeline/"+XCoords+","+YCoords+"?key="+APIkey);

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
}
