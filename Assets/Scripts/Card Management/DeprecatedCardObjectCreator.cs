// using System;
// using TMPro;
// using UnityEngine;
// using UnityEngine.UI;
//
// D
// E
// P
// R
// E
// C
// A
// T
// E
// D
//
// public static class DeprecatedCardObjectCreator // Here for... historical purpose
// {
//     // I still think GameObject.Instantiate looks better than UnityEngine.Object.Instantiate, but whatever you say, God
//     public static GameObject MakeCardGameObject() => UnityEngine.Object.Instantiate(Resources.Load<GameObject>("Prefabs/CardCanvas"));
// 
//     public static Type ParsedCardSubclass(CardData data, out CardObject.CardType cardType) // Outing here is kinda sloppy but gets the job done i guess
//     {
//         if (!Enum.TryParse(data.type, true, out CardObject.CardType type))
//             throw new Exception($"Couldn't parse invalid card type [{data.type}] for card [{data.name}]");
// 
//         cardType = type;
// 
//         return type switch // SWITCH EXPRESSIONS MY BELOVED
//         {
//             CardObject.CardType.Gun => typeof(GunCardObject),
//             CardObject.CardType.Consumable => typeof(ConsumableCardObject),
//             _ => throw new Exception("I don't know how you managed to fuck it up this bad, but woah, congrats.")
//             // Pretty sure there's some way to convert strings to types but whatever, yandev style it is.
//             // Post-deprecation edit: yeah
//         };
//     }
// 
//     public static CardObject InitializeCardValues(CardData data)
//     {
//         Type cardSubclass = ParsedCardSubclass(data, out var type); // Subclasses amirite
//         CardObject cardObj = (CardObject)MakeCardGameObject().AddComponent(cardSubclass);
// 
//         cardObj.cardType = type;
// 
//         CardObject.CardModifiers[] mods = new CardObject.CardModifiers[data.mods.Length];
// 
//         for (int i = 0; i < data.mods.Length; i++)
//         {
//             if (Enum.TryParse(data.mods[i], true, out CardObject.CardModifiers modifier)) mods[i] = modifier;
//             else throw new Exception($"Couldn't parse invalid modifier [{data.mods[i]}] for card [{data.name}]");
//         }
//         cardObj.modifiers = mods;
// 
//         cardObj.cardName = data.name;
//         cardObj.cardDescription = data.description;
// 
//         cardObj.cardBase = Resources.Load<Sprite>($"Sprites/{data.type}_Card_Template");
//         cardObj.cardArt = Resources.Load<Sprite>(data.pathToSprite);
// 
//         if (cardObj.cardBase == null || cardObj.cardArt == null)
//             throw new Exception($"Sprite not found (Custom card types are already forbidden, so it's likely the card art (at {data.pathToSprite}))");
// 
//         Transform t = cardObj.transform;
// 
//         Image GetImage(string name) => t.Find(name).GetComponent<Image>();
//         TMP_Text GetText(string name) => t.Find(name).GetComponent<TMP_Text>();
// 
//         GetImage("CardBase").sprite = cardObj.cardBase;
//         GetImage("CardArt").sprite = cardObj.cardArt;
//         GetText("CardName").text = cardObj.cardName;
//         GetText("CardDescription").text = cardObj.cardDescription;
// 
//         if (cardObj is IInitializable ini) ini.InitializeValues(data);
// 
//         cardObj.state = CardObject.State.Dropped;
// 
//         cardObj.name = data.name;
// 
//         return cardObj;
// 
//         // yipee
//     }
// }
// 
// 