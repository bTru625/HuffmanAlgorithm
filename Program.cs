using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using HuffmanAlgorithm.Services;
using HuffmanAlgorithm.Entities;

class Program
{
    static void Main(string[] args)
    {
        Service service = new Service();

        bool running = true;

        Dictionary<char, List<bool>>? cypherTable = null;
        List<List<bool>> encodedString = new List<List<bool>>();

        //I know that saving data as a List<bool> is not the most efficient in terms of memory, a simple string of 0s and 1s would be better, but for the sake of the exercise, I thought that a List<bool> would be a better stand in for a collection of bits.

        Console.Clear();

        while (running)
        {

            Console.WriteLine("Menu:");
            Console.WriteLine("--------------------------------------");
            Console.WriteLine("1. Encode string");
            Console.WriteLine("2. Decode string");
            Console.WriteLine("3. Display encoded string");
            Console.WriteLine("4. Display cypher table");
            Console.WriteLine("5. Exit");
            Console.WriteLine("--------------------------------------");
            Console.Write("Enter selection : ");

            string choice = Console.ReadLine().Trim();
            choice = choice[0].ToString();

            switch (choice)
            {
                case "1":
                    if (cypherTable != null)
                    {
                        Console.WriteLine
                            ("A string has already been encoded. This program can only handle one message at a time." + "\nDo you want to encode a new string?"
                            + "\n(Y / N): ");

                        string overWrite = Console.ReadLine().Trim().ToUpper();
                        if (overWrite != "Y")
                        {
                            break;
                        }
                    }

                    bool isValidInput = false;
                    while (!isValidInput)
                    {
                        Console.Write("Enter string to encode and compress: ");
                        string? originalString = Console.ReadLine().Trim();
                        if (string.IsNullOrEmpty(originalString))
                        {
                            Console.WriteLine("The input string was empty.");
                        }
                        else
                        {
                            try
                            {
                                cypherTable = service.BuildCypherTable(
                                     service.TurnPriorityQueueIntoTree(
                                         service.BuildFrequencyQueue(
                                             service.BuildFrequencySet(originalString))));

                                encodedString = service.EncodeString(originalString, cypherTable);

                            }
                            catch (InvalidOperationException e)
                            {
                                Console.WriteLine($"Error: {e.Message}");
                            }

                            isValidInput = true;
                            Console.WriteLine("String encoded successfully.");
                            Console.WriteLine("--------------------------------------\n");
                        }
                    }
                    break;



                case "2":
                    if (cypherTable != null && encodedString.Count > 0)
                    {
                        try
                        {
                            string decodedString = service.DecodeString(encodedString, cypherTable);
                            Console.WriteLine($"Decoded string:\n\n" +
                                $"{decodedString}\n\n");

                            Console.WriteLine("--------------------------------------\n");
                        }
                        catch (InvalidOperationException e)
                        {
                            Console.WriteLine($"Error: {e.Message}");
                        }

                    }
                    else
                    {
                        Console.WriteLine("No encoded string available. Encode first.");
                        Console.WriteLine("--------------------------------------\n");
                    }
                    break;


                case "3":
                    if (encodedString != null || encodedString.Count > 0)
                    {
                        string result = string.Join(" ",
                            encodedString.Select(inner => $"[{string.Join("", inner.Select(b => b ? "1" : "0"))}]"));
                        Console.WriteLine($"Encoded string: \n{result}");
                        Console.WriteLine("--------------------------------------\n");
                    }
                    else
                    {
                        Console.WriteLine("No encoded string available. Encode first.");
                        Console.WriteLine("------------------------------------------\n");
                    }
                    break;


                case "4":
                    if (cypherTable != null && cypherTable.Count > 0)
                    {
                        Console.WriteLine("Cypher Table:");
                        foreach (var entry in cypherTable)
                        {
                            string path = string.Join("", entry.Value.Select(b => b ? "1" : "0"));
                            Console.WriteLine($"{entry.Key} : [{path}]");
                        }
                        Console.WriteLine("--------------------------------------\n");
                    }
                    else
                    {
                        Console.WriteLine("No cypher table available. Encode first.");
                        Console.WriteLine("----------------------------------------\n");
                    }
                    break;



                case "5" or "E" or "e":
                    Console.WriteLine("Exiting...");
                    running = false;
                    break;


                default:
                    Console.WriteLine("Invalid entry, please only enter the corresponding number.");
                    Console.WriteLine("----------------------------------------------------------\n");
                    break;

            }
        }
    }
}