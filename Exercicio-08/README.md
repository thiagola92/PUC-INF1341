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
