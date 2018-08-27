# Aula 03
Aula vai ser sobre:
* Prepared Statements
* Visões
* Procedures
* Triggers

### Plano de acesso
Pelo o nome você consegue ter uma idéia que é o plano de alguém para acessar algo.  
**Plano de acesso**: O caminho e forma de acesso que vão ser utilizados para atender uma requisição SQL. Em outras palavas, como você vai acessar os dados.   

### Otimizador  
Pelo o nome você já sabe que a idéia dele é otimizar algo.  
**Otimizador**: É responsável por fazer o *plano de acesso* aos dados. Em outras palavras, descobrir a maneira mais eficiente de acessar os dados.  

A idéia é que o usuário não precisa saber como os dados estão armazenados para poder fazer a consulta SQL.  
Você não sabe se os arquivos estão em ordem sequencial, se o acesso deles é por hash, se é uma arvore B+... Você sabe que o otimizador vai tentar pegar o melhor caminho para a sua consulta.  

# Etapas de uma consulta
![Etapas da consulta](/Aula-03/etapas_consulta.png)  
**Parse Query**: Verifica se está escrita corretamente e se os objetos são objetos do banco  
**Check de Semântica**: Válida a semântica   
**Query Rewrite**: Re-escreve a query de forma que seja melhor aproveitada para a otimização do plano de acesso  
**Otimização do Plano de Acesso**: Escolhe o melhor caminho para os dados  
**Geração de Código**: Transforma em código para acessar o HD  

A idéia aqui é mostrar que não é só escrever um comando e ele executa, o comando vai passar por uma série de etapas quando mandado para o banco de dados.   

# Prepared Statements
(Comandos Preparados)  

A ultima etapa em "Etapas de uma consulta" é gerar o código. Nós passamos um comando SQL e ele gera um código na linguagem do banco de dados.  
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

Pode até parecerem indiferentes, mas em um você já deixa o comando compilado antes de precisar dele. Quando precisar, basta substituir os valores e utilizar.  

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

21:52
