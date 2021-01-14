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


- Decide what to do with "You is You" rules
==> Baba is You use it to enforce not moving you.
==> Decide not to enable it here


 - You is Stop could be bypassed when falling. Could be an interesting puzzle
  ==> need to implement movement detection based on speed or if falling
Puzzle: You can only shoot when you move, but enabling this rule makes you stop.

- Design notes: What is hard/challenging when making this game ?

have to make working systems for two games + their coupling
time/difficulty is > than making each game separately

## Coupling
* Logic code: physics events vs rules propagation
* Interaction Design: Which keys are common, behave the same -- coherence in UI
+ Two UIs to make with different target action spaces
* Level Design: Be interesting in interaction between the two design spaces
* Graphical Design: Recreate both aethetics and make them coexist

Time management => Can interrupt things, use unusual ways
Logic breaking => everything had to be flexible, detectable, manipulatable which is not that easy in Unity (JavaScript.. ;n;)

## Level design:
- Puzzle in the sense of having to find a solution to create in the rules
- Puzzle in the sense of understanding the specific actions to take to win with specific rules
- Challenges in the form of dexterity, aiming, speed
  under specific rules

dexterity + Puzzle = "mental gymnastic"

### Puzzle parts & Emergence:
Mapping how the possible rules can combine to create mechanics, puzzle parts or challenges.
It was helpful to then decide which puzzles pieces or challenges combo to feature in a level.
Rules < Mechanics < Level Design
emergence of higher gameplay structures through combination of smaller parts

### Process

Specify which are the aimed Challenges/Puzzles of the level,
start with a first draft, evualate it on clarity/difficulty/levelof information available tothe player at this point
Until satisfied

# Special Thanks to
- Hempuli, dev of the incredible [Baba is You](https://www.hempuli.com/)
- The whole [Super Hot](https://superhotgame.com/superhot-prototype/) team
- André Cardoso's [Superhot Bullet](https://github.com/mixandjam/Superhot-BulletTime) time Unity project
- Caldric Clement for letting me use their [*Baba is You* theme remix](https://www.youtube.com/watch?v=KJd5A739W5E)
- The [Spriter Ressource](https://www.spriters-resource.com/pc_computer/babaisyou/sheet/115231/)
- The person who made this [shader tutorial](https://www.youtube.com/watch?v=GXE0VqH08sc)
- MBoffin for their [Unity controller example](https://github.com/MBoffin/SimpleFPSController) and the Secret Santa coincidences
- scrummer03 for patiently waiting for me to finish this game ;u;


## Outreach

**Comments**

- Do you have the (legal) right to do that ??
- Did not know I wanted this / cool mashup
- Mindblowing



**Articles**

**Streamers**

Very good to see what are the things that work well, that are comprehensible enough, if the learning curve is nice etc.

* Choosing which order to do the levels was useful for at least 1 person who was stuck and then came back

* (Funny memes about Baba, Superhot and even JoJo)

* Level order might be too rough from 1 to 2 but apart from that, seems good => 2 might be too much open-ended with too much implicit

* Some bugs were there (clipping, enemy crouching) but did not render the game unplayable

* Everyone had very surprising ways to finish a level that I didn't expect, and that's GREAT ! Sometimes they are very clever, sometimes the yare totally unexpected but still work

* Curiosity about rules "Can I become Time ? .. oh that would have been cool but what would that mean"

* Feeling that its not just getting the puzzle solution but also overcoming a challenge: Great

* Feeling that's its both short but also self-sufficient

* Appreciated the music transition!

* Confusion about number of bullets and up to which point a gun can be picked but players can get a sense of that

* Experimentation loop

* Players have Idea for levels! a little desire to experiment more
