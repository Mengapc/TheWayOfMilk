# 👾 Lactrópolis

<img width="612" alt="Slide1" src="https://github.com/user-attachments/assets/16c57659-ecfd-45b9-8669-f401d385cc65" />

**Estúdio:** Weird Milk Studios

<img width="400" alt="weirdmilk_branco" src="https://github.com/user-attachments/assets/751be303-41ba-4c02-8c86-1d411036379c" />


Este repositório funciona como um **GDD (Game Design Document)** vivo para o protótipo de "Lactrópolis", reunindo todas as informações, sistemas e decisões que formam o nosso jogo.

---

## 👥 Equipe de Desenvolvimento – Weird Milk Studios

<img width="300" height="400" alt="image" src="https://github.com/user-attachments/assets/f889c94c-69fc-4626-8a91-0f9a315e5a5b" />

### Hamud Michel 
**Função:** Artista 2D e UI/UX

*Desenvolveu os concepts do personagem e posters presentes no jogo.*

---

<img width="300" height="300" alt="image" src="https://github.com/user-attachments/assets/8ace4551-27d4-4c31-a519-cbe3bcd78aac" />

### Gabriel Dias  
**Função:** Programador e Game Designer

*Responsável pelas mecânicas principais, build do jogo e Game Design/Level Design.*

---

<img width="300" height="400" alt="image" src="https://github.com/user-attachments/assets/058b7ae2-d52b-4b41-a3ba-09b6021d14a4" />

### Gabriel Furlan Mengarelli                                                                                                                    
**Função:** Programador e Sonorizador

*Configurou e programou os efeitos da câmera, criou todas as músicas/sfx e a documentação.*

---

<img width="300" height="600" alt="image" src="https://github.com/user-attachments/assets/08ff0561-ac29-4d10-9b74-c562538fbe09" />

### Guilherme Alves  
**Função:** Artista 3D e VFX

*Criou todas as artes, modelos 3D e efeitos visuais utilizados no jogo.*

---

## 🐄 Sobre o Jogo (O Jogo Completo)

💥 **O Desastre Lácteo Começou!** 💥

Mergulhe em Lactrópolis, uma aventura de **Puzzle/Plataforma 3D** que vai testar sua mira e seu senso de humor. Prepare-se para guiar Arny Longsing, o último defensor dos bovinos, em sua missão mais urgente: desvendar o mistério da Caseína-235 e encontrar a última sobrevivente, a Vaca Xuxa, antes que a LactoNuke transforme o mundo em um laticínio radioativo!

### A História: Leite, Ciência e Destino Atômico

Anos 40: a TupperWare (sim, a dos potes) tropeça em uma descoberta sinistra. Ao tentar criar um plástico super-resistente, seus cientistas transformam a caseína, a principal proteína do leite, na **Caseína-235**, um explosivo atômico instável.

Dezessete anos depois, a TupperWare virou a tirana **LactoNuke**, e o leite é a nova fonte de poder mundial. As vacas estão desaparecendo. É nesse cenário distópico e absurdo que entra **Arny Longsing**, um pesquisador recluso e profundo estudioso bovino, que jurou deter a LactoNuke.

Seu objetivo: encontrar a **Vaca Xuxa**, a última de sua espécie, que estaria protegida em uma fazenda-labirinto repleta de enigmas... e armadilhas.

### 🕹️ A Jogabilidade: Mente, Mira e Leite Explosivo

Em Lactrópolis, você navegará por salas de câmera **Side-Scroller** com uma perspectiva visual que brinca com a profundidade (2.5D). Cada sala é um enigma, e a única chave para a porta de saída é o raciocínio rápido e a precisão do seu arremesso:

* **Arremesso Preciso:** Sua principal ferramenta é o **jarro de leite**. Use-o para acertar alvos, ativar ou desativar **pedestais de energia** e manipular o ambiente.
* **Quebra-Cabeças de Fluxo:** Solucione problemas de timing, física e lógica para abrir a próxima porta. Aventure-se por um bunker, onde cada sala esconde uma peça do quebra-cabeça que pode salvar (ou explodir) o futuro das vacas.

---

## 🧠 Design, Arte e Narrativa

<a href="https://www.youtube.com/watch?v=ELZIf4M19T8">
  Vídeo de Relatório<br>
  <img src="https://img.youtube.com/vi/ELZIf4M19T8/maxresdefault.jpg" width="300">
</a>

