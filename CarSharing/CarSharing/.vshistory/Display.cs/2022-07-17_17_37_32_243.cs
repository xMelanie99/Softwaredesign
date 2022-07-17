using System.Text.RegularExpressions;
// Singleton
// Darstellung der Oberfläche
public class Display
{
    // Instance Variable -> Weil dies eim Singletonpattern ist | Neues Objekt damit eine Instanz erstellt wird
    private static Display instance = new Display();

    public static Display GetInstance()
    {
        return instance;
    }

    private Display()
    {

    }
    // session = zum überprüfen der Logins | null = kein Login vorhanden
    private DataLogin session = null;
    // carFilter = zum erstellen des Filters
    private DataCarFilter carFilter = null;

    private string adminUsername = "admin";
    private string adminPassword = "1234";

    private string appName =
            "   ___           __ _                _             \n" +
            "  / __\\__ _ _ __/ _\\ |__   __ _ _ __(_)_ __   __ _ \n" +
            " / /  / _` | '__\\ \\| '_ \\ / _` | '__| | '_ \\ / _` |\n" +
            "/ /__| (_| | |  _\\ \\ | | | (_| | |  | | | | | (_| |\n" +
            "\\____/\\__,_|_|  \\__/_| |_|\\__,_|_|  |_|_| |_|\\__, |\n" +
            "                                             |____/ \n";
    // Zeig die Startseite an
    public void ShowStartScreen()
    {
        // Lädt die Daten (Login und Autos)
        FileLogin.GetInstance().LoadLoginData();
        FileCar.GetInstance().LoadCarList();

        Console.Clear();
        // Enthält Titel und Login Status
        ShowHeader();
        // Kein Login -> Login und Register werden angezeigt
        if (session == null)
        {
            Console.WriteLine("[l] Login");
            Console.WriteLine("[r] Register");
        }
        Console.WriteLine("[s] Search for car");
        // Falls admin eingeloggt ist: kann neues Auto hinzugefügt werden
        if (session != null && session.Username == adminUsername)
        {
            Console.WriteLine("[a] Add car");
        }
        // Falls nur user eingeloggt ist: Standart
        if (session != null)
        {
            Console.WriteLine("[logout] Logout");
        }
        Console.WriteLine(" ");
        Console.WriteLine("[e] exit");
        // Info für User 
        ChooseAction();
        // Liest den input des users und speichert diesen in userInput
        string userInput = GetConsoleInput();

        // .ToLower = Wandelt Großbuchstaben in Kleinbuchstaben um
        // Beispiel
        // "A".ToLower() == "a"
        // "AAaBBb".ToLower() == "aaabbb"
        if (userInput.ToLower() == "l" && session == null)
        {
            ShowLogin();
        }
        else if (userInput.ToLower() == "r" && session == null)
        {
            ShowRegisterUser();
        }
        else if (userInput.ToLower() == "s")
        {
            ShowSearchCar();
        }
        else if (userInput.ToLower() == "a")
        {
            ShowAddCar();
        }
        else if (userInput.ToLower() == "logout")
        {
            session = null;
            ShowStartScreen();
        }
        else if (userInput.ToLower() == "e")
        {
            // nichts weil exit
        }
        else
        {
            ShowStartScreen();
        }
    }
    // Darstellung für die Filterung der Autos
    public void ShowFilterCars()
    {   // --
        if (carFilter == null)
        {
            carFilter = new DataCarFilter();
        }
        // --
        bool isFinished = false;
        string userInput = "";
        //--
        while (!isFinished)
        {
            Console.Clear();
            // Titel und Login Status
            ShowHeader();

            //-- (?) Berechnet aus der Methode GetTimeFromMinutes die korrekte Zeit und stellt sie dann in der Konsole da
            Console.WriteLine("[time] Time: " + GetTimeFromMinutes(carFilter.StartTime) + " - " + GetTimeFromMinutes(carFilter.EndTime));
            Console.WriteLine("[dur] Duration: " + carFilter.Duration + "minutes");
            Console.WriteLine("[avail] Show only available cars (yes/no): " + carFilter.Available);
            Console.WriteLine("[engine] Engine type: " + carFilter.Engine);
            Console.WriteLine("[price] Total price: " + carFilter.TotalPrice + " \u20AC");

            Console.WriteLine(" ");
            Console.WriteLine("[r] Reset");
            Console.WriteLine("[b] Back");

            ChooseAction();
            userInput = GetConsoleInput();

            if (userInput.ToLower() == "time")
            {
                // Falls ein Eintrag in time erstellt wird, wird nach start und ende abgefragt (Angabe)
                Console.Write("Enter start time (in minutes): ");
                string input_startTime = GetConsoleInput();
                Console.WriteLine("");
                Console.Write("Enter end time (in minutes): ");
                string input_endTime = GetConsoleInput();
                // --
                carFilter.StartTime = int.Parse(input_startTime);
                carFilter.EndTime = int.Parse(input_endTime);
            }
            else if (userInput.ToLower() == "dur")
            {
                Console.Write("Enter duration (in minutes): ");
                string inp = GetConsoleInput();
                carFilter.Duration = int.Parse(inp);
            }
            else if (userInput.ToLower() == "avail")
            {
                Console.Write("Enter availability (yes/no): ");
                string inp = GetConsoleInput();
                carFilter.Available = inp;
            }
            else if (userInput.ToLower() == "engine")
            {
                Console.Write("Enter engine type: ");
                string inp = GetConsoleInput();
                carFilter.Engine = inp;
            }
            else if (userInput.ToLower() == "price")
            {
                Console.Write("Enter maximum total price: ");
                string inp = GetConsoleInput();
                carFilter.TotalPrice = float.Parse(inp);
            }
            else if (userInput.ToLower() == "r")
            {
                carFilter = null;
                isFinished = true;
            }
            else if (userInput.ToLower() == "b")
            {
                isFinished = true;
            }
            else
            {
                isFinished = true;
            }
        }

        if (userInput.ToLower() == "b")
        {
            ShowSearchCar();
        }
        else
        {
            ShowFilterCars();
        }
    }

