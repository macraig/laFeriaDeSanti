﻿using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Common;
using Assets.Scripts.Games.Shipments;
using UnityEngine;
using Vectrosity;
using Random = UnityEngine.Random;

public class MapGenerator : MonoBehaviour {

    public List<MapPlace> Places;
    public GameObject Map;
    public Ruler Ruler;

    public Texture dottedLineMaterial;
    public Material DottedLineMateriallMaterial;

    public Sprite[] CrosSprites;
    public Sprite[] PlacesSprites;

    private List<VectorLine> lines;

    // Use this for initialization
    void Start()
    {

/*
        LocatePlaces();
*/
  /*      foreach (MapPlace place in Places)
        {
            foreach (MapPlace otherPlace in Places)
            {
                Vector3 position = place.CrossReference.gameObject.transform.position;

                if (place.Equals(otherPlace)) continue;
                if (Random.Range(0f, 1f) < 0.1)
                {
                    Vector3 otherPosition = otherPlace.CrossReference.gameObject.transform.position;
                    VectorLine line = new VectorLine("Curve", new List<Vector2> { new Vector2(position.x, position.y), new Vector2(otherPosition.x, otherPosition.y) }, null, 8.0f, LineType.Continuous);

                    line.material = DottedLineMateriallMaterial;
                    line.textureScale = 1f;

                    line.Draw();
                    Debug.Log(line.GetLength());
                }
            }
        }*/
    }

    public void LocatePlaces(List<ShipmentNode> nodes, List<ShipmentEdge> edges)
    {
        foreach (MapPlace mapPlace in Places.GetRange(1, Places.Count - 1))
        {
            mapPlace.Id = -1;
        }


        float scaleFactor = FindObjectOfType<Canvas>().scaleFactor;
        float edge = Places[0].GetComponent<RectTransform>().sizeDelta.x;
        float distanceMin = Mathf.Sqrt(2)*edge;
        Vector2 mapSize = Map.GetComponent<RectTransform>().sizeDelta;
        float xMax = mapSize.x - edge / 2;
        float yMax = mapSize.y * scaleFactor - edge;

        List<MapPlace> locatedPlaces = new List<MapPlace>(Places.Count);
        SetFirstPlace(nodes[0], xMax, yMax);
        locatedPlaces.Add(Places[0]);
        int i = 1;
        for (; i < nodes.Count; i++)
        {
            /*
                        if(nodes[i].Id == 0) continue;
            */
            Places[i].SetData(nodes[i].Id, PlacesSprites[nodes[i].Id], CrosSprites[nodes[i].Type == ShipmentNodeType.Other ? 1 : 0], nodes[i].Type);
            List<int> edgeDistances = new List<int>();
         /*   List<MapPlace> listOfPlaces =
                locatedPlaces.FindAll(
                    e =>
                    {
                        ShipmentEdge shipmentEdge = ShipmentsView.instance.Model.GetEdgesByIdNode(Places[i].Id)
                            .Find(f => f.IdNodeA == e.Id || f.IdNodeB == e.Id);
                        if (shipmentEdge != null) edgeDistances.Add(shipmentEdge);
                        return shipmentEdge != null;
                    });*/
            List<MapPlace> listOfPlaces = locatedPlaces.FindAll(
                e =>
                {
                    ShipmentEdge shipmentEdge = edges.Find(f => (f.IdNodeA == Places[i].Id && f.IdNodeB == e.Id) || (f.IdNodeB == Places[i].Id && f.IdNodeA == e.Id));
                    if(shipmentEdge != null) edgeDistances.Add(shipmentEdge.Length);
                    return shipmentEdge != null;
                });
            do
            {
                LocatePlace(Places[i], xMax, yMax);
               
            } while (!CheckEdgeDistances(Places[i], listOfPlaces, edgeDistances) || !CheckMinDistances(Places[i], locatedPlaces, distanceMin * 2));
            Places[i].gameObject.SetActive(true);
        }
        for (int j = Places.Count - 1; j >= i; j--)
        {
            Places[j].gameObject.SetActive(false);
        }
    }

    private void SetFirstPlace(ShipmentNode node, float xMax, float yMax)
    {
        Places[0].SetData(node.Id, PlacesSprites[node.Id], CrosSprites[node.Type == ShipmentNodeType.Other ? 1 : 0], node.Type);
        Places[0].transform.localPosition = new Vector2(-xMax, yMax);
    }

