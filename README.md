# AIGameRemaster

Programe um Robo herdando de Player e coloque-o na arena para batalhar.

Todas as operações gastam energia e alimentos aumentam a geração desse recurso além de vida.

Use sensores para perceber ao redor e atire bombas para destruir inimigos.

Sensores disponíveis:

Sonar forte:
    - Conta a quantidade de entidades (players e alimentos) ao redor
    - 400 pixeis de raio

Sonar acurado:
    - Registra a posição exata de entidades (players e alimentos) ao redor
    - 200 pixeis de raio

Infravermelho:
    - Registra a posição e o tipo exato de entidade (players e alimentos) em uma linha reta
    - Distância praticamente infinita

Sensor de dano:
    - Aponta a posição da última bomba que atingiu o player
    - Não gasta energia