    public void ShowSearchCar(int currentPage = 1)
    {
        Console.Clear();

        ShowHeader();

        List<DataCar> cars = FileCar.GetInstance().GetCarList(carFilter);
        // Count = Gibt die Anzahl der Elemente in der Liste zurück || Ceiling = aufrunden (gibt einen double Wert zurück --> muss wieder zurückgewandelt werden in int) || /10 = Weil 10 Einträge pro Seite
        int maxPage = (int)Math.Ceiling(cars.Count / 10.0);
        Console.WriteLine("Page " + currentPage + " of " + maxPage);
        Console.WriteLine(" ");
        Console.WriteLine("[1]  " + GetCarDetails(GetCarAtIndex(cars, (currentPage - 1) * 10 + 0)));
        Console.WriteLine("[2]  " + GetCarDetails(GetCarAtIndex(cars, (currentPage - 1) * 10 + 1)));
        Console.WriteLine("[3]  " + GetCarDetails(GetCarAtIndex(cars, (currentPage - 1) * 10 + 2)));
        Console.WriteLine("[4]  " + GetCarDetails(GetCarAtIndex(cars, (currentPage - 1) * 10 + 3)));
        Console.WriteLine("[5]  " + GetCarDetails(GetCarAtIndex(cars, (currentPage - 1) * 10 + 4)));
        Console.WriteLine("[6]  " + GetCarDetails(GetCarAtIndex(cars, (currentPage - 1) * 10 + 5)));
        Console.WriteLine("[7]  " + GetCarDetails(GetCarAtIndex(cars, (currentPage - 1) * 10 + 6)));
        Console.WriteLine("[8]  " + GetCarDetails(GetCarAtIndex(cars, (currentPage - 1) * 10 + 7)));
        Console.WriteLine("[9]  " + GetCarDetails(GetCarAtIndex(cars, (currentPage - 1) * 10 + 8)));
        Console.WriteLine("[10] " + GetCarDetails(GetCarAtIndex(cars, (currentPage - 1) * 10 + 9)));

        Console.WriteLine(" ");
        Console.WriteLine("[n] Next page");
        Console.WriteLine("[p] Previous page");

        Console.WriteLine(" ");
        Console.WriteLine("[f] Filter");
        Console.WriteLine("[b] Back");

        ChooseAction();
        string userInput = GetConsoleInput();

        if (userInput.ToLower() == "1")
        {
            ShowCarDetails(GetCarAtIndex(cars, (currentPage - 1) * 10 + 0));
        }
        else if (userInput.ToLower() == "2")
        {
            ShowCarDetails(GetCarAtIndex(cars, (currentPage - 1) * 10 + 1));
        }
        else if (userInput.ToLower() == "3")
        {
            ShowCarDetails(GetCarAtIndex(cars, (currentPage - 1) * 10 + 2));
        }
        else if (userInput.ToLower() == "4")
        {
            ShowCarDetails(GetCarAtIndex(cars, (currentPage - 1) * 10 + 3));
        }
        else if (userInput.ToLower() == "5")
        {
            ShowCarDetails(GetCarAtIndex(cars, (currentPage - 1) * 10 + 4));
        }
        else if (userInput.ToLower() == "6")
        {
            ShowCarDetails(GetCarAtIndex(cars, (currentPage - 1) * 10 + 5));
        }
        else if (userInput.ToLower() == "7")
        {
            ShowCarDetails(GetCarAtIndex(cars, (currentPage - 1) * 10 + 6));
        }
        else if (userInput.ToLower() == "8")
        {
            ShowCarDetails(GetCarAtIndex(cars, (currentPage - 1) * 10 + 7));
        }
        else if (userInput.ToLower() == "9")
        {
            ShowCarDetails(GetCarAtIndex(cars, (currentPage - 1) * 10 + 8));
        }
        else if (userInput.ToLower() == "10")
        {
            ShowCarDetails(GetCarAtIndex(cars, (currentPage - 1) * 10 + 9));
        }
        else if (userInput.ToLower() == "n")
        {
            currentPage += 1;
            if (currentPage > maxPage)
            {
                currentPage = maxPage;
            }

            ShowSearchCar(currentPage);
        }
        else if (userInput.ToLower() == "p")
        {
            currentPage -= 1;
            if (currentPage < 1)
            {
                currentPage = 1;
            }

            ShowSearchCar(currentPage);
        }
        else if (userInput.ToLower() == "f")
        {
            ShowFilterCars();
        }
        else if (userInput.ToLower() == "b")
        {
            ShowStartScreen();
        }
        else
        {
            ShowSearchCar();
        }
    }

