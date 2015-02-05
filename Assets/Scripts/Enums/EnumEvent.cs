using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
///  All of the possible event must be listed here in order to be used.
/// </summary>
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
    SPACESHIPDESTROYED,
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
    SCOREUPDATEQA,
    SERVERIPRECEIVED,
    PRINTONSCREEN,
    ADDGAME
};
