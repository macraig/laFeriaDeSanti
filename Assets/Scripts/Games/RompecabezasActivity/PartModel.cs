using System;
using Assets.Scripts.Common;
using UnityEngine;
using System.Collections.Generic;

namespace Assets.Scripts.Games.RompecabezasActivity {
	public class PartModel : IEquatable<PartModel> {
		public Direction direction, previousDir;
		public int col, row;

		public PartModel(Direction d, Direction previousDir, int column, int row) {
			direction = d;
			this.previousDir = previousDir;
			col = column;
			this.row = row;
		}

		public Sprite GetSprite(List<Sprite> parts) {
			if((previousDir == Direction.LEFT || previousDir == Direction.RIGHT) && (direction == Direction.LEFT || direction == Direction.RIGHT))
				return parts[24];
			if((previousDir == Direction.UP || previousDir == Direction.DOWN) && (direction == Direction.UP || direction == Direction.DOWN))
				return parts[23];
			if((previousDir == Direction.RIGHT && direction == Direction.UP) || (previousDir == Direction.DOWN && direction == Direction.LEFT))
				return parts[15];
			if((previousDir == Direction.LEFT && direction == Direction.UP) || (previousDir == Direction.DOWN && direction == Direction.RIGHT))
				return parts[14];
			if((previousDir == Direction.LEFT && direction == Direction.DOWN) || (previousDir == Direction.UP && direction == Direction.RIGHT))
				return parts[13];
			if((previousDir == Direction.RIGHT && direction == Direction.DOWN) || (previousDir == Direction.UP && direction == Direction.LEFT))
				return parts[12];
			

			return null;
		}

		#region IEquatable implementation

		public bool Equals(PartModel other) {
			return col == other.col && row == other.row;
		}

		#endregion
	}
}