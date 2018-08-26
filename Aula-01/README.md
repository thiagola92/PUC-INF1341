# Aula 01

Breve conversa sobre o que vai ser dada na matéria.

Banco de dados client/servidor  
Banco de dados distribuidos  
Requisitos de hardware e software  
Banco de dados homogeneos heterogeneos  
Particionalmente e alocação de dados  
Arquitetura paralela e distruibuida  
Discos raids  

# ACID
Propriedades ACID: https://en.wikipedia.org/wiki/ACID_(computer_science)

* Atomicidade
* Consistência
* Isolamento
* Durabilidade

# Arquitetura Centralizada
Os terminais são considerados **terminal burro** quando tudo que eles fazem é enviar a input do usuário para a máquina principal, receber a resposta e exibir para o usuário.  
Nesse caso a mainframe está controlando tudo e é responsável por controlar tudo (tirando pegar input do usuário e mostrar na tela do usuário).  

![Centralizada](/Aula-01/centralizada.jpg)  

# Arquitetura Cliente-Servidor
Com o tempo ficou mais barato e fácil dar ao Cliente CPU, Memória... Então agora o cliente também fazia boa parte das funções que antes ficava apenas na Mainframe.  
As tarefas que o Server tinha foram diminuindo, até um pouco que ficou responsável por armazenar dados e entregar ao Cliente.  

![Cliente-Servidor](/Aula-01/cliente-servidor.jpg)  

Client-Server: https://en.wikipedia.org/wiki/Client%E2%80%93server_model

## SGBDD
__S__istema de __G__erência de __B__anco de __D__ados __D__ istribuídos  

Banco de dados distribuidos é armazenas as informações da empresa em banco de dados diferentes/separados.  
Por exemplo, as informações que são muito acessadas por pessoas do Rio de Janeiro, deixar no Banco de Dados do Rio de Janeiro.    

![SGBDD](/Aula-01/sgbdd.jpg)  

Distributed Database: https://en.wikipedia.org/wiki/Distributed_database

### Bancos de Dados Homogêneos  
Fazer com que os Bancos de Dados utilizem as mesmas ferramentas para cuidar dos dados.  
Banco de dados diferentes costumam ter linguagens diferentes, tipos de dados diferentes, programas diferentes.  
Então mover dados entre banco de dados diferentes pode dar trabalho pois alguém teria que fazer um conversão e tratar possíveis erros.  

Exemplo: Banco de dados do Rio é **Oracle** e de São Paulo é **Oracle**  

### Bancos de Dados Heterogêneos  
Bancos de Dados utilizarem ferramentas diferentes para cuidar dos dados.  
Isso permite aproveitar a vantagem de cada ferramenta de Banco de Dados, algumas podem ser melhores para situação X e outras para situação Y.  
Exemplo: Banco de dados do Rio é **Oracle** e de São Paulo é **SQL Server**  

Homogeneous vs Heterogeneous: https://www.quora.com/What-are-homogeneous-hetrogeneous-distributed-DBMS

# Arquitetura Paralela

## Compartilhamento Total
A memória e discos são compartilhados entre todos os processadores.
Existe uma rede rápida que permite que os processadores façam as consultas e executem os programas que estão nas memórias e discos.  
O problema é que varios processadores usando a mesma rede acaba criando um tráfico enorme na rede.  

__Vantagem__: Qualquer processador pode ter acesso aos dados em memória, sem precisar mover da memória de um processador para o outro.  
__Desvantagem__: Está limitado ao uso de muitos procesadores, pois podem causar muito tráfico para acessar a memória e disco.  

Lembrando que uma CPU tem varios processadores.  

## Compartilhamento de Disco
Apenas os discos são compartilhados.  
A memória não é compartilhada, cada um tem a sua memória.  
O problema de tráfico enorme na rede ainda existe, você diminuiu o compartilhamento e diminuiu o tráfico mas ainda existe.  

__Vantagem__: Não existe mais o tráfico para acessar a memória.  
__Desvantagem__: Está limitado ao uso de muitos processadore, pois podem causar muito tráfico para acessar o disco.  

## Sem Compartilhamento  
Não compartilha nada, cada processadores tem seu disco e sua memória.  
Acaba com o problema de tráfico mas acaba com qualquer compartilhamento.  

__Vantagem__: Pode ter bem mais processadores pois não vão gerar quase nenhum tráfico na rede.  
__Desvantagem__: Não compartilha nada, precisam passar dados de um para o outro para compartilhar.  

## Hibrido
Tem memória compartilhada e memória local.
