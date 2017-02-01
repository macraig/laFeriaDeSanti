using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Common;
using UnityEngine;
using Vectrosity;
using Random = UnityEngine.Random;

public class MapGenerator : MonoBehaviour {

    public List<MapPlace> Places;
    public GameObject Map;
    public Ruler Ruler;

    public Texture dottedLineMaterial;
    public Material DottedLineMateriallMaterial;

    // Use this for initialization
    void Start()
    {
        LocatePlaces();

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

    public void LocatePlaces()
    {
        float scaleFactor = FindObjectOfType<Canvas>().scaleFactor;
        float edge = Places[0].GetComponent<RectTransform>().sizeDelta.x;
        float distanceMin = Mathf.Sqrt(2) * edge;
        Vector2 mapSize = Map.GetComponent<RectTransform>().sizeDelta;
        float xMax = mapSize.x * scaleFactor - edge;
        float yMax = mapSize.y * scaleFactor - edge;

        List<MapPlace> locatedPlaces = new List<MapPlace>(Places.Count);
        SetFirstPlace(xMax, yMax);
        locatedPlaces.Add(Places[0]);

        foreach (MapPlace mapPlace in Places.GetRange(1, Places.Count - 1))
        {
            do
            {
                LocatePlace(mapPlace, xMax, yMax);
            } while (!CheckMinDistances(mapPlace, locatedPlaces, distanceMin));

            locatedPlaces.Add(mapPlace);

        }

        /*foreach (MapPlace mapPlace in Places)
        {
            mapPlace.transform.localPosition = new Vector2(xMax - edge, yMax - edge);
        }*/
    }

    private void SetFirstPlace(float xMax, float yMax)
    {
        Places[0].transform.localPosition = new Vector2(-xMax, yMax);
    }

    private bool CheckMinDistances(MapPlace mapPlace, List<MapPlace> locatedPlaces, float distanceMin)
    {
        foreach (MapPlace locatedPlace in locatedPlaces)
        {
            float distance = Vector2.Distance(locatedPlace.transform.localPosition, mapPlace.transform.localPosition);
            //float referenceDistance = Vector2.Distance(locatedPlace.CrossReference.transform.localPosition, mapPlace.CrossReference.transform.localPosition);
            if (distance < distanceMin || Math.Abs((distance / Ruler.GetUnityDistances()) % 1) > 0.1)
                return false;
        }

        return true;
    }

    private void LocatePlace(MapPlace mapPlace, float xMax, float yMax)
    {
        mapPlace.transform.localPosition = new Vector2((Randomizer.RandomBoolean() ? 1  : -1) * Random.Range(0, xMax), (Randomizer.RandomBoolean() ? 1 : -1) * Random.Range(0, yMax));
    }


}
