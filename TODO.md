# Todo

- Code source
- jam tags
- meme: started/ended
- musician guy Twitter
- RT ?
- delete 10

# Bugfix

[X] Calling Restart or Escape when fading at start causes issues
==> prevent R or Escp during the first few seconds of a level

[ ] Ennemies have weird rotation when player is above
==> Hard to fix right now


Level 4:
[X] Can fall when In baba Mode pause (how to change that?? Disable gravity)
[X] You is Shoot as effect is only triggered if also You is Shoot as assert
[?] You is Move is still triggered by arrow key presses -- if no "You is Move" or "You is Stop" is enabled


[X] Highlighting is wrong for one frame when moving words
==> Script order issue

[?] "Super is Hot" win message is shaded when cursor is shaded to
  ==> Because they have the same material and its linked. Create Material copy in cursor/weapon code

[X] Vertical overflow: check the height and use the smallest vertical/horizontal decomposition
[X] Pressing the key triggers You is Move even when you can't


//
Only when starting from level directly:
- Player can't be killed -- When loading level directly
- Starting directly from a level and coming back to menu creates two Instances of LevelHandler + Player

- A is B w is (X)
makes "A is B" not parsed if (X) isn't valid.
==> acceptable

TO CHECK
- Time is Move by default might change ?
Clearer that Time is Move impacts time, an objective to remove

# Tweaks
- M to mute

- consider order of panels ?
[X] Make gun do a first dull tick before throwing
- highlight gun when pickable ?
-


# Roadmap

- [ ] Levels
  - [X] Handy levels system
  - [ ] Map out ~10 levels
  - [X] Level selection screen


# Done



- Gameplay Loop:
  - [X] Win condition
  - [X] Loose condition


- Music
  [X] Create normal/remix track
  [X] Loop through them
  [X] Switch between them

- Polish sounds
  - [X] Time Deformation effect
  - [X] Space Deformation effect
  - [X] Gun shoot sounds player + [X] ennemy
  - [X] Gun reload sounds
  - [X] Gun pick up sounds
  - [X] Ennemy death sound


- [X] Implement all interactions as Assert, Trigger and Effect (EAT)
  - [X] You   is Move => Detects when falling
  - [X] Time  is Move
  - [X] Shoot is Move
  - [X] You   is Stop
  - [X] Time  is Stop
  - [X] Shoot is Stop
  - [X] You   is Dead
  - [X] Time  is Dead
  - [X] Shoot is Dead
  - [X] Super is Hot

  - [X] You is Shoot
    => Activates only when you shoot (~once = 30 frames)
    => Makes you shoot once

  - [X] Shoot is You
    => Teleport you to the (last) bullet

-  General Design
  - [X] Bullet limit in guns (3 to 6)
  - [X] Sphere cast for picking gun
    [X] Highlighting pickable gun
  - [X] Ennemy shoot you if there's a line of sight
  - [X] Parametrizable frequency and delay for shooting
    ===> can only delay animation
  - [X] Controls for Baba: FixedUpdate, check frequency and consistency
  ==> kinda no choice but to use a key up way

- [X] Ergonomy
  - [X] Display controls: Arrow keys, E for Baba Mode, R to restart, Undo with back space <=]
  - [X] Sphere cast instead of raycast for picking objects ?
  - [X] See rules activated
  - [X] Cancel button for baba
  - [X] Choose: images (style) or text (have to scale, can make them lit)
    [X] create individual images for doodle shader to work
    OR add black outlines


- [x] Unlocking mechanic
  - [X] Variable tile
  - [X] Name on top of enemy
  - [x] Updating baba world when killed


  - Menu and transitions
    - [X] Death transition
      Red fade -- restart
    - [X] Restart transition (= Death ?)
      White fade
    - [X] Win transition
      [X] Stop - [X] Voice clip - [X] White fade
      [X]"SUPER HOT" text (?)


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
- SKIP: skips time ??
- RED: impact ennemies. could make them reverse, loop, invisible or invincible



  - Dead is Shoot
  Makes bullets deadly ?
  - Dead is You (?)
  Makes you kill on touch ?
