using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EvoType.src;
public class Population
{
    public int Size { get; private set; }
    readonly char[] source;
    Keyboard[] keyboards;
    double[] penalties;
    double[] fitnesses;
    double[] probabilities;

    public Population(int size, char[] source)
    {
        this.source = source;
        Size = size;
        penalties     = new double[size];
        fitnesses     = new double[size];
        probabilities = new double[size];

        keyboards = new Keyboard[size];
        keyboards = [.. keyboards.AsParallel().Select(x => new Keyboard())];
    }

    public void RandomizePopulation()
    {
        keyboards = [.. keyboards.AsParallel().Select(x => {
                x.RandomizeKeySet();
                return x;
            })];
    }

    public void Evaluate()
    {
        penalties = [.. keyboards.AsParallel().Select(x => x.EvaluatePenalty(source))];
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

        for (int individual = 0; individual < Size; individual++)
        {
            double rouletteSpin = random.NextDouble() * prefixProbs[Size - 1];
            int selectedIndex = Array.BinarySearch(prefixProbs, rouletteSpin);

            newKeyboards[individual] = keyboards[~selectedIndex];
        }

        keyboards = newKeyboards;
    }

    
}