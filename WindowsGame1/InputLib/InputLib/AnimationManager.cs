using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace InputLib
{
    public class AnimationManager : Microsoft.Xna.Framework.GameComponent
    {
        private Dictionary<string, Animation> animations =
            new Dictionary<string, Animation>();
        private Dictionary<string, Texture2D> textures =
            new Dictionary<string, Texture2D>();
        private string contentPath;

        public AnimationManager(Game game, string contentPath)
            : base(game)
        {
            this.contentPath = contentPath;

            if (this.contentPath.LastIndexOf('\\')
                < this.contentPath.Length - 1)
            {
                this.contentPath += "\\";
            }
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public void AddAnimation(string animationKey, string textureName,
            FrameCount frameCount, int framesPerSecond)
        {
            if (!textures.ContainsKey(textureName))
            {
                textures.Add(textureName, Game.Content.Load<Texture2D>
                    (contentPath + textureName));
            }

            int frameWidth = (int)(textures[textureName].Width /
                frameCount.NumberOfColumns);
            int frameHeight = (int)(textures[textureName].Height / 
                frameCount.NumberOfRows);

            int numberOfFrames = frameCount.NumberOfColumns * 
                frameCount.NumberOfRows;

            AddAnimation(animationKey, textureName, 
                new FrameRange(1, 1, frameCount.NumberOfColumns,
                frameCount.NumberOfRows), frameWidth, frameHeight,
                numberOfFrames, framesPerSecond);
        }

        public void AddAnimation(string animationKey, string textureName,
            FrameRange frameRange, int frameWidth, int frameHeight,
            int numberOfFrames, int framesPerSecond)
        {
            Animation animation = new Animation(textureName, frameRange,
                framesPerSecond);

            if (!textures.ContainsKey(textureName))
            {
                textures.Add(textureName, Game.Content.Load<Texture2D>(
                    contentPath + textureName));
            }

            animation.FrameWidth = frameWidth;
            animation.FrameHeight = frameHeight;
            animation.NumberOfFrames = numberOfFrames;
            animation.FramesPerRow = textures[textureName].Width / frameWidth;

            if (animations.ContainsKey(animationKey))
            {
                animations[animationKey] = animation;
            }
            else
            {
                animations.Add(animationKey, animation);
            }
        }

        public void ToggleAnimation(string animationKey)
        {
            if (animations.ContainsKey(animationKey))
            {
                animations[animationKey].Paused = !animations[animationKey].Paused;
            }
        }

        public override void Update(GameTime gameTime)
        {
            foreach (KeyValuePair<string, Animation> animation in animations)
            {
                Animation frameAnimation = animation.Value;

                if (frameAnimation.Paused)
                {
                    continue;
                }

                frameAnimation.TotalElapsedTime += 
                    (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (frameAnimation.TotalElapsedTime > frameAnimation.TimePerFrame)
                {
                    frameAnimation.Frame++;
                    frameAnimation.Frame = frameAnimation.Frame % (frameAnimation.NumberOfFrames);
                    frameAnimation.TotalElapsedTime -= frameAnimation.TimePerFrame;
                }
            }

           base.Update(gameTime);
        }

        public void Draw(GameTime gameTime, string animationKey,
            SpriteBatch spriteBatch, Vector2 position)
        {
            Draw(gameTime, animationKey, spriteBatch,
                position, Color.White);
        }

        public void Draw(GameTime gameTime, string animationKey,
            SpriteBatch spriteBatch, Vector2 position, Color color)
        {
            if (!animations.ContainsKey(animationKey))
            {
                return;
            }

            Animation frameAnimation = animations[animationKey];

            int xIncrease = (frameAnimation.Frame + frameAnimation.FrameRange.FirstFrameX - 1);
            int xWrapped = xIncrease % frameAnimation.FramesPerRow;
            int x = xWrapped * frameAnimation.FrameWidth;

            int yIncrease = xIncrease / frameAnimation.FramesPerRow;
            int y = (yIncrease + frameAnimation.FrameRange.FirstFrameY - 1) * frameAnimation.FrameHeight;

            Rectangle frame = new Rectangle(x, y, frameAnimation.FrameWidth,
                frameAnimation.FrameHeight);
            spriteBatch.Draw(textures[frameAnimation.TextureName], position, frame, color);
        }
    }
}
