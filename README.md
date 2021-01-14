
Source code repository for **SUPER IS HOT**.   

Download it now at [http://pyrofoux.itch.io/super-is-hot](http://pyrofoux.itch.io/super-is-hot) !

## SUPER IS HOT

<p><strong>&nbsp;a SUPERHOT ❌ Baba is You crossover game</strong></p>
<p>Break the rules of time and rearrange them to get yourself out of tricky situations. Or at least, die trying.</p>
<p><strong>CONTROLS</strong></p>
<ul><li>Move: WASD / ZQSD / Arrow keys</li><li>Jump: Space</li><li>Shoot: Left click</li><li>Throw gun: Right click</li><li>E: Edit Rules</li><li>R: Restart</li><li>U: Undo</li><li>Escape: Main menu / Quit</li></ul>
<p>Basic joystick support (tested with an Xbox controller on Windows)<br></p>
<p><em>« It's the most innovative shooter I've painfully debugged in years »</em><br></p>
<p>Made by <a href="https://twitter.com/Pyrofoux">Pyrofoux</a> for 7DFPS, ProcJam and Secret Santa Jam.<br><br></p>
<p><strong>Special Thanks to</strong><br></p>
<ul><li><a href="https://www.hempuli.com/">Hempuli</a>, dev of the incredible Baba is You (original inspiration, graphics and music)</li><li>The whole <a href="https://superhotgame.com/superhot-prototype/">Super Hot</a> team (original inspiration, graphics and sounds)</li><li>André Cardoso's Superhot Bullet time <a href="https://github.com/mixandjam/Superhot-BulletTime">Unity project</a></li><li>Caldric Clement for letting me use their <a href="https://www.youtube.com/watch?v=KJd5A739W5E">Baba is You theme remix</a></li><li>The <a href="https://www.spriters-resource.com/pc_computer/babaisyou/sheet/115231/">Spriter Ressource</a> for their super handy sprites</li><li>The person who made this <a href="https://www.youtube.com/watch?v=GXE0VqH08sc">shader tutorial</a></li><li>MBoffin for their <a href="https://github.com/MBoffin/SimpleFPSController">Unity controlle</a>r example and the Secret Santa coincidences</li><li>scrummer03 for patiently waiting for me to finish this game (;u;)</li></ul>
<p></p>
<p></p>

## How to make custom levels?

- Copy an existing level scene and name "Level-X" with `X` being the number of the level
- Add the new scene to the build, or it won't be able to be loaded by the game
- Edit the FPS scene by adding pre-existing prefabs to the scene (enemies, guns, tables, walls)
- Add a new string for the level in LevelHandler.cs
- Each character in the string is mapped to a tile in the Baba world, look them up in previous levels!
- If you edit more than the first 6 levels, you might have to hack the level selection screen ;)

If you have questions, reach out to me on my [Twitter](https://twitter.com/Pyrofoux) or by mail, at :

![hi my mail is 'yrabii' dot 'X' at gmail.com and X is the word 'eggs', sorry for the inconvenience](./Com/mail.png)