Esta seção detalha as **decisões de design** tomadas durante o desenvolvimento, explicando como integramos tema, mecânicas, arte e som.

### 1. Narrativa e Tema
*A premissa de "Lactrópolis" nasceu de uma sátira ao "Atompunk" (popularizado por **Fallout**) e à cultura de consumo. A decisão de usar a "TupperWare" como vilã inicial foi para criar um contraste cômico imediato entre o banal (potes de plástico) e o apocalíptico (energia nuclear). A narrativa é contada visualmente através dos pôsteres de propaganda da LactoNuke e pela própria progressão do jogador, que explora um mundo onde algo tão comum quanto o leite se tornou uma arma de destruição.*

### 2. Arte e Estética
*Na direção de arte queriamos um "Retrofuturismo Cômico". Enquanto a inspiração em **Fallout** dita a paleta de cores (tons pastéis, verdes nucleares) e a estética dos anos 50, a inspiração em **Little Nightmares** ditou a nossa decisão de câmera. Escolhemos uma câmera 2.5D fixa (Side-Scroller com profundidade) para: 1) Criar uma sensação claustrofóbica de "bunker" e 2) Simplificar o design dos puzzles, focando a mira do jogador em um plano mais controlado, mas ainda permitindo a exploração de profundidade.*

*Durante o desenvolvimento dos assets que seriam usados no jogo, nós queriamos manter a estética Atompunk que tinhamos imaginado mas também trazer um lado comico que a história do jogo tem, então decidimos usar materiais simples, combinados com um shader de outline. Então com isso conseguimos manter a estetica com os modelos e paletas seguindo a estética Atompunk, e com o outline conseguimos dar um visual de cartoon para o jogo, reforçando o tom cômico que tem no jogo.*

### 3. Mecânicas e Códigos
*A principal decisão de design foi limitar a interação do jogador a **uma única mecânica: o arremesso**. Isso nos forçou a criar puzzles diversos usando apenas esta ferramenta. O jarro de leite não é apenas uma "chave", ele é a forma de ativar pedestais, mover plataformas e (em puzzles futuros) quebrar objetos. Tecnicamente, isso foi centralizado no script `ObjectGrabbing.cs`, que gerencia o estado do jogador (livre, segurando, mirando), e no script `Activate.cs`, que usa `UnityEvents` para permitir que o Level Designer conecte o pedestal a qualquer outro objeto (portas, elevadores, etc.) sem precisar escrever código novo.*

*Com essa mecânica de 'arremesso' como ferramenta única, nossa filosofia de Level Design foi focada na **complexidade crescente**. Em vez de dar novas habilidades ao jogador, cada sala introduz uma **nova variável de puzzle** (como pedestais de timing, o elevador, ou alvos múltiplos). As salas iniciais ensinam a mecânica de forma isolada, e a sala final age como um 'exame', exigindo que o jogador **combine todos os sistemas** que aprendeu para resolver um quebra-cabeça maior.*

### 4. Som e Música
*A sonorização foi desenhada para reforçar o tom. A música ambiente mistura suspense com um toque "industrial" e burocrático, refletindo a LactoNuke. Os efeitos sonoros (SFX) são exagerados de propósito: o som de "pegar" o galão, o "blip" dos diálogos (controlado pelo `RandomLoopingSpeaker.cs`), e os sons de ativação dos pedestais são todos desenhados para dar um feedback claro e satisfatório ao jogador, informando que sua ação teve um resultado imediato.*

*Para acentuar o tom cômico, os efeitos sonoros dos personagens (Arny e a Vaca) foram criados internamente. Nosso sonorizador gravou as próprias imitações de voz, que foram então digitalmente alteradas em *pitch* e *timbre*. Essa abordagem intencionalmente 'caseira' (DIY) reforça a sátira do jogo e transforma até as interações mais básicas, como os passos de Arny ou o esforço do arremesso, em momentos de humor.*

---

## 🎨Concepts, Artes e Animações

### 1. Concept Rosto Arny:
<img width="400" alt="pers_Concept_Face (1)" src="https://github.com/user-attachments/assets/176f23c1-77a8-4650-82af-67b5f7d1d9d6" />

### 2. Concept Geral Arny:
<img width="400" alt="Pers_Concept_Body tank" src="https://github.com/user-attachments/assets/fcfbdb0d-a1ff-43d4-bdca-b08ff490ccb4" />

