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

        var navigation = new Navigation(input.First());
        input.RemoveAll(l => l.Equals(navigation.LeftRight)); // remove left/right instructions
        input.RemoveAll(string.IsNullOrWhiteSpace); // remove empty line between left/right instructions and nodes
        
        var nodes = input.Select(nodeLine => new Node(nodeLine)).ToList();
        var currentNode = nodes.FirstOrDefault(node => node.Id.Equals("AAA"));

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
            
            if (currentNode != null && currentNode.Id.Equals("ZZZ"))
            {
                goalFound = true;
            }
            
        } while (!goalFound);
        
        Console.WriteLine($"Steps taken to reach ZZZ: {navigation.Steps}");
    }
}