Legend:
(b)   breakable
(!b)  unbreakable
(m)   makeable
(!m)  unmakeable
(v)   there
(x)   not there

Process to test a level:
- Is it winnable the way intended ?
- Can I get stuck easily ?
- Can I do it another way than intended ?
  Are these other ways too easy to execute or come up with?

Process to order level:
Rank them according to concepts or mastery
- "Do I need to understand this concept before seeing that one ?"
- "Do I need to master more the logic of the game for this level ?"

## Main Theme ideas:

- [x] *First steps*

3 ennemies (Super, Is, Hot)
(v) Time is Stop, You is Move
(!u) You is Shoot
Use the gun to kill them but bullets are stopped
Must make Time is Move, kill ennemies
and then create Super is Hot

Makes understand: rule making, time stop mechanic, unlocking mechanic,
win mechanic, death mechanic, gun shooting

- [X] *Classic*

Lots of enemies (6), can't be passed without slowmo.

Mech: Locked time. Sol = Slow mo + Kill order
Makes understand: Kill order, rule making, When mechanic

Final:
```
@___________
__________M_
__T=M<Y=S___
____________
_#####T=M##_
____________
1_____Y=s___
2_____=_____
3_____S_____
```

- [X] *Reversal*

Revert the implication created before.
Helps you understand how things can be rearranged to create new opportunities or challenges.

Mech: Locked movement. Sol = Sliding + (Reloading latency ?) + (Friendly Fire ?)
Friendly Fire to get first gun (possible to pause with editing rules)
Reloading latency to help shooting after that

Final:
```
___#__##__#___T__Y
@__#T=M<Y=M___=__=
___#Y###T##___S__S
__________________
____2_=_1_________
______________Y=#s
```

- [ ] *Flipper*

Activate the "Shoot is You"

In the main room to kill multiple enemies, "Rocket Me" style, like a pool ball.

Might have to use Time is Stop to aim
==> unlock it first ?

Mech: Mitraillette + Teleport Gun or Rocket Me

Note: I intended to do this level with "Shoot is Move > Time is Move"
but "You is Shoot  > Time is Move" is quite nice too

Note: Friendly fire makes you cautious about your path, nice
Note: Might be frustrating to redo the rules each try

Bug: Can't pick up gun sometimes ?
Bug: fast restart fucks thing sup > Restart when fading (initial scene) fucks things up.
prevent it during fading = the first few seconds (along with escape)

- [X] *Self-termination*

Give words available to try combinations but the only one working should be
Super is Hot < You is Die

(its ok if there's other clever solutions)

Mech: Locked movement or shoot. Gravity to create bullet, Suicide to win.

Makes understand: "DEAD" word, ability to pause whenever

+ Puzzle where need movement to shoot a bullet ?

Note: Gravity is a bit too hard to apply  

- [X] *Just in time*

(v) Time is Move < Shoot is Move #Necessary
(v) You is Dead < Shoot is Stop  #Killswitch

Shoot one bullet to activate time moving, but when it stops you die. You have to make the trajectory very long before impact.

Use this time to kill the right enemy by throwing your gun,
and write "Shoot is You" making the shoot never stopping
==> Shoot is You doesn't replace killing a bullet

Mech: Create the window, Teleport/Rocket Gun

Note: Need to have seen Teleport Gun before
Note: Final design is super challenging and very interesting in terms of mechanics usage
Note: Need to close roof if we don't the player to go anywhere
Note: Player may clip through walls, but not win this way

## Mechanics / Puzzle Pieces:

- *Slow motion*
  Unlocks: Time movement, control

  (v) Time is Stop

  Solution:
  Time is Move < You is Move

- *Reverse slow motion*
  Unlocks: Time movement
  Challenge: Hard to control
  (v) Time is Move < You is Stop

- *Gravity*
  Unlocks: Rule activation.
  Challenge: Movement is blocked when desired
  Requires: Initial Movement

  (x) (Desired rules)
  (v) (Desired rules) < You is Move
          (XOR)
      You is Move

  Solution: To unlock,
  activate the rule in mid-air, to activate it when descending.

  Need to know: that You is Move is activated when you fall (classic SuperHot style)
  Use ideas: You is Shoot, to find a way to shoot.

- *Friendly Fire*
  Unlocks: Killing enemies, Unlocking words, Getting gun
  Challenge: Must evade enemy trajectories

  Can't shoot, no available guns.
  Solution: Must let enemies kill each other, by moving between them.

- *Acceptance / Suicide*
  Unlocks: Winning.
  Challenge: Counter-intuitive
  Requires: Movement, Shooting bullet, Time pausing (solo)

  (v) You is Move
  (v) You is Shoot
  (v) You, is, Dead, When

  Solution: counter-intuitive use of
  Super is Hot < You is Dead.
  Must either kill yourself or let others kill you.

- *Sliding*
  Unlocks:  Movement
  Challenge: Hard to control
  Requires: Time moving

  (X) You is Move

  Solution: You is Move < Time is Move

- *Mitraillette*
  Challenge: use all bullets in one go.

  (!u) You is Shoot < You is Shoot

- *Teleport gun*
  Unlocks: Movement
  Requires: Shooting bullet, Time moving

  (v) You is Stop

  Solution: Shoot is You < Shoot is Dead

- *Rocket move*
  Unlocks:    Movement
  Challenge:  Hard to control
  Requires:   Shooting bullet, Time moving

  Solution:   Shoot is You

- *Rocket move*
  Unlocks:    Movement
  Challenge:  Hard to control
  Requires:   Shooting bullet, Time moving

  Solution:   Shoot is You

- *Time charges*
  Challenge: Time passing/pausing is a bullet-dependent resource
  Requires: Shooting bullet
  (!u) Time is Move/Stop < You is Shoot

- *Run charges*
  Challenge: Movement is a bullet-dependent resource
  Requires: Shooting bullet
  (!u) You is Move/Stop < You is Shoot

- *Create the window*
  Challenge: Rule activation in a time dependent of bullet travelling distance
  Long trajectory = Long activation time
  Requires: Shooting bullet

  (Undesired Rule activation)  < Shoot is Dead
  eg: You is Dead
  or
  (Desired Rule activation)  < Shoot is Move
  eg: Time is Move

- *Kill Order*
  Challenge: Must think of killing order while surviving enemy shots.
  Only a fraction of unlocked words are actually accessible.

- *Reloading latency*
  Challenge: Shooting makes you stop.
  (v) You is Stop < You is Shoot
  Negative if you can control your movement, or under pressure
  Positive if movement is out of control and out of threat

- *Frozen Bridge*
  Unlocks:    Access to height or passing over an obstacle
  Challenge:  Hard to jump precisely on an item, hard to control where the item will be
  Solution: Stopping time for bullets or for a gun, and then jumping over it

## Natural implications:

  Getting Gun + Time Passing (reload) => Shooting Bullet
  Shooting Bullet + Bullet Moving => Killing enemies
  Killing enemies => Getting Gun, Unlocking Words
  Unlocking Words => Rule Activation
