Individual but linked systems:
- Player units spawn (component that is a client for the Object Pool system)
    - UnitsSpawner.cs (used by both player and enemies)
- Player units school-of-fish movement
- PLayer units firing, weapons, damage
- Player units animation (is driven by the other systems bit can also override when it comes to it)

Individual but linked systems:
- Enemies spawn (component that is a client for the Object Pool system)
- Enemies movement (component)
- Enemies attack
- Enemy animations system

Basic UI system
- everything in one level, no main menu. A settings button which shows everything

Requirements:
- Below 100 MB a MUST since addressables is additional time and logistic and money hassle
- Runs at least 30 FPS on my two androids
- doesn't heat the battery.

Object pool system for all the things (enemies, soldiers, effects, bullets)


I need GameFeel system.

Make a mood board and draw what it should look like in the end so I have a clear idea and not keep adding things.

Systems:
Object pool system
Level generation (enemies, pickups) system
Player units manager (spawning, movement, control) system
input system





