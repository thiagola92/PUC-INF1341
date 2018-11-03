# Camadas
A idéia é separar em camadas para melhor organização, manutenção, flexibilidade, reusabilidade, escalabilidade, etc.  
É normal se separar em 3 camadas:  
- Interface com o usuário
- Lógica de negócio
- Dados

Dois tipos de particionamentos:  
- Particionamento físico
  - Dividir um sistema complexo em varias unidades físicas
- Particionamento lógico
  - Dividir um sistema em módulos, onde cada um tem um objetivo/funcionalidade

Normalmente quem faz o físico também faz o lógico.  

## Particionamento Lógico
Alguns motivo do particionamento lógico:  
- Adicionar características sem ter que reprojetar a aplicação.  
- Modificar lógica de negócio sem afetar os dados ou cliente.  
- Alterar o banco de dados sem afetar a lógica ou cliente.  

A idéia principal é poder alterar cada pate sem afetar a outra.  

### MVC
É a arquitetura de particionamento lógico mais popular atualmente, MVC (model-view-controller).  

- Modelo
  - É a camada responsável por armazenar os dados, valida-los e conferir se seguem as regras dos dados.  
    - Servidor de Banco de Dados.  
- Visão
  - É a camada que interage com o usuário, toda parte que o usuário consegue ver.  
    - Não tem lógica de funcionamento ou navegação.
    - Apenas gera eventos que interagem com o Controlador.  
- Controlador
  - É a camada responsável pela comunicaçaõ entre Modelo e Visão.
    - Passa para a Visão o que ela deve apresentar.
    - Faz pedido de dados ao Modelo.

Vantagens:  
Fácil de manter, testar, atualizar e incrementar o sistema.  

Desvantagens:  
Custa tempo aprender sobre todas as partes, custa tempo entender o relacionamento das camadas e não é aconselhável para ppequenas aplicações.  

28:30
