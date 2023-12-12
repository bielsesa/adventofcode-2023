namespace AdventOfCode;

public class Node
{
    public Node(string id, string leftNode, string rightNode)
    {
        Id = id;
        LeftNode = leftNode;
        RightNode = rightNode;
    }

    /// <summary>
    /// Create a <c>Node</c> entity using a <c>String</c> similar to:
    /// <list type="bullet">
    ///     <item>
    ///         <description>AAA = (BBB, BBB)</description>
    ///     </item>
    /// </list>
    /// </summary>
    /// <param name="nodeLine">A string containing a line that represents a <c>Node</c>.</param>
    /// <exception cref="ArgumentException">The provided string is not in the expected format.</exception>
    public Node(string nodeLine)
    {
        var splitByEquals = nodeLine.Split('=', StringSplitOptions.TrimEntries);
        if (splitByEquals.Length < 2)
        {
            throw new ArgumentException("nodeLine should contain an equals symbol.", nameof(nodeLine));
        }
        
        Id = splitByEquals[0];

        var splitByComma = splitByEquals[1].TrimStart('(').TrimEnd(')').Split(',', StringSplitOptions.TrimEntries);
        if (splitByComma.Length < 2)
        {
            throw new ArgumentException("nodeLine should contain a comma symbol.", nameof(nodeLine));
        }
        
        LeftNode = splitByComma[0];
        RightNode = splitByComma[1];
    }

    public string Id { get; set; }
    public string LeftNode { get; set; }
    public string RightNode { get; set; }
}