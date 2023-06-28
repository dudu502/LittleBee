using UnityEngine;

namespace Synchronize.Game.Lockstep.FSM
{
    public class Timer
    {
        public float startTime;
        public float Elapsed => Time.time - startTime;

        public Timer()
        {
            Reset();
        }

        public void Reset() { startTime = Time.time; }

        public static bool operator >(Timer timer, float duration)
            => timer.Elapsed > duration;

        public static bool operator <(Timer timer, float duration)
            => timer.Elapsed < duration;

        public static bool operator >=(Timer timer, float duration)
            => timer.Elapsed >= duration;

        public static bool operator <=(Timer timer, float duration)
            => timer.Elapsed <= duration;

        public static float operator /(Timer timer, float duration)
            => timer.Elapsed / duration;
    }
}
