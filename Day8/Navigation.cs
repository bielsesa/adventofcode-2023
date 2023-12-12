namespace AdventOfCode;

public class Navigation
{
    public Navigation(string leftRight)
    {
        LeftRight = leftRight;
        CurrentNavigationIndex = 0;
        Steps = 0;
    }
    
    public string LeftRight { get; set; }
    
    public int CurrentNavigationIndex { get; set; }
    
    public int MaxNavigationIndex => LeftRight.Length - 1;
    
    public long Steps { get; set; }

    /// <summary>
    /// Returns the next direction to take and increments the <c>Steps</c> and <c>CurrentNavigationIndex</c>.
    /// </summary>
    /// <returns>A <c>char</c> containing the direction to take.</returns>
    public char NextStep()
    {
        var step = LeftRight[CurrentNavigationIndex];
        Steps++;

        if (CurrentNavigationIndex == MaxNavigationIndex)
        {
            CurrentNavigationIndex = 0;
        }
        else
        {
            CurrentNavigationIndex++;
        }
        
        return step;
    }
}