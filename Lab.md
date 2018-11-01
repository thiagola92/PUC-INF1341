# Laboratório

Abre uma conexão com o banco de dados e finaliza a conexão com o banco de dados
```C#
OracleConnection orcl = new OracleConnection("Data Source = (DESCRIPTION = (ADDRESS_LIST = " + "(ADDRESS=(PROTOCOL=TCP)(HOST=139.82.3.27)(PORT=1521)))(CONNECT_DATA=(SERVER=DEDICATED)(SID = orcl))); " + " User Id = C##1721629; Password = 1721629");
orcl.Open();
orcl.Close();
```

Responsável por mandar o pedido de uma query para o banco de dados e ler a resposta dela
```C#
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
```

Cria uma query `SELECT * FROM CLIENTE` e utiliza `imprimeTabela` para executar a query e imprimir a resposta.
```C#
static void SQLSimples(OracleConnection orcl)
{
    // COMANDO SIMPLES DE SQL
    OracleCommand cmd = new OracleCommand("select * from CLIENTE", orcl);
    imprimeTabela(cmd);
}
```

Cria uma query
````SQL
SELECT *
  FROM CLIENTE
  WHERE ID = :identificador
```
Mas o `:identificador` está ali para ser substituido pelo paramêtro passado na função (id_usuario).  
A diferença é que a query é montada antes e depois você substitui pelos paramêtros passados.  
Por final passa para `imprimiTabela` para executar a query e exibir o resultado.  
```C#
static void SQLParametro(OracleConnection orcl, int id_usuario)
{
    // COMANDO COM PARAMETRO
    OracleCommand cmd2 = new OracleCommand("select * from cliente where id =:identificador", orcl);
    OracleParameter param = new OracleParameter("identificador", id_usuario);
    cmd2.Parameters.Add(param);
    imprimeTabela(cmd2);
}
```
