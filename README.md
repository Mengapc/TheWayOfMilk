# 👾 Lactrópolis

![weirdmilk_branco](https://github.com/user-attachments/assets/1a855ed0-f8e7-425f-867f-ce45523614ee)

**Estúdio:** Weird Milk Studios

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

Mergulhe em Lactrópolis, uma aventura de **Puzzle/Plataforma 3D** que vai testar sua mira e seu senso de humor. Prepare-se para guiar Arny Longsing, o último defensor dos bovinos, em sua missão mais urgente: desvendar o mistério da Caseína-235 e encontrar a útlima sobrevivente, a Vaca Xuxa, antes que a LactoNuke transforme o mundo em um laticínio radioativo!

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

Esta seção detalha as **decisões de design** tomadas durante o desenvolvimento, explicando como integramos tema, mecânicas, arte e som.

### 1. Narrativa e Tema
*A premissa de "Lactrópolis" nasceu de uma sátira ao "AtomPunk" (popularizado por **Fallout**) e à cultura de consumo. A decisão de usar a "TupperWare" como vilã inicial foi para criar um contraste cômico imediato entre o banal (potes de plástico) e o apocalíptico (energia nuclear). A narrativa é contada visualmente através dos pôsteres de propaganda da LactoNuke e pela própria progressão do jogador, que explora um mundo onde algo tão comum quanto o leite se tornou uma arma de destruição.*

### 2. Arte e Estética
*A direção de arte busca um "Retrofuturismo Cômico". Enquanto a inspiração em **Fallout** dita a paleta de cores (tons pastéis, verdes nucleares) e a estética dos anos 50, a inspiração em **Little Nightmares** ditou a nossa decisão de câmera. Escolhemos uma câmera 2.5D fixa (Side-Scroller com profundidade) para: 1) Criar uma sensação claustrofóbica de "bunker" e 2) Simplificar o design dos puzzles, focando a mira do jogador em um plano mais controlado, mas ainda permitindo a exploração de profundidade.*

### 3. Mecânicas e Códigos
*A principal decisão de design foi limitar a interação do jogador a **uma única mecânica: o arremesso**. Isso nos forçou a criar puzzles diversos usando apenas esta ferramenta. O jarro de leite não é apenas uma "chave", ele é a forma de ativar pedestais, mover plataformas e (em puzzles futuros) quebrar objetos. Tecnicamente, isso foi centralizado no script `ObjectGrabbing.cs`, que gerencia o estado do jogador (livre, segurando, mirando), e no script `Activate.cs`, que usa `UnityEvents` para permitir que o Level Designer conecte o pedestal a qualquer outro objeto (portas, elevadores, etc.) sem precisar escrever código novo.*

### 4. Som e Música
*A sonorização foi desenhada para reforçar o tom. A música ambiente mistura suspense com um toque "industrial" e burocrático, refletindo a LactoNuke. Os efeitos sonoros (SFX) são exagerados de propósito: o som de "pegar" o galão, o "blip" dos diálogos (controlado pelo `RandomLoopingSpeaker.cs`), e os sons de ativação dos pedestais são todos desenhados para dar um feedback claro e satisfatório ao jogador, informando que sua ação teve um resultado imediato.*

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
| **SetTextToDisplay.cs** | Gerencia o fluxo do sistema de diálogo. Envia textos para o `TypeWritterEffect`, aguarda o input (Espaço) e chama o `SceneLoader` ao final da lista de falas. |

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

![fallout-resume](https://github.com/user-attachments/assets/b6e6eddd-2631-4c4e-8358-3783d461b30b)

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
