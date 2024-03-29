﻿using System;
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
        public async Task<bool> readMdb()
        {
            try
            {
                Mail mail = new Mail();
                string today = "";
                string lastaweek = "";
                logs = new List<Logs>();
                var myDataTable = new DataTable();
                var startOfWeek = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek + (int)DayOfWeek.Monday);
                var justDate = DateTime.Now.ToString("yyyy-MM-dd");
                //önce bugünü hesapla ------------
                using (var conection = new OleDbConnection("Provider=Microsoft.JET.OLEDB.4.0;" + "data source=C:\\Program Files\\ZKTeco\\att2000.mdb;"))
                {
                    conection.Open();
                   
                    var query = "Select Name,CHECKTIME,SENSORID From CHECKINOUT INNER JOIN USERINFO  on CHECKINOUT.USERID = USERINFO.USERID where Format([CHECKTIME ],\"yyyy-mm-dd\")>='"+justDate+"' order by CHECKTIME";
                    var command = new OleDbCommand(query, conection);
                    var reader = command.ExecuteReader();
                   
                    while (reader.Read())
                    {
                        var ts = reader[2].ToString().Contains("101");
                        if (Convert.ToDateTime(reader[1]) >= Convert.ToDateTime(justDate))
                        {
                            if (ts == true)
                            {
                                logs.Add(new Logs { UserName = reader[0].ToString(), InDate = reader[1].ToString(), SensorId = Convert.ToInt32(reader[2]) });
                            }

                            else
                            {
                                var log = logs.Where(x=>x.UserName == reader[0].ToString() && x.OutDate == null).FirstOrDefault();
                                if (log != null)
                                {
                                    log.OutDate = reader[1].ToString();
                                }

                            }
                        }
                       
                    }
                  
                    logs = logs.Where(x => Convert.ToDateTime(x.OutDate) >= Convert.ToDateTime(justDate)).OrderBy(x=>x.UserName).ToList();
                   
                    foreach (var item in logs)
                    {
                        if(item.OutDate != null)
                            today = today + "<tr><td>" + item.UserName + "</td> <td>" + item.InDate + " </td><td>" + item.OutDate + "  </td></tr>";
                    }
                  

                }
                justDate = DateTime.Now.AddDays(-1).AddDays(-7).ToString("yyyy-MM-dd");
                string beforeDay = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
                using (var conection = new OleDbConnection("Provider=Microsoft.JET.OLEDB.4.0;" + "data source=C:\\Program Files\\ZKTeco\\att2000.mdb;"))
                {
                    conection.Open();

                    var query = "SELECT Name, CHECKTIME, SENSORID FROM CHECKINOUT INNER JOIN USERINFO ON CHECKINOUT.USERID = USERINFO.USERID WHERE CDate(CHECKTIME) >= #" + justDate + "# AND CDate(CHECKTIME) <= #" + beforeDay + "# ORDER BY CHECKTIME";

                    var command = new OleDbCommand(query, conection);
                    var reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        var ts = reader[2].ToString().Contains("101");
                        if (Convert.ToDateTime(reader[1]) >= Convert.ToDateTime(justDate))
                        {
                            if (ts == true)
                            {
                                logs.Add(new Logs { UserName = reader[0].ToString(), InDate = reader[1].ToString(), SensorId = Convert.ToInt32(reader[2]) });
                            }

                            else
                            {
                                var log = logs.Where(x=>x.UserName == reader[0].ToString() && x.OutDate == null).FirstOrDefault();
                                if (log != null)
                                {
                                    log.OutDate = reader[1].ToString();
                                }

                            }
                        }

                    }

                    logs = logs.Where(x => Convert.ToDateTime(x.OutDate) >= Convert.ToDateTime(justDate)).OrderBy(x => x.UserName).ToList();

                    foreach (var item in logs)
                    {
                        if (item.OutDate != null)
                            lastaweek = lastaweek + "<tr><td>" + item.UserName + "</td> <td>" + item.InDate + " </td><td>" + item.OutDate + "  </td></tr>";
                    }


                }
                mail.SendMail(today,lastaweek);
                return true;
            }
            catch (Exception ex)
            {
                File.WriteAllText("log.txt",ex.Message.ToString());
                throw new Exception(ex.Message);
            }

        }

    }

    public class Logs
    {
        public string UserName { get; set; }
        public string InDate { get; set; }
        public string OutDate { get; set; }
        public int SensorId { get; set; }
    }
}
