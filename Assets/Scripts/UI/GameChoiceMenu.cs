using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


/// <summary>A serializable class that contains the list of the game the client can play to.</summary>
[System.Serializable]
public class GameList
{
    /// <summary>Default constructor.</summary>
    public GameList()
    {
        md5List = new List<string>();
    }

    /// <summary>The list containing the md5 names.</summary>
    public List<string> md5List;
}

/// <summary>The class of the menu than displays the list of the games.</summary>
public class GameChoiceMenu : MonoBehaviour {

    /// <summary>Inher class used to simplify the set up through the editor.</summary>
    [System.Serializable]
    public class EnumGameDict
    {
        public EnumGame game;
        public Button button;
    }

    /// <summary>This list is used as a dictionnary but can be used in the editor. It contains the button that will be activated is the game is available.</summary>
    [SerializeField]
    private List<EnumGameDict> buttonDict;

    /// <summary>The object containing the game list. This object is here to link the instance datas with the xml.</summary>
    GameList gameList;

    /// <summary>Called if the server is recieved via udp.</summary>
    private List<EnumGame> availableGames;

    /// <summary>Reference to the level loader object.</summary>
    [SerializeField]
    private LevelLoader levelLoader;


	// Use this for initialization
	void Start ()
    {
        loadGameList();
        EventManager<string>.AddListener(EnumEvent.ADDGAME, onGameAdded);
	}

    /// <summary>Loads the datas from the xml. And activates the buttons.</summary>
    /// <returns>void</returns>
    private void loadGameList()
    {
        availableGames = new List<EnumGame>();
        try
        {
            gameList = BinarySerializer.DeserializeData();
        }
        catch
        {
            gameList = new GameList();
        }

        foreach(string s in gameList.md5List)
        {
            checkAndAddGame("SpaceWar", s);
            checkAndAddGame("Pong", s);
            checkAndAddGame("SpaceInvader", s);
            checkAndAddGame("Tetris", s);
            checkAndAddGame("MoonPatrol", s);
            checkAndAddGame("Asteroids", s);
            checkAndAddGame("LunarLander", s);
            checkAndAddGame("Mario", s);
            checkAndAddGame("Zelda", s);
            checkAndAddGame("Pacman", s);
        }
        onGameAdded("Tetris");
        onGameAdded("Pong");
        onGameAdded("SpaceWar");
        onGameAdded("LunarLander");
        updateactiveButton();
    }

    /// <summary>Compares the available list and the button list to activate the buttons.</summary>
    /// <returns>void</returns>
    void updateactiveButton()
    {
        foreach (EnumGame e in availableGames)
            foreach (EnumGameDict p in buttonDict)
                if (p.game == e)
                    p.button.gameObject.SetActive(true);
    }


    /// <summary>Compares an entry with a game name and add it to the available list if it's true.</summary>
    /// <param name="clearName">The clear name to be compared.</param>
    /// <param name="hash">The hashed name.</param>
    /// <returns>void</returns>
    private void checkAndAddGame(string clearName, string hash)
    {
        if (Crypto.verifyMd5(clearName, hash))
            availableGames.Add(IdConverter.levelToGame(clearName));
    }

    /// <summary>Adds a game to the list. The server will activate the games during the course using this function.</summary>
    /// <param name="name">The ame of the game to be activated.</param>
    /// <returns>void</returns>
    public void onGameAdded(string name)
    {
        EnumGame g = IdConverter.levelToGame(name);
        if (!availableGames.Contains(g))
            availableGames.Add(g);
        gameList.md5List.Add(Crypto.encryptMd5(name));
        BinarySerializer.SerializeData(gameList);
        updateactiveButton();
    }

    /// <summary>Called when the client click on a level button.</summary>
    /// <param name="level">The ame of the level to be loaded.</param>
    /// <returns>void</returns>
    public void loadLevel(string level)
    {
        levelLoader.loadLevel(level);
    }
}
