using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


[System.Serializable]
public class GameList
{
    public GameList()
    {
        md5List = new List<string>();
    }

    public List<string> md5List;
}

public class GameChoiceMenu : MonoBehaviour {

    [System.Serializable]
    public class EnumGameDict
    {
        public EnumGame game;
        public Button button;
    }
    [SerializeField]
    private List<EnumGameDict> buttonDict;

    GameList gameList;
    private List<EnumGame> availableGames;

    [SerializeField]
    private LevelLoader levelLoader;


	// Use this for initialization
	void Start ()
    {
        loadGameList();
        EventManager<string>.AddListener(EnumEvent.ADDGAME, onGameAdded);
	}

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

        foreach (EnumGame e in availableGames)
            foreach (EnumGameDict p in buttonDict)
                if (p.game == e)
                    p.button.gameObject.SetActive(true);
    }

    private void checkAndAddGame(string clearName, string hash)
    {
        if (Crypto.verifyMd5(clearName, hash))
            availableGames.Add(IdConverter.levelToGame(clearName));
    }

    public void onGameAdded(string name)
    {
        EnumGame g = IdConverter.levelToGame(name);
        if (!availableGames.Contains(g))
            availableGames.Add(g);
        gameList.md5List.Add(Crypto.encryptMd5(name));
        BinarySerializer.SerializeData(gameList);
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void loadLevel(string level)
    {
        levelLoader.loadLevel(level);
    }
}
