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
    public Keyboard[] Keyboards { get; private set; }
    double[] penalties;
    double[] fitnesses;
    double[] probabilities;

    public Population(int size, char[] source)
    {
        this.source = source;
        Size = size;
        penalties = new double[size];
        fitnesses = new double[size];
        probabilities = new double[size];

        Keyboards = new Keyboard[size];
        Keyboards = [.. Keyboards.AsParallel().Select(x => new Keyboard())];
    }

    public void Evaluate()
    {
        penalties = [.. Keyboards.AsParallel().Select(x => x.EvaluatePenalty(source))];
        fitnesses = [.. penalties.AsParallel().Select(x => 1.0 / (1 + x))];
        double fitnessesSum = fitnesses.Sum();
        probabilities = [.. fitnesses.AsParallel().Select(f => f / fitnessesSum)];
        probabilities.Print();
        System.Console.WriteLine();
    }


    public void SelectNextGen()
    {
        double[] prefixProbs = new double[Size];
        Array.Copy(probabilities, prefixProbs, Size);
        for (int i = 1; i < Size; i++)
        {
            prefixProbs[i] += prefixProbs[i - 1];
        }

        Random random = new Random();

        Keyboard[] newKeyboards = new Keyboard[Size];

        Keyboards = [.. newKeyboards.AsParallel().Select(x => {
            double rouletteSpin = random.NextDouble() * prefixProbs[Size - 1];
            int selectedIndex = Array.BinarySearch(prefixProbs, rouletteSpin);
            if (selectedIndex < 0) selectedIndex = ~selectedIndex;
            return Keyboards[selectedIndex];
        })];
    }

    public static (Keyboard, Keyboard) Crossover(Keyboard par1, Keyboard par2)
    {

        Random random = new Random();

        int left = random.Next() % Keyboard.keyboardSize;
        int right = random.Next() % Keyboard.keyboardSize;

        if (left > right) (left, right) = (right, left);

        Keyboard Cross(Keyboard par1, Keyboard par2)
        {
            Keyboard child = new();

            bool[] visited = new bool[256];
            for (int i = left; i < right; i++)
            {
                child.KeySet[i] = par1.KeySet[i];
                visited[child.KeySet[i]] = true;
            }

            for (int i = right, j = right; ;)
            {
                if (i == left) break;
                if (visited[par2.KeySet[j]])
                {
                    j = (j + 1) % Keyboard.keyboardSize;
                    continue;
                }

                child.KeySet[i] = par2.KeySet[j];
                visited[par2.KeySet[j]] = true;

                i = (i + 1) % Keyboard.keyboardSize;
                j = (j + 1) % Keyboard.keyboardSize;
            }

            if (!child.ValidKeySet())
            {
                throw new Exception("CrossOver Failed");
            }
            return child;
        }

        return (Cross(par1, par2), Cross(par2, par1));
    }

}