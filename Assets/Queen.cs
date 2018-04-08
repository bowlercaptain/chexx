using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Queen : Piece {
	public Queen(int owner) : base(owner) { }
	public override List<Point2> getMovablePositions(Point2 pos, Point2 boardSize) {
		List<Point2> options = new List<Point2>();
		for (int i = 0; i < boardSize.x; i++) {
			if (i != pos.x) {
				if (boardSize.inBounds(i, i + pos.y - pos.x)) {
					options.Add(new Point2(i, i + pos.y - pos.x));
				}
				if (boardSize.inBounds(i, -i + pos.y + pos.x)) {
					options.Add(new Point2(i, -i + pos.y + pos.x));
				}
				options.Add(new Point2(i, pos.y));
			}
			Point2 consider = new Point2(pos.x, i);
			if (consider != pos) {
				options.Add(consider);
			}
		}
		return options;
	}
	public override string getCharacter() {
		if (owner == 0) {
			return "♕";
		} else {
			return "♛";
		}
	}
}
