# 1

## a
Quando se utiliza diversos bancos de dados, ligados entre si por uma rede, para armazenar os dados.  

## b
Quando os bancos de dados distribuidos são diferentes, um pode ser oracle, outro mysql, outro mongodb, etc.  

## c
Fragmentação Vertical  
Fragmentação Horizontal  
Replicação  

## d
Ferramentas de porte pequeno tem aumentado o desempenho e diminuido o custo, pode estar valendo mais ter várias ferramentas que cuidam de cada nó do que uma ferramenta responsável por todos.  
Se maioria dos problemas podem ser resolvidos em nós pequenos, diminui o custo de comunicação pois você não precisa se comunicar com outros nós.  

## e
Aumenta a segurança pois você pode fazer replicações entre os nós.  
Podem aumentar a qualidade atráves de fragmentações e replicões, deixando dados mais próximos de quem utiliza.  

# 2
Cada nó tem que ter autonomia sobre seus dados    
Cada nó deve não ser dependente do outro fisicamente     
A localização de cada nó não deve afetar como o banco é implementado  
O usuário deve ver o banco como se fosse um só  
Cada nó deve funcionar sobre seus dados sem precisar pedir permissão dos outros  
O SGBDD deve otimizar os caminhos dos acessos aos dados  
Reestruturação lógica da ferramenta não deve impactar o banco, então o SGBDD deve fornecer suporte para restruturar de forma a minimizar os danos  
Prover segurança de acesso sobre os bancos e dados  

# 3
**Opções de Distribuição**:  
Fragmentação Horizontal  
Fragmentação Vertical  
Replicação  

**Opções de Atualizações**:  
Assíncrona Mestre-Escravo  
Síncrona Mestre-Escravo  
Assíncrona Grupo  
Síncrona Grupo  
