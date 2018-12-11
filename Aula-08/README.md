**Top-down**: Projetando um sistema novo, geralmente é um sistema homogêneo pois você quer deixar todos os bancos padronizados  
**Bottom-up**: Trabalhando com bancos de dados que já existem em diversos locais, você tem que projetar a união deles  

# Distribuição dos dados
Maneira de distribuir dados de forma que traga algum benefício.  

* A localidade do dado influência muito na velocidade para obter-lo, um banco de dados perto de você é mais rápido para receber e responder um pedido de query.  
  * Fragmentação horizontal
* Um banco de dados pode sofrer danos ou ser destruido, uma cópia dele pode sempre ajudar a restaurar os dados.  
  * Replicação  
* Pessoas com acesso a um banco de dados A podem ser bloquiadas de ver toda a informação de alguém, por exemplo o salário dela. Melhor forma de impedir é não botando as informações naquela banco de dados A.  
  * Fragmentação vertical  

## Fragmentação Horizontal
Suponha que na sua empresa tenha a seguinte tabela de funcionários  

| Nome | Regiao |
| ---  | ------ |
| Bob  | RJ     |
| Sandy | RJ    |
| Matheus | RJ  |
| Billy | SP    |
| Miguel | SP   |
| Nunes | SP    |
| Thiago | BA   |
| Leo   | BA    |

E que sua empresa tenha um banco de dados em cada uma das regiões  

Banco de dados RJ  
Banco de dados SP  
Banco de dados BA  

Supondo que  
informações dos funcionários de RJ sejam geralmente acessadas por pessoas do RJ  
informações dos funcionários de SP sejam geralmente acessadas por pessoas do SP  
informações dos funcionários de BA sejam geralmente acessadas por pessoas do BA  

A melhor distribuição seria fazer a **fragmentação horizontal** da tabela, de forma a deixar os dados nos locais mais usados  

Banco de dados RJ  

| Nome | Regiao |
| ---  | ------ |
| Bob  | RJ     |
| Sandy | RJ    |
| Matheus | RJ  |

Banco de dados SP

| Nome | Regiao |
| ---  | ------ |
| Billy | SP    |
| Miguel | SP   |
| Nunes | SP    |

Banco de dados BA

| Nome | Regiao |
| ---  | ------ |
| Thiago | BA   |
| Leo   | BA    |

## Replicação
Suponha que na sua empresa os funcionários estejam separados em 3 banco de dados  

Banco de dados RJ  

| Nome | Regiao |
| ---  | ------ |
| Bob  | RJ     |
| Sandy | RJ    |
| Matheus | RJ  |

Banco de dados SP

| Nome | Regiao |
| ---  | ------ |
| Billy | SP    |
| Miguel | SP   |
| Nunes | SP    |

Banco de dados BA

| Nome | Regiao |
| ---  | ------ |
| Thiago | BA   |
| Leo   | BA    |

Para garantir a segurança dos dados você replica eles em outro banco de dados  

Banco de dados RJ  

| Nome | Regiao |
| ---  | ------ |
| Bob  | RJ     |
| Sandy | RJ    |
| Matheus | RJ  |
| Thiago | BA   |
| Leo   | BA    |

Banco de dados SP

| Nome | Regiao |
| ---  | ------ |
| Billy | SP    |
| Miguel | SP   |
| Nunes | SP    |
| Bob  | RJ     |
| Sandy | RJ    |
| Matheus | RJ  |

Banco de dados BA

| Nome | Regiao |
| ---  | ------ |
| Thiago | BA   |
| Leo   | BA    |
| Billy | SP    |
| Miguel | SP   |
| Nunes | SP    |

Agora se o do RJ quebrar, você pode recuperar informações do RJ olhando para o de SP  
Se o de SP quebrar, você pode recuperar informações de SP olhando para o da BA  
Se o da BA quebrar, você pode recuperar informações da BA olhando para o do RJ    

**Replicação**: Pode ser snapshot/visão materializada/on-line/...  

## Fragmentação Vertical
Suponha que na sua empresa tenha a seguinte tabela de funcionários  

| Nome | Regiao | Salário |
| ---  | ------ | ---- |
| Bob  | RJ     | 1200 |
| Sandy | RJ    | 1400 |
| Matheus | RJ  | 1350 |
| Billy | SP    | 1370 |
| Miguel | SP   | 1040 |
| Nunes | SP    | 1840 |
| Thiago | BA   | 1084 |
| Leo   | BA    | 1155 |

Pessoas do RH não precisam saber o salário então a melhor maneira de proteger essa informação é com **fragmentação vertical**  

Banco de dados para o RH  

| Nome | Regiao |
| ---  | ------ |
| Bob  | RJ     |
| Sandy | RJ    |
| Matheus | RJ  |
| Billy | SP    |
| Miguel | SP   |
| Nunes | SP    |
| Thiago | BA   |
| Leo   | BA    |

Banco de dados para os donos da empresa  

| Nome | Regiao | Salário |
| ---  | ------ | ---- |
| Bob  | RJ     | 1200 |
| Sandy | RJ    | 1400 |
| Matheus | RJ  | 1350 |
| Billy | SP    | 1370 |
| Miguel | SP   | 1040 |
| Nunes | SP    | 1840 |
| Thiago | BA   | 1084 |
| Leo   | BA    | 1155 |

## Extra
Good: Uma tabela dividida entre vários bancos, quer dizer que você pode executar uma busca na tabela do banco X e outro no banco Y. Trazendo uma espécie de paralelismo nessa tabela.  
Bad: Se você necessita acessar dados que estão distribuidos entre as tabelas, você gasta processamento de vários bancos.  
