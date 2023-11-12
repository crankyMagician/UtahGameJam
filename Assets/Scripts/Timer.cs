using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

namespace J
{
	public class Timer: MonoBehaviour
	{
		public static bool Delay(float second, Action action)
		{
			if (Instance)
			{
				Instance.AddDelay(second, action);
				return true;
			}
			return false;
		}

		public static int ClearTimers()
		{
			int count = Instance.timers.Count();
			Instance.timers.Clear();
			return count;
		}

		static Timer Instance;

		private List<TimerInfo> timers = new List<TimerInfo>()	;

		private class TimerInfo
		{
			public float delay;
			public Action action;

			public TimerInfo(float d, Action a)
			{
				delay = d;
				action = a;
			}
		}

		public void Awake()
		{

			Instance = this;
		}

		private void AddDelay(float seconds, Action action)
		{
			timers.Add(new TimerInfo(seconds, action));
		}

		private void Update()
		{
			bool executed = false;

			List<Action> actionsToExecute = new List<Action>();
			foreach (var timer in timers)
			{
				timer.delay -= Time.deltaTime;
				if (timer.delay <= 0)
				{
					actionsToExecute.Add(timer.action);
					executed = true;
				}
			}

			if (executed)
				timers = timers.Where( x => x.delay > 0 ).ToList();

			foreach (var action in actionsToExecute)
				action?.Invoke();
		}
	}
}
