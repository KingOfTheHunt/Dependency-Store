﻿# Dependency Store

Dependency Store é um projeto que desenvolvi no curso [Dominando Injeção de Dependência](https://balta.io/carreiras/advanced-dotnet-developer) do balta.io com o objetivo de aprender sobre injeção de dependência no C#.


## Aprendizados

Aprendi a diferença entre Injeção de Dependência, Inversão de Controle e Princípio da Injeção de Dependência. A injeção de dependência e o princípio da injeção de dependência são princípios e a injeção de dependência é a técnica que utilizamos para aplicar esses princípios.

Existem algumas formas de injeção de dependência, e a mais conhecida é a injeção via construtor. Outra forma de injeção de dependência é via método.

Com a injeção de dependência a classe só precisa do que ela precisa para funcionar e não de quem vai prover. E aplicando o DIP (Dependency Injection Principle ou Princípio da Injeção de Dependência) conseguimos criar sistemas que contém baixo acoplamento, que são fáceis de testar, fáceis de entender e fáceis de dar manutenção. Mas por quê o DIP ajuda criar esse tipo de sistema? O motivo é simples, pois ele foca em depender da abstração não da implementação.

Aprendi sobre o registro de uma dependência e o seu ciclo de vida dentro do .NET. Existem três ciclos: **AddTransient**, **AddScoped** e **AddSingleton**.
- **AddTransient**: Sempre vai criar uma nova instância do objeto.
- **AddScoped**: Cria um objeto por requisição. Caso 2 serviços ou mais que dependam do mesmo objeto sejam chamados, a mesma instância será utilizada para eles.
- **AddSingleton**: Cria uma instância assim que a aplicação inicia. E sempre vai usar a mesma instância até que a aplicação seja encerrada ou reiniciada. 