    public void ShowCarDetails(DataCar car)
    {
        Console.Clear();

        ShowHeader();

        Console.WriteLine("manufacturer: " + car.Manufacturer);
        Console.WriteLine("model: " + car.Model);
        Console.WriteLine("color: " + car.Color);
        Console.WriteLine("type: " + car.Type);
        Console.WriteLine("engine: " + car.Engine);
        Console.WriteLine("earliest usage time: " + car.EUT);
        Console.WriteLine("latest usage time: " + car.LUT);
        Console.WriteLine("minimun usage duration: " + car.MUD);
        Console.WriteLine("Flat rate: " + car.Flatrate);
        Console.WriteLine("Price per minute: " + car.PPM);
        Console.WriteLine("seats: " + car.Seats);
        Console.WriteLine("trunk space: " + car.Trunk);
        Console.WriteLine("hp: " + car.HP);

        Console.WriteLine(" ");
        Console.WriteLine("[book] Book");
        Console.WriteLine("[b] Back");

        ChooseAction();

        string userInput = GetConsoleInput();

        if (userInput.ToLower() == "book")
        {
            Console.WriteLine("You booked this car.");
            Console.WriteLine("Enter any input to continue");
            string input = GetConsoleInput();
            ShowSearchCar();
        }
        else if (userInput.ToLower() == "b")
        {
            ShowSearchCar();
        }
        else
        {
            ShowCarDetails(car);
        }

    }

