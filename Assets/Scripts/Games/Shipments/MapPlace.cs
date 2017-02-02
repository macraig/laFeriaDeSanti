using Assets.Scripts.Games.Shipments;
using UnityEngine;
using UnityEngine.UI;

public class MapPlace : MonoBehaviour
{
    public Image CrossReference;
    private int _id;

    public int Id
    {
        get { return _id; }
        set { _id = value; }
    }

    void Start()
    {
        Id = -1;
        GetComponent<Button>().onClick.AddListener(OnClickMapPlace);
    }

    public void SetData(int id, Sprite placesSprite, Sprite crossSprite)
    {
        this.Id = id;
        CrossReference.sprite = crossSprite;
        GetComponent<Image>().sprite = placesSprite;
    }

    private void OnClickMapPlace()
    {
        ShipmentsView.instance.OnClickMapPlace(_id);
    }
}