### 3. Modelo Final Arny Logsing:
<img width="400" alt="Pose 1" src="https://github.com/user-attachments/assets/c8fb19a2-ea3b-4d59-bcd2-2a4343a3101a" />

### 4. Botton Vaca:
<img width="400" alt="Concept_BottonVacaB" src="https://github.com/user-attachments/assets/5f58ca8d-946c-4aca-b26d-de03f837285b" />

### 5. Cenario 1:
<img width="450" alt="Bunker1" src="https://github.com/user-attachments/assets/5197f41d-064b-42f6-aa5b-65f8dec680df" />

### 6. Cenario 2:
<img width="450" alt="Bunker2" src="https://github.com/user-attachments/assets/34d17a77-1eca-42fa-bd50-124c830a9a24" />

### 7. Cenario 3:
<img width="450" alt="Bunker3" src="https://github.com/user-attachments/assets/240816a7-d270-431b-aa39-a10e6035da1f" />

### 8. Elemento de Cenario:
<img width="250" alt="GranadasArtBible" src="https://github.com/user-attachments/assets/6160bfd0-2269-4043-b999-bbc2ce9bd4b5" />

### 9. Item Principal:
<img width="250" alt="LitroLeiteArtBible" src="https://github.com/user-attachments/assets/f71535cc-92e3-46af-892d-6d4657ea0a34" />

### 10. Caixa de Dialogo Xuxa:
<img width="400" alt="CaixaDialogo_baseB" src="https://github.com/user-attachments/assets/f7b352ad-3462-4a84-8be6-4e7850b5fd8d" />

### 11. Idle:
https://github.com/user-attachments/assets/fc97ab63-659e-4d35-90ee-c1582371ed06

### 12. Walk:
https://github.com/user-attachments/assets/50a0ee85-c379-4063-81ea-7fc5f230c55d

### 13. Canalizando:
https://github.com/user-attachments/assets/67dfc2ed-8ea7-4336-a2fd-2b06e3a31d0e

### 14. Arremesso:
https://github.com/user-attachments/assets/024e5819-3e30-41b4-b312-78ad7387b06b

### 15. Coletando:
https://github.com/user-attachments/assets/a7416547-009e-4c57-a2fc-c2228ae35e6a






---

## 📦 Instalação e Configuração

