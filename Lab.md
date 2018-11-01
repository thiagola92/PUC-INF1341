# Laboratório

## 0
`OracleConnection` representa a conexão com o banco de dados.  
`orcl.Open();` abre essa conexão com o banco de dados.  
`orcl.Close();` finaliza essa conexão com o banco de dados.  
```C#
OracleConnection orcl = new OracleConnection("Data Source = (DESCRIPTION = (ADDRESS_LIST = " + "(ADDRESS=(PROTOCOL=TCP)(HOST=139.82.3.27)(PORT=1521)))(CONNECT_DATA=(SERVER=DEDICATED)(SID = orcl))); " + " User Id = C##1721629; Password = 1721629");
orcl.Open();
orcl.Close();
```

## 1
Responsável por mandar o pedido de uma query para o banco de dados e ler a resposta dele.  
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

## 2
`OracleCommand` armazena um comando do tipo query a ser executada no banco de dados. Utilizamos isso para armazenar `SELECT * FROM CLIENTE`   
`imprimiTabela` executa a query, obtem a resposta e exibe.  
```C#
static void SQLSimples(OracleConnection orcl)
{
    // COMANDO SIMPLES DE SQL
    OracleCommand cmd = new OracleCommand("select * from CLIENTE", orcl);
    imprimeTabela(cmd);
}
```

## 3
Cria um comando do tipo query
```SQL
SELECT *
  FROM CLIENTE
  WHERE ID = :identificador
```
Mas o `:identificador` está ali para ser substituido pelo paramêtro passado na função (id_usuario).  
`OracleParameter` armazena o paramêtro e quem ele deve substituir na query.  
`OracleCommand` possui o método `Add()` para justamente adicionar o paramêtro criado em `OraclaParameter`.  
`imprimiTabela` executa a query, obtem a resposta e exibe.  
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

# 4
Para chamar uma função você precisa mudar o tipo do comando para `StoredProcedure`, com isso você só precisa dizer o nome da função.  
O nome procurado é passado utilizando a classe da Oracle para paramêtro novamente `OracleParameter`.  
StoreProcedured pode retornar uma resposta e precisamos armazenar essa resposta, podemos utilizar `OracleParameter` para armazenar essa resposta também.
Igualmente ao exercicio anterior, você precisa botar no comando oracle os paramêtros (no caso o nome do usuário procurado e onde deve armazenar a resposta da função).
`objCmd.ExecuteNonQuery();` executa o comando, agora dentro desse objeto fica armazenado os paramêtros passados anteriormente e a resposta.  
```C#
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
```

## 5
Funciona que nem `Funcao` mas dessa vez você não obtem um resultado de resposta.  
E executa a query para mostrar como o resultaod realmente foi executado.  
```C#
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
```

## 6
Demonstra um exemplo de como uma string não tratada pode alterar totalmente a idéia do comando original.  
Originalmente seria `select * from cliente where id = 1`  
Porém ficou alterado para `select * from cliente where id = 2 or 'a'='a'`
```C#
static void SQLInjection(OracleConnection orcl)
{
    // String id = "1"; // Código normal
    String id = "2 or 'a'='a'"; // Injection
    OracleCommand cmd2 = new OracleCommand("select * from cliente where id = " + id, orcl);
    imprimeTabela(cmd2);
}
```

## 7
Mostra como você pode inserir código destrutivo em um simple comando SQL.  
Após executar esse comando, verifique como toda a tabela CLIENTE foi apagada.
```C#
static void SQLInjectionDestrutivo(OracleConnection orcl)
{
    String id = "203)'; execute immediate 'delete from cliente where (1 = 1"; // Injection
    //String id = "203"; // SQL Normal
    OracleCommand cmd2 = new OracleCommand("begin " + " execute immediate 'insert into cliente (id) values (" + id + ")'; " + " end; ", orcl);
    cmd2.ExecuteNonQuery();
}
```
