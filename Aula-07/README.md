## Banco de Dados Distribuído
* Vários bancos de dados, que embora sejam banco de dados separados eles estão ligados logicamente.  
  * Por exemplo, você ter um banco de dados no Rio de Janeiro e outro em São Paulo mas ambos se comunicam um com o outro pela rede.  
* Cada banco de dados tem um certo nível de autonomia, tomam certas decisões sozinhos  

## Sistemas de Gerência de Bancos de Dados Distribuídos (SGBDD)
* A idéia é fazer o usuário enxergar como se fosse um SGBD sem ser distribuído
  * Dessa maneira facilitando o desenvolvimento
* Pode ficar mais complicado dependendo do quanto os bancos são diferentes, um banco NoSQL com integração de um SQL seria bem mais complicado.  
  * Bancos de dados podem ter tipo de variáveis diferentes, por exemplo oracle tem VARCHAR2
  * Bancos de dados podem armazenar de maneiras diferente os dados, por exemplo acesso sequencial, arvore b+, hash...

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

### Distribuição dos dados
Normalmente quando os dados são distribuidos, eles são deixados perto do local que mais utilizam eles.    

Banco de dados RJ  
Banco de dados SP  
Banco de dados BA  

Se tivermos uma empresa com esses 3 banco de dados e que armazenam informações dos funcionários, as informações dos funcionários de RJ devem acabar no banco de dados do RJ pois é mais provável que o pessoal do RJ utilize mais esses dados. O mesmo vale pros outros.  24:32 banco-08
