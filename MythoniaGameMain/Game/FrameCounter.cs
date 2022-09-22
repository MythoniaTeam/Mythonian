using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mythonia.Game
{
    public class FrameCounter : DrawableGameComponent
    {
        public static FrameCounter Ins { get; private set; } = null;

        public FrameCounter(MGame game) : base(game)
        {
            Ins ??= this;
        }

        public int FrameCount { get; private set; } = 0;


        public long TotalFrames { get; private set; }
        public float TotalSeconds { get; private set; }
        public float AverageFramesPerSecond { get; private set; }
        public float CurrentFramesPerSecond { get; private set; }


        public const int MAXIMUM_SAMPLES = 100;

        private readonly List<DateTime> _sampleBuffer = new();

        private DateTime _timeRecord;

        public override void Initialize()
        {
            UpdateOrder = -1;
            _timeRecord = DateTime.Now;
        }


        
        public override void Update(GameTime gameTime)
        {
            FrameCount++;
        }

        public override void Draw(GameTime gameTime)
        {
            float deltaTime = (float) (DateTime.Now - _timeRecord).TotalSeconds;
            _timeRecord = DateTime.Now;
            CurrentFramesPerSecond = 1.0f / deltaTime;

            _sampleBuffer.Add(DateTime.Now);

            if (_sampleBuffer.Count > MAXIMUM_SAMPLES)
            {
                _sampleBuffer.RemoveAt(0);
                //AverageFramesPerSecond = _sampleBuffer.Average(i => i);
            }
            /*else
            {
                AverageFramesPerSecond = CurrentFramesPerSecond;
            }*/
            AverageFramesPerSecond = (float)(_sampleBuffer.Count / (_sampleBuffer[^1] - _sampleBuffer[0]).TotalSeconds);
            if (AverageFramesPerSecond > 99999) AverageFramesPerSecond = 99999;

            TotalFrames++;
            TotalSeconds += deltaTime;
            //return true;
        }
    }
}