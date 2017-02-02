using System;
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
        lines = new List<VectorLine>();

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
        float xMax = mapSize.x * scaleFactor - edge;
        float yMax = mapSize.y * scaleFactor - edge;

        List<MapPlace> locatedPlaces = new List<MapPlace>(Places.Count);
        SetFirstPlace(nodes[0], xMax, yMax);
        locatedPlaces.Add(Places[0]);
        int i = 1;
        for (; i < nodes.Count; i++)
        {
            Places[i].SetData(nodes[i].Id, PlacesSprites[nodes[i].Id], CrosSprites[nodes[i].Type == ShipmentNodeType.Other ? 1 : 0]);
            do
            {
                LocatePlace(Places[i], xMax, yMax);
            } while (!CheckMinDistances(Places[i], locatedPlaces.FindAll(
                e =>
                {
                    ShipmentEdge shipmentEdge = edges.Find(f => (f.IdNodeA == Places[i].Id && f.IdNodeB == e.Id) || (f.IdNodeB == Places[i].Id && f.IdNodeA == e.Id));
                    return shipmentEdge != null;
                }), distanceMin));
            Places[i].gameObject.SetActive(true);
        }
        for (int j = Places.Count - 1; j >= i; j--)
        {
            Places[j].gameObject.SetActive(false);
        }
    }

    private void SetFirstPlace(ShipmentNode node, float xMax, float yMax)
    {
        Places[0].SetData(node.Id, PlacesSprites[node.Id], CrosSprites[node.Type == ShipmentNodeType.Other ? 1 : 0]);
        Places[0].transform.localPosition = new Vector2(-xMax, yMax);
    }

    private bool CheckMinDistances(MapPlace mapPlace, List<MapPlace> locatedPlaces, float distanceMin)
    {
        foreach (MapPlace locatedPlace in locatedPlaces)
        {
            float distance = Vector2.Distance(locatedPlace.transform.localPosition, mapPlace.transform.localPosition);
            float referenceDistance = Vector2.Distance(locatedPlace.CrossReference.transform.position, mapPlace.CrossReference.transform.position);
            float f = referenceDistance / Ruler.GetUnityDistances();
            Debug.Log(f);
            if (distance < distanceMin || Math.Abs(f % 1) > 0.1 || Mathf.RoundToInt(f) > 10)
                return false;          
        }
        
        return true;
    }

    private void LocatePlace(MapPlace mapPlace, float xMax, float yMax)
    {
        mapPlace.transform.localPosition = new Vector2((Randomizer.RandomBoolean() ? 1  : -1) * Random.Range(0, xMax), (Randomizer.RandomBoolean() ? 1 : -1) * Random.Range(0, yMax));
    }


    public void TraceEdges(List<ShipmentEdge> edges)
    {
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

            line.Draw();
            lines.Add(line);
            line.rectTransform.transform.SetParent(Ruler.transform.parent);

        }
        Ruler.transform.SetAsLastSibling();
    }
}
