﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Extensions
{
	public static class RectExtensions
	{
		public static Rect NULL = new Rect(MathfExtensions.NULL_FLOAT, MathfExtensions.NULL_FLOAT, MathfExtensions.NULL_FLOAT, MathfExtensions.NULL_FLOAT);

		public static Rect Move (this Rect rect, Vector2 movement)
		{
			rect.position += movement;
			return rect;
		}
		
		public static RectInt Move (this RectInt rect, Vector2Int movement)
		{
			rect.position += movement;
			return rect;
		}

		public static Rect SwapXAndY (this Rect rect)
		{
			return Rect.MinMaxRect(rect.min.y, rect.min.x, rect.max.y, rect.max.x);
		}
		
		public static bool IsEncapsulating (this Rect r1, Rect r2, bool equalRectsRetunsTrue)
		{
			if (equalRectsRetunsTrue)
			{
				bool minIsOk = r1.min.x <= r2.min.x && r1.min.y <= r2.min.y;
				bool maxIsOk = r1.max.x >= r2.max.x && r1.max.y >= r2.max.y;
				return minIsOk && maxIsOk;
			}
			else
			{
				bool minIsOk = r1.min.x < r2.min.x && r1.min.y < r2.min.y;
				bool maxIsOk = r1.max.x > r2.max.x && r1.max.y > r2.max.y;
				return minIsOk && maxIsOk;
			}
		}
		
		public static bool IsIntersecting (this Rect r1, Rect r2, bool equalRectsRetunsTrue = true)
		{
			if (equalRectsRetunsTrue)
				return r1.xMin <= r2.xMax && r1.xMax >= r2.xMin && r1.yMin <= r2.yMax && r1.yMax >= r2.yMin;
			else
				return r1.xMin < r2.xMax && r1.xMax > r2.xMin && r1.yMin < r2.yMax && r1.yMax > r2.yMin;
		}

		public static Vector2[] GetCorners (this Rect rect)
		{
			Vector2[] output = new Vector2[4];
			output[0] = rect.min;
			output[1] = new Vector2(rect.xMax, rect.yMin);
			output[2] = new Vector2(rect.xMin, rect.yMax);
			output[3] = rect.max;
			return output;
		}
		
		public static bool IsExtendingOutside (this Rect r1, Rect r2, bool equalRectsRetunsTrue)
		{
			if (equalRectsRetunsTrue)
			{
				bool minIsOk = r1.min.x <= r2.min.x || r1.min.y <= r2.min.y;
				bool maxIsOk = r1.max.x >= r2.max.x || r1.max.y >= r2.max.y;
				return minIsOk || maxIsOk;
			}
			else
			{
				bool minIsOk = r1.min.x < r2.min.x || r1.min.y < r2.min.y;
				bool maxIsOk = r1.max.x > r2.max.x || r1.max.y > r2.max.y;
				return minIsOk || maxIsOk;
			}
		}
		
		public static Rect ToRect (this Bounds bounds)
		{
			return Rect.MinMaxRect(bounds.min.x, bounds.min.y, bounds.max.x, bounds.max.y);
		}

		public static Rect Combine (Rect[] rectsArray)
		{
			Rect output = rectsArray[0];
			for (int i = 1; i < rectsArray.Length; i ++)
			{
				if (rectsArray[i].min.x < output.min.x)
					output.min = new Vector2(rectsArray[i].min.x, output.min.y);
				if (rectsArray[i].min.y < output.min.y)
					output.min = new Vector2(output.min.x, rectsArray[i].min.y);
				if (rectsArray[i].max.x > output.max.x)
					output.max = new Vector2(rectsArray[i].max.x, output.max.y);
				if (rectsArray[i].max.y > output.max.y)
					output.max = new Vector2(output.max.x, rectsArray[i].max.y);
			}
			return output;
		}

		public static Rect FromPoints (Vector2[] points)
		{
			Vector2 point = points[0];
			Rect output = Rect.MinMaxRect(point.x, point.y, point.x, point.y);
			for (int i = 1; i < points.Length; i ++)
			{
				point = points[i];
				if (point.x < output.min.x)
					output.min = new Vector2(point.x, output.min.y);
				if (point.y < output.min.y)
					output.min = new Vector2(output.min.x, point.y);
				if (point.x > output.max.x)
					output.max = new Vector2(point.x, output.max.y);
				if (point.y > output.max.y)
					output.max = new Vector2(output.max.x, point.y);
			}
			return output;
		}

		public static Rect Expand (this Rect rect, Vector2 amount)
		{
			Vector2 center = rect.center;
			rect.size += amount;
			rect.center = center;
			return rect;
		}
		
		public static Rect Set (this Rect rect, RectInt rectInt)
		{
			rect.center = rectInt.center;
			rect.size = rectInt.size;
			return rect;
		}

		public static Rect ToRect (this RectInt rectInt)
		{
			return Rect.MinMaxRect(rectInt.xMin, rectInt.yMin, rectInt.xMax, rectInt.yMax);
		}

		public static Vector2 ClosestPoint (this Rect rect, Vector2 point)
		{
			return point.ClampComponents(rect.min, rect.max);
		}

		public static Vector2 ToNormalizedPosition (this Rect rect, Vector2 point)
		{
			return Rect.PointToNormalized(rect, point);
			// return Vector2.one.Divide(rect.size) * (point - rect.min);
		}

		public static Vector2 ToNormalizedPosition (this RectInt rect, Vector2Int point)
		{
			return Vector2.one.Divide(rect.size.ToVec2()).Multiply(point.ToVec2() - rect.min.ToVec2());
		}

		public static Rect SetToPositiveSize (this Rect rect)
		{
			Rect output = rect;
			output.size = new Vector2(Mathf.Abs(output.size.x), Mathf.Abs(output.size.y));
			output.center = rect.center;
			return output;
		}

		public static Circle2D GetSmallestCircleAround (this Rect rect)
		{
			return new Circle2D(rect.center, rect.size.magnitude / 2);
		}

		// public static Rect GetExactFitRectForCircle (Circle2D circle)
		// {
		// }

		public static Rect AnchorToPoint (this Rect rect, Vector2 point, Vector2 anchorPoint)
		{
			Rect output = rect;
			output.position = point - (output.size * anchorPoint);
			return output;
		}

		public static LineSegment2D[] GetEdges (this Rect rect)
		{
			return new LineSegment2D[4] { new LineSegment2D(rect.min, new Vector2(rect.xMin, rect.yMax)), new LineSegment2D(rect.min, new Vector2(rect.xMax, rect.yMin)), new LineSegment2D(rect.max, new Vector2(rect.xMin, rect.yMax)), new LineSegment2D(rect.max, new Vector2(rect.xMax, rect.yMin)) };
		}

		public static Bounds ToBounds (this Rect rect)
		{
			return new Bounds(rect.center, rect.size);
		}

		public static Bounds ToBounds (this RectInt rect)
		{
			return new Bounds(rect.center, rect.size.ToVec2());
		}

		public static BoundsInt ToBoundsInt (this RectInt rect)
		{
			return new BoundsInt(rect.center.ToVec3Int(), rect.size.ToVec3Int());
		}

		public static Rect GrowToPoint (this Rect rect, Vector2 point)
		{
			rect.min = rect.min.SetToMinComponents(point);
			rect.max = rect.max.SetToMaxComponents(point);
			return rect;
		}
	}
}