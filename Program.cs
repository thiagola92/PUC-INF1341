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
            string databaseInfo;

            {
                // Embora no SQLDeveloper apareça xe_thiagola92, meu usuário na verdade era THIAGOLA92.
                // Username é sensível a caracter e não sei o porque o meu foi criado todo em maiusculo.
                // Para descobrir fiz a query "SELECT * FROM ALL_USERS;" e procurei o username que acessou mais recentemente.  
                string userId = "User Id=THIAGOLA92;";
                string password = "Password=098890;";

                // Todas essas informações disponíveis em C:\oraclexe\app\oracle\product\11.2.0\server\network\ADMIN\sample\tnsnames.oRA
                string databaseProtocol = "(PROTOCOL = TCP)";
                string databaseHost = "(HOST = ThiagoLA92-Windows)";
                string databasePort = "(PORT = 1521)";
                string databaseServer = "(SERVER = DEDICATED)";
                string databaseName = "(SERVICE_NAME = XE)";

                databaseInfo = "Data Source = (DESCRIPTION="
                                                 + "(ADDRESS = " + databaseProtocol + databaseHost + databasePort + ")"
                                                 + "(CONNECT_DATA=" + databaseServer + databaseName + ")); "
                                                 + userId + password;
            }
            
            OracleConnection oracle = new OracleConnection(databaseInfo);
            oracle.Open();

            oracle.Close();
            Console.WriteLine("Finalizando");
            Console.ReadKey();
        }
    }
}
