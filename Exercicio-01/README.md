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
```

Erro ao tentar excluir cliente que não existe na tabela
```SQL
CALL excluir_cliente(11);

SELECT * 
    FROM cliente;
```
