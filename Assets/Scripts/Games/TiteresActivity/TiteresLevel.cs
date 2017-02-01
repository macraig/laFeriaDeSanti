using System;
using SimpleJSON;
using System.Collections.Generic;
using Assets.Scripts.Common;

public class TiteresLevel {
	bool withTime;
	List<int> personQuantity, personDifficulty;
	List<TiteresDirection> actions;

	public TiteresLevel(JSONClass source) {
		withTime = source["withTime"].AsBool;

		if(withTime){
			personQuantity = new List<JSONNode>(source["personQuantity"].Childs).ConvertAll((n) => n.AsInt);
			List<List<int>> diffs = new List<JSONNode>(source["personDifficulty"].Childs)
				.ConvertAll((n) => new List<JSONNode>(n.AsArray.Childs).ConvertAll((inner) => inner.AsInt));

			RandomizeDifficulties(diffs);

			SetActions();
		}
	}

	void SetActions() {
		actions = new List<TiteresDirection>();
		Direction[] dirs = (Direction[]) Enum.GetValues(typeof(Direction));
		Randomizer dirRandomizer = Randomizer.New(dirs.Length - 1);
		TiteresDirection newDir = null;

		while(actions.Count < personDifficulty.Count) {
			int current = actions.Count;
			Direction dir = dirs[dirRandomizer.Next()];
			Tuple<Direction, int> relativeDir = null;

			switch(personDifficulty[current]) {
			case 1:
				newDir = new TiteresDirection(dir, TiteresAction.NONE, -1, 1);
				break;
			case 2:
				newDir = new TiteresDirection(dir, RandomAction(true), -1, 2);
				break;
			case 3:
				newDir = new TiteresDirection(dir, RandomAction(false), -1, 3);
				break;
			case 4:
				relativeDir = RandomRelativeDir();
				newDir = new TiteresDirection(relativeDir.Item1, TiteresAction.NONE, relativeDir.Item2, 4);
				break;
			case 5:
				relativeDir = RandomRelativeDir();
				newDir = new TiteresDirection(relativeDir.Item1, RandomAction(true), relativeDir.Item2, 5);
				break;
			case 6:
				relativeDir = RandomRelativeDir();
				newDir = new TiteresDirection(relativeDir.Item1, RandomAction(false), relativeDir.Item2, 6);
				break;
			}

			if(!actions.Contains(newDir) && newDir != null) actions.Add(newDir);
		}
	}

	Tuple<Direction, int> RandomRelativeDir() {
		int relativeTo = -1;
		Randomizer puppetRandomizer = Randomizer.New(actions.Count - 1);
		while(relativeTo == -1) {
			TiteresDirection puppet = actions[puppetRandomizer.Next()];

		}
		return null;
	}

	TiteresAction RandomAction(bool sitStand) {
		return (sitStand ? new List<TiteresAction>{ TiteresAction.SIT, TiteresAction.STANDING } : 
			new List<TiteresAction> { TiteresAction.LEFT_ARM, TiteresAction.RIGHT_ARM })
				[Randomizer.RandomInRange(1)];
	}

	void RandomizeDifficulties(List<List<int>> diffs) {
		personDifficulty = diffs.ConvertAll((List<int> diff) => {
			if(diff.Count == 1) return diff[0];
			else return diff[Randomizer.RandomInRange(diff.Count - 1)];
		});
	}

	public bool HasTime(){ return withTime; }

	public List<int> PersonQuantity() {
		return personQuantity;
	}

	public List<int> PersonDifficulty(){
		return personDifficulty;
	}
}