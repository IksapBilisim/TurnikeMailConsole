using System.Data.OleDb;
using System.Data;
using System.Diagnostics;
using System.IO;

namespace TurnikeMailConsole;

internal class Program
{
    static async Task Main(string[] args)
    {
        try
        {
            foreach (var process in Process.GetProcessesByName("Att"))
            {
                process.Kill();
                Console.WriteLine("---OK---");
            }
            ReadMdb readMdb = new ReadMdb();
            await readMdb.readMdb();
            Process.Start("C:\\Program Files\\ZKTeco\\Att.exe");

        }
        catch(Exception ex)
        {
            File.WriteAllText("log.txt", ex.Message.ToString());
            throw new Exception(ex.Message);
        }
       
    }

   

}