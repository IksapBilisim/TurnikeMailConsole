using System.Data.OleDb;
using System.Data;

namespace TurnikeMailConsole;

internal class Program
{
    static void Main(string[] args)
    {
        try
        {

            ReadMdb readMdb = new ReadMdb();
            readMdb.readMdb();
        }
        catch(Exception ex)
        {
            File.WriteAllText("log.txt", ex.Message.ToString());
            throw new Exception(ex.Message);
        }
       
    }

   

}