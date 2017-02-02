using System;
using Assets.Scripts.Common;
using System.Collections.Generic;

public class TiteresDirection : IEquatable<TiteresDirection> {
	public Direction direction;
	public TiteresAction action;
	public int relativeToPuppetNumber;
	public int difficulty;

	public TiteresDirection(Direction d, TiteresAction a, int r, int diff) {
		direction = d;
		action = a;
		relativeToPuppetNumber = r;
		difficulty = diff;
	}

	public string GetText(List<TiteresDirection> actions) {
		string result = TiteresActivityModel.NAMES[actions.IndexOf(this)] + " ESTÁ ";

		if(action == TiteresAction.SIT) {
			//TODO shit!
			result += "SENTADA ";
		} else if(action == TiteresAction.STANDING){
			result += "PARADA ";
		}

		if(direction == Direction.LEFT) {
			result += "A LA IZQUIERDA ";
		} else if (direction == Direction.RIGHT){
			result += "A LA DERECHA ";
		} else if (direction == Direction.UP){
			result += "ARRIBA ";
		} else if (direction == Direction.DOWN){
			result += "DEBAJO ";
		}

		if(relativeToPuppetNumber != -1){
			result += "DE " + TiteresActivityModel.NAMES[relativeToPuppetNumber];
		} else {
			//TODO object?
			result += "OBJECT ";
		}

		if(action == TiteresAction.LEFT_ARM){
			result += "LEVANTANDO EL BRAZO IZQUIERDO";
		} else if(action == TiteresAction.RIGHT_ARM){
			result += "LEVANTANDO EL BRAZO DERECHO";
		}

		return result;
	}

	#region IEquatable implementation

	public bool Equals(TiteresDirection other) {
		return direction == other.direction && relativeToPuppetNumber == other.relativeToPuppetNumber;
	}

	#endregion
}