using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using Microsoft.Xna.Framework;
using XnaGamesInfrastructure.ObjectModel;
using SpaceInvadersGame.ObjectModel;

namespace SpaceInvadersGame
{
    /// <summary>
    /// The delegate is used by the InvadersMatrix to inform that all
    /// invaders in the matrix are dead
    /// </summary>
    public delegate void NoRemainingInvadersDelegate();

    /// <summary>
    /// The delegate is used by the InvadersMatrix to inform that a certain
    /// invader in the matrix reached the allowed Y position
    /// </summary>
    public delegate void InvaderReachedScreenEndDelegate();

    /// <summary>
    /// Holds all the invaders in the game and control their moves
    /// </summary>
    public class InvadersMatrix : GameComponent
    {
        private const int k_EnemiesInLineNum = 9;
        private const int k_NumOfEnemiesLines = 5;

        private const int k_EnemyWidth = 32;
        private const int k_EnemyHeight = 32;

        private const int k_EnemyMotionYVal = 16;

        // The percent will decrease in the time it takes the enemies 
        // to move. used to increase the enemies speed
        private const float k_IncreaseEnemiesSpeedFactor = .85f;

        // The time we want to wait between two enemies shoots
        private readonly TimeSpan r_DefaultTimeBetweenShots = TimeSpan.FromSeconds(1.5f);

        public event NoRemainingInvadersDelegate AllInvaderssEliminated;

        public event InvaderReachedScreenEndDelegate InvaderReachedScreenEnd;        

        private bool m_ChangeInvadersDirection = false;

        // A time counter that contains a random seconds for the time space between
        // the enemies shoots
        private TimeSpan m_PrevShotTime;               

        private int m_RemainigEnemiesNum;

        // A two dimentional array that represents the enemies matrix.
        // each cell in the matrix contains the type of the enemy that will
        // be dinamically created (using reflection) later on
        private Type[,] m_EnemiesMatrix = new Type[k_NumOfEnemiesLines, k_EnemiesInLineNum] 
                                            { 
                                              { typeof(PinkInvader), typeof(PinkInvader), 
                                                typeof(PinkInvader), typeof(PinkInvader), 
                                                typeof(PinkInvader), typeof(PinkInvader), 
                                                typeof(PinkInvader), typeof(PinkInvader), 
                                                typeof(PinkInvader) },                         
                                              { typeof(BlueInvader), typeof(BlueInvader), 
                                                typeof(BlueInvader), typeof(BlueInvader), 
                                                typeof(BlueInvader), typeof(BlueInvader), 
                                                typeof(BlueInvader), typeof(BlueInvader), 
                                                typeof(BlueInvader) },                         
                                              { typeof(BlueInvader), typeof(BlueInvader), 
                                                typeof(BlueInvader), typeof(BlueInvader), 
                                                typeof(BlueInvader), typeof(BlueInvader), 
                                                typeof(BlueInvader), typeof(BlueInvader), 
                                                typeof(BlueInvader) },               
                                              { typeof(YellowInvader), typeof(YellowInvader), 
                                                typeof(YellowInvader), typeof(YellowInvader), 
                                                typeof(YellowInvader), typeof(YellowInvader), 
                                                typeof(YellowInvader), typeof(YellowInvader), 
                                                typeof(YellowInvader) },                         
                                              { typeof(YellowInvader), typeof(YellowInvader), 
                                                typeof(YellowInvader), typeof(YellowInvader), 
                                                typeof(YellowInvader), typeof(YellowInvader), 
                                                typeof(YellowInvader), typeof(YellowInvader), 
                                                typeof(YellowInvader)
                                              }
                                            };

        // The invaders matrix
        private List<List<Invader>> m_Enemies;

        private float m_MaxInvadersYPositionYVal;

        public InvadersMatrix(Game i_Game) : base(i_Game)
        {
            m_Enemies = new List<List<Invader>>();
            m_PrevShotTime = r_DefaultTimeBetweenShots;

            m_RemainigEnemiesNum = k_EnemiesInLineNum * k_NumOfEnemiesLines;            
        }

        /// <summary>
        /// A property for setting the maximum value an invader is allowed
        /// to reach in the Y axis    
        /// </summary>
        public float    InvaderMaxPositionY
        {
            set
            {
                m_MaxInvadersYPositionYVal = value;

                updateInvadersMaxYValue(); 
            }
        }

