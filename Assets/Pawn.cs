using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : Piece {
	public Pawn(int owner) : base(owner) { }
	public override List<Point2> getMovablePositions(Point2 pos, Point2 boardSize) {
		List<Point2> options = new List<Point2>();
		if (boardSize.inBounds(pos.x, pos.y + 1)) {
			int forward;
			if (owner == 0) { forward = 1; } else { forward = -1; }
			if (boardSize.inBounds(pos.x, pos.y + forward)) {
				options.Add(new Point2(pos.x, pos.y + forward));
			}
			if ((owner == 0 && pos.y == 1) || (owner == 1 && pos.y == 6)){
				options.Add(new Point2(pos.x, pos.y + forward * 2));
			}
		}
		return options;
	}

	public override List<Point2> getAttackablePositions(Point2 pos, Point2 boardSize) {
		List<Point2> options = new List<Point2>();
		int forward;
		if (owner == 0) { forward = 1; } else { forward = -1; }
		if (boardSize.inBounds(pos.x-1, pos.y+forward)){
			options.Add(new Point2(pos.x - 1, pos.y + forward));
		}
		if (boardSize.inBounds(pos.x + 1, pos.y + forward)) {
			options.Add(new Point2(pos.x + 1, pos.y + forward));
		}
		return options;
	}
	public override string getCharacter() {
		if (owner == 0) {
			return "♙";
		} else {
			return "♟";
		}
	}
}
