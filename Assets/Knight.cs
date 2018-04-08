using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : Piece {
	public Knight(int owner) : base(owner) { }
	public override List<Point2> getMovablePositions(Point2 pos, Point2 boardSize) {
		List<Point2> options = new List<Point2>();
		for (int x = -2; x <= 2; x++) {
			int ysize = 3 - Mathf.Abs(x);
			if(x!=0){
				foreach (int y in new int[] { ysize, -ysize }) {
					if(boardSize.inBounds(pos.x+x,pos.y+y)){
						options.Add(new Point2(pos.x+x, pos.y+y));
					}
				}
			}
		}
		return options;
	}
	public override string getCharacter() {
		if (owner == 0) {
			return "♘";
		} else {
			return "♞";
		}
	}
}