        /// <summary>
        /// Initialize the component by creating the enemies matrix and initialize
        /// the invaders maximum Y value to be the screen height
        /// </summary>
        public override void    Initialize()
        {
            base.Initialize();

            m_MaxInvadersYPositionYVal = Game.GraphicsDevice.Viewport.Height;

            initInvadersList();            
        }

        /// <summary>
        /// Initialize the enemies matrix
        /// </summary>
        private void    initInvadersList()
        {
            float startingPositionX = 0;

            float startingPositionY = ((float)
                Game.GraphicsDevice.Viewport.Height / 2);

            Vector2 currPosition = new Vector2(startingPositionX, startingPositionY);
            Invader currEnemy;
            Type prevRowType = null;
            int currInvaderRow = 1;

            // Creates all the enemies according to the enemies two dimentional 
            // array            
            for (int i = k_NumOfEnemiesLines - 1; i >= 0; i--)
            {                
                List<Invader> currList = new List<Invader>();
                currInvaderRow = 1;

                for (int j = 0; j < k_EnemiesInLineNum; j++)
                {
                    if (prevRowType != null)
                    {
                        // If it's the first invader in the list, we'll check
                        // if the invader equals the previous one so that will
                        // change the starting texture                        
                        if ((j == 0) &&
                            (prevRowType.Equals(m_EnemiesMatrix[i, j])))
                        {
                            currInvaderRow = 2;
                        }
                    }

                    // Dynamically creates the enemy according to the type in 
                    // the two dimentional array that represents the enemies
                    // matrix
                    currEnemy = (Invader)Activator.CreateInstance(
                                                         m_EnemiesMatrix[i, j], 
                                                         Game,                                                          
                                                         UpdateOrder - 1,
                                                         currInvaderRow);

                    currEnemy.PositionForDraw = currPosition;
                    currEnemy.InvaderMaxPositionY = m_MaxInvadersYPositionYVal;
                    currEnemy.ReachedScreenBounds += new InvaderReachedScreenBoundsDelegate(invader_ReachedScreenBounds);
                    currEnemy.Disposed += invader_Disposed;

                    currList.Add(currEnemy);
                    currPosition.X += k_EnemyWidth * 2;
                    prevRowType = m_EnemiesMatrix[i, j];
                }

                currPosition.Y -= k_EnemyHeight;
                currPosition.Y -= (k_EnemyHeight / 2);
                currPosition.X = startingPositionX;
                m_Enemies.Add(currList);                
            }
        }

        /// <summary>
        /// Change the invaders matrix by changing their Y position, increase
        /// their moving speed, and change their moving direction on the X axis
        /// </summary>
        /// <param name="i_YMotionFactor">The factor that we want to move the 
        /// enemies in the Y axis by</param>
        private void    changeInvadersMatrixPositions(float i_YMotionFactor)
        {
            // Move on the entire enemies matrix and change the enemy position
            // by the given factor
            foreach (List<Invader> enemies in m_Enemies)
            {
                foreach (Invader enemy in enemies)
                {
                    // Increase the number of times the enemy moves in a second
                    // (by that we increase the invaders speed)
                    TimeSpan moveTime = TimeSpan.FromSeconds(enemy.TimeBetweenMoves.TotalSeconds * 
                                                             k_IncreaseEnemiesSpeedFactor);
                    enemy.TimeBetweenMoves = moveTime;

                    // Change the Y position so that the enemy will go down                                                            
                    Vector2 position = enemy.PositionForDraw; 
                    position.Y += i_YMotionFactor;
                    enemy.PositionForDraw = position;

                    // Change the enemy direction
                    enemy.SwitchPosition();
                }
            }
        }

