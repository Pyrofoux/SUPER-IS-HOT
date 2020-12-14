# Todo

- All trigger once events:
You is Shoot (when you fire)
Shoot is Dead (on impact)
You is Dead (when you die) // maybe thats not the case ?
Shoot is You (on teleport)


- Recheck all interactions. Which are implemented? Ambiguity? Distinction between asserts and effects? Which ones should be there/ are the core ?


# Bugfix

- Rules are not parsed at startup

- "*" tile at each end of line


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

- [ ] Implement all interactions as Assert, Trigger and Effect (EAT)
  - [x] You   is Move
  - [x] Time  is Move
  - [x] Shoot is Move
  - [X] You   is Stop
  - [X] Time  is Stop
  - [X] Shoot is Stop
  - [~] You   is Dead
  - [?] Time  is Dead
  - [X] Shoot is Dead
  - [ ] Super is Hot

  - [X] You is Shoot
    => Activates only when you shoot (~once)
    => Makes you shoot once

  - [X] Shoot is You
    => Teleport you to the (last) bullet
    => Thrust you ?

# Notes


- If you make bullet don't destroy they can have cute/weird trajectories, even with just
  straight lines!

Consider:
- Making player immune to their bullets when Shoot is You ==> solve that don't follow
  special detection is already happening in Shoot is You, can detect player even without collider
- Making player follow the LAST bullet. (Intent can change things, a way to navigate space by flying)
- taking time to organize things :)
  - gather level ideas
    - +one where shoot bullet through wall, make it become after going through wall of ennemies
  - create temp project page

- Guns can't fire more than one bullet when Time is stopped
Because of reloading time. ==> Make cursor in another color when ready/not

==> You can't shoot if your gun is not reloaded, even if you're forced to !
In flipper level, this means the Teleportations stop if you're teleporting too quickly !  

-


 - You is Stop could be bypassed when falling. Could be an interesting puzzle
  ==> need to implement movement detection based on speed or if falling
Puzzle: You can only shoot when you move, but enabling this rule makes you stop.

# Done

- [x] Unlocking mechanic
  - [X] Variable tile
  - [X] Name on top of enemy
  - [x] Updating baba world when killed



  - Player speed is quite high. Check if it happens in Fixed Update or not
  FIXED: Set Applciation.targetFrameRate to 60
  NOTE: If you put Players update in FixedUpdate, it will depend on time flowing or not
  which may break a lot of things. Keep it in Update.*

  - Ambiguity with default behaviour
    Maybe everything moves when TIME IS MOVE un less if they are STOP
    And if TIME IS STOP then they are STOP too, unless they explicitely have X IS MOVE
    Check this logic with basic examples:
    T=S < Y=S
    T=M < Y=M
    are bullets moving ? can you move ?

    - Shoot is Dead doesn't trigger Shoot is You correctly
    FIXED: had to specify the order of Scripts (BulletControl before EffectsApplicator) in Project Settings

# Possible future dev

- REVERSE: path is reversed, if you go back to origin you disappear
- LOOP: path loops: you must choose your first steps wisely
- SKIP: skips ??
- RED: impact ennemies. could make them reverse, loop, invisible or invincible
