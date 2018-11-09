**Dupla**: Thiago Lages de Alencar e Pedro Alvarez   
**Matricula**: 1721629 e 1210881  

# 1
Primeira maneira seria utilizando `OracleParameter` para armazenar o valor e depois inserir com o `OracleCommand.Parameters.Add()`.  
```C#
static void SQLInjectionDestrutivo(OracleConnection orcl)
{
  String id = "203)'; execute immediate 'delete from cliente where (1 = 1";
  
  OracleCommand cmd = new OracleCommand("begin execute immediate 'insert into cliente (id) values (" + :id + ")'; end; ", orcl);
  OracleParameter param = new OracleParameter("id", id);
  
  cmd.Parameters.Add(param);
  cmd.ExecuteNonQuery();
}
```

Segunda maneira é verificar utilizando regex para saber se os apostrofe se fecham ou verficar se tem número impar.  

# 2a

```C#
string sql = "CREATE TABLE cliente("
                  + "id INTEGER NOT NULL,"
                  + "nome VARCHAR2(30) ,"
                  + "salario DECIMAL(10,2) ,"
                  + "endereco VARCHAR2(50) ,"
                  + "sexo CHAR(1) ,"
                  + "datanasc DATE,"
                  + "PRIMARY KEY (id)"
                + ")";
OracleCommand command = new OracleCommand(sql, oracle);
command.ExecuteReader();
```

# 2b

```C#
string sql = "CREATE TABLE conta("
                  + "id_cliente INTEGER NOT NULL,"
                  + "agencia INTEGER ,"
                  + "numero CHAR(11) ,"
                  + "saldo DECIMAL(10,2) ,"
                  + "PRIMARY KEY (id_cliente, agencia)"
                + ")";
OracleCommand command = new OracleCommand(sql, oracle);
command.ExecuteReader();
```

# 2c

```C#
string sql = "CREATE TABLE cli_conta("
                  + "id_tabela_cliente INTEGER,"
                  + "id_tabela_conta INTEGER,"
                  + "agencia_tabela_conta INTEGER,"
                  + "FOREIGN KEY (id_tabela_cliente) REFERENCES cliente(id),"
                  + "FOREIGN KEY (id_tabela_conta, agencia_tabela_conta) REFERENCES conta(id_cliente, agencia)"
                + ")";
OracleCommand command = new OracleCommand(sql, oracle);
command.ExecuteReader();
```

# 2d

NOT NULL
```C#
string sql = "ALTER TABLE conta"
              + " MODIFY id_cliente INTEGER NOT NULL";
OracleCommand command = new OracleCommand(sql, oracle);
command.ExecuteReader();

sql = "ALTER TABLE conta"
      + " MODIFY agencia INTEGER NOT NULL";
command = new OracleCommand(sql, oracle);
command.ExecuteReader();

sql = "ALTER TABLE cliente"
      + " MODIFY id INTEGER NOT NULL";
command = new OracleCommand(sql, oracle);
command.ExecuteReader();
```

Saldo máximo 1000  
```C#
string sql = "ALTER TABLE conta"
              + " ADD CONSTRAINT saldoMaximo"
              + " CHECK ( saldo <= 1000 )";
OracleCommand command = new OracleCommand(sql, oracle);
command.ExecuteReader();
```

# 2e

```C#
string sql = "INSERT INTO cliente(id, nome, salario, endereco, sexo, datanasc) " +
                "VALUES(:id, :nome, :salario, :endereco, :sexo, TO_DATE(:data, 'DD/MM/YYYY'))";
OracleCommand command = new OracleCommand(sql, oracle);

OracleParameter id = new OracleParameter("id", 7);
command.Parameters.Add(id);

OracleParameter nome = new OracleParameter("nome", "ARI");
command.Parameters.Add(nome);

OracleParameter salario = new OracleParameter("salario", 1000);
command.Parameters.Add(salario);

OracleParameter endereco = new OracleParameter("endereco", "RUA A N. 1");
command.Parameters.Add(endereco);

OracleParameter sexo = new OracleParameter("sexo", "M");
command.Parameters.Add(sexo);

OracleParameter data = new OracleParameter("data", "01/01/2001");
command.Parameters.Add(data);

command.ExecuteReader();
```

# 2f

```C#
string sql = "CREATE PROCEDURE alterar_saldo(idprocurado INTEGER, saldonovo DECIMAL)" +
                " AS" +
                " BEGIN" +
                "  UPDATE conta" +
                "   SET saldo = saldonovo" +
                "   WHERE id_cliente = idprocurado;" +
                " END;";
OracleCommand command = new OracleCommand(sql, oracle);
command.ExecuteReader();
```

```C#
string sql = "alterar_saldo";
OracleCommand command = new OracleCommand(sql, oracle);
command.CommandType = System.Data.CommandType.StoredProcedure;
command.Parameters.Add("idprocurado", 1);
command.Parameters.Add("saldonovo", 150);
command.ExecuteNonQuery();
```
