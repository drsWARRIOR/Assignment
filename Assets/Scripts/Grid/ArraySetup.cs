using System;

// Making the class serializable, so that it can be displayed and edited in unity editor
[System.Serializable]
public class ArraySetup
{
    [System.Serializable]
    public struct row //Nested struct defines row of the grid
    {
        public bool[] rows; // array of boolean represents columns
    }

    // Grid of 10 x 10
    public row[] table = new row[10];
}
