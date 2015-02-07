using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
///  All of the possible event must be listed here in order to be used.
/// </summary>
public enum EnumEvent
{
    PLAYSOUND,              //Play a sound
    LOADLEVEL,              //Load unity scene + change state
    QUESTIONRCV,            //When a question is recieved
    QUESTIONRESULT,         //When the result is recieved
    ANSWERSELECT,           //Answer selected
    ANSWERRCV,              //Answer recieved
	ENEMYDEATH,             //Enemy died
    SPACESHIPDESTROYED,     //Spaceship destroyed
	GAMEOVER,               //Game ends (win or lose)
	PAUSEGAME,              //Game paused
    RESTARTGAME,            //Restart the game
    CHANGEPARAM,            //Parameter changed
    CONNECTIONSTATE,        //Jump to sonnection state
    CONNECTIONTOUNITY,      //Connection to unity succedeed
    DISCONNECTFROMUNITY,    //Disconnected from unity
	CLOSEMENU,              //Close all menus
	SERVERUI,               //Server menu open
    AUTHFAILED,             //Authentication failed
    AUTHSUCCEEDED,          //Authentication succeeded
    SCOREUPDATEQA,          //Question score updated
    SERVERIPRECEIVED,       //Recieved the server IP (udp broadcast)
    PRINTONSCREEN,          //Print a string on screen (mobile phone debug)
    ADDGAME,                //Unlock a game for the client's solo
	ENCOUNTER,				//When the player collides with something
	MINIGAME_START,			//Minigame starts
	MINIGAME_WIN,			//The minigame is won
	MINIGAME_LOST,			//The minigame is lost
	MINIGAME_TO,			//Minigame timeout
	MINIGAME_TERMINATE,		//Minigame stops
	SCATTERMODE,			//Activate scatter mode (Pacman)
	FRIGHTENED,				//Activate frighten mode (Pacman)
	MOVING,					//Movement allowed
	SENTENCE_LOST,			//When the minigame is lost
	SENTENCE_TO,			//When the time for the minigame is over
	SENTENCE_WIN,			//When the minigame is lost
	GHOST_EATEN,			//A ghost is eaten (Pacman)
	DOT_EATEN,				//A dot is eaten (Pacman)
    RESTARTSTATE,			//Reset the state of each object
    GOODANSWERRATIO,		//The client recieved the good answer ratio
    UPDATEGAMESCORE         //Notify the gamestate that the score got updated
};
