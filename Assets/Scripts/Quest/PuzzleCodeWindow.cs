﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class PuzzleCodeWindow
{
    public EventManager.Event eventData;
    QuestData.Puzzle questPuzzle;
    public PuzzleCode puzzle;
    public List<int> guess;

    public PuzzleCodeWindow(EventManager.Event e)
    {
        eventData = e;
        Game game = Game.Get();

        guess = new List<int>();
        questPuzzle = e.qEvent as QuestData.Puzzle;

        if (game.quest.puzzle.ContainsKey(questPuzzle.name))
        {
            puzzle = game.quest.puzzle[questPuzzle.name] as PuzzleCode;
        }
        else
        {
            puzzle = new PuzzleCode(questPuzzle.puzzleLevel, questPuzzle.puzzleAltLevel);
        }

        CreateWindow();
    }

    public void CreateWindow()
    {
        Destroyer.Dialog();
        DialogBox db = new DialogBox(new Vector2(UIScaler.GetHCenter(-14f), 0.5f), new Vector2(28f, 22f), "");
        db.AddBorder();

        // Puzzle goes here

        float hPos = UIScaler.GetHCenter(-13f);
        if (!puzzle.Solved())
        {
            for (int i = 1; i <= questPuzzle.puzzleAltLevel; i++)
            {
                int tmp = i;
                new TextButton(new Vector2(hPos, 1.5f), new Vector2(2f, 2), i.ToString(), delegate { GuessAdd(tmp); });
                hPos += 2.5f;
            }
        }

        float vPos = 7f;
        foreach (PuzzleCode.CodeGuess g in puzzle.guess)
        {
            hPos = UIScaler.GetHCenter(-13f);
            foreach (int i in g.guess)
            {
                db = new DialogBox(new Vector2(hPos, vPos), new Vector2(2f, 2f), i.ToString());
                db.textObj.GetComponent<UnityEngine.UI.Text>().fontSize = UIScaler.GetMediumFont();
                db.AddBorder();
                hPos += 2.5f;
            }

            hPos = UIScaler.GetHCenter();
            for (int i = 0; i < g.CorrectSpot(); i++)
            {
                db = new DialogBox(new Vector2(hPos, vPos), new Vector2(2f, 2f), "");
                db.textObj.GetComponent<UnityEngine.UI.Text>().fontSize = UIScaler.GetMediumFont();
                db.AddBorder();
                hPos += 2.5f;
            }
            for (int i = 0; i < g.CorrectSpot(); i++)
            {
                db = new DialogBox(new Vector2(hPos, vPos), new Vector2(2f, 2f), "");
                db.textObj.GetComponent<UnityEngine.UI.Text>().fontSize = UIScaler.GetMediumFont();
                db.AddBorder();
                hPos += 2.5f;
            }
            vPos += 2.5f;
        }

        db = new DialogBox(new Vector2(UIScaler.GetHCenter(7f), 13f), new Vector2(6f, 2f), "Moves:");
        db.textObj.GetComponent<UnityEngine.UI.Text>().fontSize = UIScaler.GetMediumFont();

        db = new DialogBox(new Vector2(UIScaler.GetHCenter(8.5f), 15f), new Vector2(3f, 2f), puzzle.guess.Count.ToString());
        db.textObj.GetComponent<UnityEngine.UI.Text>().fontSize = UIScaler.GetMediumFont();
        db.AddBorder();

        if (puzzle.Solved())
        {
            new TextButton(new Vector2(UIScaler.GetHCenter(-13f), 23.5f), new Vector2(8f, 2), "Close", delegate {; }, Color.grey);
            new TextButton(new Vector2(UIScaler.GetHCenter(5f), 23.5f), new Vector2(8f, 2), eventData.GetButtons()[0].label, delegate { Finished(); });
        }
        else
        {
            new TextButton(new Vector2(UIScaler.GetHCenter(-13f), 23.5f), new Vector2(8f, 2), "Close", delegate { Close(); });
            new TextButton(new Vector2(UIScaler.GetHCenter(5f), 23.5f), new Vector2(8f, 2), eventData.GetButtons()[0].label, delegate {; }, Color.grey);
        }
    }

    public void GuessAdd(int symbolType)
    {
        guess.Add(symbolType);

        if (guess.Count >= questPuzzle.puzzleLevel)
        {
            FinishedGuess();
            return;
        }
        float hPos = UIScaler.GetHCenter(-13f) + (guess.Count * 2.5f);
        DialogBox db = new DialogBox(new Vector2(hPos, 4f), new Vector2(2f, 2f), symbolType.ToString());
        db.textObj.GetComponent<UnityEngine.UI.Text>().fontSize = UIScaler.GetMediumFont();
        db.AddBorder();
    }

    public void FinishedGuess()
    {
        puzzle.AddGuess(guess);
        guess = new List<int>();
        CreateWindow();
    }

    public void Close()
    {
        Destroyer.Dialog();
        Game game = Game.Get();
        if (game.quest.puzzle.ContainsKey(questPuzzle.name))
        {
            game.quest.puzzle.Remove(questPuzzle.name);
        }
        game.quest.puzzle.Add(questPuzzle.name, puzzle);

        game.quest.eManager.currentEvent = null;
        game.quest.eManager.currentEvent = null;
        game.quest.eManager.TriggerEvent();
    }

    public void Finished()
    {
        Destroyer.Dialog();
        Game game = Game.Get();
        if (game.quest.puzzle.ContainsKey(questPuzzle.name))
        {
            game.quest.puzzle.Remove(questPuzzle.name);
        }

        game.quest.eManager.EndEvent();
    }
}