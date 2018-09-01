# Aula 03
Aula vai ser sobre:
* Prepared Statements
* Visões
* Stored Procedures
* Triggers

### Plano de acesso
Pelo o nome você consegue ter uma idéia que é o plano de alguém para acessar algo.  
**Plano de acesso**: O caminho e forma de acesso que vão ser utilizados para atender uma requisição SQL. Em outras palavas, como você vai acessar os dados.   

### Otimizador  
Pelo o nome você já sabe que a idéia dele é otimizar algo.  
**Otimizador**: É responsável por fazer o *plano de acesso* aos dados. Em outras palavras, descobrir a maneira mais eficiente de acessar os dados.  

A idéia é que o usuário não precisa saber como os dados estão armazenados para poder fazer a query SQL.  
Você não sabe se os arquivos estão em ordem sequencial, se o acesso deles é por hash, se é uma arvore B+... Você sabe que o otimizador vai tentar pegar o melhor caminho para a sua query.  

# Etapas de uma query
![Etapas da query](/Aula-03/etapas_consulta.png)  
**Parse Query**: Verifica se está escrita corretamente e se os objetos são objetos do banco  
**Check de Semântica**: Válida a semântica   
**Query Rewrite**: Re-escreve a query de forma que seja melhor aproveitada para a otimização do plano de acesso  
**Otimização do Plano de Acesso**: Escolhe o melhor caminho para os dados  
**Geração de Código**: Transforma em código para acessar o HD  

A idéia aqui é mostrar que não é só escrever um comando e ele executa, o comando vai passar por uma série de etapas quando mandado para o banco de dados.   

# Prepared Statements
(Comandos Preparados)  

A ultima etapa em "Etapas de uma query" é gerar o código. Nós passamos um comando SQL e ele gera um código na linguagem do banco de dados.  
Você pode achar que o banco de dados gera esse código, usa ele e joga ele fora. Isso seria ineficiente, pois a chance de alguém fazer uma query idêntica é muito grande e se gasta tempo passando por todas as etapas para gerar código.  

**Prepared Statements**: Uma versão compilada de um comando SQL mantida salva.  

Após você enviar uma primeira versão da query, ele armazena essa versão já compilada (prepared statement). Dessa maneira se a mesma query for pedida o SGBD pode executar o Prepared Statement sem ter que compilar antes.  

Quando você sabe que vai executar um comando várias vezes, você pode já pedir para criar um Prepared Statement, dessa maneira já compilando.  
A melhor parte disso tudo é que você pode deixar alterar os parâmetros deses Prepared Statements.  

Como normalmente você faria uma query SQL em Java:  
```Java
Statement stmt = con.createStatement();

String sale = 75;
String coffeeName = "'Colobian'";
String updateString = "UPDATE COFFEES SET SALE = " + sale + " WHERE COF_NAME LIKE " + coffeName;

stmt.executeUpdate(updateString);
```

Como você já pode deixar uma query SQL preparada em Java:  
```Java
PreparedStatement updateSales = con.preparedStatement("UPDATE COFFEES SET SALE = ? WHERE COF_NAME LIKE ?");

updateSales.setInt(1, 75);
updateSales.setString(2, "Colombian");

updateSales.executeUpdate();
```

Podem até parecerem indiferentes, mas em um você já deixa o comando compilado antes de precisar dele. Quando precisar, basta substituir os valores e utilizar.  

#### Extra
Se o comando executada é de UPDATE/INSERT/DELETE, ele retorna o número de linhas afetadas (pode retornar 0 linhas afetadas).  
Se o comando executado é uma DDL (data definition language) retorna 0. Por exemplo, criar uma tabela retorna 0.    
```Java
int n = updateSales.executeUpdate();
```

Java Database Connectivity: https://docs.oracle.com/javase/tutorial/jdbc/basics/index.html  

