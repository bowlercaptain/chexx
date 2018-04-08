using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Point2 {
	public int x;
	public int y;



	public Point2(int x, int y) {
		this.x = x;
		this.y = y;
	}

	public bool inBounds(int x, int y) {
		return x >= 0
		&& y >= 0
		&& x < this.x
		&& y < this.y;
	}

	public bool inBounds(Point2 point){
		return inBounds(point.x, point.y);
	}

	public static Point2 operator +(Point2 a, Point2 b) {
		return new Point2(a.x + b.x, a.y + b.y);
	}

	public static bool operator ==(Point2 a, Point2 b) {
		return a.x == b.x && a.y == b.y;
	}

	public static bool operator !=(Point2 a, Point2 b) {
		return !(a == b);
	}

	public static implicit operator Vector2(Point2 point) {
		return new Vector2(point.x, point.y);
	}

	public static implicit operator Point2(Vector2 vector) {
		return new Point2((int)vector.x, (int)vector.y);
	}
}
