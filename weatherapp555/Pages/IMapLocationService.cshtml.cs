public interface IMapLocationService {
    Task<CustomMapPoint> GetLatLongFromAddressAsync(string address);
}