# SQL Injection
É uma técnica de [code injection](https://en.wikipedia.org/wiki/Code_injection) onde o usuário malicioso consegue injetar código na nossa query SQL. Isso pode levar a vários problemas, ele pode roubar dados ou danificar o banco de dados ou alterar dados...  

Agora o professor mostrou uma uma série de exemplos de quando o nosso código pode dar chance do usuário injetar código SQL.  

Exemplo 1, o parâmetro da query é passado direto do URL para a query  
`http://www.thecompany.com/pressRelease.jsp?pressReleaseID=5`  
<sub>(Talvez você precise ter feito programação para web para entender que o 5 vai ser passado em diante pela função `request.getParameter()`)</sub>  

Código Java montando a query  
```Java
String query = "SELECT title, description, releaseDate, body FROM pressReleases
WHERE pressReleaseID = " + request.getParameter("pressReleaseID");

Statement stmt = dbConnection.createStatement();
ResultSet rs = stmt.executeQuery(query);
```

A idéia normal seria passar o valor `5` para a query  
```SQL
SELECT title, description, releaseDate, body FROM pressReleases WHERE pressReleaseID = 5
```  

Mas esse código da tanta chance do usuário injetar código no nosso... Vamos supor que o usuário digitou  
`http://www.thecompany.com/pressRelease.jsp?pressReleaseID=5 OR 1=1`  
Esse `1=1` é uma comparação que vai retorna true.  

O que vai acontecer com a query no final?  
```SQL
SELECT title, description, releaseDate, body FROM pressReleases WHERE pressReleaseID = 5 OR 1=1
```  

#### Soluções
1. Válidar a entrada
2. Utilizar **Bind Variables**
3. Transferir código de forma adequada para Procedimentos Armazenados  
...  

Existem várias soluções mas vamos ver apenas a *Bind Variables*.  

**Bind Variables**: São simplesmente placeholders, buracos na chamada SQL que você tem que substituir com as variáveis válidas. Ao tentar subsituir, acontece uma verificação para validar, ter certeza que você passou o tipo de variável certa.  

```Java
PreparedStatement updateSales = con.preparedStatement("UPDATE COFFEES SET SALE = ? WHERE COF_NAME LIKE ?");

// Recebendo valores do usuário
int sale = request.getParameter("sale");
String name = request.getParameter("name");

// Bind Variables
updateSales.setInt(1, sale);
updateSales.setString(2, name);

updateSales.executeUpdate();
```

As duas funções responsáveis por inserir as variáveis vão verificar elas.  
Por exemplo, `setString(2, name)` provávelmente está verificando se você não inseriu apóstrofe no meio da string de forma a tentar inserir código na query SQL ao mesmo tempo que faz um parse para o tipo desejado.  

# Views e Materialized Views

## View
É uma **tabela virtual** criada atráves de uma query, ou seja, após fazer uma query e obter uma tabela, essa **query** é armazenada para futuro uso.  

* Essa tabela não é uma cópia das informações da original, mas sim um ponteiro para elas.  
* Como View é uma query já executada, você ganha o mesmo benefícios do Prepared Statements, onde não precisa gerar o código da query novamente.  
  * Porém, Prepared Statement permite passar parâmetros diferentes pra mesma query, Views não permitem.  
* Como Views são referências a dados de tabelas, se você atualizar as informações da tabela, essas Views também vão ser atualizadas.  
  * Logo os programas que trabalham usando essas Views, não vão parar de funcionar.  

### Extra
Esse código vai gerar uma View exibindo todas as informações dos funcionários que forem do RJ.  
```SQL
CREATE VIEW FuncionariosRJ AS
SELECT col1, col2, ..., coln
FROM Funcionarios
WHERE base = 'RJ'
```

Problema disso é que se depois alguém tentar inserir nessa View, ela vai conseguir inserir pessoas que não são do RJ.  
```SQL
INSERT INTO FuncionariosRJ (..., base)
VALUES (..., 'SP')
```

Para impedir esse tipo de inserção precisamos deixar claro que não aceitamos items que não passam da condição `WHERE`. Isso é efeito usando `WITH CHECK OPTION`.  
```SQL
CREATE VIEW FuncionariosRJ AS
SELECT col1, col2, ..., coln
FROM Funcionarios
WHERE base = 'RJ'
WITH CHECK OPTION
```
<sub>O `WITH` e `OPTION` também eram para estar em roxo</sub>

Com essa opção, você criou uma View onde toda condição do definida pelo `WHERE` tem que ser obrigatório. Elementos que não tiverem essas condições não podem ser inseridos na View.  

## Materialized View
É uma **tabela real** criada atráves de uma query, ou seja, após fazer uma query e obter uma tabela, essa **tabela** é armazenada para futuro uso.  

* Essa tabela é uma cópia das informações da original.  
* Como Materialized Views são copia da original, dependendo de quando copiou podem estar desatualizadas.
  * Por isso se estabelece de quanto em quanto tempo se atualiza.  

### Velocidade

**Sem View**: Você passa por todas etapas de uma query para transformar em código, depois executar e obter a tabela.  
**View**: Você já tem o código da query pronto, só precisa executar e obter a tabela.  
**Materialized View**: Você já tem uma tabela da query pronta.  

Lembre que se o banco de dados for *muito muito muito* grande, uma query pode custar um tempo grande. Mesmo uma tabela desatualizada por X segundos pode ser bem vista.  

# Stored Procedures
Banco de Dados não armazena apenas Dados, também armazenam **Procedures** e **Functions** (funções e procedimentos). Uma vez armazenados os dois são chamados de Stored Procedures.    

SGBD utiliza deles para garantir regras de negócios e segurança, usando funções e procedimentos você consegue ter certeza que o usuário está alterando/inserindo/deletando de forma correta.  
Você não precisa mais dar poder ao usuário de acessar as tabelas, eles utilizam das funções e procedimentos para conseguir o que procuram.  

Functions e Procedures:  
* **Ambos** são armazenados no Banco de Dados  
* **Ambos** são executados pelo Servidor  
* **Ambos** utilizam linguagem SQL misturado com um pouco de linguagem de programação  
* **Ambos** durante a criação também passam por aquelas etapas para criação de código, mas uma vez criado ficam armazenados no banco para utilizar  
* **Functions** retornam valor  
* **Ambos** começam a executar quando o usuário chama ou outro código chama  

**Lado Negativo**: Stored Procedures não tem uma padronização, ou seja, cada banco de dados criou sua própria linguagem. Migrar de banco vai fazer com que você tenha que reescrever os procedimentos.  

## Exemplos PL/SQL   
> Eu fiz banco de dados 1 em 2012.2, estamos em 2018.2.  
Então não confie em nada que eu escrever. Além disso vou tentar ser minimalista nas partes de código  

**Procedure**
```SQL
CREATE PROCEDURE procedure_name(arg1 data_type, ...)
AS
  -- declare variáveis aqui
BEGIN
  -- código
END;
```
Como a procedure pode já existir é recomendado usar o comando `CREATE OR REPLACE` no lugar do simples `CREATE`.  

**Functions**
```SQL
CREATE FUNCTION function_name(arg1 data_type, ...)
  RETURN return_datatype
AS
  -- declare variáveis aqui
BEGIN
  -- código
END;
```
Como a função pode já existir é recomendado usar o comando `CREATE OR REPLACE` no lugar do simples `CREATE`.  

PL/SQL Stored Procedures:  
https://docs.oracle.com/cd/B28359_01/appdev.111/b28370/subprograms.htm#LNPLS008  
https://www.tutorialspoint.com/plsql/plsql_procedures.htm  
https://www.tutorialspoint.com/plsql/plsql_functions.htm  

## Exemplo Java
```Java
String createProcedure =
  "CREATE PROCEDURE procedure_name " +
  "AS " +
  "FROM first_table, second_table " +
  "WHERE first_table.id = second_table.id "

Statement stmt = con.createStatement();
stmt.executeUpdate(createProcedure);

CallableStatement cs = con.prepareCall("{CALL procedure_name}");
ResultSet rs = cs.executeQuery();
```

Java Stored Procedure: https://docs.oracle.com/javase/tutorial/jdbc/basics/storedprocedures.html  

# Triggers
Conhecidos como gatilhos, são programas executados *após*/*antes*/*em vez de* algum evento ocorrer.  

SGBD utiliza eles pelos mesmos motivos dos Stored Procedures. Garantir regras de negócios e segurança.  

* **Triggers** são armazenados no Banco de Dados  
* **Triggers** são executados pelo Servidor  
* Utilizam linguagem SQL misturado com um pouco de linguagem de programação
* **Triggers** durante a criação também passam por aquelas etapas para criação de código, mas uma vez criado fica armazenado no banco para utilizar  
* **Triggers** começam a executar quando algum evento ocorre  
* **Triggers** podem abortar os eventos  

**Lado Negativo**: Também não existe padronização, ou seja, cada banco de dados criou sua própria linguage. Alguns podem nem ter essa funcionalidade.  
Você está botando mais tarefas no banco de dados.  

## Exemplo PL/SQL  
```SQL
CREATE TRIGGER trigger_name
  BEFORE
  DELETE
  ON table_name
DECLARE
  -- declare variáveis aqui
BEGIN
  -- código
END;
```

Igual a funções e procedimentos, é recomendado trocar `CREATE` por `CREATE OR REPLACE`.  
Você pode trocar `BEFORE` por `AFTER` ou `INSTEAD OF`.  
Você pode trocar o evento `DELETE` por `INSERT` ou `UPDATE`.  
Você pode fazer ocorrer em mais de um evento, `DELETE OR INSERT OR UPDATE`.  
Você pode limitar a apenas uma coluna específica, `OF col_name ON table_name`.  
Você pode dizer se quer que ocorra para cada linha da tabela que for alvo daquele evento, `FOR EACH ROW`.  

Se você utilizar `FOR EACH ROW`, você pode adicionar uma condição usando `WHEN(x == y)`  
Se você utilizar `FOR EACH ROW`, PL/SQL grava nas variáveis :NEW e :OLD o valor velho e novo da coluna, para que você possa fazer operações com isso.  
Se você utilizar `WHEN`, para referênciar `:NEW` e `:OLD` você não precisa do `:`  

```SQL
CREATE OR REPLACE TRIGGER trigger_name
  AFTER
  DELETE OR INSERT OR UPDATE
  OF col_name
  ON table_name
  FOR EACH ROW
  WHEN(NEW.id > 0)
DECLARE
  -- declare variáveis aqui
BEGIN
  -- código
END;
```

| variável | INSERT   | UPDATE | DELETE   |
| -------- | -------- | ------ | -------- |
| :NEW     | Existe   | Existe |          |
| :OLD     |          | Existe | Existe   |


PL/SQL Triggers:  
https://www.tutorialspoint.com/plsql/plsql_triggers.htm  
https://www.guru99.com/triggers-pl-sql.html  
