# Informações do Database
A primeira coisa que você tem que conseguir são as informações para conectar ao banco  
Se você criou o seu banco de dados que nem eu, maioria das informações para se conectar a ele estão em   
`C:\oraclexe\app\oracle\product\11.2.0\server\network\ADMIN\tnsnames.ora`  
No arquivo tnsnames.ora você encontra algo do tipo   
```
XE =
  (DESCRIPTION =
    (ADDRESS = (PROTOCOL = TCP)(HOST = ThiagoLA92-Windows)(PORT = 1521))
    (CONNECT_DATA =
      (SERVER = DEDICATED)
      (SERVICE_NAME = XE)
    )
  )
```

Após isso você tem que saber seu usuário e senha, mas note que **ambos** são case sensitive então você precisa ter certeza que está escrevendo certo.  
Para ter certeza do meu usuário eu utilizei o SQLDeveloper para executar a query `SELECT * FROM ALL_USERS;`  
Com isso eu descobri que meu usuário é todo em maiúsculo  
username: THIAGOLA92  
password: 111111  

Por fim você deve formar uma string utilizando as informações do banco de dados e do seu usuário e senha  
Mas troque pelas informações suas, seu usuário, sua senha e seu ip  
```
Data Source = (DESCRIPTION =
    (ADDRESS = (PROTOCOL = TCP)(HOST = 127.0.0.1)(PORT = 1521))
    (CONNECT_DATA =
      (SERVER = DEDICATED)
      (SERVICE_NAME = XE)
    )
  ); User Id = THIAGOLA92; Password = 111111;
```

# Conectando ao Oracle Database
```C#
string databaseInfo = "Data Source = (DESCRIPTION ="
                        + "(ADDRESS = (PROTOCOL = TCP)(HOST = 127.0.0.1)(PORT = 1521))"
                        + "(CONNECT_DATA ="
                        + " (SERVER = DEDICATED)"
                        + " (SERVICE_NAME = XE)"
                        + ")"
                      + ");"
                      + "User Id = THIAGOLA92;"
                      + "Password = 111111;";
                            
OracleConnection oracle = new OracleConnection(databaseInfo);
oracle.Open();

// ...

oracleConnection.Close();
```

# Execute Query
```C#
string sql = "CREATE TABLE cliente("
                  + "id INTEGER NOT NULL,"
                  + "nome VARCHAR2(30) ,"
                  + "salario DECIMAL(10,2) ,"
                  + "endereco VARCHAR2(50) ,"
                  + "sexo CHAR(1) ,"
                  + "datanasc DATE,"
                  + "CONSTRAINT cliente_pk PRIMARY KEY"
                  + "(id) ENABLE"
                + ")";
OracleCommand command = new OracleCommand(sql, oracle);
command.ExecuteReader();
```

# Print Query
```C#
string sql = "SELECT * FROM CLIENTE";
OracleCommand command = new OracleCommand(sql, oracle);
OracleDataReader reader = command.ExecuteReader();

// Exibir os nomes das colunas
for (int i = 0; i < reader.FieldCount; i++)
    Console.Write(reader.GetName(i).ToString() + "\t");
Console.WriteLine();

// Exibir as informações das colunas
while (reader.Read())
{
    for (int i = 0; i < reader.FieldCount; i++)
        Console.Write(reader.GetValue(i).ToString() + "\t");
    Console.WriteLine();
}

reader.Close();
```

# Prepared Statement
Você pode criar a query uma vez e apenas substituir o parâmetro quando precisa usar ela novamente  
(essa maneira protege contra SQL Injection)  
```C#
// Cria query
string sql = "SELECT * FROM CLIENTE WHERE nome = :nome";
OracleCommand command = new OracleCommand(sql, oracle);

// Substitui :nome pelo o que a variável donoDaConta tiver segurando
string donoDaConta = "thiago";
OracleParameter parameter = new OracleParameter("nome", donoDaConta);
command.Parameters.Add(parameter);
command.ExecuteReader();

// Substitui :nome pelo o que a variável donoDaConta tiver segurando
string donoDaConta2 = "miguel";
OracleParameter parameter2 = new OracleParameter("nome", donoDaConta2);
command.Parameters.Add(parameter2);
command.ExecuteReader();
```
