using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

static class IdConverter
{
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
}
