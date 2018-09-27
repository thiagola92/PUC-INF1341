# Tabelas

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
CREATE TABLE Cliente(
    Codigo              INTEGER         PRIMARY KEY,
    Nome                VARCHAR(255),
    Logradouro          VARCHAR(255),
    Numero              INTEGER,
    Complemento         VARCHAR(255),
    Cidade              VARCHAR(255),
    Estado              VARCHAR(2),
    NumeroContribuinte  INTEGER
);
```
