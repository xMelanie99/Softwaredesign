using System.Collections;
using System.Text.Json;

//Singleton
public class FileLogin
{

    private static FileLogin instance = new FileLogin();

    public static FileLogin GetInstance()
    {
        return instance;
    }

    private FileLogin()
    {

    }

    private List<DataLogin> logins = new List<DataLogin>();

    private string filename = "login_data.json";

    // speichert die Login Daten
    public void SaveLogin(string username, string password)
    {
        //Data_Login tmp = new Data_Login(username, password);
        DataLogin tmp = new DataLogin();
        tmp.Username = username;
        tmp.Password = password;

        logins.Add(tmp);

        // Macht automatische ein JSON (= Datenformat) string aus der Data Login Liste (logins) - Wandelt die Data_Login Liste in ein JSON Format um
        // JsonSerializerOptions options = new JsonSerializerOptions { WriteIndented = true } = sorgt dafür dass der JSON String zeilenweise dargestellt wird (unterainander) 
        //https://docs.microsoft.com/en-us/dotnet/standard/serialization/system-text-json-how-to?pivots=dotnet-6-0
        JsonSerializerOptions options = new JsonSerializerOptions { WriteIndented = true };
        string json = JsonSerializer.Serialize(logins, options);
        // über "WriteAllText" fahren für die Beschreibung
        //Console.WriteLine(json);
        File.WriteAllText(System.IO.Directory.GetCurrentDirectory() + "/" + filename, json);
    }

    //https://docs.microsoft.com/en-us/dotnet/standard/serialization/system-text-json-how-to?pivots=dotnet-6-0 für speichern und laden
    public void LoadLoginData()
    {
        try
        {
            string jsonString = File.ReadAllText(System.IO.Directory.GetCurrentDirectory() + "/" + filename);
            logins = JsonSerializer.Deserialize<List<DataLogin>>(jsonString) ?? new List<DataLogin>();
        }
        catch (Exception e)
        {
        }

        //Console.WriteLine(jsonString);
    }

    // -- Überprüft ob es den Username bereits schon gibt -- 
    public bool DoesUsernameExist(string username)
    {
        foreach (DataLogin login in logins)
        {
            if (login.Username == username)
            {
                return true;
            }
        }
        return false;
        //return logins.Where(x => x.Username == username).Any();
    }


    public bool DoesLoginExist(string username, string password)
    {
        foreach (DataLogin login in logins)
        {
            if ((login.Username == username) && (login.Password == password))
            {
                return true;
            }
        }
        return false;
        //return logins.Where(x => (x.Username == username) && (x.Password == password)).Any();
    }
}