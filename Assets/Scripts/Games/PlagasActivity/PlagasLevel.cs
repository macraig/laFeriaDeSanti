using System;
using SimpleJSON;
using System.Collections.Generic;

public class PlagasLevel {
	bool withTime;
	int moleQuantity;
	List<int> spawnTimes, molesInSpawn;

	public PlagasLevel(JSONClass source) {
		withTime = source["withTime"].AsBool;

		if(withTime){
			spawnTimes = new List<JSONNode>(source["spawnTimes"].Childs).ConvertAll((n) => n.AsInt);
			molesInSpawn = new List<JSONNode>(source["molesInSpawn"].Childs).ConvertAll((n) => n.AsInt);
		} else {
			moleQuantity = source["moleQuantity"].AsInt;
		}
	}

	public bool HasTime(){ return withTime; }

	public int MoleQuantity(){ return moleQuantity; }
}