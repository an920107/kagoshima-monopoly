using System;

public class DiceHasResultEventArgs : EventArgs {
    public DiceHasResultEventArgs(int points) {
        Points = points;
    }

    public int Points { get; }
}