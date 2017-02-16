using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EditorComponentDoor : EditorComponent
{
    QuestData.Door doorComponent
    EditorSelectionList colorList;

    public EditorComponentDoor(string name) : base()
    {
        Game game = Game.Get();
        doorComponent = game.quest.qd.components[name] as QuestData.Door;
        component = doorComponent;
        name = component.name;
        Update();
    }
    
    override public Update()
    {
        base.Update();
        Game game = Game.Get();
        CameraController.SetCamera(doorComponent.location);

        TextButton tb = new TextButton(new Vector2(0, 0), new Vector2(3, 1), "Door", delegate { QuestEditorData.TypeSelect(); });
        tb.button.GetComponent<UnityEngine.UI.Text>().fontSize = UIScaler.GetSmallFont();
        tb.button.GetComponent<UnityEngine.UI.Text>().alignment = TextAnchor.MiddleRight;
        tb.ApplyTag("editor");

        tb = new TextButton(new Vector2(3, 0), new Vector2(16, 1), name.Substring("Door".Length), delegate { QuestEditorData.ListDoor(); });
        tb.button.GetComponent<UnityEngine.UI.Text>().fontSize = UIScaler.GetSmallFont();
        tb.button.GetComponent<UnityEngine.UI.Text>().alignment = TextAnchor.MiddleLeft;
        tb.ApplyTag("editor");

        tb = new TextButton(new Vector2(19, 0), new Vector2(1, 1), "E", delegate { Rename(); });
        tb.button.GetComponent<UnityEngine.UI.Text>().fontSize = UIScaler.GetSmallFont();
        tb.ApplyTag("editor");

        DialogBox db = new DialogBox(new Vector2(0, 2), new Vector2(4, 1), "Position");
        db.ApplyTag("editor");

        tb = new TextButton(new Vector2(4, 2), new Vector2(1, 1), "><", delegate { GetPosition(); });
        tb.button.GetComponent<UnityEngine.UI.Text>().fontSize = UIScaler.GetSmallFont();
        tb.ApplyTag("editor");

        tb = new TextButton(new Vector2(0, 4), new Vector2(8, 1), "Rotation (" + d.rotation + ")", delegate { Rotate(); });
        tb.button.GetComponent<UnityEngine.UI.Text>().fontSize = UIScaler.GetSmallFont();
        tb.ApplyTag("editor");

        tb = new TextButton(new Vector2(0, 6), new Vector2(8, 1), "Color", delegate { Colour(); });
        tb.button.GetComponent<UnityEngine.UI.Text>().fontSize = UIScaler.GetSmallFont();
        tb.ApplyTag("editor");

        tb = new TextButton(new Vector2(0, 8), new Vector2(8, 1), "Event", delegate { QuestEditorData.SelectAsEvent(name); });
        tb.button.GetComponent<UnityEngine.UI.Text>().fontSize = UIScaler.GetSmallFont();
        tb.ApplyTag("editor");

        game.tokenBoard.AddHighlight(doorComponent.location, "DoorAnchor", "editor");

        game.quest.ChangeAlpha(doorComponent.name, 1f);
    }

    public void Rotate()
    {
        if (doorComponent.rotation == 0)
        {
            doorComponent.rotation = 90;
        }
        else
        {
            doorComponent.rotation = 0;
        }
        Game.Get().quest.Remove(doorComponent.name);
        Game.Get().quest.Add(doorComponent.name);
        Update();
    }

    public void Colour()
    {
        List<string> colours = new List<string>();
        foreach (KeyValuePair<string, string> kv in ColorUtil.LookUp())
        {
            colours.Add(kv.Key);
        }
        colorList = new EditorSelectionList("Select Item", colours, delegate { SelectColour(); });
        colorList.SelectItem();
    }

    public void SelectColour()
    {
        doorComponent.colourName = colorList.selection;
        Game.Get().quest.Remove(doorComponent.name);
        Game.Get().quest.Add(doorComponent.name);
        SelectComponent(doorComponent.name);
    }

}
