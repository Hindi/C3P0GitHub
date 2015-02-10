using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

static class IdConverter
{
    /// <summary>
    /// Converts a StateEnum into an EnumGame.
    /// </summary>
    public static EnumGame stateToGame(StateEnum state)
    {
        switch(state)
        {
            case StateEnum.SPACEWAR:
                return EnumGame.SPACEWAR;
            case StateEnum.SPACEINVADER:
                return EnumGame.SPACEINVADER;
            case StateEnum.PONG:
                return EnumGame.PONG;
            case StateEnum.LUNARLANDER:
                return EnumGame.LUNARLANDER;
            case StateEnum.PACMAN:
                return EnumGame.PACMAN;
            case StateEnum.TETRIS:
                return EnumGame.TETRIS;
            case StateEnum.MARIO:
                return EnumGame.MARIO;
            case StateEnum.ASTEROIDS:
                return EnumGame.ASTEROIDS;
            case StateEnum.ZELDA:
                return EnumGame.ZELDA;
            case StateEnum.MOONPATROL:
                return EnumGame.MOONPATROL;
            default:
                return (EnumGame)(-1);
        }
    }

    /// <summary>
    /// Converts a level name into an EnumGame.
    /// </summary>
    public static EnumGame levelToGame(string lvlname)
    {
        return stateToGame(levelToState(lvlname));
    }

    /// <summary>
    /// Converts a course ID to a game
    /// </summary>
    /// <param name="id">The number of the course</param>
    /// <returns>The game associated to this ID</returns>
    public static EnumGame courseToGame(int id)
    {
        switch(id)
        {
            case 1:
                return EnumGame.SPACEWAR;
            case 2:
                return EnumGame.PONG;
            case 3:
                return EnumGame.SPACEINVADER;
            case 4:
                return EnumGame.ASTEROIDS;
            case 5:
                return EnumGame.LUNARLANDER;
            case 6:
                return EnumGame.PACMAN;
            case 7:
                return EnumGame.TETRIS;
            case 8:
                return EnumGame.MOONPATROL;
            case 9:
                return EnumGame.MARIO;
            case 10:
                return EnumGame.ZELDA;
            default:
                return (EnumGame)(-1);
        }
    }

    /// <summary>
    /// Returns the ID to which a game is associated
    /// </summary>
    /// <param name="game">The game we want to know the course ID of</param>
    /// <returns>The course ID</returns>
    public static int gameToID(EnumGame game)
    {
        switch (game)
        {
            case EnumGame.SPACEWAR:
                return 1;
            case EnumGame.PONG:
                return 2;
            case EnumGame.SPACEINVADER:
                return 3;
            case EnumGame.ASTEROIDS:
                return 4;
            case EnumGame.LUNARLANDER:
                return 5;
            case EnumGame.MOONPATROL:
                return 8;
            case EnumGame.PACMAN:
                return 6;
            case EnumGame.TETRIS:
                return 7;
            case EnumGame.MARIO:
                return 9;
            case EnumGame.ZELDA:
                return 10;
            default:
                return -1;
        }
    }

    public static string courseToLevel(int id)
    {
        switch (id)
        {
            case 1:
                return "SpaceWar";
            case 2:
                return "Pong";
            case 3:
                return "SpaceInvader";
            case 4:
                return "Asteroids";
            case 5:
                return "LunarLander";
            case 6:
                return "Pacman";
            case 7:
                return "Tetris";
            case 8:
                return "MoonPatrol";
            case 9:
                return "Mario";
            case 10:
                return "Zelda";
            default:
                return "";
        }
    }

    /// <summary>
    /// Converts a level name into a StateEnum.
    /// </summary>
    public static StateEnum levelToState(string lvlName)
    {
        switch(lvlName)
        {
            case "SpaceWar":
                return StateEnum.SPACEWAR;
            case "SpaceInvader":
                return StateEnum.SPACEINVADER;
            case "Pong":
                return StateEnum.PONG;
            case "LunarLander":
                return StateEnum.LUNARLANDER;
            case "Pacman":
                return StateEnum.PACMAN;
            case "Tetris":
                return StateEnum.TETRIS;
            case "Mario":
                return StateEnum.MARIO;
            case "Asteroids":
                return StateEnum.ASTEROIDS;
            case "Zelda":
                return StateEnum.ZELDA;
            case "MoonPatrol":
                return StateEnum.MOONPATROL;
            case "ServerLobby":
                return StateEnum.SERVERCONNECTION;
            case "Connection":
                return StateEnum.CONNECTION;
            case "QuestionAnswer":
                return StateEnum.QUESTIONANSWER;
            default:
                return (StateEnum)(-1);
        }
    }
}
