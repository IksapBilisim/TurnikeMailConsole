using System.Data.OleDb;
using System.Data;

namespace TurnikeMailConsole;

internal class Program
{
    static void Main(string[] args)
    {
        ReadMdb readMdb = new ReadMdb();
        readMdb.readMdb();
    }

   

}