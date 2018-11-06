**Nome**: Thiago Lages de Alencar  
**Matricula**: 1721629  

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

# 2

