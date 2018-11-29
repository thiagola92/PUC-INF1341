# Replicação
Quando um objeto está sendo muito acessado dentro de um banco dados X, é interessante criarmos réplica nos outros bancos de dados para diminuir o acesso a aquele banco de dados X.  
Nem sempre a réplica está na mesma versão da original, afinal precisa de tempo para atualizar réplica.  
<sub>*Obs: objetos podem ser tabelas, dados, etc*</sub>

* Vantagens
  * Desempenho
    * Você cria réplicas da informações no seu computador, para se acessar no futuro já estar local
    * Você pode criar réplica só de parte de uma tabela, dessa forma também facilita a consulta
  * Disponibilidade
    * Banco de dados **X** está inacessível mas temos réplicas dos dados dele no banco de dados **Y**
    * Mobile costuma manter um réplica dos dados enquanto está offline e atualiza-los quando fica online

## Modelos de Replicação
Como vamos armazenar as réplicas

### Master-Slave
* Cada objeto possui somente um dono
  * Exemplo:
    * O dono da tabela FuncionariosRJ é o Banco de Dados X
* O banco de dados dono do objeto possui a **cópia primária** dele  
  * É possível **ler** e **escrever** na cópia primária  
  * Exemplo:
    * Cópia primária da tabela FuncionariosRJ é do Banco de Dados X
* Os bancos de dados que não são donos possuem **cópias secundárias**
  * Só é possível **ler** na cópia secundária
  * Exemplo:
    * Banco de Dados Y, Banco de Dados W e Banco de Dados Z só possuem só possuem cópias secundárias da tabela FuncionariosRJ


* Nó mestre: Os objetos nele são **cópias primárias**
* Nó escravo: Os objetos nele são **cópias secundárias**
* Nó mestreescravo: Os objetos nele podem ser **cópias primárias** ou **cópias secundárias**


### Grupo
* Todo banco de dados que possuir uma cópia, é dono dela e pode fazer o que quiser com ela
  * Ele pode **ler** e **escrever** na cópia sem problema
  * É como se todos fossem nó mestre  
* Se qualquer banco de dados falhar, você tem outro funcionando corretamente

## Estratégias de Propagação de Dados
Como vamos atualizar as réplicas

### Síncrona ou Eager
* Quando atualizamos um objeto, **tentamos** atualizar todas as réplicas do objeto
* Transação só da commit na atualização quando todas as réplicas do objeto estão atualizadas
  * Exemplo:
    * Uma tabela é atualizada, o SGBD fica tentando atualizar todas as réplicas daquela tabela. Quando todos os outros bancos de dados com réplicas disserem que conseguiram atualizar, todos dão commits junto  
    * Se um deles não conseguir atualizar, todos os outros dão rollback
* Perda de performance pois você pode perder muito tempo atualizando vários bancos de dados para no final um deles falar que não foi possível atualizar, agora você vai ter que dar rollback em todos

### Assíncrona ou Lazy
* Após atualizarmos o objeto e darmos commit, mandamos atualizar todas as réplicas
* Dados ficam temporariamente desatualizados nas réplicas
  * Exemplo:
    * Banco de dados X atualizou o objeto
    * Banco de dados X mandou pedido de atualização do objeto 1 para banco de dados Y
    * Banco de dados Y não conseguiu atualizar ainda
    * Banco de dados Y não conseguiu atualizar ainda
    * Banco de dados Y não conseguiu atualizar ainda
    * Banco de dados Y atualizou o objeto
    * *Você pode ter pego o objeto desatualizado do banco de dados Y*

20:33 Aula12
