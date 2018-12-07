# 1

```SQL
CREATE DATABASE LINK carros 
CONNECT TO C##USREXTERNO IDENTIFIED BY u$rext3rn0  
USING 'BASE_EXTERNA'; 
```

# 2

## a
```SQL
SELECT estado
FROM (
    SELECT cgc as c
    FROM (
        SELECT codigo AS c 
        FROM automoveis@carros
        WHERE modelo = 'AX'
        AND fabricante = 'Citroen'
    ), garagens@carros
    WHERE codigo = c
), revendedoras@carros
WHERE c = cgc
GROUP BY estado;
```

## b
```SQL
SELECT f AS fabricante, m AS modelo, a AS ano, p AS preco
FROM (
    SELECT f,m,a,p
    FROM (
        SELECT codigo AS c, fabricante AS f, modelo AS m, ano AS a, preco_tabela as p 
        FROM automoveis@carros
    ), garagens@carros
    WHERE codigo = c
    GROUP BY f,m,a,p
    ORDER BY p 
)
WHERE ROWNUM < 2;
```

## c
````SQL
SELECT cgc, proprietario
FROM (
    SELECT *
    FROM (
        SELECT  proprietario AS prop, COUNT(DISTINCT(estado)) AS cont
        FROM revendedoras@carros
        GROUP BY proprietario
        ORDER BY proprietario
    )
    WHERE cont > 1
), revendedoras@carros
WHERE proprietario = prop
ORDER BY proprietario;
```

## d
```SQL
SELECT fabricante, pais
FROM automoveis@carros
GROUP BY fabricante, pais
ORDER BY fabricante;
```

# 3
```SQL
SELECT cpf, nome, sobrenome
FROM consumidores@carros
WHERE cpf NOT
IN (
    SELECT  cpf
    FROM negocios@carros
);
```

```SQL
CREATE SYNONYM negocios
FOR negocios@carros;

SELECT cpf, nome, sobrenome
FROM consumidores@carros
WHERE cpf NOT
IN (
    SELECT  cpf
    FROM negocios
);
```
