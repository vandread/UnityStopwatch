using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Threading;
using System;

public class UnityStopwatchTests
{
	

	[UnityTest]
	public IEnumerator GetTimestamp()
	{
		var ts1 = UnityStopwatch.GetTimestamp();
		yield return null;
		var ts2 = UnityStopwatch.GetTimestamp();
		Assert.AreNotEqual(ts1, ts2);

	}

	[UnityTest]
	public IEnumerator ConstructStartAndStop()
	{
		var watch = new UnityStopwatch();
		Assert.False(watch.IsRunning);
		watch.Start();
		Assert.True(watch.IsRunning);
		yield return null;
		Assert.True(watch.Elapsed > TimeSpan.Zero);

		watch.Stop();
		Assert.False(watch.IsRunning);

		var e1 = watch.Elapsed;
		yield return null;
		var e2 = watch.Elapsed;
		Assert.AreEqual(e1, e2);
		Assert.AreEqual((long)e1.TotalMilliseconds, (long)watch.ElapsedMilliseconds);

		var t1 = watch.ElapsedTicks;
		yield return null;
		var t2 = watch.ElapsedTicks;
		Assert.AreEqual((long)t1, (long)t2);
	}

	[UnityTest]
	public IEnumerator StartNewAndReset()
	{
		var watch = UnityStopwatch.StartNew();
		Assert.True(watch.IsRunning);
		watch.Start(); // should be no-op
		Assert.True(watch.IsRunning);
		yield return null;

		Assert.True(watch.Elapsed > TimeSpan.Zero);

		watch.Reset();
		Assert.False(watch.IsRunning);
		Assert.AreEqual(TimeSpan.Zero, watch.Elapsed);
	}

	[UnityTest]
	public IEnumerator StartNewAndRestart()
	{
		var watch = UnityStopwatch.StartNew();
		Assert.True(watch.IsRunning);

		yield return null;

		var elapsedSinceStart = watch.Elapsed;
		Assert.True(elapsedSinceStart > TimeSpan.Zero);

		const int MaxAttempts = 5; // The comparison below could fail if we get very unlucky with when the thread gets preempted
		int attempt = 0;
		while (true) {
			watch.Restart();
			Assert.True(watch.IsRunning);

			try {
				Assert.True(watch.Elapsed < elapsedSinceStart);
			} catch {
				if (++attempt < MaxAttempts)
					continue;
				throw;
			}
			break;
		}
	}

}
