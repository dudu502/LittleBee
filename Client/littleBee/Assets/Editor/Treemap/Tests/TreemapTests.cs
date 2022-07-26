using System;
using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

namespace Treemap
{
	[TestFixture]
	public class TreemapTests
	{
		static readonly Rect s_Rect = new UnityEngine.Rect (0, 0, 100, 100);

		[Test]
		public void Empty()
		{
			Assert.Throws<ArgumentException>(() => Utility.GetTreemapRects (new float[0], s_Rect));
		}

		[Test]
		public void One()
		{
			var result = Utility.GetTreemapRects (new [] { 1.0f }, s_Rect);
			Assert.AreEqual (1, result.Length);
			Assert.AreEqual (s_Rect, result[0]);
		}

		[Test]
		public void TwoEquallySized()
		{
			var result = Utility.GetTreemapRects (new [] { 1.0f, 1.0f }, s_Rect);
			Assert.AreEqual (2, result.Length);

			Assert.AreEqual (new Rect (0, 0, 100, 50), result [0]);
			Assert.AreEqual (new Rect (0, 50, 100, 50), result [1]);
		}

		[Test]
		public void Zero()
		{
			Assert.Throws<ArgumentException>(() => Utility.GetTreemapRects (new [] { 0f }, s_Rect));
		}

		[Test]
		public void NegativeNumber()
		{
			Assert.Throws<ArgumentException>(() => Utility.GetTreemapRects (new [] { -3f }, s_Rect));
		}

		[Test, TestCaseSource("InterestingDataSets")]
		public void ResultRectsHaveCorrectArea(float[] floats)
		{
			var results = Utility.GetTreemapRects (floats, s_Rect);
			Assert.AreEqual (floats.Length, results.Length); 

			var sum = floats.Sum ();
			var totalSize = s_Rect.width * s_Rect.height;

			for(int i =0; i!= floats.Length; i++)
			{
				var f = floats [i];
				var r = results [i];

				var size = r.width * r.height;
				var ratio = size / totalSize;

				UnityEngine.Assertions.Assert.AreApproximatelyEqual (f / sum, ratio);
			}
		}

		[Test, TestCaseSource("InterestingDataSets")]
		public void ResultRectsDoNotOverlap(float[] floats)
		{
			var results = Utility.GetTreemapRects (floats, s_Rect);
			Assert.AreEqual (floats.Length, results.Length); 

			for(int i1 =0; i1!= floats.Length; i1++)
			{
				var rect1 = results [i1];
				for (int i2 = i1 + 1; i2 != floats.Length; i2++) {
					var rect2 = results [i2];

					float overlapSize = SizeOfOverlappingRectOf(rect1, rect2);

					const float tolerance = 0.000001f;
					if (overlapSize > tolerance * rect1.width * rect1.height || overlapSize > tolerance * rect2.width * rect2.height)
						throw new AssertionException ("too big overlap. overlapSize: " + overlapSize + " rect1:" + rect1 + " rect2: " + rect2);
				}
			}
		}

		private static Rect RectIntersect(Rect a, Rect b)
		{
			float xMin = Mathf.Max(a.x, b.x);
			float xMax = Mathf.Min(a.x + a.width, b.x + b.width);
			float yMin = Mathf.Max(a.y, b.y);
			float yMax = Mathf.Min(a.y + a.height, b.y + b.height);
			if (xMax >= xMin && yMax >= yMin)
				return new Rect(xMin, yMin, xMax - xMin, yMax - yMin);
			return new Rect(0f, 0f, 0f, 0f);
		}

		static float SizeOfOverlappingRectOf(Rect rect1, Rect rect2)
		{
			var result = RectIntersect (rect1, rect2);
			return result.width * result.height;
		}

		static IEnumerable InterestingDataSets()
		{
			yield return new TestCaseData (Enumerable.Range (100, 10).Select (i => (float)i).ToArray ()).SetName ("1000 floats counting up");
			yield return new TestCaseData (Enumerable.Repeat (10, 100).Select (i => (float)i).ToArray ()).SetName ("100 identical floats");
		}
	}
}

