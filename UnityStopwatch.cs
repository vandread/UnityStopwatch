using System.Collections;
using System;

namespace UnityEngine
{
	/// <summary>
	/// Provides a set of methods and properties that uses <see cref="UnityEngine.Time.realtimeSinceStartup"/> to measure elapsed time.
	/// </summary>
	public struct UnityStopwatch
	{
		public static float GetTimestamp()
		{
			return UnityEngine.Time.realtimeSinceStartup;
		}

		public static bool IsHighResolution {
			get { return false; }
		}

		/// <summary>
		/// Gets the frequency of the timer as the number of ticks per second.
		/// </summary>
		public static long Frequency {
			get { return 1; }
		}


		private float _elapsed;
		private float _startTimeStamp;
		private bool _isRunning;

#if false
		/// <summary>
		/// Initializes a new <see cref="UnityStopwatch"/> instance.
		/// </summary>
		public UnityStopwatch()
		{
			_elapsed = 0f;
			_isRunning = false;
			_startTimeStamp = 0f;
		}
#endif
		

		public bool IsRunning {
			get { return _isRunning; }
		}

		public TimeSpan Elapsed {
			get { return TimeSpan.FromSeconds(this.GetElapsedDateTimeTicks()); }
		}

		public float ElapsedMilliseconds {
			get { return this.GetElapsedDateTimeTicks() * 1000; }
		}

		public float ElapsedTicks {
			get { return GetRawElapsedTicks(); }
		}



		/// <summary>
		/// Stops time interval measurement and resets the elapsed time to zero.
		/// </summary>
		public void Reset()
		{
			_elapsed = 0f;
			_isRunning = false;
			_startTimeStamp = 0f;
		}

		/// <summary>
		/// Starts, or resumes, measuring elapsed time for an interval.
		/// </summary>
		public void Start()
		{
			// Calling start on a running UnityStopwatch is a no-op.
			if (!_isRunning) {
				_startTimeStamp = GetTimestamp();
				_isRunning = true;
			}
		}

		/// <summary>
		/// Initializes a new <see cref="UnityStopwatch"/> instance, sets the elapsed time property to zero, and starts measuring elapsed time.
		/// </summary>
		/// <returns>A <see cref="UnityStopwatch"/> that has just begun measuring elapsed time.</returns>
		public static UnityStopwatch StartNew()
		{
			var s = new UnityStopwatch();
			s.Start();
			return s;
		}

		/// <summary>
		/// Stops measuring elapsed time for an interval.
		/// </summary>
		public void Stop()
		{
			// Calling stop on a stopped UnityStopwatch is a no-op.
			if (_isRunning) {
				var endTimeStamp = GetTimestamp();
				var elapsedThisPeriod = endTimeStamp - _startTimeStamp;
				_elapsed += elapsedThisPeriod;
				_isRunning = false;

				if (_elapsed < 0) {
					// When measuring small time periods the StopWatch.Elapsed* properties can return negative values.
					// This is due to bugs in the basic input/output system (BIOS) or the hardware abstraction layer (HAL) on machines with variable-speed CPUs (e.g. Intel SpeedStep).
					_elapsed = 0;
				}
			}
		}

		/// <summary>
		/// Stops time interval measurement, resets the elapsed time to zero, and starts measuring elapsed time.
		/// </summary>
		/// <remarks>
		/// <para>
		/// A <see cref="UnityStopwatch"/> instance calculates and retains the cumulative elapsed time across multiple time intervals, until the instance is reset or restarted.
		/// Use <see cref="Stop()"/> to stop the current interval measurement and retain the cumulative elapsed time value.
		/// Use <see cref="Reset()"/> to stop any interval measurement in progress and clear the elapsed time value.
		/// Use <see cref="Restart()"/> to stop current interval measurement and start a new interval measurement.
		/// </para>
		/// <para>
		/// Convenience method for replacing {sw.Reset(); sw.Start();} with a single sw.Restart().
		/// </para>
		/// </remarks>
		public void Restart()
		{
			_elapsed = 0;
			_startTimeStamp = GetTimestamp();
			_isRunning = true;
		}
		
		
		private float GetRawElapsedTicks()
		{
			var timeElapsed = _elapsed;
			if (_isRunning) {
				// If the StopWatch is running, add elapsed time since the UnityStopwatch is started last time. 
				var currentTimeStamp = GetTimestamp();
				var elapsedUntilNow = currentTimeStamp - _startTimeStamp;
				timeElapsed += elapsedUntilNow;
			}
			return timeElapsed;
		}

		// Get the elapsed ticks.        
		private float GetElapsedDateTimeTicks()
		{
			return GetRawElapsedTicks();
		}

	}
}
