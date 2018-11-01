# Laboratório

```C#
OracleConnection orcl = new OracleConnection("Data Source = (DESCRIPTION = (ADDRESS_LIST = " + "(ADDRESS=(PROTOCOL=TCP)(HOST=139.82.3.27)(PORT=1521)))(CONNECT_DATA=(SERVER=DEDICATED)(SID = orcl))); " + " User Id = C##1721629; Password = 1721629");
orcl.Open();
orcl.Close();
Console.Write(Environment.NewLine);
Console.Write("FINALIZOU");
Console.ReadKey();
```
