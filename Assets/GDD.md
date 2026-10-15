## Systems:
- Object pool system (got it from third party)
- Level generation (enemies, pickups) system (WIP)
- Player units manager (spawning, movement, control) system (WIP)
- input system
- UI system

  
#### UI system
- everything in one level, no main menu. A settings button which shows everything
#### ~~Object Pool system~~

## Assets
- Enemies
- Player soldiers
- Boss enemy
- Gates
- Weapons (how many can you squeeze in)
- Animations (see references)
  - enemy walk
  - boss walk
  - boss strike
  - player walk
- font for gates
- shader for gates
- environment

## Requirements:

- Must be done until November 8 max
- Below 100 MB a MUST since addressables is additional time and logistic and money hassle
- Runs at least 30 FPS on my two androids
- doesn't heat the battery.

## Gameplay decisions

- Boss doesn't kill you immediately, but it does get beefier and should represent the cut-off threshold on the curve. Players should navigate the game in order to get ahead of the curve and be able to kill the boss before he is able to kill all soldiers.
  - I dont't know if I can go the way of insta kill bosses cause then if the players kill the first boss they will be unstoppable, bosses should act like sort of restarts for player level in the same playthrough.
- Dilemma: rapid firing rate from start and a lot of enemies and slowly raising it or do it like Last War from start right away?
  - rapid firing
- Dilemma: should gates be able to be pumped to increase numbers?
  - ~~yes, cause then you have extra decision making for the player. how long to pump the numbers for~~
  - but actually this wouldn't work with rapid firing, then player would pump numbers way too fast. so No
- dilemma: should gates be on both sides? or gate on one side and barrels with upgrades on other side?
  - both sides so players get confused? yeah, cause both sides also means players can't reach a gate until they destroy a barell.