    private bool CheckEdgeDistances(MapPlace mapPlace, List<MapPlace> locatedPlaces, List<int> edgeDistances)
    {
        for (var i = locatedPlaces.Count - 1; i >= 0; i--)
        {
            MapPlace locatedPlace = locatedPlaces[i];
/*
            float distance = Vector2.Distance(locatedPlace.transform.localPosition, mapPlace.transform.localPosition);
*/
            float referenceDistance = Vector2.Distance(locatedPlace.CrossReference.transform.position, mapPlace.CrossReference.transform.position);
            float f = referenceDistance / Ruler.GetUnityDistances();
            if (Math.Abs(f - edgeDistances[i]) > 0.1)
            {
                return false;
            }

        }
    
        return true;
    }


    private bool CheckMinDistances(MapPlace mapPlace, List<MapPlace> locatedPlaces, float distanceMin)
    {

        Debug.Log("checking min distances");

        foreach (MapPlace locatedPlace in locatedPlaces)
        {
            if(locatedPlace.Id == mapPlace.Id) continue;;
            float distance = Vector2.Distance(locatedPlace.transform.localPosition, mapPlace.transform.localPosition);
            float refDistance = Vector2.Distance(locatedPlace.CrossReference.transform.position, mapPlace.CrossReference.transform.position);
            /*
                        float referenceDistance = Vector2.Distance(locatedPlace.CrossReference.transform.position, mapPlace.CrossReference.transform.position);

                        float f = referenceDistance / Ruler.GetUnityDistances();
            */
            if (distance < distanceMin /*|| Math.Abs(f % 1) > 0.1 || Mathf.RoundToInt(f) > 10*/)
                return false;          
        }
        
        return true;
    }



    public void LocatePlace(MapPlace mapPlace, float xMax, float yMax)
    {/*
        mapPlace.transform.localPosition = new Vector2((Randomizer.RandomBoolean() ? 1 : -1)*Random.Range(0, xMax),
            (Randomizer.RandomBoolean() ? 1 : -1)*Random.Range(0, yMax));*/

        mapPlace.transform.localPosition = new Vector2(Random.Range(0, xMax),
            (Randomizer.RandomBoolean() ? 1 : -1)*Random.Range(-yMax, 0));
    }


    public void TraceEdges(List<ShipmentEdge> edges)
    {
        if (lines == null)
        {
            lines = new List<VectorLine>();

        }
        foreach (VectorLine line in lines)
        {
            line.rectTransform.gameObject.SetActive(false);
        }


        foreach (ShipmentEdge shipmentEdge in edges)
        {
            MapPlace mapPlace = Places.Find(e => e.Id == shipmentEdge.IdNodeA);
            MapPlace place = Places.Find(e => e.Id == shipmentEdge.IdNodeB);
            Vector3 position = mapPlace.CrossReference.gameObject.transform.position;
            Vector3 vector3 = place.CrossReference.gameObject.transform.position;
            VectorLine line = new VectorLine("Curve", new List<Vector2> { position, vector3 }, null, 8.0f, LineType.Continuous);

            line.SetCanvas(FindObjectOfType<Canvas>());

            line.material = DottedLineMateriallMaterial;
            line.textureScale = 1f;
            line.textureScale = 1f;
            line.Draw();
            lines.Add(line);
            line.rectTransform.transform.SetParent(Ruler.transform.parent);

        }
        Ruler.transform.SetAsLastSibling();
    }

    public void SafeLocatePlaces(List<ShipmentNode> nodes, List<ShipmentEdge> edges)
    {
        ResetIds();
        float edge = Places[0].GetComponent<RectTransform>().sizeDelta.x / 2;
        float distanceMin = Mathf.Sqrt(2) * edge;
        Vector2 mapSize = Map.GetComponent<RectTransform>().sizeDelta / 2;
        float xMax = mapSize.x  - edge;
        float yMax = mapSize.y - edge;
        List<MapPlace> locatedPlaces = new List<MapPlace>(Places.Count);

        /*SetFirstPlace(nodes[0], xMax, yMax);
        locatedPlaces.Add(Places[0]);*/
        int i = 0;
        nodes.Sort((node1, node2) => ShipmentsView.instance.Model.GetEdgesByIdNode(node2.Id).Count - ShipmentsView.instance.Model.GetEdgesByIdNode(node1.Id).Count);
        for (; i < nodes.Count; i++)
        {
            Places[i].gameObject.SetActive(true);
            Places[i].SetData(nodes[i].Id, PlacesSprites[nodes[i].Id], CrosSprites[nodes[i].Type == ShipmentNodeType.Other ? 1 : 0], nodes[i].Type);

            do
            {             
                SafeLocatePlace(Places[i], locatedPlaces, ShipmentsView.instance.Model.GetEdgesByIdNode(nodes[i].Id), xMax, yMax, distanceMin);
            } while (!CheckMinDistances(Places[i], locatedPlaces, distanceMin));


        }
        if (locatedPlaces.Count != nodes.Count)
        {
            Debug.Log("Error");
        }
        // Oculto los places no usados
        for (int j = Places.Count - 1; j >= i; j--)
        {
            Places[j].gameObject.SetActive(false);
        }
    }

    

