
public class Program
{

    public static void Main(string[] args)
    {
        // Für das Euro Zeichen - Unicode (Zeichenformat)
        Console.OutputEncoding = System.Text.Encoding.Unicode;

        Display.GetInstance().ShowStartScreen();
    }


}