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
SELECT cgc, p, e
FROM (
    SELECT p, e
    FROM (
        SELECT proprietario AS p, estado AS e
        FROM revendedoras@carros
        ORDER BY p
    )
    GROUP BY p, e
    ORDER BY p
), revendedoras@carros
WHERE proprietario = p;
```
