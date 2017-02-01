using System;
using SimpleJSON;
using System.Collections.Generic;
using Assets.Scripts.Common;

public class TiteresLevel {
	bool withTime;
	List<int> personQuantity, personDifficulty;

	public TiteresLevel(JSONClass source) {
		withTime = source["withTime"].AsBool;

		if(withTime){
			personQuantity = new List<JSONNode>(source["personQuantity"].Childs).ConvertAll((n) => n.AsInt);
			List<List<int>> diffs = new List<JSONNode>(source["personDifficulty"].Childs)
				.ConvertAll((n) => new List<JSONNode>(n.AsArray.Childs).ConvertAll((inner) => inner.AsInt));

			RandomizeDifficulties(diffs);
		}
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