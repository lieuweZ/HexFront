# TMC 1

## THINK

### Teamnaam 
Voor het bedenken van de spel naam was er eerst 2 minuten gegeven om namen te bedenken voor het spel.<br>
Toen deze stap gedaan werd er weer 2 minuten gegeven om te stemmen op welke naam we wilden iedereen kreeg mogelijkheid om op twee te stemmen.
![MoodBoard](./img/names.png)

### Inspiratie analyse:
We begonnen met het verzamelen van van inspiratie, hiervoor heeft iedereen een plek gekregen waar ze afbeeldingen van hun inspiratie voor de game in konden zetten zie afbeeldingen hieronder.

Moodboard Daamin

![MoodBoard](./img/daamin_mood.png){width=500}

Moodboard Lehan

![MoodBoard](./img/lehan_mood.png){width=500}

Moodboard Lieuwe

![MoodBoard](./img/lieuwe_mood.png){width=500}

Moodboard Nicolaas

![MoodBoard](./img/nicolaas_mood.png){width=500}

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

### Feedback Loops
- Positive feedback loop : Maak tegenspeler gebouwen kapot -> minder gebouwen van tegenspeler -> makkelijker om gebouwen van tegenspeler kapot te maken.
- Positive feedback loop : Gebouw bouwen -> krijgt resources -> meer gebouwen bouwen -> krijgt meer resources;
- Negatieve feedback loop : Je bouwt veel gebouwen (sterk) -> resource nodes raken op -> je hebt minder resources (zwak) -> je moet meer gebouwen buiten/resource nodes zoeken;
- Negatieve feedback loop : Je bent relatief sterk -> algemene kracht van aangeboden gebouwen wordt lager -> wordt zwakker -> algemene kracht van aangeboden gebouwen wordt hoger;

## MAKE 

### Paper Prototype
Voor het papier prototype was er eerst een bord getekent met zeshoeken.<br>
Toen werden er 2 vierkanten erop gezet die de functie van hoofd gebouw hadden.<br>
Toen dit werd gedaan was er een soort interface gemaakt waar al de bouwbare objecten op zaten.<br>
Er werd toen resource nodes toe gevoegt.<br>
Hiernaa werd er een soort playtest gedaan met het getekende versie.<br>

![prototype](./img/play.png){width=500}

Tijdens het spelen werden er veranderingen toegevoegt aan de regels en veranderingen gebracht werden aan bestaande regels.<br>
 
### Digitale versie

![Main menu](./img/mainmenu.png "main menu")<br>
Bij deze foto is het ook zichtbaar hoe de hoofd menu eruit ziet met de logo van het spel. <br>
De achtergrond heeft als mogelijkheid om als een soort geheime mini game te kunnen werken. Het werkt als volgt je moet op elke plaat een vierkant plaatsen.<br>
Op deze grid kan je zien dat er op gebouwt is je kan dit zien door de bluawe vierkant wat op de twee cellen zijn.<br>
Deze grid kan ook een indicator geven voor wanneer de muis in een cell is.<br>
De cell is 1 van de zeshoeken in de grid.<br>

## CHECK