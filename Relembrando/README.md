# Relembrando SQL
Básicamente um bando de exemplo de comandos SQL e PL/SQL.  

# Convenção
Nos meus exemplos eu vou seguir o seguinte:
* Reserved keywords maiúsculas => `CREATE TABLE`
* Nome de tabelas, primeira letra maiúscula => `Clientes`
* Nome de dados, minúsculo => `salario`

# Tipos de Dados

| Tipo de dado          | Variação |
| --------------------- | -------- |
| INTEGER               | INT      |
| SMALLINT              |          |
| NUMERIC               |          |
| DECIMAL               |          |
| REAL                  |          |
| DOUBLE PRECISION      |          |
| FLOAT                 |          |
| BIT                   |          |
| BIT VARYING           |          |
| DATE                  |          |
| TIME                  |          |
| TIMESTAMP             |          |
| CHARACTER             | CHAR     |
| CHARACTER VARYING     | VARCHAR  |
| INTERVAL              |          |

# Operadores Lógicos

| Operador |
| -------- |
| AND      |
| OR       |
| NOT      |

# Outros Operadores

| Operador    |
| ----------- |
| IS NULL     |
| IS NOT NULL |
| BETWEEN     |
| LIKE        |
| IN          |

# DDL (Data Definition Language)
* CREATE
* DROP
* ALTER
* TRUNCATE
* COMMENT
* RENAME

# DML (Data Manipulation Language)
* SELECT
* INSERT
* UPDATE
* DELETE

# DCL (Data Control Language)
* GRANT
* REVOKE

# TCL (Transaction Control Language)
* COMMIT
* ROLLBACK
* SAVEPOINT
* SET TRANSACTION

# Criar Tabela

```SQL
CREATE TABLE CD_de_musica (
  codigo_cd         INTEGER       NOT NULL,
  codigo_gravadora  INTEGER       NULL,
  nome_cd           VARCHAR(60)   NULL,
  preco_venda       DECIMAL(16,2) NULL,
  data_lancamento   DATE          DEFAULT SYSDATE,
  cd_indicado       INTEGER       NULL,

  PRIMARY KEY(codigo_cd),
  FOREIGN KEY(codigo_gravadora)
  REFERENCES Gravadora(codigo_gravadora),
  CHECK(preco_venda > 0)
);
```

# Substituir Tabela

```SQL
CREATE OR REPLACE TABLE CD_de_musica (
  ...
);
```

# Deletar Tabela

```SQL
DROP TABLE CD_de_musica;
```

# Inserir na Tabela

```SQL
INSERT INTO CD_de_musica (codigo_cd, nome_cd)
  VALUES
    (420, 'a nata'),
    (69, 'o random');
```

# Atualizando Tabela

```SQL
UPDATE CD_de_musica
  SET preco_venda = 15
  WHERE preco_venda > 15;
```

# Deletando da Tabela

```SQL
DELETE FROM CD_de_musica
  WHERE codigo_gravadora = 3;
```

# Pesquisar na Tabela

`*` = Tudo  
```SQL
SELECT *
  FROM CD_de_musica;
```

```SQL
SELECT nome_cd, preco_venda
  FROM CD_de_musica
  WHERE codigo_gravadora > 3
    AND codigo_gravadora IS NOT NULL
  ORDER BY nome_cd, preco_venda;
```

# Visão

```SQL
CREATE VIEW nome_da_view
AS
  SELECT codigo_cd, codigo_gravadora
  FROM CD_de_musica;
```

# Tabela Temporária

`GLOBAL` = todos podem ver
`LOCAL` = apenas quem criou pode ver

```SQL
CREATE GLOBAL TEMPORARY TABLE temp_cd (
  codigo INTEGER,
  nome    VARCHAR(60),
  preco   DECIMAL(16,2),
  PRIMARY KEY*codigo(
);
```

```SQL
CREATE LOCAL TEMPORARY TABLE temp_cd (
  codigo INTEGER,
  nome    VARCHAR(60),
  preco   DECIMAL(16,2),
  PRIMARY KEY*codigo(
);
```

# Bloco de Comandos

```SQL
DECLARE
  nome    VARCHAR(60)
  idade   INTEGER := 24;
BEGIN
  nome := 'Thiago';
  idade := idade + 10;
EXCEPTION
  -- exceptions
END;
```

# Procedimento

```SQL
CREATE OR REPLACE PROCEDURE deleta_cd(preco DECIMAL)
AS
BEGIN
    DELETE
        FROM CD_de_musica
        WHERE preco_venda = preco;
END;
```

Se não tiver parametros, não utilizar parênteses
```SQL
CREATE OR REPLACE PROCEDURE deleta_cd
AS
BEGIN
    DELETE
        FROM CD_de_musica
        WHERE preco_venda > preco;
END;
```

# Função

```SQL
CREATE OR REPLACE FUNCTION deleta_cd(preco DECIMAL)
    RETURN DECIMAL
AS
    return_value DECIMAL;
BEGIN
    SELECT *
        INTO return_value
        FROM CD_de_musica
        WHERE preco_venda = preco;
        
    RETURN return_value;
END;
```

# Savepoint

```SQL
SAVEPOINT nome_savepoint;
```

# Rollback

```SQL
ROLLBACK nome_savepoint;
```

# Níveis de Isolamento

`SERIALIZABLE` =  
`READ COMMITED` =  
`REPEATABLE READ` =  
`READ UNCOMMITED` =  

```SQL
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
```
```SQL
SET TRANSACTION ISOLATION LEVEL READ COMMITED
```
```SQL
SET TRANSACTION ISOLATION LEVEL REPEATABLE READ
```
```SQL
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITED
```
