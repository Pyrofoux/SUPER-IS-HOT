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

- [ ] *Naked*
Lots of enemies (6), can't be passed without slowmo.
No gun in your hand.
Only the last ennemy unlocks "Hot"
Lock 1 to 5 are inacessible

Mech: Locked time. Sol = Slow mo + Kill order
Makes understand: Kill order, rule making, When mechanic

- [ ] *Reversal*
Revert the implication created before.
Helps you understand how things can be rearranged to create new
opportunities or challenges.

Mech: Locked movement. Sol = Sliding + (Reloading latency ?) + (Friendly Fire ?)
Friendly Fire to get first gun (possible to pause with editing rules)
Reloading latency to help shooting after that

-

- *Flipper*


Mech: Mitraillette + Teleport Gun

- *Let me die*

Mech: Locked movement or shoot. Gravity to create bullet, Suicide to win.


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

- *I am bullet*
  Unlocks:    Movement
  Challenge:  Hard to control
  Requires:   Shooting bullet, Time moving

  Solution:   Shoot is You

- *Limited Time*
  Challenge: Time passing/pausing is a bullet-dependent resource
  Requires: Shooting bullet
  (!u) Time is Move/Stop < You is Shoot

- *Limited Movement*
  Challenge: Movement is a bullet-dependent resource
  Requires: Shooting bullet
  (!u) You is Move/Stop < You is Shoot

- *Create the window*
  Challenge: Rule activation in a time dependent of bullet travelling distance
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

## Natural implications:

  Getting Gun + Time Passing (reload) => Shooting Bullet
  Shooting Bullet + Bullet Moving => Killing enemies
  Killing enemies => Getting Gun, Unlocking Words
  Unlocking Words => Rule Activation
