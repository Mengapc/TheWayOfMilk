# Lactrópolis

## 👥 Equipe de Desenvolvimento – Weird Milk Studios

![weirdmilk_branco](https://github.com/user-attachments/assets/1a855ed0-f8e7-425f-867f-ce45523614ee)

---

<img width="300" height="400" alt="image" src="https://github.com/user-attachments/assets/f889c94c-69fc-4626-8a91-0f9a315e5a5b" />

### Hamud Michel 
**Função:** Artista 2D e 3D 

Desenvolveu os concepts do personagem e posters presentes no jogo.

---

<img width="300" height="300" alt="image" src="https://github.com/user-attachments/assets/8ace4551-27d4-4c31-a519-cbe3bcd78aac" />

### Gabriel Dias  
**Função:** Programador e Game Designer  

Responsável pelas mecânicas principais, build do jogo e Game Design/Level Design.

---

<img width="300" height="400" alt="image" src="https://github.com/user-attachments/assets/058b7ae2-d52b-4b41-a3ba-09b6021d14a4" />

### Gabriel Furlan Mengarelli                                                                                                                            
**Função:** Programador e Sonorizador  

Configurou e programou os efeitos da câmera, criou todas as músicas/sfx e a documentação do GitHub.

---

<img width="300" height="600" alt="image" src="https://github.com/user-attachments/assets/08ff0561-ac29-4d10-9b74-c562538fbe09" />

### Guilherme Alves  
**Função:** Artista 3D e VFX  

Criou todas as artes e modelos 3D utilizados no jogo.

---

**Estúdio:** Weird Milk Studios

👾 Bem-Vindo(a) ao Repositório do projeto **"Lactrópolis"**
Este repositório funciona como um **GDD(Game Desing Document)** vivo reunindo todas as informações, ideias, sistemas e anotações que fazem parte do desenvolvimento do nosso jogo.

Aqui você vai encontrar:

- 🛠️ **Guia de instalação e execução**
- 🗂️ **Estrutura de pastas e scripts principais**
- 🕹️ **Descrição do projeto**
- 🧠 **Conceitos de gameplay e narrativa**
- 🐛 **Roadmap e problemas conhecidos**
- 📚 **Tutoriais, links úteis e referências**

# 📦 Instalação e Configuração

## Requisitos

| Item | Versão |
|------|--------|
| Unity Editor | 6000.0.35f1 |
| Render Pipeline | Universal Render Pipeline (URP) |
| Git | 2.47.1 |
| Git LFS | Instalado (`git lfs install`) |
| DOTWEEN | 1.2.765 |
| CineMachine | 3.1.4 |

### Passo a passo

1. Clone o repositório:
   ```
   git clone https://github.com/Mengapc/TheWayOfMilk.git
   cd TheWayOfMilk
   git lfs install
   git lfs pull
   ```
2. Abra o projeto no Unity (6000.0.35f1)

3. Cena inicial:

       · Assets/Scenes/Menu.unity

4. Recomendado: build manual via File > Build Settings

# Documentação - Links, tutoriais, etc.

 ## 💻️ Comandos do git

### 🔹 Inicializar repositório
 ```
 git init
 ```
### 🔹 Adicionar arquivos para commit
 ```
 git add .
 ```
### 🔹 Fazer commit com mensagem
 ```
 git commit -m "Nome_commit"
 ```
### 🔹 Enviar mudanças para o repositório remoto (main)
 ```
 git push -u origin main
 ```
### Após a primeira vez, você pode usar apenas:
 ```
 git push
 ```
### 🔹 Atualizar sua branch com as mudanças da main (do remoto)
 ```
 git pull origin main
 ```
### Após a primeira vez, você pode usar apenas:
 ```
 git pull
 ```
### 🔹 Trocar de branch
 ```
 git checkout <nome_da_branch>
 ```
 ### 🔹 Junta as mudanças da sua branch na main
 ```
 git merge <nome_da_branch>
 ```
Exemplo: Se estiver na main e rodar git merge minha-branch, as mudanças da minha-branch serão aplicadas na main.

🧹 Remover arquivos já versionados e ignorá-los corretamente
Se você adicionou pastas como Logs ou UserSettings no .gitignore depois que elas já estavam sendo versionadas, siga o passo-a-passo abaixo para limpar o repositório:

### 🔹 1. Remover os arquivos do versionamento, mas manter no seu computador
 ```
git rm --cached -r BaseProject/Logs/
git rm --cached -r BaseProject/UserSettings/
 ```
### 🔹 2. Fazer commit da remoção
 ```
git commit -m "Removendo arquivos de Logs e UserSettings do versionamento"
 ```
### 🔹 3. Enviar as mudanças para o repositório remoto
 ```
git push
 ```
### 🔹 4. Limpar arquivos não monitorados do seu diretório local
Use este comando para remover arquivos não rastreados que ainda estão no seu computador, evitando conflitos futuros ao mudar de branch.
 ```
git clean -fd
 ```

# 📜 Organização do Código

## Scripts principais

| Script                       | Função                                           |
| ---------------------------- | ------------------------------------------------ |
| MenuManager.cs | Gerencia os efeitos e fluxo das interações com o menu.         |
| Elevator.cs  | GABRIEL, TEM QUE COLOCAR UMA DESCRIÇÃO |
| MoveBox.cs             | GABRIEL, TEM QUE COLOCAR UMA DESCRIÇÃO  |
| MovementScale.cs               | GABRIEL, TEM QUE COLOCAR UMA DESCRIÇÃO                 |
| TubesPuzzleManager.cs           | GABRIEL, TEM QUE COLOCAR UMA DESCRIÇÃO              |

### Lógicas aplicadas
### TEMOS QUE ATUALIZAR ESSA PORRA AQUI

· Armas montadas via Struct com 9 arrays:

      · Tipo (espada, cajado, martelo)
      · Gema (vento, água, galáxia)
      · Prefabs específicos de ataque (Ex: Martelo_Vento)

· Sistema de mapa procedural:

    · Armazena os tiles instanciados num Dictionary<Vector3, GameObject>
    · Usa Clear() ao fim da fase para resetar o mapa

· Sistema de habilidades:

    · SkillsManager armazena um dicionário de funções por tipo de arma
    · Ativação dinâmica conforme ataque executado

# 🎮 Build & Execução

## Plataformas

### 🖥️ Windows (.exe)

### 🌐 WebGL (previsto para Itch.io)

## Passos para build

1. File > Build Settings

2. Selecione a plataforma desejada

3. Clique em Add Open Scenes e inclua testeprocedura.unity

4. Configure se quiser em Player Settings

5. Clique em Build

_Não há configurações personalizadas no Player Settings._

## Introdução ao mundo

No início dos anos 40, a empresa hoje conhecida como LactoNuke, mas que na época se chamava TupperWare, focava em desenvolver materiais sintéticos. Seus cientistas, em busca de um plástico mais resistente, voltaram sua atenção para a caseína, a principal proteína do leite, conhecida por sua maleabilidade e resistência natural. A equipe de pesquisa estava tentando entender sua estrutura molecular em um nível atômico. Para isso, eles usaram a Cristalografia por Difração de Elétrons, uma técnica de ponta da época. A ideia era criar cristais de caseína e bombardeá-los com um feixe de elétrons para mapear sua estrutura. No entanto, durante um dos testes, algo inesperado aconteceu. O feixe de elétrons, ao atingir o cristal, não o difratou como esperado. Em vez disso, a estrutura atômica da caseína colapsou em um instante, rearranjando-se em uma nova e instável configuração. Um brilho esverdeado, como o de uma lâmpada fluorescente quebrando, preencheu a câmara de vácuo, seguido por uma onda de calor que derreteu parte do equipamento. O pesquisador, chocado e fascinado, conseguiu isolar essa nova forma de caseína, apelidando-a de 'Caseína-235' - uma alusão sombria ao Urânio-235. Ele descobriu que essa nova substância, ao ser ativada por uma carga elétrica mínima, liberava uma explosão de energia atômica, abrindo um caminho destrutivo e inimaginável. A partir desse momento, a TupperWare deixou de fazer potes e passou a construir um novo e terrível arsenal, selando o destino do mundo e das vacas para sempre.

## Arny Longsing

Dezessete anos após a descoberta que mudou o mundo, o pesquisador Arny Longsing emergiu de seu esconderijo. Um homem que cresceu em uma fazenda, Arny sempre teve uma conexão profunda com as vacas. Quando um incêndio na plantação de trigo do vizinho tirou a vida de seu pai, ele precisou amadurecer rápido para ajudar sua mãe a cuidar da fazenda da família. Enquanto aprendia a arar a terra e alimentar os animais, sua paixão pelas vacas cresceu, transformando-o em um estudioso dedicado.

Em 1942, sua expertise chamou a atenção da TupperWare. A empresa, famosa por seus potes, fez uma proposta que prometia mudar o futuro. Eles queriam que Arny usasse seu conhecimento para aumentar drasticamente a produção de leite, que seria transformado no que eles chamaram de 'leite nuclear'. Furioso, Arny viu a proposta como um ciclo de crueldade e exploração. Ele expressou seu repúdio pela ideia, sendo rapidamente expulso da sede e juramentado a nunca revelar o projeto.

Esse encontro assustador foi o ponto de virada. Arny se isolou em sua fazenda, transformando-a em um esconderijo para reunir o máximo de informações sobre as vacas e mapear os problemas que a TupperWare (agora LactoNuke) estava causando. Ele documentou o desaparecimento das populações de bovinos, a ascensão da Lactrópolis - uma sociedade construída sobre o dinheiro do leite nuclear - e os eventos catastróficos que se seguiam. Por anos, ele tentou avisar amigos e conhecidos com poder, mas ninguém se importava com suas informações. Agora, com a raça à beira da extinção, Arny escutou um rumor que reacendeu sua esperança: a lenda da vaca Xuxa, a última de sua espécie, protegida em uma fazenda-labirinto por um fazendeiro com enigmas indecifráveis. Arny decide então se aventurar, com o objetivo de encontrar a lendária vaca e, talvez, descobrir uma forma de salvar a espécie da aniquilação total.

# 🚧 Roadmap & Problemas Conhecidos

## Funcionalidades

| Status | Item                | Descrição                                    |
| ------ | ------------------- | -------------------------------------------- |
| ✅      | Altar de Sacrifício | Mecânica principal que ajusta a dificuldade. |
| ✅      | Geração Procedural  | Mapas e armas com combinações únicas.        |
| ⚙️     | Mecânicas de Armas  | Algumas feitas, outras planejadas.           |
| 🔜     | Loja 2x1            | Troca de dois itens por um melhor.           |
| 🔜     | IA Variada          | Novos padrões de inimigos.                   |
| 🔜     | Bossfights          | Temáticos, como “medo do escuro”.            |
| 🔜     | Estéticas de Fase   | Ex: fase com visibilidade reduzida.          |

## Bugs Conhecidos

| Área      | Problema            | Descrição                         |
| --------- | ------------------- | --------------------------------- |
| Knockback | Força instável      | Pode jogar inimigos fora do mapa. |
| Player    | Rotação na morte    | Personagem revive deitado.        |
| Dash      | Player fora do mapa | Há respawn, mas precisa melhoria. |

## Organização

· Código-fonte: GitHub

![25231](https://github.com/user-attachments/assets/404d27a5-61c0-4625-bbff-45b3bf03d08c)

· Tarefas: Hack n Plan

![images](https://github.com/user-attachments/assets/cb43d901-921f-4dbd-a7a6-b34029d296b2)

## 📚️📚️ Links de Auxílio

 | Aulas | Resumos |
 |------|---------|
 | Git LFS em Unity | - [Link](https://www.youtube.com/watch?v=_ewoEQFEURg) |
 | Arquivos base git | - [Link](https://www.patreon.com/posts/63076977) |
 | Unity - Collaborating with version control| - [Link](https://learn.unity.com/tutorial/collaborate-with-plastic-scm#631f4f5dedbc2a27152629c3) |

 - [Digital Innovation One](https://web.dio.me/home).https://learn.unity.com/tutorial/collaborate-with-plastic-scm#631f4f5dedbc2a27152629c3
 - [Documentação Git](https://git-scm.com/doc)
 - [Documentação GitHub](https://docs.github.com/)
 - [Github Material de Apoio](https://github.com/elidianaandrade/dio-curso-git-github)
 - [Apresentação Versionamento de Código](https://academiapme-my.sharepoint.com/:p:/g/personal/renato_dio_me/EYjkgVZuUv5HsVgJUEPv1_oB_QWs8MFBY_PBQ2UAtLqucg?rtime=FOF68ttW3Ug)

### 🐛🐛 Resolução de bugs


 ## 🎬️🎬️ Vídeos de Auxílio
 
 ### 1 - [How To Add Sound Effects the RIGHT Way | Unity Tutorial](https://www.youtube.com/watch?v=DU7cgVsU2rM&t=87s)
 
[![Watch the video](https://i.sstatic.net/Vp2cE.png)](https://www.youtube.com/watch?v=DU7cgVsU2rM&t=87s)
 
 ### 2 - [CineMachine](https://www.youtube.com/watch?v=wB-EQH7jvFY)
 
[![Watch the video](https://i.sstatic.net/Vp2cE.png)](https://www.youtube.com/watch?v=wB-EQH7jvFY)


 
 ## 🔎 Inspirações e Referências

 | Jogos | Inspirou | Link |
 | ----------------- | ------------------- | ------------------------------------------------------------------------------ |
 |   Série Fallout   |       Universo      | - [Link](https://store.steampowered.com/bundle/3782/Fallout_Franchise_Bundle/) |
 | Little Nightmares | 2D com Profundidade | - [Link](https://store.steampowered.com/app/424840/Little_Nightmares/ )        |

![fallout-resume](https://github.com/user-attachments/assets/b6e6eddd-2631-4c4e-8358-3783d461b3b0)
![littlenightmares](https://github.com/user-attachments/assets/46bd2138-b2b2-4160-b49f-3a192773f952)
