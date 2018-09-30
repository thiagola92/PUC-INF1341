# Tabelas

## Bases de dados do sistema de vendas

```SQL
CREATE TABLE NotasVenda(
    Numero          INTEGER,
    DataEmissao     DATE,
    FormaPagamento  VARCHAR(50),
    CodigoCliente   INTEGER,
    CPFVendedor     VARCHAR(50),
    
    PRIMARY
        KEY(Numero),
    FOREIGN
        KEY(CodigoCliente)
        REFERENCES Cliente(Codigo)
);
```

```SQL
CREATE TABLE ItensNota(
    Numero              INTEGER,
    NumeroMercadoria    INTEGER,
    Quantidade          INTEGER,
    ValorUnitario       NUMBER(10,2),
    
    PRIMARY
        KEY(Numero, NumeroMercadoria),
    FOREIGN
        KEY(NumeroMercadoria)
        REFERENCES Mercadorias(NumeroMercadoria)
);
```

```SQL
CREATE TABLE Mercadorias(
    NumeroMercadoria        INTEGER,
    Descricao               VARCHAR(255),
    QuantidadeEstoque       INTEGER,
    
    PRIMARY
        KEY(NumeroMercadoria)
);
```

```SQL
CREATE TABLE Cliente(
    Codigo              INTEGER,
    Nome                VARCHAR(255),
    Telefone            VARCHAR(255),
    Logradouro          VARCHAR(255),
    Numero              INTEGER,
    Complemento         VARCHAR(255),
    Cidade              VARCHAR(255),
    Estado              VARCHAR(2),
    NumeroContribuinte  INTEGER,
    
    PRIMARY
        KEY(Codigo)
);
```

## Base de dados do apoio ao controle de funcionários

```SQL
CREATE TABLE Funcionario(
    CPF                 VARCHAR(15),
    Nome                VARCHAR(255),
    Telefone            INTEGER,
    Logradouro          VARCHAR(255),
    Numero              INTEGER,
    Complemento         VARCHAR(255),
    Cidade              VARCHAR(255),
    Estado              VARCHAR(2),
    CodigoDepartamento  INTEGER,
    
    PRIMARY
        KEY(CPF),
    FOREIGN
        KEY(CodigoDepartamento)
        REFERENCES Departamento(CodigoDepartamento)
        
);
```

```SQL
CREATE TABLE CargosFunc(
    CPF                 VARCHAR(15),
    CodigoCargo         INTEGER,
    DataInicio          DATE,
    DataFim             DATE,
    
    PRIMARY
        KEY(CPF, CodigoCargo, DataInicio),
    FOREIGN
        KEY(CPF)
        REFERENCES Funcionario(CPF),
    FOREIGN
        KEY(CodigoCargo)
        REFERENCES Cargo(Codigo)
);
```

```SQL
CREATE TABLE Departamento(
    CodigoDepartamento  INTEGER,
    Nome                VARCHAR(255),
    CPFChefe            VARCHAR(15),
    
    PRIMARY
        KEY(CodigoDepartamento)
);
```

```SQL
CREATE TABLE Cargo(
    Codigo      INTEGER,
    Descricao   VARCHAR(255),
    SalarioBase NUMBER(15,2),
    
    PRIMARY
        KEY(Codigo)
);
```

# 1

## Base de dados do sistema de compras

```SQL
CREATE TABLE NotaFiscal(
    Numero                  INTEGER,
    NumeroDaCompra          INTEGER,
    NumeroMercadoria        INTEGER,
    DataCompra              DATE,
    CodigoFornecedor        INTEGER,
    
    PRIMARY
        KEY(Numero),
    FOREIGN
        KEY(NumeroDaCompra, NumeroMercadoria)
        REFERENCES ItensComprados(Numero, NumeroMercadoria),
    FOREIGN
        KEY(CodigoFornecedor)
        REFERENCES Fornecedor(Codigo)
);
```

```SQL
CREATE TABLE ItensComprados(
    Numero              INTEGER,
    NumeroMercadoria    INTEGER,
    Quantidade          INTEGER,
    ValorUnitario       NUMBER(10,2),
    ValorTotal          NUMBER,
    
    PRIMARY
        KEY(Numero, NumeroMercadoria),
    FOREIGN
        KEY(NumeroMercadoria)
        REFERENCES Mercadorias(NumeroMercadoria)
);
```

```SQL
CREATE TABLE Fornecedor(
    Codigo              INTEGER,
    Nome                VARCHAR(255),
    Telefone            VARCHAR(255),
    Logradouro          VARCHAR(255),
    Numero              INTEGER,
    Complemento         VARCHAR(255),
    Cidade              VARCHAR(255),
    Estado              VARCHAR(2),
    NumeroContribuinte  INTEGER,
    
    PRIMARY
        KEY(Codigo)
);
```

# 2

```SQL
CREATE OR REPLACE FUNCTION HistoricoDeEmpresa(Empresa VARCHAR)
RETURN
    INTEGER -- TODO
AS
    -- Codigo Cliente/Fornecedor
    CC      INTEGER;
    CF      INTEGER;
    
    -- Numero da Compra/Venda
    NdC     INTEGER;
    NdV     INTEGER;
BEGIN
    SELECT Codigo
        INTO CC
        FROM Cliente
        WHERE Empresa = Nome;
        
    SELECT Numero
        INTO NdV
        FROM NotasVenda
        WHERE CodigoCliente = CC;
    
    SELECT Codigo
        INTO CF
        FROM Fornecedor
        WHERE Empresa = Nome;
        
    SELECT NumeroDaCompra
        INTO NdC
        FROM NotaFiscal
        WHERE CodigoFornecedor = CF;
        
    SELECT *
        FROM ItensComprados
        WHERE Numero = NdC;
    
    SELECT *
        FROM ItensNota
        WHERE Numero = NdV;
        
    RETURN 1; -- TODO
END;
```

