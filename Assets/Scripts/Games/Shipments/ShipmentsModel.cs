using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Common;

namespace Assets.Scripts.Games.Shipments
{
    public class ShipmentsModel : LevelModel
    {

        private const int NODES = 8;
        private int _currentLevel;
        private List<ShipmentNode> _nodes; 
        private List<ShipmentEdge> _edges;


        public ShipmentsModel()
        {
            _nodes = new List<ShipmentNode>();
            _edges = new List<ShipmentEdge>();
            _currentLevel = 0;
        }

        public List<ShipmentNode> Nodes
        {
            get { return _nodes; }
        }

        public List<ShipmentEdge> Edges
        {
            get { return _edges; }
        }


        public void NextExercise()
        {
            int nodes;

            switch (_currentLevel)
            {
                case 0:
                    nodes = 2;
                    GenerateNodes(nodes);
                    Edges.Clear();
                    ShipmentEdge edge1 = new ShipmentEdge
                    {
                        IdNodeA = _nodes[0].Id,
                        IdNodeB = _nodes[_nodes.Count - 1].Id
                    };

                    Edges.Add(edge1);
                    _currentLevel++;
                    break;
                case 1:
                    nodes = 3;
                    GenerateNodes(nodes);
                    Edges.Clear();
                    for (int i = 0; i < _nodes.Count - 1; i++)
                    {
                        ShipmentEdge edge = new ShipmentEdge
                        {
                            IdNodeA = _nodes[i].Id,
                            IdNodeB = _nodes[i + 1].Id
                        };

                        Edges.Add(edge);
                    }
                    
                    break;
            }
        }

        private void GenerateNodes(int totalNodes)
        {
            Nodes.Clear();
            GenerateStartNode();
            Randomizer nodeRandomizer = Randomizer.New(NODES - 1);
            nodeRandomizer.ExcludeNumbers(new List<int>() {0});
            for (int i = totalNodes - 3; i >= 0; i--)
            {
                ShipmentNode shipmentNode = new ShipmentNode
                {
                    Id = nodeRandomizer.Next(),
                    Type = ShipmentNodeType.Other
                };
                Nodes.Add(shipmentNode);
            }
            GenerateLastNode(nodeRandomizer.Next());

        }

        private void GenerateLastNode(int id)
        {
            ShipmentNode shipmentNode = new ShipmentNode
            {
                Id = id,
                Type = ShipmentNodeType.Finish
            };
            Nodes.Add(shipmentNode);
        }

        private void GenerateStartNode()
        {
            ShipmentNode shipmentNode = new ShipmentNode
            {
                Id = 0,
                Type = ShipmentNodeType.Start
            };
            Nodes.Add(shipmentNode);

        }
    }

    public class ShipmentNode
    {
        public int Id { get; set; }

        public ShipmentNodeType Type { get; set; }

        public bool Equals(ShipmentNode otherNode)
        {
            return Id == otherNode.Id;
        }
    }

    public enum ShipmentNodeType
    {
        Start, Finish, Other
    }

    public class ShipmentEdge
    {
        public int IdNodeA { get; set; }

        public int IdNodeB { get; set; }
    }
}