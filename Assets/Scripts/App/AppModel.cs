using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.App
{
    internal class AppModel
    {
        private int currentArea;
        private int currentGame;
        private int currentLevel;

        //  private List<List<List<string>>> gameNames;
        //  private List<List<List<string>>> descriptions;
        //   private List<List<int>> levelsByGame;

        public AppModel()
        {

        }

        //public AppModel(List<int> gamesQuantity, List<List<List<List<string>>>> gamesInfo, List<List<int>> gamesLevels)
        //{
        //    InitDescriptions(gamesQuantity, gamesInfo, gamesLevels);
        //    currentArea = -1;
        //    currentGame = -1;
        //    currentLevel = -1;
        //}

        //private void InitDescriptions(List<int> gamesQuantity, List<List<List<List<string>>>> gamesInfo, List<List<int>> gamesLevels)
        //{
        //    descriptions = new List<List<List<string>>>(gamesInfo.Count);
        //    gameNames = new List<List<List<string>>>(gamesInfo.Count);
        //    levelsByGame = new List<List<int>>(gamesInfo.Count);
        //    for (int area = 0; area < gamesInfo.Count; area++)
        //    {
        //        descriptions.Add(new List<List<string>>(gamesInfo[area].Count));
        //        gameNames.Add(new List<List<string>>(gamesInfo[area].Count));
        //        levelsByGame.Add(new List<int>(gamesInfo[area].Count));
        //        for (int game = 0; game < gamesInfo[area].Count; game++)
        //        {
        //            descriptions[area].Add(new List<string>(2));
        //            descriptions[area][game].Add(gamesInfo[area][game][1][0]);
        //            descriptions[area][game].Add(gamesInfo[area][game][1][1]);

        //            gameNames[area].Add(new List<string>(2));
        //            gameNames[area][game].Add(gamesInfo[area][game][0][0]);
        //            gameNames[area][game].Add(gamesInfo[area][game][0][1]);

        //            levelsByGame[area].Add(gamesLevels[area][game]);
        //        }
        //    }
        //}

        //internal List<List<int>> GetLevelsOfGames()
        //{
        //    return levelsByGame;
        //}

        internal int GetCurrentGame(){
            return currentGame;
        }

        //internal int GetLevelsOfCurrentGame()
        //{
        //    return levelsByGame[currentArea][currentGame];
        //}

        internal void SetCurrentGame(int currentGame){
            this.currentGame = currentGame;
        }

        internal int GetCurrentLevel()
        {
            return currentLevel;
        }

        internal void SetCurrentLevel(int level)
        {
            currentLevel = level;
        }

        internal int GetCurrentArea()
        {
            return currentArea;
        }

        internal void SetCurrentArea(int area)
        {
            this.currentArea = area;
        }

        //internal List<List<List<string>>> GetGameNames()
        //{
        //    return gameNames;
        //}
    }
}