using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece {

		public Piece(int owner){
		this.owner = owner;
		}

	public int owner = 0;

	public virtual List<Point2> getMovablePositions(Point2 pos, Point2 boardSize) {
		List<Point2> options = new List<Point2>();
		for (int x = 0; x < boardSize.x; x++) {//rook
			Point2 consider = new Point2(x, pos.y);
			if (consider != pos) {
				options.Add(consider);
			}
		}
		for (int y = 0; y < boardSize.y; y++) {//rook
			Point2 consider = new Point2(pos.x, y);
			if (consider != pos) {
				options.Add(consider);
			}
		}
		return options;
	}
	public virtual List<Point2> getAttackablePositions(Point2 pos, Point2 boardSize) {
		return getMovablePositions(pos, boardSize);
	}
	public List<Point2> getAllTargetablePositions(Point2 pos, Point2 boardSize) {
		var moveables = getMovablePositions(pos, boardSize);//mutated into all positions
		var attackables = getAttackablePositions(pos, boardSize);
		foreach (Point2 target in attackables) {
			if (!moveables.Contains(target)) {
				moveables.Add(target);
			}
		}
		return moveables;
	}

	public virtual string getCharacter(){
		return owner.ToString();
	}
}
