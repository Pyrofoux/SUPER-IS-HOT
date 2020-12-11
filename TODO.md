# Todo

- Recheck all interactions. Which are implemented? Ambiguity? Distinction between asserts and effects? Which ones should be there/ are the core ?


# Bugfix

- "*" tile at each end of line

- bullet can disappear by hitting each other.
  Nice if they are not both from Player, but prevent it otherwise

- Weapons picked up from ennemies can shoot infinite bullet without reloading time
  + cross gets bigger each time
  => might be cause it happens when time is paused -- delay for recharging isn't paused ?
  ==> infact, the bullet limit doesn't work -- might just have to fix the limit


# Roadmap

- [ ] Ergonomy
  - [ ] Sphere cast instead of raycast for picking objects ?
  - [ ] See rules activated
  - [ ] Cancel button for baba
  - [ ] Choose: images (style) or text (have to scale, can make them lit)


- Gameplay Loop:
  - [ ] Win condition
  - [ ] Loose condition


- [ ] Map out levels
- [ ] Level selection screen

- Implement all interactions as Assert, Trigger and Effect (EAT)
  - [x] You   is Move
  - [x] Time  is Move
  - [x] Shoot is Move
  - [X] You   is Stop
  - [X] Time  is Stop
  - [ ] Shoot is Stop
  - [ ] You   is Dead
  - [ ] Time  is Dead
  - [ ] Shoot is Dead
  - [ ] You   is Shoot
  - [ ] Shoot is You
  - [ ] Super is Hot

  - [ ] You is Shoot
    => Activates only when you shoot (~once)
    => Makes you shoot done

  - [ ] Shoot is You
    => Teleport you to the (last) bullet
    => Thrust you ?

# Notes

- Ambiguity with default behaviour
  Maybe everything moves when TIME IS MOVE un less if they are STOP
  And if TIME IS STOP then they are STOP too, unless they explicitely have X IS MOVE
  Check this logic with basic examples:
  T=S < Y=S
  T=M < Y=M
  are bullets moving ? can you move ?

- Y=s<s=S, this teleportation might excplicitely contain that you teleport to this bullet when a bullet stops
Might make it so you teleport to the first bullet (the one that hit first the wall)

Have to maintain a List of current bullets.
Bullets can know if they are the first on the list.
When they destroy themselves, they remove themselves from the list.
Variable in RuleHandler that detects when event style "Shoot is Dead" happens
Shoot is Dead => Shoot is Stop ~=> Shoot is You should be possible
Might have issue that a bullet destroys itself, and you loose the reference to teleport to it

- Guns can't fire more than one bullet when Time is stopped
Because of reloading time. ==> Make cursor in another color when ready/not

 - You is Stop could be bypassed when falling. Could be an interesting puzzle
  ==> need to implement movement detection based on speed or if falling

# Done

- [x] Unlocking mechanic
  - [X] Variable tile
  - [X] Name on top of enemy
  - [x] Updating baba world when killed



  - Player speed is quite high. Check if it happens in Fixed Update or not
  FIXED: Set Applciation.targetFrameRate to 60
  NOTE: If you put Players update in FixedUpdate, it will depend on time flowing or not
  which may break a lot of things. Keep it in Update.

# Possible future dev

- REVERSE: path is reversed, if you go back to origin you disappear
- LOOP: path loops: you must choose your first steps wisely
- SKIP: skips ??
- RED: impact ennemies. could make them reverse, loop, invisible or invincible
