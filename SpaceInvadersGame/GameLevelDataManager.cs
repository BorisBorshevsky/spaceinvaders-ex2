using System;
using System.Collections.Generic;
using System.Text;
using SpaceInvadersGame.ObjectModel;
using XnaGamesInfrastructure.ObjectModel;
using SpaceInvadersGame.Interfaces;
using Microsoft.Xna.Framework;

namespace SpaceInvadersGame
{
    /// <summary>
    /// Manages all the level data objects
    /// </summary>
    public class GameLevelDataManager : GameService, IGameLevelDataManager
    {
        private const int k_LevelsNum = 5;
        private const int k_IncreaseLevelScoreVal = 50;
        private const int k_MotherShipScore = 500;
        private const int k_YellowInvaderScore = 100;
        private const int k_BlueInvaderScore = 200;
        private const int k_PinkInvaderScore = 300;
        private const int k_Barrier1LevelSpeed = 0;
        private const int k_Barrier2LevelSpeed = 40;
        private const float k_IncreaseBarrierSpeed = .25f;
        private const int k_InvadersColumnNum = 9;        
        private const float k_TimeBetweenInvadersShootsFactor = .05f;
        private readonly TimeSpan r_DefaultTimeBetweenShots = 
            TimeSpan.FromSeconds(.75f);

        private GameLevelData[] m_LevelsData;

        public GameLevelDataManager(Game i_Game) 
            : base(i_Game, Int32.MinValue)
        {
            m_LevelsData = new GameLevelData[k_LevelsNum];
            initLevelsData();
        }

        /// <summary>
        /// Creates a score map of all the invaders scores in the next level
        /// according to the current level map
        /// </summary>
        /// <param name="i_CurrentLevelInvadersMap">A map of the current
        /// level scores used to create the new scores map</param>
        /// <returns>A map of the invaders score for the next level</returns>
        private Dictionary<eInvadersType, int>    createNextLevelScoreMap(
            Dictionary<eInvadersType, int> i_CurrentLevelInvadersMap)
        {
            Dictionary<eInvadersType, int> retVal = new Dictionary<eInvadersType, int>();

            foreach (KeyValuePair<eInvadersType, int> key in i_CurrentLevelInvadersMap)
            {
                retVal[key.Key] = key.Value + k_IncreaseLevelScoreVal;
            }

            return retVal;
        }

        /// <summary>
        /// Gets a map of the invaders score in the first level
        /// </summary>
        /// <returns>A map of the invaders type score in the first game 
        /// level</returns>
        private Dictionary<eInvadersType, int>  getFirstLevelScoreMap()
        {
            Dictionary<eInvadersType, int> retVal = 
                new Dictionary<eInvadersType, int>();

            retVal.Add(eInvadersType.YellowInvader, k_YellowInvaderScore);
            retVal.Add(eInvadersType.BlueInvader, k_BlueInvaderScore);
            retVal.Add(eInvadersType.PinkInvader, k_PinkInvaderScore);

            return retVal;
        }

        /// <summary>
        /// Initialize the level data array
        /// </summary>
        private void    initLevelsData()
        {
            Dictionary<eInvadersType, int> currLevelInvadersScore =
                getFirstLevelScoreMap();

            TimeSpan currInvadersShootsTime =
                    r_DefaultTimeBetweenShots;

            // Create first level game data
            m_LevelsData[0] = new GameLevelData(
                k_Barrier1LevelSpeed,
                k_InvadersColumnNum,
                k_MotherShipScore,
                getFirstLevelScoreMap(),
                currInvadersShootsTime,
                Constants.k_AllowedInvadersBulletsNum);            

            // Create all the rest game levels data
            for (int i = 1; i < k_LevelsNum; i++)
            {
                currLevelInvadersScore =
                    createNextLevelScoreMap(currLevelInvadersScore);                
                currInvadersShootsTime -= 
                    TimeSpan.FromSeconds(k_TimeBetweenInvadersShootsFactor);

                m_LevelsData[i] = new GameLevelData(
                    k_Barrier2LevelSpeed + (int)(k_IncreaseBarrierSpeed * (i - 1)),
                    k_InvadersColumnNum + i,
                    k_MotherShipScore + (i * k_IncreaseLevelScoreVal),
                    currLevelInvadersScore,
                    currInvadersShootsTime,
                    Constants.k_AllowedInvadersBulletsNum + i);
            }
        }

        /// <summary>
        /// Read only indexer that returns a game level data
        /// </summary>
        /// <param name="i_LevelNum">The level number that we want to get
        /// the data for</param>
        /// <returns>The game data of the given level num</returns>
        public GameLevelData    this[int i_LevelNum]
        {
            get 
            {
                int levelNum = (i_LevelNum % k_LevelsNum) - 1;
                levelNum = (levelNum >= 0) ?
                    levelNum : k_LevelsNum + levelNum;

                return m_LevelsData[levelNum];
            }
        }
    }
}
