# Amazing 3D TicTacToe
## Installation
You can install Amazing TicTacToe3D from [Releases Page](https://github.com/mentapro/TicTacToe3D/releases).
 - **TicTacToe3D_alpha.zip** - unzip it and launch *TicTacToe3D.exe*.

## About game
I made this game with Unity3D. Extra libraries: [Zenject for Unity3D](https://github.com/modesttree/Zenject),
[Newtonsoft JSON](http://www.newtonsoft.com/json).
### Features
- Several players can play this game.
- There are no *Xs* or *Os* here, but you can choose some color for your badge.
- All parameters are customizable: 
  - *Dimension*
  - *Step size*
  - *Badges to win*
  - *Count of players*
  - *Timer type*
  - *and many others!*
- Implemented **Artificial Intelligence**.
- There is a calculation of statistics for each player.
- The ability to save and load the game.
- Very beautiful and **modern user interface** (Space theme)
- Sound accompaniment.

### Code description
- **MVP** pattern is the basis of this game.
- Fully implemented the principles of **SOLID**.
- All dependencies are injected with Zenject.
- No Unity3D's `Update`, `Start`, `Awake` abuse.
  - And in general, the use of `UnityEngine` library is minimal.
- Implemented **service-oriented architecture**, **event-driven architecture**.
- Basic **state-driven design**.
- Сode is re-used.
- There is an opportunity to expand the project easily.

### Preferences description
- *Dimension* - The dimension of the playing field along the three axes.
- *Step Size* - The number of badges that player can put in one turn.
- *Badges To Win* - The number of badges needed to win.
- *Step Confirmation* - The user will have to confirm each step. This option also allows you to cancel your current move.
- *Game Over After First Winner* - If selected, the game will end after the first winner, even if other players can still play.
- *Timer Types*:
  - *None* - Timer has no effect.
  - *Fixed time per step* - Each player has the same amount of time per turn.
  - *Fixed time per round* - Each player has the same amount of time for the whole game.
  - *Dynamic time* - Time for each step will be calculated depending on the game progress. (Not implemented yet)

## Game guide
### Main Menu Scene
In the `Main Menu` you can see the main game menus. To play on one PC - select `New Game`.
![TicTacToe 3D Main Menu](https://c1.staticflickr.com/5/4218/35082438052_2b623fd9db_o.png)
In `New Game Menu` you can choose how many players will play, select name and badge color.
You can also change the default settings to custom settings in `Advanced Panel`.
![TicTacToe 3D New Game menu](https://c1.staticflickr.com/5/4202/35247841085_7935f62491_o.png)
Click the `Start Game` button after setting all parameters and play!

### Game Board Scene
Be careful - the computer plays very well! :)

Put your badge, wait for other players and go on!
![TicTacToe 3D Playing process](https://c1.staticflickr.com/5/4223/35082742452_e374a97941_o.png)

## My Contacts
- [Facebook - Stanislav Herasymiuk](https://www.facebook.com/stanislav.herasymiuk)
- [LinkedIn - Stanislav Herasymiuk](https://www.linkedin.com/in/mentapro/)
