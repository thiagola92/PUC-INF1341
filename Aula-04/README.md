# Bloco de Comandos

```SQL
DECLARE
  idade       NUMBER := 50;
  altura      NUMBER;
  mes         NUMBER(5, 2);  -- 5 digits, 3 before decimal 2 after decimal
  letra       CHAR(1) := 'T';
  comentarios CHAR(50);
  aprovado    BOOLEAN;
  data        DATE;
  x           tabela.coluna%TYPE;
  y           tabela%ROWTYPE;
BEGIN
  altura := 2
  comentarios := 'texto qualquer'

  dbms_output.put_line('print na tela')
  dbms_output.put_line('idade ' || idade)
EXCEPTION
  -- exceptions
END;
```

Para o print aparecer, talvez seja preciso chamar `SET SERVEROUTPUT ON FORMAT WRAPPED;`

# IF THEN ELSE
```SQL
IF idade = 50 THEN
  -- comandos
ELSIF idade <= 49 THEN
  -- comandos
ELSE
  -- comandos
END IF;
```

# WHILE
```SQL
WHILE idade = 50 LOOP
  -- comandos
END LOOP;
```

# FOR
Começa em 1 e vai para 20.  
```SQL
FOR i IN 1..20 LOOP
  -- comandos
END LOOP;
```

Começa em 20 e vai para 1.  
```SQL
FOR i IN REVERSE 1..20 LOOP
  -- comandos
END LOOP;
```

`EXIT` sai doo loop.  
```SQL
FOR i IN 1..20 LOOP
  -- comandos
  EXIT
END LOOP;
```

# Cursor

20:23

# Stored Procedure

```SQL

```

# Trigger
