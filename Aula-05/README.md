# Transações
Uma transação:  
1) Começa com o banco em um estado inicial  
2) Realiza ações no banco de dados  
3) Termina com o banco em um estado final  

Se alguma problema ocorrer durante a realização das ações, você tem que ser capaz de reverter para o estado inicial.  

É preciso garantir que:  
* A transação pode ser executada concorrentemente com outras.  
* Podem acontecer falhas durante a execução de uma transação.  
* Após a mudança de estado, o banco continua consistente.  

Utilizaremos os comandos:  
`COMMIT` para efetivar a transação  
`ROLLBACK` para desfazer a transação   

### Problemas de transações
Um problema clássico de transações é você querer uma ao mesmo tempo que outra pessoa quer fazer também, nos mesmo dados que você.  
Problema clássico de concorrência.  

Pessoa A: Pega o valor de X  
Pessoa B: Pega o valor de X  
Pessoa A: Calcula X+1  
Pessoa B: Calcula X+10  
Pessoa A: Salva X como X+1  
Pessoa B: Salva X comoo X+10  
\-- O novo valor de X é X+10 --   

No final é como se a operação da pessoa A nunca tivesse ocorrido.

Um problema normal de transação é algum erro ou falha ocorrer no meio, você precisa estar preparado para restaurar o estado anterior.  
Por exemplo, internet cai e o cliente estava fazendo uma transação, você precisa ser capaz de reverter de forma que não destrua o banco de dados.  

Por isso precisamos tratar para respeitas as propriedades ACID.  

### Propriedades das Transações

#### Atomicidade  
Uma transação é tratada como uma unidade de uma operação.  
\- Ou todas as ações que compõem a transação são completas, ou nenhuma delas é.  
Se uma transação for interrompida por qualquer falha, decidir o que fazer:  
\- Terminar de completar as ações que ainda faltam  
\- Voltar atrás com todas as ações feitas antes da falha  

#### Consistência
Uma transação está correta se leva de um estado consistente para outro estado consistente

#### Isolamento
Uma transação consegue bloquear os dados que ela está usando, para que outras transações não alterem/vejam os dados que ela está utilizando antes dela dar commit  

Níveis de isolamento:  
* Read Uncommitted
* Read Committed
* Repeatable Read
* Serializable

Problemas que lidam:  
* Dirty Read  
  * Outras transações conseguem ver os dados alterados pela transação A sendo que ela ainda não confirmou alterações.  
* Nonrepeatable (Fuzzy) Read  
  * Alguma transação apagou ou modificou alguma linha que a transação A poderia estar utilizando.  
* Phantom Read  
  * Alguma transação inseriu alguma linha enquanto transação A estava fazendo consultas.    

| Nível de Isolamento | Dirty Reads | Nonrepeatable (Fuzzy) Reads | Phantom Reads |
| ------------------- | ----------- | --------------------------- | ------------- |
| Read Uncommitted    |             |                             |               |
| Read Committed      | Não ocorre  |                             |               |
| Repeatable Read     | Não ocorre  | Não ocorre                  |               |
| Serializable        | Não ocorre  | Não ocorre                  | Não ocorre    |

#### Durabilidade
Uma transação dar commit, garante que os dados são permanentes e não vão ser apagados  

### Transações Finalizando
Uma transação é constituida de comandos DML  
\- SELECT  
\- INSERT  
\- UPDATE  
\- DELETE  

Em Oracle ela é finalizada quando  
\- COMMIT  
\- ROLLBACK  
\- DDL  
\--- CREATE  
\--- DROP  
\--- ALTER  
\--- TRUNCATE  
\--- COMMENT  
\--- RENAME  
\- DCL  
\--- GRANT  
\--- REVOKE  
