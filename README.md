# Manifest Destiny: Masters of the New World #

### A real-time strategy game developed in Unity for [MiniLD 44 - #7DRTS](http://www.ludumdare.com/compo/2013/07/05/minild-44-announcement/). ###

Check out the Ludum Dare page for this game [here](http://www.ludumdare.com/compo/minild-44/?action=preview&uid=22017).

## Game Objective ##
The basic objective of Manifest Destiny is to build a chain of settlements from a starting town on the east coast all the way to the west coast. This is done by building up your population and marching them across the harsh new world, competing for territory with natives and opposing nations every step of the way.

## Basic Controls ##
* **WASD / Arrow Keys / Middle Click + Drag / Cursor to Edge** - Move Camera
* **Left Click** - Select Unit/Path/City
* **Right Click** - Move Soldiers/Settlers, Train Soldiers/Settlers
* **Mouse Scroll** - Zoom
* **ESC** - Options Menu

## Gameplay ##
Players start by selecting a nation (each with unique properties) and selecting which path to take to the new world (determines starting location). 

The core of the game is managing three resources:

* Food (Blue) - Used to create soldiers (2 food), settlers (1 food), and send out soldiers (1 food). Do this by clicking on the blue food stack and then right-click and hold on the green settlers or red soldier stacks.
* Settlers (Green) - Settlers generate food and more settlers. They are also the only units that can capture a new city. They have limited defense or attack capability. Settlers can also be turned into soldiers (cost 1 food) by clicking on their green stack and right-clicking and holding on the red soldier stack.
* Soldiers (Red) - Soldiers are used for attacking and defending cities. Can be converted into settlers at no cost (prevents you from trapping yourself). Die less frequently moving across paths.

Moving between cities is dangerous. Click on a path to get more info about its length and danger rating. Pave the road with the blood to make the traveling easier! As units die on a path, the difficulty rating of that path will decrease. 

Win the game by raising your flag over a settlement on the west coast.

### Tips ###
* Make More Settlers - Unless you think you're going to get attacked or you need to do some attacking, don't bother with soldiers.

## Known Bugs / Incomplete Features ##

* No unit pathfinding - As a result, you can only move units to directly neighboring settlements.
* Poor balancing - Too many knobs, not enough time.
* Poor AI - The AI controlled players don't actively pursue victory. However, they can still occasionally win if they get lucky with the random number generation. 
* Occasional Framerate Issues - When moving a lot of units around, expect the framerate to drop.

## Dropped Features ##
Things I originally planned to do when I started that I eventually dropped.

* Natives - Prepopulate some inner nodes on the map with units belonging to a Native American faction that isn't competing to win the game.
* Seasons - A periodic cycling through spring/summer/fall/winter. During colder weather, traveling would be more difficult and food production reduced. 
* Nation Traits - Each playable faction has unique characteristics: move faster, produce units faster, stronger soldiers, etc. Considering that I didn't have time to properly balance things without these, I decided not to pursue them. 

## Credits ##

### Developed by ###
Chase Pettit (twitter: [@chasepettit](http://twitter.com/chasepettit))

### Tools ###
- Unity 4.1 (w/ NGUI, SuperSplines)
- Photoshop CS5.1
- BFXR
- Audacity

### Art ###
- Jasper F. Crospey, "Starrucca Viaduct", 1865
- Marc-Aurèle de Foy Suzor-Coté, "Jacques Cartier rencontre les Indiens à Stadaconé, 1535", 1907
- Anonymous, "The Noord-Nieuwland in Table Bay", 1762
- Benjamin West, "Penn's Treaty with the Indians", 1771
- William H. Powell, "Discovery of the Mississippi By De Soto", 1853
- Johann Walter, "Gustavus Adolphus of Sweden at the Battle of Breitenfeld", 1632
- Beige Paper, Konstantin Ivanov, http://subtlepatterns.com/

### Misc Assets ###
* Unity Standard Assets
* Unity Terrain Assets

### Music ###
- Beethoven, Egmont Overture, 1787. Performed by [Kevin MacLeod](http://incompetech.com/email.html)

### Misc Audio ###
- AMBIENT LOOP - Wilderness Hillside - RAW + Outdoor Air Noise.mp3, [Arctura](http://www.freesound.org/people/Arctura/sounds/39832/)
- Wind Chimes - PLF.wav, [Patrick6410](http://www.freesound.org/people/Patrick6410/sounds/83512/)
- marching boots.wav, [klankbeeld](http://www.freesound.org/people/klankbeeld/sounds/172369/)
- Horse_Whinny.wav, [foxen10](http://www.freesound.org/people/foxen10/sounds/149024/)