### Requisitos

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
   ```bash
   git clone [https://github.com/Mengapc/TheWayOfMilk.git](https://github.com/Mengapc/TheWayOfMilk.git)
   cd TheWayOfMilk
   git lfs install
   git lfs pull

2. Abra o projeto no Unity (6000.0.35f1)

3. Cena inicial:

       · Assets/Scenes/Menu.unity

---

## 🗂️ Estrutura de Pastas (Assets)

O projeto está organizado na Unity com a seguinte estrutura de pastas, facilitando a localização de assets e scripts.

Assets/

├── _Project/

│   ├── _Audio/

│   │   ├── Ambience/               ← Áudios de ambiente

│   │   ├── Music/                  ← Trilhas musicais

│   │   └── SFX/                    ← Efeitos sonoros

│   │       ├── Cenário/

│   │       ├── Física/

│   │       ├── Interface/

│   │       ├── Objetos/

│   │       └── Personagens/

│   ├── Material/

│   │   ├── Arny/

│   │   ├── Assets2D/

│   │   ├── Decoracao/

│   │   ├── LitroLeite/

│   │   ├── Porta/

│   │   ├── Sala1/

│   │   ├── Sala2/

│   │   └── Sala3/

│   ├── Prefabs/

│   │   ├── Packd's/

│   │   └── Prefebs_Prontos/

│   │       ├── Componentes_Salas/

│   │       ├── Controles/

│   │       ├── SalasCompletas/

│   │       └── UI/

│   ├── Scenes/

│   │   ├── Sala1/

│   │   ├── Sala2/

│   │   └── Sala3/

│   ├── Scripts/

│   │   ├── Audio/                 ← Controle e gerenciamento de áudio

│   │   ├── Effects/               ← Scripts de efeitos

│   │   ├── Elevador.Portas/       ← Mecânica de portas do elevador

│   │   ├── GeralPuzzles/          ← Scripts gerais de puzzles

│   │   ├── Player/                ← Controle do player

│   │   └── UI/                    ← Scripts de interface

│   └── Settings/                  ← Configurações do projeto

---

## 📜 Organização do Código (Mecânicas e Códigos)

Esta é a documentação completa de todos os scripts principais do projeto, explicando sua função.

### Scripts do Jogador e Interação

| Script | Função |
| :--- | :--- |
| **Movement.cs** | Controla o movimento (`CharacterController`), rotação, gravidade e passos do jogador. Também gerencia a interação com o `Elevator` e permite o *override* de rotação pela mira. |
| **PlayerAnimationController.cs** | Gerencia o Animator, recebendo comandos de outros scripts (como `Movement` e `ObjectGrabbing`) para ativar as animações corretas (andar, segurar, arremessar). |
| **Direction.cs** | Calcula o vetor de direção 3D (no chão) do jogador até a posição do mouse, usando `Plane.Raycast`. Este vetor é usado para a mira do arremesso. |
| **ObjectGrabbing.cs** | **(Script Central)** Gerencia a mecânica de "agarrar e arremessar". Controla a detecção (trigger), o "pegar", o "carregar" (com força variável) e o "arremesso" (física) do jarro de leite. |
| **AimIndicator.cs** | Controla a visibilidade e rotação do indicador visual (seta) de mira, ativando-o apenas durante o carregamento do arremesso (`IsCharging`). |
| **BallController.cs** | Script do galão de leite (a "Ball"). Aplica gravidade customizada, toca som de impacto (`hitSound`) e instancia o efeito 'LeiteQuebrado'. |
| **RespawnLeite.cs** | Garante que sempre exista um galão de leite. Instancia um novo prefab de leite (`leitePrefab`) em um `pontoRespanw` se o `leiteAtivo` for destruído ou nulo. |

### Scripts de Puzzles e Nível

| Script | Função |
| :--- | :--- |
| **Activate.cs** | Script do pedestal de ativação. Captura o `BallController` (galão), anima sua posição/rotação até um ponto e, ao final, invoca um `UnityEvent` (ex: para abrir uma porta). |
| **Elevator.cs** | Gerencia toda a sequência do elevador. Controla o movimento da cabine (usando `Lerp` e `AnimationCurve`), troca de câmeras (Cinemachine), trava/libera o jogador e coordena as portas e áudio. |
| **ElevatorCollider.cs** | Script de Trigger que detecta a entrada/saída do jogador. Informa ao script `Movement` do jogador qual elevador está "ativo" para interação. |
| **OpenCloseDoor.cs** | Anima a abertura e fechamento de portas usando `SkinnedMeshRenderer` (Blend Shapes). Gerencia a troca de colisores e os efeitos sonoros de abertura/fechamento. |
| **DoorController.cs** | Controla a animação da porta final. Executa uma sequência de 'destrava' (Blend Shape) e 'abertura' (Rotação com `AnimationCurve`). |
| **FloorBallsFX.cs** | Script de trigger (no chão ou armadilhas). Detecta o galão de leite ('Ball'), toca um som de impacto, chama o `InstantiateEfect` do galão e o destrói. |

### Scripts de Sistema e Carregamento

| Script | Função |
| :--- | :--- |
| **SceneLoader.cs** | Classe estática para gerenciamento de cenas. Armazena a 'próxima cena' a ser carregada e chama a 'TelaCarregamento'. |
| **LoadingScreenController.cs**| Controla a cena 'TelaCarregamento'. Carrega a cena alvo (definida pelo `SceneLoader`) de forma assíncrona, exibindo uma barra de progresso com tempo de espera simulado. |
| **SwithScene.cs** | Script de Trigger (colisor) que, ao ser tocado pelo jogador, chama o `SceneLoader` para carregar a próxima cena (fase). |
| **ExitGame.cs** | Script de utilidade (provavelmente em um botão) que executa `Application.Quit()` para fechar o jogo. |

### Scripts de UI e Cinemática

| Script | Função |
| :--- | :--- |
| **MenuManager.cs** | Gerencia o menu principal (Singleton). Controla as animações de fade/movimento (DOTween) dos botões na entrada e o fade-out ao carregar uma cena. |
| **ButtonEffect.cs** | Script individual de botão (UI). Controla os efeitos de DOTween (hover, click), toca sons e informa ao `MenuManager` ou `SceneLoader` qual cena carregar. |
| **UpdateInfoPlayer_UI.cs** | Gerencia toda a UI contextual do jogador, exibindo/escondendo o slider de força e os textos de informação (Pegar, Usar Elevador, etc.) com base no estado do jogador. |
| **CheckObjetive.cs** | Script de feedback visual que muda a cor de um texto (UI) para verde, provavelmente chamado por um `UnityEvent` ao completar um puzzle. |
| **ObjetiveCanvasManager.cs** | Gerencia a UI de objetivos. Carrega e exibe a lista de objetivos (texto) correta com base no nome da cena atual. |
| **HUDAnimator.cs** | Script de animação (DOTween) para elementos da UI, como pulsar a escala de um objeto ou fazer um texto piscar (fade), usado para feedback. |
| **TypeWritterEffect.cs** | Controla o efeito de "máquina de escrever" para `TextMeshProUGUI`, revelando o texto caractere por caractere com pausas para pontuação. |
| **SlideshowController.cs** | Gerencia uma sequência de "slideshow" (fade de `CanvasGroup`) para a cena final, controlando a transição e o fade-out do áudio. |
| **BranchingDialogueManager.cs** | Sistema de diálogo com ramificações. Gerencia árvores de conversa (Nós e Escolhas), instancia botões de decisão, sincroniza áudio de 'fala' e carrega cenas ao final. |
| **RadioDisplayAnimator.cs** | Anima o rádio do menu. Controla efeitos de entrada (Fade/Punch), loop de escala (pulso), troca de sprites e cria um letreiro digital com scroll infinito de texto. |

### Scripts de Efeitos Visuais (VFX)

| Script | Função |
| :--- | :--- |
| **MudancaPedestal.cs** | Controla o material do pedestal. Observa o script `Activate` e usa `Color.Lerp` para animar a cor base e a emissão do material (de 'Off' para 'On'). |
| **AtivacaoPedestal.cs** | Controla os VFX do pedestal. Observa o script `Activate` e usa `Color.Lerp` para animar a cor de um `ParticleSystem` e de uma `Light` (de 'Off' para 'On'). |
| **LightControler.cs** | Controla uma luz, animando sua intensidade (ligando/desligando) com uma `AnimationCurve` e um efeito de 'flicker' aleatório. |
| **LeiteDissolve.cs** | Controla um shader de 'dissolve'. Anima um valor (`_Cutoff`) do material ao longo do tempo (usando `Lerp`) e destrói o objeto ao final da animação. |

### Scripts de Áudio

| Script | Função |
| :--- | :--- |
| **SoundFXManager.cs** | **(Script Central de Áudio)** Singleton (DontDestroyOnLoad) que gerencia todos os SFX. Instancia prefabs de `AudioSource` no local do evento, toca um clipe (único ou aleatório) e o destrói após a reprodução. |
| **SoundMixerManager.cs** | Interface para o `AudioMixer`. Recebe valores (de Sliders) e ajusta os volumes ('masterVolume', 'soundFXVolume', 'musicVolume') no mixer. |
| **RandomLoopingSpeaker.cs** | Simula o som de 'fala' (blips) para diálogos. Toca clipes aleatórios em sequência (um após o outro) e gerencia um 'fade out' suave ao ser interrompido. |
| **AmbientSoundPlayer.cs** | Toca sons de ambiente em intervalos de tempo aleatórios (entre `minWaitTime` e `maxWaitTime`), utilizando o `SoundFXManager` para a reprodução. |

---

## 📚 Inspirações, Referências e Créditos

### Inspirações de Jogos

| Jogo | Inspirou |
| :--- | :--- |
| **Série Fallout** | A estética "AtomPunk", a narrativa satírica de um futuro distópico e o tom cômico-absurdo. |
| **Little Nightmares** | A direção de câmera (Side-Scroller 2.5D), que cria uma sensação de profundidade e claustrofobia. |

<img width="400" alt="FalloutVaultBoyArtwork" src="https://github.com/user-attachments/assets/b0eb8572-7fd6-402c-940a-a4e460e2bd1d" />

![littlenightmares](https://github.com/user-attachments/assets/46bd2138-b2b2-4160-b49f-3a192773f952)

### Ferramentas e Assets

* **Engine:** Unity 6000.0.35f1 (Universal Render Pipeline)
* **Plugins Unity:** Cinemachine (v3.1.4), DOTWEEN (v1.2.765)
* **Arte:** Blender, Figma, Krita
* **Versionamento:** Git, GitHub
* **Organização:** Hack n Plan
* **Assistência (IA):** Gemini (para auxílio na estruturação de código e documentação)
* **Assets:** *Todos os modelos 3D, artes 2D, músicas e efeitos sonoros são criações originais da Weird Milk Studios.*

---
