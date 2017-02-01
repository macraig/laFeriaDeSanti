using UnityEngine;

namespace Assets.Scripts.Games.Shipments
{
    public class ShipmentsView : LevelView
    {

        public MapGenerator MapGenerator;
        private ShipmentsModel _shipmentsModel;

        // Use this for initialization
        void Start () {
            _shipmentsModel = new ShipmentsModel();
		    //Next(true);
        }
	
        public override void Next(bool first = false)
        {
            _shipmentsModel.NextExercise();
            MapGenerator.LocatePlaces(_shipmentsModel.Nodes);
            MapGenerator.TraceEdges(_shipmentsModel.Edges);
        }

        


        
    }
}
