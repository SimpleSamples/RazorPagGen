using System.Text.Json;

//var page = new RazorPage
//{
//    ProjectDir = @"G:\Sam\Projects",
//    ProjectNs = "sswa",
//    ProjectArea = "MFC",
//    ProjectMdl = "CListCtrl"
//};

const string fileName = "AppSettings.json";
string? jsonString;

//  *  *  *  *  *  *  *  *

jsonString = File.ReadAllText(fileName);
if (jsonString == null)
{
    Console.WriteLine("Read error");
    return;
}
RazorPage? page = JsonSerializer.Deserialize<RazorPage>(jsonString);

Boolean More =true;
while (More)
    ShowPage(page);

return;

jsonString = JsonSerializer.Serialize(page);
if (jsonString == null)
{
    Console.WriteLine("Serialization error");
    return;
}
File.WriteAllText(fileName, jsonString);

//  *  *  *  *  *  *  *  *

void ShowPage(RazorPage? page)
{
    Console.WriteLine($"Directory [D]: {page.ProjectDir}");
    Console.WriteLine($"Namespace [N]: {page.ProjectNs}");
    Console.WriteLine($"     Area [A]: {page.ProjectArea}");
    Console.WriteLine($"      Mdl [M]: {page.ProjectMdl}");
    Console.WriteLine(" Generate [G]");
    Console.WriteLine("Press the Escape (Esc) key to quit: \n");
    ConsoleKeyInfo response = Console.ReadKey();
    switch (response.Key)
    {
        case ConsoleKey.Escape:
            More = false;
            break;
        case ConsoleKey.D:
            Console.WriteLine("\r\nEnter application directory (absolute name, including namespace name): ");
            page.ProjectDir = Console.ReadLine();
            break;
        case ConsoleKey.N:
            Console.WriteLine("\r\nEnter namespace name (application name): ");
            page.ProjectNs = Console.ReadLine();
            break;
        case ConsoleKey.A:
            Console.WriteLine("\r\nEnter area (subdirecory): ");
            page.ProjectArea = Console.ReadLine();
            break;
        case ConsoleKey.M:
            Console.WriteLine("\r\nEnter model (page filename): ");
            page.ProjectMdl = Console.ReadLine();
            break;
        case ConsoleKey.G:
            break;
    }
}

public class RazorPage
{
    public string? ProjectDir { get; set; }
    public string? ProjectNs { get; set; }
    public string? ProjectArea { get; set; }
    public string? ProjectMdl { get; set; }
}
