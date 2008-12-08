using System;
using System.Collections.Generic;
using System.Text;
using XnaGamesInfrastructure.ObjectModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XnaGamesInfrastructure.ObjectInterfaces;

namespace SpaceInvadersGame.ObjectModel
{
    public class Barrier : CollidableSprite
    {
        private const string k_AssetName = @"Content\Sprites\Barrier_44x32";

        private const int k_XMotionSpeed = 100;

        private bool m_FirstUpdate = true;
        private float m_MaxXValue = 0;
        private float m_MinXValue = 0;

        List<int> m_CollidingPixels = new List<int>();
        private int m_MinCollidingPixel;

        public Barrier(Game i_Game)
            : base(k_AssetName, i_Game)
        {            
            MotionVector = new Vector2(k_XMotionSpeed, 0);
        }

        public override void    Update(GameTime i_GameTime)
        {
            // On the first update, we'll set the barrier maximum and
            // minimum bounds 
            if (m_FirstUpdate)
            {
                m_FirstUpdate = false;
                m_MinXValue = Bounds.Left - Texture.Width / 2;
                m_MaxXValue = Bounds.Right + Texture.Width / 2;
            }

            base.Update(i_GameTime);

            // If the barrier reached one of the allowed bounds, we'll switch
            // the movment direction
            if (Bounds.Left <= m_MinXValue || Bounds.Right >= m_MaxXValue)
            {
                MotionVector *= -1;
            }
        }        

        // TODO: Move to CollidableSprite

        public bool     CheckPixelCollision(ICollidable i_OtherComponent)
        {
            int top = Math.Max(Bounds.Top, i_OtherComponent.Bounds.Top);
            int bottom = Math.Min(Bounds.Bottom, i_OtherComponent.Bounds.Bottom);
            int left = Math.Max(Bounds.Left, i_OtherComponent.Bounds.Left);
            int right = Math.Min(Bounds.Right, i_OtherComponent.Bounds.Right);           

            bool retVal = false;

            Color[] dataA = ColorData;
            Color[] dataB = i_OtherComponent.ColorData;

            m_CollidingPixels.Clear();
            m_MinCollidingPixel = -1;

            // TODO: Remove the remark

            /*Texture.GetData<Color>(dataA);
            i_OtherComponent.Texture.GetData<Color>(dataB);            */

            // Check every point within the intersection bounds
            for (int y = top; y < bottom; y++)
            {
                for (int x = left; x < right; x++)
                {
                    // Get the color of both pixels at this point
                    Color colorA = dataA[(x - Bounds.Left) +
                                         (y - Bounds.Top) * Bounds.Width];
                    Color colorB = dataB[(x - i_OtherComponent.Bounds.Left) +
                                         (y - i_OtherComponent.Bounds.Top) * i_OtherComponent.Bounds.Width];

                    // If both pixels are not completely transparent,
                    if (colorA.A != 0 && colorB.A != 0)
                    {
                        // then an intersection has been found

                        if (m_MinCollidingPixel == -1)
                        {
                            m_MinCollidingPixel = (x - Bounds.Left) +
                                         (y - Bounds.Top) * Bounds.Width;
                        }

                        m_CollidingPixels.Add((x - Bounds.Left) +
                                         (y - Bounds.Top) * Bounds.Width);

                        retVal = true;
                    }
                }
            }

            // No intersection found
            return retVal;
        }

        public override bool    CheckForCollision(ICollidable i_OtherComponent)
        {
            bool retVal = base.CheckForCollision(i_OtherComponent);
 	     
            if (retVal)
            {
                retVal = CheckPixelCollision(i_OtherComponent);
            }

            return retVal;
        }

        public override void    Collided(ICollidable i_OtherComponent)
        {
            Color[] colors = ColorData;

            if (i_OtherComponent is Enemy)
            {
                foreach (int pixel in m_CollidingPixels)
                {
                    Vector4 color = colors[pixel].ToVector4();
                    color.W = 0;
                    colors[pixel] = new Color(color);
                }
            }
            else
            {
                int pixelToTransperentNum = (int)(.75f * (i_OtherComponent.Texture.Width *
                                         i_OtherComponent.Texture.Height));

                // Calculate the direction which we need to transparent the 
                // pixels accroding to the colliding component movement
                // direction
                int transperentDirection = (int)
                    (i_OtherComponent.MotionVector.Y / 
                     Math.Abs(i_OtherComponent.MotionVector.Y));

                int currPixel = m_MinCollidingPixel;
                bool finish = false;

                while (pixelToTransperentNum > 0 && !finish)
                {
                    int widthPixel = currPixel;
                    bool finishWidth = false;

                    // Calculate the last pixel we need to transpaernt in the
                    // current line
                    int finishPixel = currPixel + 
                        (i_OtherComponent.Texture.Width * transperentDirection);

                    // Calculate the last pixel that is in the current pixel 
                    // texture line
                    int boundPixel = currPixel + 
                            transperentDirection * (currPixel % Texture.Width);

                    // If we're close to the bounds (left or right), we
                    // need to make sure that when will tansperent a barrier
                    // pixel we won't Accidentally reach the other side 
                    // (for example if the component collided with a pixel 
                    // in the left side and decreasing/increasing the pixel 
                    // to transperent can reach a pixel in the right side)
                    if (transperentDirection < 0)
                    {
                        finishPixel = Math.Max(finishPixel, boundPixel);
                    }
                    else
                    {
                        finishPixel = Math.Min(finishPixel, boundPixel);
                    }

                    while (pixelToTransperentNum > 0 &&
                           widthPixel != finishPixel && 
                           !finishWidth)
                    {
                        Vector4 color = colors[widthPixel].ToVector4();
                        color.W = 0;
                        colors[widthPixel] = new Color(color);

                        widthPixel += transperentDirection;
                                                
                        pixelToTransperentNum--;
                        
                        finishWidth = widthPixel > colors.Length - 1 || 
                                      widthPixel < 0;
                    }

                    currPixel += Texture.Width * transperentDirection;

                    finish = currPixel > colors.Length - 1 || 
                             currPixel < 0;
                }
            }
            
            ColorData = colors;
            Texture.SetData<Color>(colors);
        }        
    }
}
