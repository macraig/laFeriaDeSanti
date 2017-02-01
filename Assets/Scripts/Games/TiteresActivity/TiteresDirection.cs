using System;
using Assets.Scripts.Common;

public class TiteresDirection : IEquatable<TiteresDirection> {
	private Direction direction;
	private TiteresAction action;
	private int relativeToPuppetNumber;
	private int difficulty;

	public TiteresDirection(Direction d, TiteresAction a, int r, int diff) {
		direction = d;
		action = a;
		relativeToPuppetNumber = r;
		difficulty = diff;
	}

	#region IEquatable implementation

	public bool Equals(TiteresDirection other) {
		return direction == other.direction && relativeToPuppetNumber == other.relativeToPuppetNumber;
	}

	#endregion
}