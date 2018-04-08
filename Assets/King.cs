using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class King : Piece {

	public King(int owner) : base(owner) { }

	public override List<Point2> getMovablePositions(Point2 pos, Point2 boardSize) {
		List<Point2> options = new List<Point2>();
		for (int x = pos.x - 1; x < pos.x + 1; x++) {
			for (int y = pos.y - 1; y < pos.y + 1; y++) {
				if (boardSize.inBounds(x, y) && !(pos.x == x && pos.y == y)) {
					options.Add(new Point2(x, y));
				}
			}
		}
		return options;
	}
	public override string getCharacter() {
		if (owner == 0) {
			return "♔";
		} else {
			return "♚";
		}
	}
}
