using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public static class HelperFunctions {
    public enum EnemyType {
        Normal,
        Intermediate,
        Advance
    }
    public static Color[] Available = new Color[] { Color.red, Color.green, Color.yellow, Color.green, Color.blue, Color.magenta };

    public class EnemyInformation {
        public EnemyType type { get; set; }
        public int Health { get; set; }
        public SpriteRenderer Color { get; set; }
    }
}