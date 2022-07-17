// Transferobjekt
//https://docs.microsoft.com/en-us/dotnet/standard/serialization/system-text-json-how-to?pivots=dotnet-6-0 
public class DataCar
{
    public int ID { get; set; }
    public string Manufacturer { get; set; }
    public string Model { get; set; }
    public string Color { get; set; }
    public string Type { get; set; }
    public string Engine { get; set; }

    public int EUT { get; set; }
    public int LUT { get; set; }
    public int MUD { get; set; }
    public float Flatrate { get; set; }
    public float PPM { get; set; }
    public int Seats { get; set; }
    public int Trunk { get; set; }
    public int HP { get; set; }
}

