using System;
using System.Collections;
using UnityEngine;
using System.Linq;
using System.Threading;
using Btools.utils;

namespace Btools.TimedEvents
{
    /// <summary>Uses Delegates combined with coroutines to make timed events, without having to do is messily</summary>
    public static class Timed
    {
        #region After Time
        /// <summary>Run an action after the specified time has finished</summary>
        /// <param name="action">Code to run</param>
        /// <param name="time">Time to wait</param>
        public static Coroutine RunAfterTime(Action action, float time) =>
            EmptyMonoBehaviour.EmptyMonobehaviour.StartCoroutine(Coroutine_AfterTime(action, time));

        private static IEnumerator Coroutine_AfterTime(Action action, float t)
        {
            yield return new WaitForSeconds(t);
            action.Invoke();
        }

        #endregion
        #region After Real Time
        /// <summary>Run an action after the specified time has finished (ignores timescale)</summary>
        /// <param name="action">Code to run</param>
        /// <param name="time">Time to wait</param>
        public static Coroutine RunAfterRealTime(Action action, float time) =>
            EmptyMonoBehaviour.EmptyMonobehaviour.StartCoroutine(Coroutine_AfterRealTime(action, time));

        private static IEnumerator Coroutine_AfterRealTime(Action action, float t)
        {
            yield return new WaitForSecondsRealtime(t);
            action.Invoke();
        }

        #endregion
        #region Until

        /// <summary> Repeats the code until the condition is met, every frame </summary>
        /// <param name="action">Code to run</param>
        /// <param name="StopRunning">The condition to stop at</param>
        public static Coroutine RepeatUntil(Func<bool> action) =>
            EmptyMonoBehaviour.EmptyMonobehaviour.StartCoroutine(Coroutine_RepeatUntil(action));

        /// <summary> Repeats the code until the condition is met, every interval </summary>
        /// <param name="action">Code to run</param>
        /// <param name="StopRunning">The condition to stop at</param>
        /// <param name="interval">The interval for the code to run</param>
        public static Coroutine RepeatUntil(Func<bool> action, float interval) =>
            EmptyMonoBehaviour.EmptyMonobehaviour.StartCoroutine(Coroutine_RepeatUntil(action, interval));

        private static IEnumerator Coroutine_RepeatUntil(Func<bool> action, float interval)
        {
            while (!action.Invoke())
            {
                yield return new WaitForSeconds(interval);
            }
        }

        private static IEnumerator Coroutine_RepeatUntil(Func<bool> action)
        {
            while (!action.Invoke())
            {
                yield return new WaitForEndOfFrame();
            }
        }

        #endregion
        #region Frames
        /// <summary>Run an action after the specified time has finished</summary>
        /// <param name="action">Code to run</param>
        /// <param name="time">Time to wait</param>
        public static Coroutine RunAfterFrames(Action action, int frames) =>
            EmptyMonoBehaviour.EmptyMonobehaviour.StartCoroutine(Coroutine_AfterFrames(action, frames));

        private static IEnumerator Coroutine_AfterFrames(Action action, int frames)
        {
            yield return new WaitForFrames(frames);
            action.Invoke();
        }
        #endregion
        #region RunCoroutine
        /// <summary>Run an action after the specified time has finished</summary>
        /// <param name="action">Code to run</param>
        /// <param name="time">Time to wait</param>
        public static Coroutine RunCoroutine(Action action, YieldInstruction yieldInstruction) =>
            EmptyMonoBehaviour.EmptyMonobehaviour.StartCoroutine(Coroutine_RunCoroutine(action, yieldInstruction));

        private static IEnumerator Coroutine_RunCoroutine(Action action, YieldInstruction yieldInstruction)
        {
            yield return yieldInstruction;
            action.Invoke();
        }
        #endregion
    }
}