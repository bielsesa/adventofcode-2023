using System.Collections.Specialized;
using Helpers;

namespace AdventOfCode;

/*
 * LLR
 * 
 * AAA = (BBB, BBB)
 * BBB = (AAA, ZZZ)
 * ZZZ = (ZZZ, ZZZ)
 *
 * This example takes 6 steps to reach ZZZ.
 * 
 * Starting at AAA, follow the left/right instructions. How many steps are required to reach ZZZ?
 */

public static class Day8
{
    public static void Main(string[] args)
    {
        var input = FilesHelper.ReadPuzzleInputToLines().ToList();

        var leftRightInstructions = input.First();
        input.RemoveAll(l => l.Equals(leftRightInstructions)); // remove left/right instructions
        input.RemoveAll(string.IsNullOrWhiteSpace); // remove empty line between left/right instructions and nodes
        
        var nodes = input.Select(nodeLine => new Node(nodeLine)).ToList();
        var startingNodes = nodes.Where(node => node.Type.Equals(NodeType.Start)).ToList();

        var allSteps = 
            startingNodes.Select(node => FindNeededStepsNumber(nodes, node, leftRightInstructions)).ToList();
        
        var steps = Lcm(allSteps);
        
        Console.WriteLine($"Steps taken to reach end node/s: {steps}");
    }

    private static long FindNeededStepsNumber(IReadOnlyCollection<Node> nodes, Node startingNode, string leftRightInstructions)
    {
        var navigation = new Navigation(leftRightInstructions);
        var currentNode = new Node(startingNode);
        var goalFound = false;
        do
        {
            var nextStep = navigation.NextStep();

            var nextNodeId = nextStep switch
                             {
                                 'L' => currentNode?.LeftNode,
                                 'R' => currentNode?.RightNode,
                                 _ => string.Empty
                             };

            currentNode = nodes.FirstOrDefault(node => node.Id.Equals(nextNodeId));
            
            if (currentNode != null && currentNode.Type.Equals(NodeType.End))
            {
                goalFound = true;
            }
            
        } while (!goalFound);

        return navigation.Steps;
    }

    private static long Gcd(long n1, long n2)
    {
        while (true)
        {
            if (n2 == 0) return n1;
            var n3 = n1;
            n1 = n2;
            n2 = n3 % n2;
        }
    }

    private static long Lcm(IEnumerable<long> numbers)
    {
        return numbers.Aggregate((s, val) => s * val / Gcd(s, val));
    }
}