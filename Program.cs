using System.Text;
using System.Text.Json;
using static System.Environment;

const string AppSettingsFn = "AppSettings.json";
const string CshtmlFn = "cshtml.txt";
const string CshtmlCsFn = "cshtmlcs.txt";
string? jsonString;

//  *  *  *  *  *  *  *  *

// Check for the LocalApplicationData folder and the two files in it

string FolderPath = Environment.GetFolderPath(SpecialFolder.LocalApplicationData);
string appData = Path.Combine(FolderPath, "RazorPagesGen");
DirectoryInfo DirInfo = new DirectoryInfo(appData);
if (!DirInfo.Exists)
{
    Console.WriteLine($"The directory {appData} does not exist");
    return;
}
string? fn;
bool filesexist = true;
fn = appData + '\\' + CshtmlFn;
FileInfo cshtmlfi = new FileInfo(fn);
if (!cshtmlfi.Exists)
{
    Console.WriteLine($"The file {fn} does not exist");
    filesexist = false;
}
fn = appData + '\\' + CshtmlCsFn;
FileInfo cshtmlCsfi = new FileInfo(fn);
if (!cshtmlCsfi.Exists)
{
    Console.WriteLine($"The file {fn} does not exist");
    filesexist = false;
}
if (!filesexist)
    return;

// Read the settings

jsonString = File.ReadAllText(AppSettingsFn);
if (jsonString == null)
{
    Console.WriteLine("Read error");
    return;
}
RazorPage? page = JsonSerializer.Deserialize<RazorPage>(jsonString);

Boolean More = true;
while (More)
    ShowPage(page);

//  *  *  *  *  *  *  *  *

void ShowPage(RazorPage? page)
{
    Console.Clear();
    Console.WriteLine($"Directory [D]: {page.ProjectDir}");
    Console.WriteLine($"Namespace [N]: {page.ProjectNs}");
    Console.WriteLine($"     Area [A]: {page.ProjectArea}");
    Console.WriteLine($"      Mdl [M]: {page.ProjectMdl}");
    Console.WriteLine("     Save [S]");
    string PagesDirInfo = page.ProjectDir + "\\Areas\\" + page.ProjectArea + "\\Pages";
    DirectoryInfo di = new DirectoryInfo(PagesDirInfo);
    bool CanGenerate = false;
    if (!di.Exists)
    {
        Console.WriteLine(PagesDirInfo);
        Console.WriteLine("Doesn't exist yet; modify above parameters some more");
    }
    else
    {
        FileInfo Cshtmlfi = new FileInfo(di.FullName + '\\' + page.ProjectMdl + ".cshtml");
        FileInfo CshtmlCsfi = new FileInfo(di.FullName + '\\' + page.ProjectMdl + ".cshtml.cs");
        if (!Cshtmlfi.Exists && !CshtmlCsfi.Exists)
        {
            Console.WriteLine(" Generate [G]");
            CanGenerate = true;
        }
    }
    Console.WriteLine("Press the Escape (Esc) key to quit\n");

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
            Save();
            if (CanGenerate)
            {
                GenerateTo(page);
            }
            break;
        case ConsoleKey.S:
            Save();
            break;
    }
}

void GenerateTo(RazorPage page)
{
    string text = string.Empty;
    // Read Cshtml
    try
    {
        fn = appData + '\\' + CshtmlFn;
        using (StreamReader sr = new StreamReader(fn))
        {
            text = sr.ReadToEnd();
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Exception reading {fn}: {ex.Message}");
    }
    // Write Cshtml
    try
    {
        fn = page.ProjectDir + "\\Areas\\" + page.ProjectArea + "\\Pages" + '\\' + page.ProjectMdl + ".cshtml";
        using (StreamWriter sw = new StreamWriter(fn))
        {
            Replace(ref text, page);
            sw.Write(text);
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Exception writing {fn}: {ex.Message}");
    }
    // Read Cshtml.cs
    try
    {
        fn = appData + '\\' + CshtmlCsFn;
        using (StreamReader sr = new StreamReader(fn))
        {
            text = sr.ReadToEnd();
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Exception reading {fn}: {ex.Message}");
    }
    // Write Cshtml.cs
    try
    {
        fn = page.ProjectDir + "\\Areas\\" + page.ProjectArea + "\\Pages" + '\\' + page.ProjectMdl + ".cshtml.cs";
        using (StreamWriter sw = new StreamWriter(fn))
        {
            Replace(ref text, page);
            sw.Write(text);
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Exception writing {fn}: {ex.Message}");
    }
}

void Replace(ref string text, RazorPage page)
{
    StringBuilder sb = new StringBuilder(text);
    sb.Replace("|A|", page.ProjectArea);
    sb.Replace("|N|", page.ProjectNs);
    sb.Replace("|M|", page.ProjectMdl);
    text = sb.ToString();
}

void Save()
{
    jsonString = JsonSerializer.Serialize(page);
    if (jsonString == null)
        Console.WriteLine("Serialization error");
    else
        File.WriteAllText(AppSettingsFn, jsonString);
}

public class RazorPage
{
    public string? ProjectDir { get; set; }
    public string? ProjectNs { get; set; }
    public string? ProjectArea { get; set; }
    public string? ProjectMdl { get; set; }
}
