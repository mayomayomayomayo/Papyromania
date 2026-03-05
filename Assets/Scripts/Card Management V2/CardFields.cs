using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CardFields : MonoBehaviour
{
    public TMP_Text NameField { get; private set; }

    public TMP_Text DescriptionField { get; private set; }
 
    public TMP_Text LeftField { get; private set; }
 
    public TMP_Text RightField { get; private set; }
 
    public Image BaseImage { get; private set; }
 
    public Image ArtImage { get; private set; }
 
    public Image LeftFieldIcon { get; private set; }
 
    public Image RightFieldIcon { get; private set; }

    private void Awake()
    {
        T Get<T>(string str) where T : MonoBehaviour => 
        transform.Find(str).GetComponent<T>();

        NameField = Get<TMP_Text>("CardName");
        DescriptionField = Get<TMP_Text>("CardDescription");
        LeftField = Get<TMP_Text>("CardFieldLeft");
        RightField = Get<TMP_Text>("CardFieldRight");
        BaseImage = Get<Image>("CardBase");
        ArtImage = Get<Image>("CardArt");
        LeftFieldIcon = Get<Image>("LeftFieldIcon");
        RightFieldIcon = Get<Image>("RightFieldIcon");
    }

    public void UpdateFields(Card card)
    {
        NameField.text = card.Name;
        DescriptionField.text = card.Description;
        LeftField.text = card.LeftField;
        RightField.text = card.RightField;
        BaseImage.sprite = card.BaseSprite;
        ArtImage.sprite = card.ArtSprite;
        LeftFieldIcon.sprite = card.LeftFieldIcon;
        RightFieldIcon.sprite = card.RightFieldIcon;
    }
}