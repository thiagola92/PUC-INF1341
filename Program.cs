using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oracle.DataAccess.Client;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            string databaseInfo = "Data Source = (DESCRIPTION ="
                                  + "(ADDRESS = (PROTOCOL = TCP)(HOST = 127.0.0.1)(PORT = 1521))"
                                  + "(CONNECT_DATA ="
                                  + " (SERVER = DEDICATED)"
                                  + " (SERVICE_NAME = XE)"
                                  + ")"
                                + ");"
                                + "User Id = THIAGOLA92;"
                                + "Password = 098890;";


            OracleConnection oracle = new OracleConnection(databaseInfo);
            oracle.Open();

            oracle.Close();
            Console.WriteLine("Finalizando");
            Console.ReadKey();
        }
    }
}
