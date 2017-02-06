using System;
using Assets.Scripts.Common;
using System.Collections.Generic;
using UnityEngine;

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

	public string GetText(List<TiteresDirection> actions, int objectIndex) {
		string puppetName = TiteresActivityModel.NAMES [actions.IndexOf (this)];
		string result = puppetName + " ESTÁ ";

		if(action == TiteresAction.SIT) {
			
			if (puppetName == "PEDRO" || puppetName == "ARTURO")
				result += "SENTADO ";
			else
				result += "SENTADA ";
			
		} else if(action == TiteresAction.STANDING){
			if (puppetName == "PEDRO" || puppetName == "ARTURO")
				result += "PARADO ";
			else
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
			result += TiteresActivityModel.OBJECT_NAMES[objectIndex].ToUpper();
		}

		if(action == TiteresAction.LEFT_ARM){
			result += " LEVANTANDO EL BRAZO IZQUIERDO";
		} else if(action == TiteresAction.RIGHT_ARM){
			result += " LEVANTANDO EL BRAZO DERECHO";
		}
		result+=".";

		return result;
	}

	public List<AudioClip> GetAudios(List<TiteresDirection> actions, int objectIndex,
		Dictionary<string,AudioClip> puppetAudios,Dictionary<string,AudioClip> puppetEndAudios,
		Dictionary<string,AudioClip> positionAudios,List<AudioClip> objectAudios) {

		List<AudioClip> result = new List<AudioClip> ();
		string puppetName = TiteresActivityModel.SIMPLE_NAMES [actions.IndexOf (this)];

		result.Add(puppetAudios[puppetName.ToLower()]);
		result.Add (positionAudios["esta"]);
			

		if(action == TiteresAction.SIT) {

			if (puppetName == "pedro" || puppetName == "arturo")
				result.Add (positionAudios["sentado"]);
			else
				result.Add (positionAudios["sentada"]);

		} else if(action == TiteresAction.STANDING){
			if (puppetName == "pedro" || puppetName == "arturo")
				result.Add (positionAudios["parado"]);
			else
				result.Add (positionAudios["parada"]);
		}

		if(direction == Direction.LEFT) {
			result.Add (positionAudios["izquierda"]);
		} else if (direction == Direction.RIGHT){
			result.Add (positionAudios["derecha"]);
		} else if (direction == Direction.UP){
			result.Add (positionAudios["arriba"]);
		} else if (direction == Direction.DOWN){
			result.Add (positionAudios["debajo"]);
		}

		if(relativeToPuppetNumber != -1){
			result.Add (puppetEndAudios[TiteresActivityModel.SIMPLE_NAMES[relativeToPuppetNumber].ToLower()]);
		} else {
			result.Add(objectAudios[objectIndex]);
		}

		if(action == TiteresAction.LEFT_ARM){
			result.Add (positionAudios["levantandoizquierdo"]);
		} else if(action == TiteresAction.RIGHT_ARM){
			result.Add (positionAudios["levantandoderecho"]);
		}

		return result;
	}

	#region IEquatable implementation

	public bool Equals(TiteresDirection other) {
		return direction == other.direction && relativeToPuppetNumber == other.relativeToPuppetNumber;
	}

	#endregion
}