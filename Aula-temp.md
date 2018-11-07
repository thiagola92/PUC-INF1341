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
                            
OracleConnection oracleConnection = new OracleConnection(databaseInfo);
oraacleConnection.Open();

// ...

oracleConnection.Close();
```