    public void ShowRegisterUser()
    {
        Console.Clear();

        ShowHeader();

        string username = "";
        bool IsUserNameAcceptable(string givenUsername)
        {
            //überprüft nach alphanumerisch und muss min. eins davon erfüllen. [] - auswahl was in der Klammer ist. + = was links davon steht soll min. einmal vorkommen
            string regularExpression = @"^[a-zA-Z0-9]+$"; // a
                                                          //string regularExpression = @"[a-z]+[A-Z]+[0-9]+"; // aA1

            //prüft mit der regular expression ob der input alphanumerisch ist
            bool isAlphanumerical = Regex.IsMatch(givenUsername, regularExpression);

            bool usernameExists = FileLogin.GetInstance().DoesUsernameExist(givenUsername);

            if (isAlphanumerical == true && usernameExists == false)
            {
                username = givenUsername;
                return true;
            }
            else
            {
                Console.WriteLine("Error: Your username " + givenUsername + " is NOT alphanumerical or does already exist");
                return false;
            }
        }
        bool isUsernameAcceptable = false;
        while (!isUsernameAcceptable)
        {
            Console.WriteLine("enter username: ");
            string userInputUN = GetConsoleInput();
            isUsernameAcceptable = IsUserNameAcceptable(userInputUN);            
        }


        Console.WriteLine("enter password: ");
        string password = GetConsoleInput();

        FileLogin.GetInstance().SaveLogin(username, password);
        Console.WriteLine("User successfully created.");


        Console.WriteLine("Enter anything to continue!");
        string userAction = GetConsoleInput();
        ShowStartScreen();
    }

    public void ShowLogin()
    {
        Console.Clear();

        ShowHeader();

        Console.WriteLine("Type in username: ");
        string usernameInput = GetConsoleInput();
        Console.WriteLine("Type in password: ");
        string passwordInput = GetConsoleInput();

        bool loginValid = FileLogin.GetInstance().DoesLoginExist(usernameInput, passwordInput);
        bool isAdminLogin = usernameInput == adminUsername && passwordInput == adminPassword;

        if (loginValid == true || isAdminLogin == true)
        {
            session = new DataLogin();

            session.Username = usernameInput;
            session.Password = passwordInput;

            Console.WriteLine("Successfully logged in!");
        }
        else
        {
            Console.WriteLine("Wrong username or password");
        }

        Console.WriteLine("Enter anything to continue!");
        string userAction = GetConsoleInput();
        ShowStartScreen();

    }

    public void ShowAddCar()
    {
        // Neues Transferobjekt
        DataCar car = new DataCar();
        bool isFinished = false;
        while (!isFinished)
        {
            Console.Clear();

            ShowHeader();

            Console.WriteLine("[man] manufacturer: " + car.Manufacturer);
            Console.WriteLine("[model] model: " + car.Model);
            Console.WriteLine("[color] color: " + car.Color);
            Console.WriteLine("[type] type: " + car.Type);
            Console.WriteLine("[engine] engine: " + car.Engine);

            Console.WriteLine("[eut] earliest usage time: " + car.EUT);
            Console.WriteLine("[lut] latest usage time: " + car.LUT);
            Console.WriteLine("[mud] minimum usage duration: " + car.MUD);
            Console.WriteLine("[flat] flatrate: " + car.Flatrate);
            Console.WriteLine("[ppm] price per minute: " + car.PPM);
            Console.WriteLine("[seats] seats: " + car.Seats);
            Console.WriteLine("[trunk] trunk (volume): " + car.Trunk);
            Console.WriteLine("[hp] hp: " + car.HP);

            Console.WriteLine(" ");
            Console.WriteLine("[save] Save");
            Console.WriteLine("[b] Back");

            ChooseAction();

            string userInput = GetConsoleInput();

            if (userInput.ToLower() == "man")
            {
                Console.Write("Enter manufacturer: ");
                string input = GetConsoleInput();
                car.Manufacturer = input;
            }
            else if (userInput.ToLower() == "model")
            {
                Console.Write("Enter model: ");
                string input = GetConsoleInput();
                car.Model = input;
            }
            else if (userInput.ToLower() == "color")
            {
                Console.Write("Enter color: ");
                string input = GetConsoleInput();
                car.Color = input;
            }
            else if (userInput.ToLower() == "type")
            {
                Console.Write("Enter vehicle type: ");
                string input = GetConsoleInput();
                car.Type = input;
            }
            else if (userInput.ToLower() == "engine")
            {
                Console.Write("Enter engine type: ");
                string input = GetConsoleInput();
                car.Engine = input;
            }
            else if (userInput.ToLower() == "eut")
            {
                Console.Write("Enter earliest usage time: ");
                string input = GetConsoleInput();
                car.EUT = int.Parse(input);
            }
            else if (userInput.ToLower() == "lut")
            {
                Console.Write("Enter latest usage time: ");
                string input = GetConsoleInput();
                car.LUT = int.Parse(input);
            }
            else if (userInput.ToLower() == "mud")
            {
                Console.Write("Enter minimum usage duration: ");
                string input = GetConsoleInput();
                car.MUD = int.Parse(input);
            }
            else if (userInput.ToLower() == "flat")
            {
                Console.Write("Enter flatrate: ");
                string input = GetConsoleInput();
                car.Flatrate = float.Parse(input);
            }
            else if (userInput.ToLower() == "ppm")
            {
                Console.Write("Enter price per minute: ");
                string input = GetConsoleInput();
                car.PPM = float.Parse(input);
            }
            else if (userInput.ToLower() == "seats")
            {
                Console.Write("Enter seat count: ");
                string input = GetConsoleInput();
                car.Seats = int.Parse(input);
            }
            else if (userInput.ToLower() == "trunk")
            {
                Console.Write("Enter trunk volume: ");
                string input = GetConsoleInput();
                car.Trunk = int.Parse(input);
            }
            else if (userInput.ToLower() == "hp")
            {
                Console.Write("Enter horsepower: ");
                string input = GetConsoleInput();
                car.HP = int.Parse(input);
            }
            else if (userInput.ToLower() == "save")
            {
                FileCar.GetInstance().SaveCar(car);
                isFinished = true;
            }
            else if (userInput.ToLower() == "b")
            {
                isFinished = true;
            }
        }
        ShowStartScreen();
    }



