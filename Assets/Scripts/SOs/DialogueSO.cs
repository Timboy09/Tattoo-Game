using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Dialogue", menuName = "Dialogues")]
public class DialogueSO : ScriptableObject
{
    public Character character;

    [TextArea(3,10)]
    public string[] dialogues;

    public int branchAStartIndex;
    public int branchBStartIndex;
    public int branchCStartIndex;

    public int branchEndIndex;

    public List<Shapes> tattooShapes;

    public bool forceBranchIndex;
}

public enum Character
{
    MIN,
    SCENE,
    PLAYER,
    REYA,
    QI,
    NAYR,
    URZA,
    NURY,
    CHOICE,
    CLUE,
    GAME,
    MAX
}

public enum Shapes
{
    MIX,
    TRIANGLE,
    CIRCLE,
    QUAD,
    MAX
}
