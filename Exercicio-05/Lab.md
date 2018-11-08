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
                  + "CONSTRAINT cliente_pk PRIMARY KEY"
                  + "(id) ENABLE"
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
                  + "PRIMARY KEY (id_cliente)"
                + ")";
OracleCommand command = new OracleCommand(sql, oracle);
command.ExecuteReader();
```
