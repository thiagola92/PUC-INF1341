# Replicação
Quando um objeto está sendo muito acessado dentro de um banco dados **X**, é interessante criarmos réplica nos outros bancos de dados para diminuir o acesso a aquele banco de dados **X**.  
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
* Se qualquer banco de dados falhar, você tem outro funcionando corretamente

## Estratégias de Propagação de Dados

### Eager

### Lazy
