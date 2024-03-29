
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace XnaGamesInfrastructure.ObjectModel.Animations.ConcreteAnimations
{
    /// <summary>
    /// Animate a sprite texture by changing between the sprite frames
    /// </summary>
    public class CelAnimation : SpriteAnimation
    {
        private const int k_DefaultStartingCel = 0;

        private TimeSpan m_FrameLength;
        private TimeSpan m_TimeLeftForFrame;
        private bool m_Loop = true;
        private int m_CurrCell = 0;
        private int m_StartCell = 0;
        private int m_NumOfCels = 1;        

        public CelAnimation(
            string i_Name,
            TimeSpan i_FrameLength,
            int i_NumOfCels,
            TimeSpan i_AnimationLength)
            : this(
                i_Name,
                i_FrameLength, 
                i_NumOfCels, 
                i_AnimationLength,
                k_DefaultStartingCel)
        {
        }

        public CelAnimation(
            string i_Name,
            TimeSpan i_FrameLength, 
            int i_NumOfCels, 
            TimeSpan i_AnimationLength,
            int i_StartingCel)
            : base(i_Name, i_AnimationLength)
        {
            this.m_FrameLength = i_FrameLength;
            this.m_TimeLeftForFrame = i_FrameLength;
            this.m_NumOfCels = i_NumOfCels;
            this.m_CurrCell = i_StartingCel;
            this.m_StartCell = i_StartingCel;

            m_Loop = i_AnimationLength == TimeSpan.Zero;
        }

        /// <summary>
        /// Move the current frame to the next one in the texture
        /// </summary>
        public void     NextFrame()
        {
            m_CurrCell++;
            if (m_CurrCell >= m_NumOfCels)
            {
                if (m_Loop)
                {
                    m_CurrCell = 0;
                }
                else
                {
                    m_CurrCell = m_NumOfCels - 1; // lets stop at the last frame
                    this.IsFinished = true;
                }
            }
        }

        /// <summary>
        /// Resets the animation.
        /// </summary>
        /// <param name="i_AnimationLength">The time we want to perform
        /// the animation</param>
        public override void    Reset(TimeSpan i_AnimationLength)
        {
            base.Reset(i_AnimationLength);

            m_CurrCell = m_StartCell;
            calcSourceRectangle();
        }        

        /// <summary>
        /// Implments the animation logic
        /// </summary>
        /// <param name="i_GameTime">A snapshot of the current game time
        /// </param>
        protected override void     DoFrame(GameTime i_GameTime)
        {
            if (m_FrameLength != TimeSpan.Zero)
            {
                m_TimeLeftForFrame -= i_GameTime.ElapsedGameTime;
                if (m_TimeLeftForFrame.TotalSeconds <= 0)
                {
                    // we have elapsed, so change frame
                    NextFrame();
                    m_TimeLeftForFrame = m_FrameLength;
                }
            }

            if (this.BoundSprite.SourceRectangle != null)
            {
                calcSourceRectangle();
            }
        }

        /// <summary>
        /// Calculte the new sprite source rectangle according to the 
        /// current cell
        /// </summary>
        private void    calcSourceRectangle()
        {
            Rectangle r = (Rectangle)this.BoundSprite.SourceRectangle;
            this.BoundSprite.SourceRectangle = new Rectangle(
                m_CurrCell * r.Width,
                r.Top,
                r.Width,
                r.Height);
        }
    }
}
