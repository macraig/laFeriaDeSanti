using System;
using SimpleJSON;
using System.Collections.Generic;
using Assets.Scripts.Common;

public class TiteresLevel {
	List<TiteresDirection> actions, actionsToShow;

	public TiteresLevel(JSONClass source, bool withTime) {
		List<int> personQuantity = new List<JSONNode>(source["personQuantity"].Childs).ConvertAll((n) => n.AsInt);
		List<List<int>> diffs = new List<JSONNode>(source["personDifficulty"].Childs)
			.ConvertAll((n) => new List<JSONNode>(n.AsArray.Childs).ConvertAll((inner) => inner.AsInt));

		List<int> difficulties = RandomizeDifficulties(personQuantity, diffs);

		SetActions(difficulties, withTime);
	}

	void SetActions(List<int> difficulties, bool withTime) {
		actions = new List<TiteresDirection>();
		Direction[] dirs = (Direction[]) Enum.GetValues(typeof(Direction));
		Randomizer dirRandomizer = Randomizer.New(dirs.Length - 2);
		TiteresDirection newDir = null;

		while(actions.Count < difficulties.Count) {
			int current = actions.Count;
			Direction dir = dirs[dirRandomizer.Next()];
			Tuple<Direction, int> relativeDir = null;

			switch(difficulties[current]) {
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

		actionsToShow = withTime ? Randomizer.RandomizeList(actions) : actions;
	}

	Tuple<Direction, int> RandomRelativeDir() {
		Randomizer puppetRandomizer = Randomizer.New(actions.Count - 1);
		int puppetNumber = puppetRandomizer.Next();
		TiteresDirection puppet = actions[puppetNumber];

		Direction d = RandomDirection(puppet.direction == Direction.LEFT || puppet.direction == Direction.RIGHT);

		return Tuple.Create(d, puppetNumber);
	}

	Direction RandomDirection(bool upDown) {
		return (upDown ? new List<Direction>{ Direction.UP, Direction.DOWN } : 
			new List<Direction> { Direction.LEFT, Direction.RIGHT })
			[Randomizer.RandomInRange(1)];
	}

	TiteresAction RandomAction(bool sitStand) {
		return (sitStand ? new List<TiteresAction>{ TiteresAction.SIT, TiteresAction.STANDING } : 
			new List<TiteresAction> { TiteresAction.LEFT_ARM, TiteresAction.RIGHT_ARM })
				[Randomizer.RandomInRange(1)];
	}

	List<int> RandomizeDifficulties(List<int> personQuantity, List<List<int>> diffs) {
		List<int> result = new List<int>();
		for(int i = 0; i < personQuantity.Count; i++) {
			for (int j = 0; j < personQuantity[i]; j++) {
				if(diffs[i].Count == 1) result.Add(diffs[i][0]);
				else result.Add(diffs[i][Randomizer.RandomInRange(diffs[i].Count - 1)]);
			}
		}
		return result;
	}

	public List<TiteresDirection> Actions() {
		return actions;
	}

	public List<TiteresDirection> ActionsToShow() {
		return actionsToShow;
	}
}