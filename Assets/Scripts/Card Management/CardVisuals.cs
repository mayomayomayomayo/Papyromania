using UnityEngine;
using TMPro;
using UnityEngine.UI;

public sealed class CardVisuals : MonoBehaviour
{
    [Header("Owner")]
    public Card owner;

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

    private void Start()
    {
        UpdateValues();
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

    public void UpdateValues()
    {
        nameField.text = owner.definition.name;
        descriptionField.text = owner.definition.description;
        baseImage.sprite = owner.definition.cardBase;
        artImage.sprite = owner.definition.cardArt;

        gameObject.name = owner.definition.name;
    }
}

