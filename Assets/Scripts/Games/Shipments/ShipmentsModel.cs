using System;
using System.Collections.Generic;
using Assets.Scripts.Common;
using Assets.Scripts.Games.Shipments;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Games.Shipments
{
    public class ShipmentsModel : LevelModel
    {

        private const int NODES = 16;
        private int _currentLevel;
        private List<ShipmentNode> _nodes; 
        private List<ShipmentEdge> _edges;
        private List<ShipmentsPath> _solutionPaths;
        private bool lastCorrect;


        public ShipmentsModel()
        {
            _nodes = new List<ShipmentNode>();
            _edges = new List<ShipmentEdge>();
            _solutionPaths = new List<ShipmentsPath>();
            _currentLevel = 3;
            lastCorrect = true;
        }

        public List<ShipmentNode> Nodes
        {
            get { return _nodes; }
        }

        public List<ShipmentEdge> Edges
        {
            get { return _edges; }
        }

        public int Scale { get; set; }


        public void NextExercise()
        {
            int nodes;
            int solutionPaths;
            List<int> edgesBySolutionPath;
            float extraEdgeProbability;
            Scale = Randomizer.RandomBoolean() ? 5 : 10;
            _solutionPaths.Clear();
            Nodes.Clear();
            Edges.Clear();
            Debug.Log("current lvl: " + (_currentLevel+1));

            switch (_currentLevel)
            {
                case 0:
                    nodes = 2;
                    solutionPaths = 1;
                    edgesBySolutionPath = new List<int>(solutionPaths) {1};
                    extraEdgeProbability = 0;
                    break;
                case 1:
                    nodes = Random.Range(3, 6);
                    solutionPaths = 1;
                    edgesBySolutionPath = new List<int>(solutionPaths) { 2 };
                    extraEdgeProbability = 0;

                    break;
                case 2:
                    nodes = Random.Range(5, 8);
                    solutionPaths = 1;
                    edgesBySolutionPath = new List<int>(solutionPaths)
                    { 2 };
                    extraEdgeProbability = 0.5f;
              
                    break;
                case 3:
                    nodes = Random.Range(6, 9);
                    solutionPaths = 1;
                    edgesBySolutionPath = new List<int>(solutionPaths)
                    { Mathf.CeilToInt((nodes - 1 ) * Random.Range(0.5f, 0.8f)),
                        Mathf.CeilToInt((nodes - 1) * Random.Range(0.5f, 0.8f)) };
                    extraEdgeProbability = 0;

                    break;
                default:
                    nodes = 9;
                    solutionPaths = 1;
                    edgesBySolutionPath = new List<int>(solutionPaths)
                    { Mathf.CeilToInt((nodes - 1) * 0.8f),
                        Mathf.CeilToInt((nodes  - 1)  * 0.8f) };
                    extraEdgeProbability = 1;
                    break;
                    /*     case 3:
                             nodes = Random.Range(6, 9);
                             solutionPaths = Random.Range(2, 4);
                             edgesBySolutionPath = new List<int>(solutionPaths);
                             for (int i = edgesBySolutionPath.Capacity - 1; i >= 0; i--)
                             {
                                 edgesBySolutionPath.Add(Mathf.CeilToInt((nodes - 1) * Random.Range(0.5f, 1)));
                             }
                             extraEdgeProbability = 0.5f;
                             maxLongEdge = 4;

                             break;
                         case 4:
                             nodes = Random.Range(6, 9);
                             solutionPaths = Random.Range(2, 4);
                             edgesBySolutionPath = new List<int>(solutionPaths);
                             for (int i = edgesBySolutionPath.Capacity - 1; i >= 0; i--)
                             {
                                 edgesBySolutionPath.Add(Mathf.CeilToInt((nodes - 1) * Random.Range(0.5f, 1)));
                             }
                             extraEdgeProbability = 0.5f;
                             maxLongEdge = 4;

                             break;
                         default:
                             nodes = Random.Range(6, 9);
                             solutionPaths = Random.Range(2, 4);
                             edgesBySolutionPath = new List<int>(solutionPaths);
                             for (int i = edgesBySolutionPath.Capacity - 1; i >= 0; i--)
                             {
                                 edgesBySolutionPath.Add(Mathf.CeilToInt((nodes - 1) * Random.Range(0.5f, 1)));
                             }
                             extraEdgeProbability = 0.5f;
                             maxLongEdge = 4;

                             break;*/
            }
           
            GenerateNodes(nodes);
       /*     ShipmentsPath shipmentsPath = new ShipmentsPath();
            shipmentsPath.NodesList = new List<ShipmentNode>(4);
            shipmentsPath.NodesList.Add(Nodes[0]);
            shipmentsPath.NodesList.Add(Nodes[1]);
            shipmentsPath.NodesList.Add(Nodes[2]);
            shipmentsPath.NodesList.Add(Nodes[Nodes.Count - 1]);

            ShipmentsPath shipmentsPath2 = new ShipmentsPath();
            shipmentsPath2.NodesList = new List<ShipmentNode>(4);
            shipmentsPath2.NodesList.Add(Nodes[0]);
            shipmentsPath2.NodesList.Add(Nodes[3]);
            shipmentsPath2.NodesList.Add(Nodes[4]);
            shipmentsPath2.NodesList.Add(Nodes[Nodes.Count - 1]);
            _solutionPaths.Add(shipmentsPath);
            _solutionPaths.Add(shipmentsPath2);*/

            GenerateSolutionPaths(solutionPaths, edgesBySolutionPath);
            while (SolutionsPathsAreEquals())
            {
                _solutionPaths.Clear();
                _edges.Clear();
                GenerateSolutionPaths(solutionPaths, edgesBySolutionPath);
            }
/*
            GenerateEdgesToSolutionPaths();
*/


/*
            GenerateExtraEdges(extraEdgeProbability);
*/



          _currentLevel++;
/*
            if (_currentLevel == 5) _currentLevel = 0;
*/
        }

        private bool SolutionsPathsAreEquals()
        {
            for (int i = _solutionPaths.Count - 1; i >= 0; i--)
            {
                for (int j = _solutionPaths.Count - 1; j >= 0; j--)
                {
                    if(i == j) continue;;
                    if (_solutionPaths[i].Equals(_solutionPaths[j])) return true;

                }
            }

            return false;
        }

        private void GenerateExtraEdges(float extraEdgeProbability)
        {
            if(Math.Abs(extraEdgeProbability) < 0.000001) return;
            foreach (ShipmentNode node in Nodes)
            {
                foreach (ShipmentNode otherNode in Nodes)
                {
                    if(node.Id == otherNode.Id) continue;
                    if(GetEdge(node.Id, otherNode.Id) != null) continue;
                    if (Random.Range(0, 1) < extraEdgeProbability && GetEdgesByIdNode(node.Id).Count < 2 && GetEdgesByIdNode(otherNode.Id).Count < 2 && Edges.Count < Nodes.Count * 2 )
                    {
                        ShipmentEdge shipmentEdge = new ShipmentEdge
                        {
                            IdNodeA = node.Id,
                            IdNodeB = otherNode.Id,
                        };
                        Edges.Add(shipmentEdge);
                    }
                }
            }
        }

     /*   private void GenerateEdgesToSolutionPaths()
        {
            foreach (ShipmentsPath shipmentsPath in _solutionPaths)
            {
                shipmentsPath.EdgesList = new List<ShipmentEdge>(shipmentsPath.NodesList.Count - 1);
                for (var i = 0; i < shipmentsPath.NodesList.Count - 1; i++)
                {

                    ShipmentEdge shipmentEdge = GetEdge(shipmentsPath.NodesList[i].Id, shipmentsPath.NodesList[i + 1].Id);
                    if (shipmentEdge == null)
                    {
                        shipmentEdge = new ShipmentEdge
                        {
                            IdNodeA = shipmentsPath.NodesList[i].Id,
                            IdNodeB = shipmentsPath.NodesList[i + 1].Id,
                        };
                        Edges.Add(shipmentEdge);
                    }


                    shipmentsPath.EdgesList.Add(shipmentEdge);
                }
            }
        }*/

        private ShipmentEdge GetEdge(int idA, int idB)
        {
            return Edges.Find(e => (e.IdNodeA == idA && e.IdNodeB == idB) || (e.IdNodeA == idB && e.IdNodeB == idA));
        }

        private void GenerateSolutionPaths(int solutionPaths, List<int> edgesBySolutionPath)
        {
            // hago - 2 xq el ultimo se lo voy a agregar al final. El 0 lo agrego si o si. 
            Randomizer nodeRandomizer = Randomizer.New(Nodes.Count - 1, 0, false);
            for (int i = solutionPaths - 1; i >= 0; i--)
            {
                nodeRandomizer.Restart();
                int pathLength = edgesBySolutionPath[i];
                ShipmentsPath shipmentsPath = new ShipmentsPath
                {
                    // Agrego el primero
                    // la cantidad de nodos es la de aristas + 1
                    NodesList = new List<ShipmentNode>(pathLength + 1) {Nodes[0]}
                };
                // es -3 xq el 0 y el ultimo ya estan fijos
                for (int j = 1; j <= shipmentsPath.NodesList.Capacity - 1; j++)
                {
                    ShipmentNode shipmentNode = Nodes[nodeRandomizer.Next()];
                    while (shipmentsPath.NodesList.Exists(e => e.Id == shipmentNode.Id) || GetEdgesByIdNode(shipmentNode.Id).Count >= 2)
                    {
                        shipmentNode = Nodes[nodeRandomizer.Next()];
                    }
                    ShipmentEdge edge = new ShipmentEdge();
                    shipmentsPath.NodesList.Add(shipmentNode);
                    edge.IdNodeA = shipmentsPath.NodesList[j - 1].Id;
                    edge.IdNodeB = shipmentsPath.NodesList[j].Id;
                    Edges.Add(edge);
                                      
                }
                // Agrego el ultimo
            /*    shipmentsPath.NodesList.Add(Nodes[Nodes.Count - 1]);
                ShipmentEdge lastEdge = new ShipmentEdge();*/
              /*  lastEdge.IdNodeA = shipmentsPath.NodesList[shipmentsPath.NodesList.Count - 2].Id;
                lastEdge.IdNodeB = shipmentsPath.NodesList[shipmentsPath.NodesList.Count - 1].Id;*/
/*
                Edges.Add(lastEdge);
*/

                _solutionPaths.Add(shipmentsPath);          
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

        public bool IsCorrectAnswer(List<ShipmentEdge> edgeAnswers)
        {
/*
            List<int> measuresList = ShipmentsView.instance.measuresList;
*/
            for (int i = 0; i < edgeAnswers.Count; i++)
            {
                edgeAnswers[i].Length = edgeAnswers[i].Length/Scale;
                ShipmentEdge edge = Edges.Find(e => e.Equals(edgeAnswers[i]));
                if (edge == null) return false;
            }
            bool isCorrectAnswer = edgeAnswers[edgeAnswers.Count - 1].IdNodeB == Nodes.Find(e => e.Type == ShipmentNodeType.Finish).Id;
            if (lastCorrect) _currentLevel++;
            lastCorrect = isCorrectAnswer;
            return isCorrectAnswer;


            /*    foreach (ShipmentsPath shipmentsPath in _solutionPaths)
            {
                if(shipmentsPath.EdgesList.Count != edgeAnswers.Count) continue;
                int i = shipmentsPath.EdgesList.Count - 1;
                for (; i >= 0; i--)
                {
                    if(shipmentsPath.EdgesList[i].IdNodeA != edgeAnswers[i].IdNodeA) break;
                    if(shipmentsPath.EdgesList[i].IdNodeB != edgeAnswers[i].IdNodeB) break;
                    if(shipmentsPath.EdgesList[i].Length != edgeAnswers[i].Length / Scale) break;
                }
                if (i == -1) return true;

            }
            return false;*/

        }

        public List<ShipmentEdge> GetEdgesByIdNode(int idNode)
        {
            return Edges.FindAll(e => e.IdNodeA == idNode || e.IdNodeB == idNode);
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

        public int Length { get; set; }

        public bool Equals(ShipmentEdge otherEdge)
        {
            return Length == otherEdge.Length && (IdNodeA == otherEdge.IdNodeA && IdNodeB == otherEdge.IdNodeB) || (IdNodeB == otherEdge.IdNodeA && IdNodeA == otherEdge.IdNodeB);

        }
    }
}

public class ShipmentsPath
{
    public List<ShipmentNode> NodesList { get; set; }

    public bool Equals(ShipmentsPath otherPath)
    {
        if (NodesList.Count != otherPath.NodesList.Count) return false;
        for (int i = NodesList.Count - 1; i >= 0; i--)
        {
            if (NodesList[i].Id != otherPath.NodesList[i].Id) return false;
        }
        return true;
    }

    /*
        public List<ShipmentEdge> EdgesList { get; set; }

        public int GetTotalCost()
        {
            int cost = 0;
            foreach (ShipmentEdge edge in EdgesList)
            {
                cost += edge.Length;
            }
            return cost;
        }
        */

}