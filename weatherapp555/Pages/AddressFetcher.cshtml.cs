public class AddressFetcher
{
    private readonly IMapLocationService _locationService;

    public AddressFetcher(IMapLocationService locationService)
    {
        _locationService = locationService;
    }

    public async Task<double[]> FetchAddressAsync(string address)
    {
        try
        {
            var latlong = await  _locationService.GetLatLongFromAddressAsync(address);

            return [latlong.Latitude, latlong.Longitude];
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return [0,0];
        }
    }
}