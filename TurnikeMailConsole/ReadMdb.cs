using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TurnikeMailConsole
{
    public class ReadMdb
    {
        public List<Logs> logs { get; set; }
        public void readMdb()
        {
            logs = new List<Logs>();
            var myDataTable = new DataTable();
            using (var conection = new OleDbConnection("Provider=Microsoft.JET.OLEDB.4.0;" + "data source=C:\\att2000.mdb;"))
            {
                conection.Open();
                var query = "Select Name,CHECKTIME,SENSORID From CHECKINOUT INNER JOIN USERINFO  on CHECKINOUT.USERID = USERINFO.USERID";
                var command = new OleDbCommand(query, conection);
                var reader = command.ExecuteReader();
                var startOfWeek = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek + (int)DayOfWeek.Monday);
                var justDate = startOfWeek.ToString("dd.MM.yyyy");
                while (reader.Read())
                    logs.Add(new Logs { UserName = reader[0].ToString(), CheckDate = reader[1].ToString() , SensorId = Convert.ToInt32(reader[2]) });

                logs = logs.Where(x => Convert.ToDateTime(x.CheckDate) >= Convert.ToDateTime(justDate)).ToList();

            }
        }
      
    }

    public class Logs
    {
        public string UserName { get; set; }
        public string CheckDate { get; set; }
        public int SensorId { get; set; } 
    }
}
