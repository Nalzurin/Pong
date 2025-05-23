using System;
using UnityEngine;
using UnityEngine.InputSystem.Utilities;

public class Data
{
    public const string BALLTAG = "Ball";
    public const string WALLTAG = "Wall";
    public enum PlayerSide
    {
        Left, Right
    }
    [Serializable]
    public enum MenuState
    {
        Main, Settings, Credits, Play, PlaySingle, PlayMultiplayer
    }
    public enum Difficulty
    {
        Easy, Normal, Hard
    }

}
