using System.Text.Json;
using System.Linq;

//Singleton
public class FileCar
{

    private static FileCar instance = new FileCar();

    public static FileCar GetInstance()
    {
        return instance;
    }

    private FileCar()
    {

    }

    private DataCarList carList;

    private string filename = "car_data.json";

    public void SaveCar(DataCar car)
    {

        InitializeCarList();

        car.ID = carList.IDStart;
        //car.ID = carList.Cars.Max(x => x.ID) + 1;
        carList.IDStart += 1;
        carList.Cars.Add(car);

        // Macht automatische ein JSON (= Datenformat) string aus dem Transferobjekt Data Car List (carList) - Wandelt carList in ein JSON Format um
        // JsonSerializerOptions options = new JsonSerializerOptions { WriteIndented = true } = sorgt dafür dass der JSON String zeilenweise dargestellt wird (unterainander) 
        // https://docs.microsoft.com/en-us/dotnet/standard/serialization/system-text-json-how-to?pivots=dotnet-6-0
        JsonSerializerOptions options = new JsonSerializerOptions { WriteIndented = true };
        // macht aus dem carList eine JSON String und speichert ihn in json ab
        string json = JsonSerializer.Serialize(carList, options);
        // über "WriteAllText" fahren für die Beschreibung
        // Console.WriteLine(json);
        // speichert den json stiring in den Pfad ein
        File.WriteAllText(System.IO.Directory.GetCurrentDirectory() + "/" + filename, json);
    }


    //https://docs.microsoft.com/en-us/dotnet/standard/serialization/system-text-json-how-to?pivots=dotnet-6-0 für speichern und laden
    public void LoadCarList()
    {
        try
        {
            string jsonString = File.ReadAllText(System.IO.Directory.GetCurrentDirectory() + "/" + filename);
            carList = JsonSerializer.Deserialize<DataCarList>(jsonString);
        }
        catch (Exception e)
        {

        }

        InitializeCarList();

    }

    // Gibt die Liste von Autos zurück, wenn ein Filter ausgewählt wird. Falls nicht dann zeigt er die ganzen Filter an
    public List<DataCar> GetCarList(DataCarFilter filter)
    {
        if (filter == null)
        {
            return carList.Cars;
        }

        List<DataCar> filteredCars = carList.Cars;

        if (filter.StartTime != -1 && filter.EndTime != -1)
        {
            filteredCars = filteredCars.Where(car => car.EUT <= filter.StartTime && car.LUT >= filter.EndTime).ToList();
        }
        if (filter.Duration != -1)
        {
            filteredCars = filteredCars.Where(car => car.MUD <= filter.Duration).ToList();
        }
        if (filter.Engine != null)
        {
            filteredCars = filteredCars.Where(car => car.Engine == filter.Engine).ToList();
        }
        if (filter.TotalPrice != -1)
        {
            filteredCars = filteredCars.Where(car => filter.TotalPrice <= car.Flatrate + car.MUD * car.PPM).ToList();
        }

        return filteredCars;
    }

    private void InitializeCarList()
    {
        if (carList == null)
        {
            carList = new DataCarList();
            carList.IDStart = 0;
            carList.Cars = new List<DataCar>();
        }
    }


}