using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


/**
 * All of the possible event must be listed here in order to be used.
 * 
 */


public enum EnumEvent
{
    PLAYSOUND,
    LOADLEVEL,
    LOADEND,
    QUESTIONRCV,
    QUESTIONRESULT,
    ANSWERSELECT,
    ANSWERRCV,
	ENEMYDEATH,
	GAMEOVER,
	PAUSEGAME,
    RESTARTGAME,
    CHANGEPARAM,
    CONNECTIONSTATE,
    CONNECTIONTOUNITY,
    DISCONNECTFROMUNITY,
	CLOSEMENU,
	SERVERUI,
    AUTHFAILED,
    AUTHSUCCEEDED,
    SCOREUPDATE
};
