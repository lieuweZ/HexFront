[![Hexagon fonts](https://see.fontimg.com/api/rf5/2xwv/ZDdiYmQ3NTc4NGEyNGVmMzkzZWQ0OGI0YWJhMGM4ZmQub3Rm/SGV4RnJvblQ/hexgon.png?r=fs&h=81&w=1250&fg=000000&bg=FFFFFF&tb=1&s=65)](https://www.fontspace.com/category/hexagon)

# Game Design Document
Een spel gemaakt door Twister, een gedreven team dat werkt aan indiegames.

## Core Concept
- Bouw een koninkrijk, leid je troepen, en vernietig het kasteel van je vijand.
- In dit strategische 2D-spel draait alles om het slim opbouwen van een basis, het verzamelen van een leger en het strategisch plannen van aanvallen. Matches zijn kort (10-20 minuten), en visueel sfeervol met een cozy uitstraling.

## Main Features
### 🎮 Visual Playstyle
- 2D artstijl;
- Zachte, rustgevende kleuren;
- Een cozy en overzichtelijke UI die spelers begeleidt zonder te overheersen.

### 🎨 Inspiratie
- Stijl / Kleurenpalet: dotAGE;
- Hexagon Grid Design: As Far As The Eye, Dorfromantik;
- Gameplay: Elementen uit Risk.

### 🧩 Interesting Mechanics
- Gebied verovering (hex-tiles) middels gebouwen en units;
- Resource management;
- Dynamische events per match (bv. weersveranderingen);
- Unieke units met speciale vaardigheden.

### 💡 Unieke Selling Points
- Korte potjes met diepgang;
- Modulaire kaart die elke keer anders is;
- Speelbaar tegen mensen (PvP);
- Eenvoudige instap met strategische diepgang.

### 📐 Design Pillars
- Toegankelijkheid: Makkelijk te leren, hoge vaardigheidsplafond;
- Aesthetic: Rustgevende visuals in contrast met tactische gameplay;
- Herhaalbaarheid: Procedurally generated maps zorgen voor variatie in elke match;
- Competitie: Spelvormen competitie ondersteunen.

### Formal Elements:

#### Player:
- Player vs Player;

#### Objecties:
- Main Objective : Capture/Construction
- Outwit as;
- Per turn krijgt de speler verschillende pieces (met verschillende kosts) die de speler kan gebruiken;
- Centrale building die ca ptured moet worden;
- Win condition = Centrale building van tegenstander tot 0 hp krijgen of alle buildings van de tegenstander destroyen of enemy surrendert;

#### Procedures:
- Begin van het spel kiest de speler waar de centrale building wordt geplaatst binnen een bereik;
- Speler bouwt gebouwen op zijn turn van de gegeven pieces;
- Turn eindigen en dan speelt de tegenstander;
- Speler kan opgeven;

#### Rules:
- De speler kan alleen binnen het grid systeem acties uitvoeren;
- De speler kan niet troepen/gebouwen van de tegenspeler manipuleren;
- De speler kan per turn één gratis building bouwen, met de optie om meerdere buildings te bouwen indien mogelijk;
- Turn limit gebaseerd op tijd : 1 minuut;
- Gebouwen hebben een specifieke tile base nodig;
- Limiet van units is afhankelijk van gebouwde gebouwen;
- Als een gebouw wordt aangevallen, krijgt die schade;
- Als een gebouw kapot is, wordt de tile free gemaakt; Beschade gebouwen kunnen repaired worden;

#### Objecten:
- Resource gathering and Troop builder Buildings;

#### Resources:
- Levens (Central Building);
- Units (Buildings, troops);
- Health (Buildings);
- Currency (Resource 1, Resource 2, Resource);
- Special Terrain ();
- Time (Turn time); 

#### Conflicten

#### Obstacles:
- Weer verschijnselen;
- Speciaal terrein;
- Resource limiet op nodes (hierdoor kan de speler niet zoveel bouwen als die wilt);
#### Opponents:
- Tegenspeler;
#### Dilemmas:
- Beperkte hoeveelheid/soort buildings per turn die de speler kan bouwen;
- Soort building per grid tile;
- Geluk spelt een rol in welke buildings de speler kan bouwen;

#### Boundaries
- Het grid systeem;

#### Outcome
- Win/Lose condition;

### Core Mechanics:

#### Primary mechanics:
- Gebouwen bouwen op de grid;
- (Turn based)?;
#### Secondary mechanics:
- RNG gebouw beschikbaarheid;
- Troepen vallen automatisch gebouwen/andere troepen aan;

## Doelgroep:
- Liefhebbers van turn-based strategy games;
- Casual gamers die van korte sessies houden;
- Fans van cozy indie games met diepgang;
- Vriendengroepen en/of families die houden van competitieve games.

## Milestones:
### Development:

### Teamrollen:
- Game Design: Hele team.
- Art & Visuals: Hele team.
- Programming: Hele team.
- Sound & Music: Placeholder.