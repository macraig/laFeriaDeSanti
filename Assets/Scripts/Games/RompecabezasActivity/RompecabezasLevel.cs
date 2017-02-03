using System;
using SimpleJSON;
using System.Collections.Generic;
using Assets.Scripts.Common;
using Assets.Scripts.Games.RompecabezasActivity;
using UnityEngine;

public class RompecabezasLevel {
	public bool hasTwoRoads;
	public int partQuantity, distractionParts;
	public const int GRID = 6;
	private List<PartModel> parts;

	public RompecabezasLevel(JSONClass source, bool withTime) {
		hasTwoRoads = source["hasTwoRoads"].AsBool;
		partQuantity = source["partQuantity"].AsInt;
		distractionParts = source["distractionParts"].AsInt;

		if(hasTwoRoads) BuildTwoRoadLevel();
		else BuildLevel();
	}

	void BuildTwoRoadLevel() {
		
	}

	void BuildLevel() {
		parts = new List<PartModel>();
		Direction[] dirs = (Direction[]) Enum.GetValues(typeof(Direction));
		Randomizer r = Randomizer.New(GRID - 1);

		//Set start wall and direction. Set start part.
		Direction wall = RandomDirection(dirs);
		int startColumn = wall == Direction.LEFT || wall == Direction.RIGHT ? 0 : r.Next();
		int startRow = wall == Direction.LEFT || wall == Direction.RIGHT ? r.Next() : 0;
		Direction startDirection = StartDirection(dirs, startColumn, startRow);

		parts.Add(new PartModel(startDirection, Direction.NULL, startColumn, startRow));

		//Set middle and final parts. Chaos.

		int newCol = startColumn, newRow = startRow;
		Direction newDir = startDirection;

		while(parts.Count < (partQuantity + 2)){
			newCol = DirectionPlusCol(newDir, newCol);
			newRow = DirectionPlusRow(newDir, newRow);

			Randomizer dirRand = Randomizer.New(dirs.Length - 1);
			Direction oldDir = newDir;
			bool done = false;
			for(int i = 0; i < dirs.Length; i++) {
				newDir = dirs[dirRand.Next()];
				if(isApplicable(newDir, newCol, newRow) && !IsSameWay(oldDir, newDir)){
					PartModel newPart = new PartModel(newDir, oldDir, newCol, newRow);
					if(!parts.Contains(newPart)) {
						parts.Add(newPart);
						done = true;
						break;
					}
				}
			}
			if(!done) {
				//If I reach this segment it means that I have no way out. In that case, restart BuildLevel(). We should use a path algorithm.
				BuildLevel();return;
			}
		}

		//End part doesn't go anywhere.
		parts[parts.Count - 1].direction = Direction.NULL;

		PrintParts();
	}

	void PrintParts() {
		foreach(var p in parts) {
			Debug.Log("De " + p.previousDir + " a " + p.direction);
		}
	}

	bool IsSameWay(Direction oldDir, Direction newDir) {
		return (oldDir == Direction.UP && newDir == Direction.DOWN) || (oldDir == Direction.DOWN && newDir == Direction.UP)
			|| (oldDir == Direction.LEFT && newDir == Direction.RIGHT) || (oldDir == Direction.RIGHT && newDir == Direction.LEFT);
	}

	int DirectionPlusRow(Direction d, int row) {
		if(d == Direction.UP) return row - 1;
		if(d == Direction.DOWN) return row + 1;
		return row;
	}

	int DirectionPlusCol(Direction d, int col) {
		if(d == Direction.LEFT) return col - 1;
		if(d == Direction.RIGHT) return col + 1;
		return col;
	}

	Direction StartDirection(Direction[] dirs, int startColumn, int startRow) {
		Randomizer dirRand = Randomizer.New(dirs.Length - 1);
		for(int i = 0; i < dirs.Length; i++) {
			Direction d = dirs[dirRand.Next()];
			if(isApplicable(d, startColumn, startRow)) return d;
		}
		return Direction.NULL;
	}

	bool isApplicable(Direction d, int col, int row) {
		if(d == Direction.LEFT && col <= 1) return false;
		if(d == Direction.RIGHT && col >= 4) return false;
		if(d == Direction.UP && row <= 1) return false;
		if(d == Direction.DOWN && row >= 4) return false;
		return true;
	}

	Direction RandomDirection(Direction[] dirs) {
		return dirs[Randomizer.New(dirs.Length - 1).Next()];
	}

	public List<PartModel> StartParts(){
		//TODO multiple starts.
		return parts.GetRange(0, 1);
	}
	public List<PartModel> EndParts(){
		//TODO multiple starts.
		return parts.GetRange(parts.Count - 1, 1);
	}

	public List<PartModel> DraggerParts() {
		return parts.GetRange(1, parts.Count - 2);
	}
}