        /// <summary>
        /// Updates the invaders state in the game by randomly releasing a shoot
        /// from one of the invaders every a couple of seconds
        /// </summary>
        /// <param name="i_GameTime">Provides a snapshot of timing values.</param>
        public override void    Update(GameTime i_GameTime)
        {
            base.Update(i_GameTime);

            m_PrevShotTime -= i_GameTime.ElapsedGameTime;

            if (m_PrevShotTime.TotalSeconds < 0)
            {
                shootThePlayer();
                m_PrevShotTime = r_DefaultTimeBetweenShots;
            }

            // In case an enemy reached the end of the screen width, we
            // need to change the invaders Y position, change their
            // X motion, and increase their moving speed
            if (m_ChangeInvadersDirection)
            {                
                changeInvadersMatrixPositions(k_EnemyMotionYVal);
                m_ChangeInvadersDirection = false;
            }            
        }             

        /// <summary>
        /// Raise an AllEnemiesEliminated event, stating that there are no more
        /// enemies in the screen
        /// </summary>
        private void    onAllEnemiesEliminated()
        {
            if (AllInvaderssEliminated != null)
            {
                AllInvaderssEliminated();
            }
        }

        /// <summary>
        /// Catch the ReachedScreenBounds event raised by an invader.
        /// The method changes the enemies movement direction in the X axis, 
        /// change the enemies position in the Y axis by moving them down in 
        /// the screen and increases the invaders moving speed
        /// </summary>
        /// <param name="i_Invader">The invader that raised the event</param>
        public void     invader_ReachedScreenBounds(Invader i_Invader)
        {            
            // If the invader reached the maximum allowed Y position, than
            // we need to raise an InvaderReachedScreenEnd event
            if (!(i_Invader.Bounds.Bottom >= m_MaxInvadersYPositionYVal))
            {
                m_ChangeInvadersDirection = true;
            }
            else
            { 
                onInvaderReachedScreenEnd();
            }
        }

        /// <summary>
        /// Raise an EnemyReachedScreenEnd event when a certain enemy in the 
        /// enemies matrix reaches the maximum allowed Y value
        /// </summary>
        private void onInvaderReachedScreenEnd()
        {
            if (InvaderReachedScreenEnd != null)
            {
                InvaderReachedScreenEnd();
            }
        }

        /// <summary>
        /// Randomly choose an enemy to shoot
        /// </summary>
        private void    shootThePlayer()
        {
            Random rand = new Random();

            // In case there are enemies we'll shoot the player from
            // a random enemy
            if (m_Enemies.Count > 0)
            {
                // Randomly choose an enemy from the enemies matrix                
                int enemyMatrixLine = rand.Next(0, m_Enemies.Count - 1);
                int enemyMatrixColumn = rand.Next(0, m_Enemies[enemyMatrixLine].Count - 1);                                
                m_Enemies[enemyMatrixLine][enemyMatrixColumn].Shoot();
            }
        }
       
        /// <summary>
        /// Catch an invader disposed event, remove it from the matrix and in
        /// case there are no enemies left raise an event
        /// </summary>
        /// <param name="i_Sender">The disposed enemy</param>
        /// <param name="i_EventArgs">The event arguments</param>
        private void    invader_Disposed(object i_Sender, EventArgs i_EventArgs)
        {
            Invader enemy = i_Sender as Invader;

            removeInvaderFromMatrix(enemy);

            m_RemainigEnemiesNum--;

            if (m_RemainigEnemiesNum <= 0)
            {
                onAllEnemiesEliminated();
            }
        }

        /// <summary>
        /// Removes an invader from the invaders matrix
        /// </summary>
        /// <param name="i_Enemy">The enemy that we want to remove from the matrix</param>
        private void    removeInvaderFromMatrix(Invader i_Enemy)
        {
            foreach (List<Invader> enemiesLine in m_Enemies)
            {
                if (enemiesLine.Contains(i_Enemy))
                {
                    enemiesLine.Remove(i_Enemy);

                    // In case it was the last invader in the line we'll remove 
                    // the entire invaders line from the matrix
                    if (enemiesLine.Count == 0)
                    {
                        m_Enemies.Remove(enemiesLine);
                    }

                    break;
                }
            }
        }

        /// <summary>
        /// Update the invaders max Y position for all the invaders in the 
        /// matrix
        /// </summary>
        private void    updateInvadersMaxYValue()
        {
            foreach (List<Invader> enemiesLine in m_Enemies)
            {
                foreach(Invader invader in enemiesLine)
                {
                    invader.InvaderMaxPositionY = m_MaxInvadersYPositionYVal;
                }
            }
        }
    }
}