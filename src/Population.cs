using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace EvoType.src;
public class Population
{
    public int Size { get; private set; }
    readonly char[] source;

    public const double crossOverRate = 0.69;

    public Individual[] Individuals { get; private set; }

    public Population(int size, char[] source)
    {
        this.source = source;
        Size = size;

        Individuals = new Individual[size];
        Individuals = [.. Individuals.AsParallel().Select(x => new Individual())];
    }

    public void Randomize()
    {
        for (int i = 0; i < Size; i++)
        {
            Individuals[i].Keyboard.RandomizeKeySet();
            Debug.Assert(Individuals[i].Keyboard is not null);
        }
    }

    public void Evaluate()
    {
        foreach (var individual in Individuals)
        {
            individual.Penalty = individual.Keyboard.EvaluatePenalty(source);
        }

        foreach (var individual in Individuals)
        {
            individual.Fitness = 1.0 / (1 + individual.Penalty);
        }

        double fitnessesSum = 0;
        foreach (var individual in Individuals)
        {
            fitnessesSum += individual.Fitness;
        }

        foreach (var individual in Individuals)
        {
            individual.Probabilty = individual.Fitness / fitnessesSum;
        }
    }


    public void SelectNextGen()
    {
        double[] prefixProbs = new double[Size];
        for (int i = 0; i < Size; i++)
        {
            prefixProbs[i] = Individuals[i].Probabilty;
            if (i > 0) prefixProbs[i] += prefixProbs[i - 1];
        }


        Random random = new Random();

        Keyboard[] newKeyboards = new Keyboard[Size];

        for (int i = 0; i < Size; i++)
        {
            double rouletteSpin = random.NextDouble() * prefixProbs[Size - 1];
            int selectedIndex = Array.BinarySearch(prefixProbs, rouletteSpin);
            if (selectedIndex < 0) selectedIndex = ~selectedIndex;

            newKeyboards[i] = Individuals[selectedIndex].Keyboard;
        }

        for (int i = 0; i < Size; i++)
        {
            Individuals[i].Keyboard = newKeyboards[i];
        }
    }

    public static (Keyboard, Keyboard) Crossover(Keyboard par1, Keyboard par2)
    {

        Random random = new Random();

        int left = random.Next(Keyboard.keyboardSize);
        int right = random.Next(Keyboard.keyboardSize);

        while (left == right) left = random.Next() % Keyboard.keyboardSize;

        if (left > right) (left, right) = (right, left);

        Keyboard Cross(Keyboard par1, Keyboard par2)
        {
            Keyboard child = new();

            bool[] visited = new bool[256];
            int[] vis = new int[33];
            for (int i = left; i < right; i++)
            {
                child.KeySet[i] = par1.KeySet[i];
                visited[child.KeySet[i]] = true;
                vis[i] = 1;
            }

            for (int i = right, j = right; ;)
            {
                if (i == left) break;
                if (visited[par2.KeySet[j]])
                {
                    j = (j + 1) % Keyboard.keyboardSize;
                    continue;
                }
                vis[i] = 1;
                child.KeySet[i] = par2.KeySet[j];
                visited[par2.KeySet[j]] = true;

                i = (i + 1) % Keyboard.keyboardSize;
                j = (j + 1) % Keyboard.keyboardSize;
            }
            return child;
        }

        return (Cross(par1, par2), Cross(par2, par1));
    }

}