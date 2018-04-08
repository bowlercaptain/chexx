using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rook : Piece {
	public Rook(int owner) : base(owner) { }
	public override List<Point2> getMovablePositions(Point2 pos, Point2 boardSize) {

		List<Point2> options = new List<Point2>();
		for (int x = 0; x < boardSize.x; x++) {//not assuming piece's base remains unchanged
			Point2 consider = new Point2(x, pos.y);
			if (consider != pos) {
				options.Add(consider);
			}
		}
		for (int y = 0; y < boardSize.y; y++) {
			Point2 consider = new Point2(pos.x, y);
			if (consider != pos) {
				options.Add(consider);
			}
		}
		return options;
	}
	public override string getCharacter() {
		if (owner == 0) {
			return "♖";
		} else {
			return "♜";
		}
	}
}
