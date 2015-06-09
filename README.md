# UnityNeedler
Creating a unity version of the needler, from Halo.

In Progress 6/8/15

![Dual-Wield Needlers with auto-aim](http://i.imgur.com/bc07Whn.png "Needler Screenshot 6/8/15")

Design was done on paper and mapped out.

#Custom Assets

##3D Models : Animations
Fbx file format
* Needle : None
* NeedleHost : Death
* Needler : Shoot, Reload

##Sounds
Ogg vorbis file format
* Shoot
* Reload
* Impact
* Expire
* Explosion

##Sprites/Sprite Sheets
Png file format
* Muzzle Flare

##Visual Effects
* Explosion
* Needle Glow
* Needle Trail

#Custom Components and Prefabs

##1. Needle
A needle is purple, glows, and flies. It dissapears after a certain amount of time. Needles find targets, which they push towards. On collision they stick into solid objects. If they hit a needle host they will not die until the host says so.

##2. NeedleSpawner
Creates new needles at it's location, provided it has the ammunition and has waited *t* seconds. The needles will travel in a random direction within a yaw and pitch spread. Triggers automatically or manually.

###Pool
The NeedleSpawner should pool it's needles. Create needles and intialize them to "expired". When a needle is needed pull from the expired pile and add it to the living pile. When a needle expires again, change piles.

##3. NeedleHost
A target for needles. It has a point to home onto, a boundary for collision, a count and array of needles it has been hit by, a duration *t* before needle count is reset, health and max health, and an explosion index which determines how many needles trigger an explosion. When an explosion occurs the host recieves damage and all needles attached to it expire. The host dies if health drops to 0, triggering an animation. After *d* seconds the target's health is refilled and they are revived.

###Health bar
A 3D health bar above the host's head.

###Moving Targets
Moving targets should move around in a choreographed way. They do not need to synchronize. Following a path they can move sequentially; 1,2,3,4,1,2,3... Or randomly; 1,3,4,3,2,2,1,4.... They can also rewind when moving sequentially; 1,2,3,4,3,2,1,2,3... They will move *f* pixels/sec. They do not need physics.

###Resting Place
When a host explodes it should face towards the final needle before exploding.

####Nice To Have
* "Ragdoll" after death
* Delay before turning around

##4. Needler
The visible body of the needler and a needle spawner. It can shoot, triggering the spawner, and if successful triggering an animation. It can reload *z* amount, if the spawner isn't full it plays an animation and refills when the animation completes.

###Muzzle Flare
A sprite animation attached to the front of the Needler. If spawner triggers successfully the animation is revealed and played.

##5. NeedlerHolder
Instantiates a Needler and attaches it as a child. The hand can pull or release the trigger to fire the needler. It can also begin reload with *z* ammunition. This is essentially used as a inventory slot.

##6. NeedlerCharacter
Has two NeedlerHolders, reserveAmmo, and reserveAmmoLimit. If a needler is empty it will begin reload with NeedlerAmmo. It can holster left, right, or both based on state.

##7. PlayerOverrideNeedlerChar
Provides keboard input for Needler Character.

##8. PlayerCamera
Perspective Camera mounted on character shoulders.

##9. Path
A GameObject with a live updating and resizable collection of empties, referred to as "Nodes". Path can return the next node index and can rewind/bounce if a bool *b* is true. Given an index *i* it will return a node.