    private void ChooseAction()
    {
        Console.WriteLine(" ");
        Console.WriteLine(" ");
        Console.WriteLine(" ");
        Console.WriteLine("Choose action: ");

    }

    private void ShowHeader()
    {
        Console.WriteLine(appName);
        Console.WriteLine(" ");

        if (session != null)
        {
            if (session.Username == adminUsername)
            {
                Console.WriteLine("Logged in as admin");
            }
            else
            {
                Console.WriteLine("Logged in as: " + session.Username);
            }
        }
        else
        {
            Console.WriteLine("You are not logged in.");
        }

        Console.WriteLine(" ");
        Console.WriteLine(" ");
    }




    private string GetConsoleInput()
    {
        // https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/operators/null-coalescing-operator
        // ?? = wenn links null wird, wird der rechte Wert benutzt anonsten der linke Wert
        /* Beipsiel - a (null) steht links vom operator (??) daher wird -1 ausgegeben
        int? a = null;
        int b = a ?? -1;
        Console.WriteLine(b);  // output: -1 */

        return Console.ReadLine() ?? "";
    }

    private string GetCarDetails(DataCar car)
    {
        if (car == null)
        {
            return "---";
        }

        string carDetails = "";
        carDetails += "{";
        carDetails += car.Manufacturer + " " + car.Model + ", ";
        carDetails += GetTimeFromMinutes(car.EUT) + " - " + GetTimeFromMinutes(car.LUT) + ", ";
        carDetails += car.MUD + " minutes, ";
        carDetails += car.Flatrate + " \u20AC (flatrate), ";
        carDetails += car.Engine + ", ";
        carDetails += car.Type;
        carDetails += "}";

        return carDetails;
    }

    private string GetTimeFromMinutes(int min)
    {
        // min = 90
        // hours = 1
        // minutes = min - hours * 60
        // minutes = min % 60 = 90 % 60 = 30 (modulo)

        // p= 5
        // p%5 = 0

        int hours = min / 60;
        int minutes = min % 60;
        string time = hours + ":" + minutes;
        return time;
    }

    // index = die Stelle in dem Car Array
    private DataCar GetCarAtIndex(List<DataCar> cars, int index)
    {
        if (index < 0 || index >= cars.Count)
        {
            return null;
        }
        else
        {
            return cars[index];
        }
    }


}