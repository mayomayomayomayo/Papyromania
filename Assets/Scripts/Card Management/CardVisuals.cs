using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CardVisuals : MonoBehaviour
{
    public Card card; // Figure out how to set this

    [Header("Fields")]
    public TMP_Text nameField;
    public TMP_Text descriptionField;
    public TMP_Text leftField;
    public TMP_Text rightField;
    public Image baseImage;
    public Image artImage;

    private void Awake()
    {
        GetRefs();
    }
    
    private void GetRefs()
    {
        TMP_Text GetText(string str) => transform.Find(str).GetComponent<TMP_Text>();
        Image GetImage(string str) => transform.Find(str).GetComponent<Image>();

        nameField = GetText("CardName");
        descriptionField = GetText("CardDescription");
        leftField = GetText("CardFieldLeft");
        rightField = GetText("CardFieldRight");
        baseImage = GetImage("CardBase");
        artImage = GetImage("CardArt");
    }

    public virtual void UpdateValues()
    {
        nameField.text = card.data.name;
        descriptionField.text = card.data.description;
        baseImage.sprite = card.data.cardBase;
        artImage.sprite = card.data.cardArt;
    }
}

