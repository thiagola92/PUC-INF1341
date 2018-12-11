## Banco de Dados Distribuído
* Vários bancos de dados, que embora sejam banco de dados separados eles estão ligados logicamente.  
  * Por exemplo, você ter um banco de dados no Rio de Janeiro e outro em São Paulo mas ambos se comunicam um com o outro pela rede.  
* Cada banco de dados tem um certo nível de autonomia, tomam certas decisões sozinhos  

## Sistemas de Gerência de Bancos de Dados Distribuídos (SGBDD)
* A idéia é fazer o usuário enxergar como se fosse um SGBD sem ser distribuído
  * Dessa maneira facilitando o desenvolvimento

**SGBD Local**: Cuida do seu banco de dados (das suas tabelas)  
**SGBD Global (SGBDD)**: Cuida de saber em quais banco de dados está cada informação (tabela).  

um banco de dados no RJ tem informações dos funcionários do RJ  
E um banco de dados no SP tem informações dos funcionários de SP  

Se você fizer uma query por todos os funcionários para o SGBD Global, ele faz uma query para cada banco de dados, combina o resultado e devolve a resposta da query.  

### Aspecto administrativo
Cada SGBD administra seus dados, podendo escolher se quer compartilhar com o SGBDD ou deixar o SGBDD alterar.  
Reflete melhor a estrutura organizacional ou geográfica de uma empresa.  

### Aspecto econômico
O preço e desempenho dos equipamentos de menor porte tem melhorado, ou seja, você não precisa de uma ferramenta gigante para gerênciar tudo. Como as ferramentas pequenas estão melhorando, você pode ter varias delas distribuidas em várias bases da empresa.  
Diminuir o custo de comunicação, pois você pode resolver quase tudo no banco de dados local.  

### Aspecto técnico
Facilita o crescimento modular, você pode melhorar partes do sistema sem afetar o resto.  
Réplicas das tabelas aumentam a segurança em caso de perda de dados.  
Aumento de eficiência particionando e replicando, além de mover para perto do local de mais utilização.  

### Dificuldade
* Pode ficar mais complicado dependendo do quanto os bancos são diferentes, um banco NoSQL com integração de um SQL seria bem mais complicado.  
  * Bancos de dados podem ter tipo de variáveis diferentes, por exemplo oracle tem VARCHAR2
  * Bancos de dados podem armazenar de maneiras diferente os dados, por exemplo acesso sequencial, arvore b+, hash...
* O desempenho global pode ser duvidoso já que pode ter problemas com trocas de mensagens, cada banco pode ter mecanismo de controle adicionais, etc.  
* Consome recursos, pois você vai precisar acessar vários bancos para conseguir a informação global
* Por isso o custo de desenvolver um SGBDD é alto  

## Camadas

É importante as alterações numa camada não afetarem a outra, por isso SGBD tem que cuidar para alterações em cada camada só afetem as camadas alteradas.  

| Camadas | |
| --- | --- |
| --- | --- |
| Camada Externa | Aplicação enxerga essa camada |
| Camada Conceitual | Lógica da aplicação, as tabelas que existem, etc |
| Camda Física | Como os dados estão sendo armazenados |

É preciso uma transparência para converter esse tipo de informação de uma camada para outra  

| Camadas | |
| --- | --- |
| --- | --- |
| Camada Externa | Aplicação enxerga essa camada |
| Transparência Modelo |  |
| Camada Conceitual | Lógica da aplicação, as tabelas que existem, etc |
| Transparência Implementação |  |
| Camda Física | Como os dados estão sendo armazenados |

Agora precisamos cuidar para que os Bancos em localizações diferentes se entendam, por isso foi criado o SGBDD.  

| Camadas | |
| --- | --- |
| --- | --- |
| Localização |  |
| Camada Externa | Aplicação enxerga essa camada |
| Transparência Modelo |  |
| Camada Conceitual | Lógica da aplicação, as tabelas que existem, etc |
| Transparência Implementação |  |
| Camda Física | Como os dados estão sendo armazenados |

Como pode ver tem todas essas camadas de informações que o usuário não precisa ver ou entender, ele precisa acessar como se tivesse pegando a informação de um lugar só.  
