namespace Quilt.VG.Tests {
	using System.Numerics;
	using NUnit.Framework;

	public class TriangleTests {
		[SetUp]
		public void Setup() {
		}

		[Test]
		public void TestIsConvexFalse() {
			Vector2 p0 = new Vector2(0, 0);
			Vector2 p1 = new Vector2(0, 10);
			Vector2 p2 = new Vector2(10, 10);

			Assert.IsFalse(Triangle.IsConvex(p0, p1, p2));
		}

		[Test]
		public void TestIsConvexTrue() {
			Vector2 p0 = new Vector2(0, 0);
			Vector2 p1 = new Vector2(0, 10);
			Vector2 p2 = new Vector2(-10, 10);

			Assert.IsTrue(Triangle.IsConvex(p0, p1, p2));
		}
	}
}
