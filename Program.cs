using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OracleClient;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            OracleConnection orcl = new OracleConnection("Data Source = (DESCRIPTION = (ADDRESS_LIST = " + "(ADDRESS=(PROTOCOL=TCP)(HOST=139.82.3.27)(PORT=1521)))(CONNECT_DATA=(SERVER=DEDICATED)(SID = orcl))); " + " User Id = C##1721629; Password = 1721629");
            orcl.Open();
            // Executa um comando de select * na tabela de clientes e imprime na            tela.
            //SQLSimples(orcl);
            // Executa o select * na tabela de cliente para o id do usuário passado por parâmetro
            //SQLParametro(orcl, 1);
            // Imprime o ID do cliente através do nome passado por parâmetro.
            //Funcao(orcl, "ANA");
            // Troca o nome do cliente com id passado por parametro pelo novo nome.
            //StoredProcedure(orcl, 2, "RIB");
            // Executa um select na tabela de usuarios para um id, porém apresenta os dados de toda a tabela
            //SQLInjection(orcl);
            // Deveria executar um insert na tabela de cliente, porém apaga o conteúdo da tabela.
            //SQLInjectionDestrutivo(orcl);
            // Insere um cliente com os parametros passados.
            //insereCliente(orcl, 100, "ADA", 100000, "Rua ADA", "F", "10/01/2001");
            //SQLParametro(orcl, 100);
            /* //Inserção manual de um cliente
            Console.Write("ID: ");
            string ident = Console.ReadLine();
            Console.Write(Environment.NewLine);
            Console.Write("Nome: ");
            string nome = Console.ReadLine();
            insereCliente(orcl, Int32.Parse(ident), nome, 0, "", "", "01/01/2001");*/
            orcl.Close();
            Console.Write(Environment.NewLine);
            Console.Write("FINALIZOU");
            Console.ReadKey();
        }
        static void SQLSimples(OracleConnection orcl)
        {
            // COMANDO SIMPLES DE SQL
            OracleCommand cmd = new OracleCommand("select * from cliente", orcl);
            imprimeTabela(cmd);
        }
        static void SQLInjection(OracleConnection orcl)
        {
            // String id = "1"; // Código normal
            String id = "2 or 'a'='a'"; // Injection
            OracleCommand cmd2 = new OracleCommand("select * from cliente where id = " + id, orcl);
            imprimeTabela(cmd2);
        }
        static void SQLInjectionDestrutivo(OracleConnection orcl)
        {
            String id = "203)'; execute immediate 'delete from cliente where (1 = 1"; // Injection
            //String id = "203"; // SQL Normal
            OracleCommand cmd2 = new OracleCommand("begin " + " execute immediate 'insert into cliente (id) values (" + id + ")'; " + " end; ", orcl);
            cmd2.ExecuteNonQuery();
        }
        static void SQLParametro(OracleConnection orcl, int id_usuario)
        {
            // COMANDO COM PARAMETRO
            OracleCommand cmd2 = new OracleCommand("select * from cliente where id =:identificador", orcl);
            OracleParameter param = new OracleParameter("identificador", id_usuario);
            cmd2.Parameters.Add(param);
            imprimeTabela(cmd2);
        }
        static void StoredProcedure(OracleConnection orcl, int id_usuario, string novo_nome)
        {
            // Criar o Comando somente com o nome da Stored Procedure
            OracleCommand objCmd = new OracleCommand("sp_alteranomecliente",
orcl);
            // Definir que o tipo de comando será uma StoredProcedure
            objCmd.CommandType = System.Data.CommandType.StoredProcedure;
            // Criar os parametros da Stored Procedure
            OracleParameter param1 = new OracleParameter("idcli", id_usuario);
            OracleParameter param2 = new OracleParameter("nomenovo", novo_nome);
            // Adicionar os parâmetros ao comando
            objCmd.Parameters.Add(param1);
            objCmd.Parameters.Add(param2);
            // Executar o comando
            objCmd.ExecuteNonQuery();
            // Imprimir os valores das colunas do Cliente com ID
            SQLParametro(orcl, id_usuario);
        }
        static void Funcao(OracleConnection orcl, string nome_usuario)
        {
            // Cria o comando somente com o nome da função a ser chamada
            OracleCommand objCmd = new OracleCommand("procuraid", orcl);
            objCmd.CommandType = System.Data.CommandType.StoredProcedure;
            // Define os parâmetros da chamada
            OracleParameter param = new OracleParameter("nomecompleto", nome_usuario);
            // Define o parâmetro de retorno
            OracleParameter paramRetorno = new OracleParameter("idcli", OracleType.Int32);
            paramRetorno.Direction = System.Data.ParameterDirection.ReturnValue;
            // Indica que o parâmetro será retornado.
            // Adiciona os parâmetros ao comando
            objCmd.Parameters.Add(param);
            objCmd.Parameters.Add(paramRetorno);
            // Executa o comando
            objCmd.ExecuteNonQuery();
            // Lê os parametros do comando, inclusive o retornado
            String nomeUsado = objCmd.Parameters["nomecompleto"].Value.ToString();
            String retorno = objCmd.Parameters["idcli"].Value.ToString();
            Console.Write("ID do Cliente " + nomeUsado + ": " + retorno);
        }
        static void imprimeTabela(OracleCommand oCmd)
        {
            // Abre um cursor no OracleDataReader
            using (OracleDataReader reader = oCmd.ExecuteReader())
            {
                // Imprime as colunas do reader
                for (int i = 0; i < reader.FieldCount; i++)
                    Console.Write(reader.GetName(i).ToString() + "\t");

                Console.Write(Environment.NewLine); // Coloca uma nova linha na tela
                // Itera nas linhas do reader - Tuplas do Cursor
                while (reader.Read())
                {
                    // Imprime os valores das colunas do cursor
                    for (int i = 0; i < reader.FieldCount; i++)
                        Console.Write(reader.GetValue(i).ToString() + "\t");

                    Console.Write(Environment.NewLine);
                }
            }
        }
        static void insereCliente(OracleConnection orcl, int id, string nome,
int salario, string endereco, string sexo, string datanasc)
        {
            // COMANDO COM PARAMETRO
            OracleCommand cmd2 = new OracleCommand("insert into cliente values (:id,:nome,:salario,:endereco, :sexo" + ",TO_DATE(:datanasc, 'dd/mm/yyyy'))", orcl);
            OracleParameter pId = new OracleParameter("id", id);
            OracleParameter pNome = new OracleParameter("nome", nome);
            OracleParameter pSalario = new OracleParameter("salario", salario);
            OracleParameter pEndereco = new OracleParameter("endereco", endereco);
            OracleParameter pSexo = new OracleParameter("sexo", sexo);
            OracleParameter pNascimento = new OracleParameter("datanasc", datanasc);
            cmd2.Parameters.Add(pId);
            cmd2.Parameters.Add(pNome);
            cmd2.Parameters.Add(pSalario);
            cmd2.Parameters.Add(pEndereco);
            cmd2.Parameters.Add(pSexo);
            cmd2.Parameters.Add(pNascimento);
            cmd2.ExecuteNonQuery();
        }
    }
}

