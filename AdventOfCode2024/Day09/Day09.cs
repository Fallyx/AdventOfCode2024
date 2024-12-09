namespace AdventOfCode2024.Day09;

internal class Day09
{
    private const string inputPath = @"Day09/Input.txt";

    internal static void Task1and2()
    {
        string diskMap = File.ReadLines(inputPath).First();
        LinkedList<int> filesystemChecksum = [];
        
        int counter = 0;
        for(int i = 0; i < diskMap.Length; i++)
        {
            int numToAdd = (i % 2 == 0 ? counter : -1);
            int xMax = int.Parse(diskMap[i].ToString());
            for (int x = 0; x < xMax; x++)
            {
                filesystemChecksum.AddLast(numToAdd);
            }

            if (numToAdd != -1)
                counter++;
        }

        Task1(new LinkedList<int>(filesystemChecksum));
        Task2(filesystemChecksum);
    }

    private static void Task1(LinkedList<int> filesystemChecksum)
    {
        long filesystemChecksumResult = 0;
        int idx = 0;

        for (LinkedListNode<int> node = filesystemChecksum.First; node != null; node = node.Next)
        {
            if (node.Value == -1)
            {
                do
                {
                    node.Value = filesystemChecksum.Last.Value;
                    filesystemChecksum.RemoveLast();
                } while (node.Value == -1);
            }
            filesystemChecksumResult += idx * node.Value;
            idx++;
        }

        Console.WriteLine($"Task 1: {filesystemChecksumResult}");
    }

    private static void Task2(LinkedList<int> filesystemChecksum)
    {
        int idxRight = filesystemChecksum.Count;
        int maxValue = int.MaxValue;
        List<LinkedListNode<int>> nodeList = [];

        for (LinkedListNode<int> node = filesystemChecksum.Last; node != null; node = node.Previous)
        {
            idxRight--;
            if (node.Value > maxValue || node.Value == -1)
                continue;

            nodeList.Add(node);

            if (node.Previous != null && node.Previous.Value == node.Value)
                continue;

            maxValue = node.Value;
            List<LinkedListNode<int>> emptyNodeList = [];
            int idxLeft = 0;

            for (LinkedListNode<int> emptyNode = filesystemChecksum.First; emptyNode != null; emptyNode = emptyNode.Next)
            {
                int currentValue = node.Value;

                if (idxRight <= idxLeft)
                    break;

                if (emptyNode.Value == -1)
                {
                    emptyNodeList.Add(emptyNode);
                    if (nodeList.Count == emptyNodeList.Count)
                    {
                        
                        for (int i = 0; i < nodeList.Count; i++)
                        {
                            nodeList[i].Value = -1;
                            emptyNodeList[i].Value = currentValue;
                        }
                        break;
                    }
                }
                else
                    emptyNodeList = [];

                idxLeft++;
            }
            nodeList = [];
        }

        long filesystemChecksumResult = 0;
        int idx = 0;

        for (LinkedListNode<int> node = filesystemChecksum.First; node != null; node = node.Next)
        {
            if (node.Value != -1)
            {
                filesystemChecksumResult += idx * node.Value;
            }
            idx++;
        }
        Console.WriteLine($"Task 2: {filesystemChecksumResult}");
    }
}
