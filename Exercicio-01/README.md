# Enunciado

```SQL
CREATE TABLE cliente(
  id INTEGER NOT NULL,
  nome VARCHAR2(30) ,
  salario DECIMAL(10,2) ,
  endereco VARCHAR2(50) ,
  sexo CHAR(1) ,
  datanasc DATE ,
  CONSTRAINT cliente_pk PRIMARY KEY
  (id) ENABLE
);

-- tabela de historico de clientes
CREATE TABLE hcli(
  id INTEGER NOT NULL,
  nome VARCHAR2(30)
);

CREATE TABLE troca_nome_cliente(
  id INTEGER NOT NULL,
  nome_antigo VARCHAR2(30),
  nome_novo VARCHAR2(30)
);

CREATE TABLE conta_corrente(
  id_cliente INTEGER,
  agencia INTEGER,
  numero CHAR(11),
  saldo DECIMAL(10,2),
  CONSTRAINT conta_corrente_PK PRIMARY KEY
  (id_cliente) ENABLE
);

INSERT INTO cliente VALUES (1,'ARI',1000,'RUA A N. 1', 'M',TO_DATE('01/01/2001', 'dd/mm/yyyy'));
INSERT INTO cliente VALUES (2,'ANA',2000,'RUA B N. 1', 'F',TO_DATE('02/02/2002', 'dd/mm/yyyy'));
INSERT INTO cliente VALUES (3,'ADA',3000,'RUA C N. 1', 'F',TO_DATE('03/03/2003', 'dd/mm/yyyy'));
INSERT INTO cliente VALUES (4,'RUI',4000,'RUA D N. 1', 'M',TO_DATE('04/04/2004', 'dd/mm/yyyy'));
INSERT INTO cliente VALUES (5,'MAX',5000,'RUA E N. 1', 'M',TO_DATE('05/05/2005', 'dd/mm/yyyy'));
INSERT INTO cliente VALUES (6,'MIN',6000,'RUA F N. 1', 'F',TO_DATE('06/06/2006', 'dd/mm/yyyy'));
INSERT INTO cliente VALUES (7,'LUC',7000,'RUA G N. 1', 'M',TO_DATE('07/07/2007', 'dd/mm/yyyy'));
INSERT INTO cliente VALUES (8,'REX',8000,'RUA H N. 1', 'M',TO_DATE('08/08/2008', 'dd/mm/yyyy'));
INSERT INTO cliente VALUES (9,'RIC',9000,'RUA I N. 1', 'M',TO_DATE('09/09/2009', 'dd/mm/yyyy'));
INSERT INTO cliente VALUES (10,'BIA',10000,'RUA J N. 1', 'F',TO_DATE('10/10/2010', 'dd/mm/yyyy'));

INSERT INTO conta_corrente VALUES (1, 101, 1001, 1001);
INSERT INTO conta_corrente VALUES (2, 102, 1002, 2001);
INSERT INTO conta_corrente VALUES (3, 103, 1003, 3001); 
```

# 1
Crie SP para excluir um cliente, sendo passado como parâmetro o identificador do
cliente. Sempre que um cliente for excluído da tabela CLIENTE, deve ser inserido o
id e o nome do cliente em uma tabela de histórico de clientes (hcli).

```SQL
CREATE OR REPLACE PROCEDURE excluir_cliente(id_cliente INTEGER)
AS
    nome_cliente CHAR(30);
BEGIN
    SELECT nome
        INTO nome_cliente
        FROM cliente
        WHERE id = id_cliente;
        
    INSERT
        INTO hcli
        VALUES (id_cliente, nome_cliente);
        
    DELETE
        FROM cliente
        WHERE id = id_cliente;
        
EXCEPTION
    WHEN NO_DATA_FOUND
    THEN raise_application_error( -20001, 'ID do Cliente Inválida' );

END;
```

Cliente excluido com sucesso
```SQL
CALL excluir_cliente(10);

SELECT * 
    FROM cliente;
    
SELECT *
    FROM hcli;
```

Erro ao tentar excluir cliente que não existe na tabela
```SQL
CALL excluir_cliente(11);

SELECT * 
    FROM cliente;
    
SELECT *
    FROM hcli;
```

# 2
Crie Trigger de exclusão de um cliente e que salve na tabela hcli o id e o nome do
cliente a ser excluído.

```SQL
CREATE OR REPLACE TRIGGER cliente_excluido
    BEFORE
      DELETE
      ON CLIENTE
      FOR EACH ROW
BEGIN
    INSERT INTO hcli VALUES (:OLD.id, :OLD.nome);
END;
```

Testando deletar manualmente para ver se o trigger é ativado
```SQL
DELETE
    FROM cliente
    WHERE id = 9;

SELECT *
    FROM cliente;
    
SELECT *
    FROM hcli;
```

# 3
Que alterações você faria em sp_excluicliente (questão 1) se já houvesse o trigger
de exclusão para a tabela cliente (questão 2)? 

```SQL
CREATE OR REPLACE PROCEDURE excluir_cliente(id_cliente INTEGER)
AS
BEGIN
    DELETE
        FROM cliente
        WHERE id = id_cliente;
        
EXCEPTION
    WHEN NO_DATA_FOUND
    THEN raise_application_error( -20001, 'ID do Cliente Inválida' );

END;
```

```SQL
CALL excluir_cliente(8);

SELECT *
    FROM cliente;
    
SELECT *
    FROM hcli;
```

# 4
Crie uma função procuraid() que recebe como parâmetro o nome do cliente e que
retorna o id do cliente.

```SQL
CREATE OR REPLACE FUNCTION procura_id(nome_cliente CHAR)
    RETURN INTEGER
AS
    id_cliente INTEGER;
BEGIN
    SELECT id
        INTO id_cliente
        FROM cliente
        WHERE nome = nome_cliente;
    
    RETURN id_cliente;
    
EXCEPTION
    WHEN NO_DATA_FOUND
    THEN raise_application_error(-20002, 'ID do Cliente Inválida');
    
END;
```

```SQL
SET SERVEROUTPUT ON FORMAT WRAPPED;

DECLARE
    id INTEGER;
BEGIN
    id := procura_id('ANA');
    DBMS_OUTPUT.PUT_LINE('id = ' || id);
END;
```

# 5
Crie SP para alterar o nome de um cliente. A SP recebe como parâmetro o id e o
novo nome do cliente. 

```SQL
CREATE OR REPLACE PROCEDURE alterar_nome(id_cliente INTEGER, novo_nome CHAR)
AS
BEGIN
    UPDATE cliente
        SET nome = novo_nome
        WHERE id = id_cliente
          AND nome <> novo_nome;
        
EXCEPTION
    WHEN NO_DATA_FOUND
    THEN raise_application_error(-20003, 'ID do Cliente Inválida');
END;
```

```SQL
CALL alterar_nome(1, 'THI');

SELECT *
    FROM cliente;
    
SELECT *
    FROM troca_nome_cliente;
```

# 6
Crie trigger para inserir na tabela troca_nome_cliente o id do cliente, o nome
antigo e o nome novo sempre que for alterado o nome de um cliente. 

```SQL
CREATE OR REPLACE TRIGGER nome_alterado
    BEFORE
        UPDATE
        ON cliente
        FOR EACH ROW
BEGIN
    INSERT
        INTO troca_nome_cliente
        VALUES(:OLD.id, :OLD.nome, :NEW.nome);
END;
```

# 7
Escreva um comando que liste todos os campos da conta_corrente de um cliente,
dado o seu nome. Utilize a função da questão 4. 