    private void ResetIds()
    {
        foreach (MapPlace mapPlace in Places.GetRange(1, Places.Count - 1))
        {
            mapPlace.Id = -1;
        }
    }


    private void SafeLocatePlace(MapPlace toLocate, List<MapPlace> locatedPlaces, List<ShipmentEdge> toLocateEdges, float xMax, float yMax, float distanceMin)
    {
        Debug.Log("Locating place");
        foreach (ShipmentEdge edge in toLocateEdges)
        {
            MapPlace located = locatedPlaces.Find(e => (e.Id != toLocate.Id) &&  (e.Id == edge.IdNodeA || e.Id == edge.IdNodeB));
            if (located == null)
            {
                continue;
            }
            /*
                        Vector2 otherPosition = located.CrossReference.transform.position;
            */
            Vector2 otherPosition = located.transform.position;

            float f;
            float x;
            float y;
            do
            {
                toLocate.transform.localPosition = new Vector2((Randomizer.RandomBoolean() ? 1 : -1) * Random.Range(0, xMax), (Randomizer.RandomBoolean() ? 1 : -1) * Random.Range(0, yMax));
                float a = Ruler.GetUnityDistances();
                float distance = edge.Length == 0 ? Random.Range(3, 9) : edge.Length;

                if (Randomizer.RandomBoolean())
                {
                    // x random
                    x = toLocate.transform.position.x;

                    
                    float f1 = distance*a;
                    float f2 = x - otherPosition.x;
                    while (Mathf.Abs(f1) < Mathf.Abs(f2))
                    {
                        if (edge.Length != 0)
                        {
                            throw new Exception("Edge have to b resized but the node is located");
                        }
                        distance++;
                        f1 = distance*a;
                    }
                    float sqrt = Mathf.Sqrt(Mathf.Pow(f1, 2) - Mathf.Pow(f2, 2));
                    y = -sqrt + otherPosition.y;
                    /*
                                    var referenceDistance = Vector2.Distance(toLocate.CrossReference.transform.position, otherPosition);
                    */
                }
                else
                {
                    // y random
                    y = toLocate.transform.position.y;

                    float f1 = distance * a;
                    float f2 = y - otherPosition.y;
                    while (Mathf.Abs(f1) < Mathf.Abs(f2))
                    {
                        if (edge.Length != 0)
                        {
                            throw new Exception("Edge have to b resized but the node is located");
                        }
         
                        distance++;
                        f1 = distance * a;
                    }
                    float sqrt = Mathf.Sqrt(Mathf.Pow(f1, 2) - Mathf.Pow(f2, 2));
                    x = -sqrt + otherPosition.x;
                    /*
                                    var referenceDistance = Vector2.Distance(toLocate.CrossReference.transform.position, otherPosition);
                    */
                }


                toLocate.transform.position = new Vector2(x, y);
                var referenceDistance = Vector2.Distance(toLocate.transform.position, otherPosition);

                f = referenceDistance / a;
            } while (f < 1.8 || Math.Abs(f % 1) > 0.1 || f > 8.1 || Math.Abs(toLocate.transform.localPosition.y) >= yMax || Math.Abs(toLocate.transform.localPosition.x) >= xMax);

            edge.Length = (int) f;
            if(!locatedPlaces.Exists(e=> e.Id == toLocate.Id)) locatedPlaces.Add(toLocate);
        }

        if (!locatedPlaces.Exists(e => e.Id == toLocate.Id))
        {
            do
            {
                toLocate.transform.localPosition = new Vector2((Randomizer.RandomBoolean() ? 1 : -1) * Random.Range(0, xMax), (Randomizer.RandomBoolean() ? 1 : -1) * Random.Range(0, yMax));
            } while (!CheckMinDistances(toLocate, locatedPlaces, distanceMin));
            locatedPlaces.Add(toLocate);
        }


        /*

        Vector2 otherPosition = located.CrossReference.transform.position;

        float f;
        do
        {
            toLocate.transform.localPosition = new Vector2((Randomizer.RandomBoolean() ? 1 : -1) * Random.Range(0, xMax), (Randomizer.RandomBoolean() ? 1 : -1) * Random.Range(0, yMax));
            var referenceDistance = Vector2.Distance(toLocate.CrossReference.transform.position, otherPosition);
            f = referenceDistance / Ruler.GetUnityDistances();
        } while (f < 1.8 || Math.Abs(f % 1) > 0.1);

        ShipmentsView.instance.measuresList.Add((int)f);*/
    }

}