# 3

```SQL
CREATE OR REPLACE TRIGGER AlteracaoEstoque
    BEFORE
        UPDATE
        ON Mercadorias
        FOR EACH ROW
BEGIN
    IF(:NEW.QuantidadeEstoque <= 3) THEN
        raise_application_error(-20000, 'Estoque baixo, enviando e-mail');
    END IF;
END;
```

# 4

```SQL
CREATE OR REPLACE TRIGGER ValorMinDeVenda
    BEFORE
        INSERT
        ON ItensNota
        FOR EACH ROW
DECLARE
    MD      DATE;           -- Maior Data (ultima compra)
    NdC     INTEGER;        -- Numero da Compra
    PUC     NUMBER(10,2);   -- Preço Ultima Compra
BEGIN
    SELECT MAX(DataCompra)
        INTO MD
        FROM NotaFiscal
        WHERE NumeroMercadoria = :NEW.NumeroMercadoria;
        
    SELECT NumeroDaCompra
        INTO Ndc
        FROM NotaFiscal
        WHERE NumeroMercadoria = :NEW.NumeroMercadoria
        AND DataCompra = MD;
        
    SELECT ValorUnitario
        INTO PUC
        FROM ItensComprados
        WHERE NumeroMercadoria = :NEW.NumeroMercadoria
        AND Numero = Ndc;
        
    IF(:NEW.ValorUnitario < PUC) THEN
        raise_application_error(-20001, 'Vendendo por menos do que comprou');
    END IF;
END;
```

# 5

```SQL
ALTER TABLE Mercadorias
    ADD CONSTRAINT checkQuantidadeEstoque
        CHECK(QuantidadeEstoque > 0);
```

# 6

```SQL
ALTER TABLE Mercadorias
    ADD QuantidadeMaxEstoque INTEGER;
```

```SQL
CREATE OR REPLACE TRIGGER LimiteEstoque
    BEFORE
        INSERT
        ON ItensComprados
        FOR EACH ROW
DECLARE
    QE      INTEGER;    -- Quantidade Estoque
    QME     INTEGER;    -- Quantidade Máximo Estoque
BEGIN
    SELECT QuantidadeEstoque
        INTO QE
        FROM Mercadorias
        WHERE NumeroMercadoria = :NEW.NumeroMercadoria;

    SELECT QuantidadeMaxEstoque
        INTO QME
        FROM Mercadorias
        WHERE NumeroMercadoria = :NEW.NumeroMercadoria;
    
    IF(:NEW.Quantidade + QE > QME) THEN
        raise_application_error(-20002, 'Ultrapassou o limite do estoque');
    END IF;
END;
```

# 7

# 8

# 9

## Consulta dos N produtos mais vendidos

Lógica
```SQL
-- Deixando apenas as colunas que importam
SELECT NumeroMercadoria, Quantidade
    FROM ItensNota;
    
-- Fazendo o somatório das linhas
SELECT NumeroMercadoria, SUM(Quantidade)
    FROM (
    SELECT NumeroMercadoria, Quantidade
        FROM ItensNota
    )
    GROUP BY NumeroMercadoria;
    
-- Renomeando para poder organizar por aquela coluna
SELECT NumeroMercadoria, SUM(Quantidade) AS Quantidade
    FROM (
    SELECT NumeroMercadoria, Quantidade
        FROM ItensNota
    )
    GROUP BY NumeroMercadoria
    ORDER BY Quantidade DESC;
    
-- Renomeando para poder organizar por aquela coluna
SELECT NumeroMercadoria, SUM(Quantidade) AS Quantidade
    FROM (
    SELECT NumeroMercadoria, Quantidade
        FROM ItensNota
    )
    GROUP BY NumeroMercadoria
    ORDER BY Quantidade DESC; 
    
-- Limitando a quantidade de linhas que aparece
SELECT *
    FROM (
    SELECT NumeroMercadoria, SUM(Quantidade) AS Quantidade
        FROM (
        SELECT NumeroMercadoria, Quantidade
            FROM ItensNota
        )
        GROUP BY NumeroMercadoria
        ORDER BY Quantidade DESC
    )
    WHERE ROWNUM <= 2;
```

```SQL
SELECT *
    FROM (
    SELECT NumeroMercadoria, SUM(Quantidade) AS Quantidade
        FROM (
        SELECT NumeroMercadoria, Quantidade
            FROM ItensNota
        )
        GROUP BY NumeroMercadoria
        ORDER BY Quantidade DESC
    )
    WHERE ROWNUM <= 2;
```

## Consulta dos N maiores clientes

```SQL
SELECT ROWNUM, Nome, Gasto
    FROM (
    SELECT Nome, Gasto
        FROM (
        SELECT CodigoCliente, Gasto
            FROM (
            SELECT Numero AS N, SUM(Quantidade * ValorUnitario) AS Gasto
                FROM ItensNota
                GROUP BY Numero
            ), NotasVenda
            WHERE N = Numero
        ), Cliente
        WHERE CodigoCliente = Codigo
        ORDER BY Gasto DESC
    )
    WHERE ROWNUM <= 3;
```

## Consulta dos N maiores fornecedores

```SQL

```
