﻿using System;
using System.Collections.Generic;
using System.Linq;

class Interval
{
    public int From { get; set; }
    public int To { get; set; }
    public List<string> Data { get; set; }

    public Interval(int from, int to, List<string> data)
    {
        From = from;
        To = to;
        Data = data;
    }
}

class Program
{
    static void Main(string[] args)
    {
        try
        {
            Console.Write("Enter the number of intervals: ");
            int n = int.Parse(Console.ReadLine());

            List<Interval> intervals = new List<Interval>();

            for (int i = 0; i < n; i++)
            {
                Console.Write($"Enter interval {i + 1} (From, To, Data): ");
                string[] input = Console.ReadLine().Split(',');
                int from = int.Parse(input[0]);
                int to = int.Parse(input[1]);
                List<string> data = input[2].Split(',').ToList();

                intervals.Add(new Interval(from, to, data));
            }

            List<Interval> rearrangedIntervals = RearrangeIntervals(intervals);

            Console.WriteLine("\nOutput Data:");
            foreach (Interval interval in rearrangedIntervals)
            {
#if DEBUG
                Console.WriteLine($"{interval.From}\t{interval.To}\t{string.Join(",", interval.Data)}");
                Console.ReadLine();
#endif
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
    static List<Interval> RearrangeIntervals(List<Interval> intervals)
    {
        intervals.Sort((a, b) => a.From.CompareTo(b.From));

        List<Interval> rearrangedIntervals = new List<Interval>();
        List<Interval> overlappingIntervals = new List<Interval>();
        foreach (Interval interval in intervals)
        {
            //if (rearrangedIntervals.Count == 0)
            //{
            //    overlappingIntervals.Add(interval);
            //    overlappingIntervals = overlappingIntervals.Where(i => !(interval.To < i.From || interval.From > i.To))
            //    .ToList();
            //}
            //else
            //{
                overlappingIntervals = rearrangedIntervals
                .Where(i => !(interval.To < i.From || interval.From > i.To))
                .ToList();
            //}
            if (overlappingIntervals.Any())
            {
                List<string> allData = overlappingIntervals.SelectMany(i => i.Data).ToList();
                List<string> OriginalData = overlappingIntervals.SelectMany(i => i.Data).ToList();
                allData.AddRange(interval.Data);

                int minFrom = overlappingIntervals.Min(i => i.From);
                int maxTo = overlappingIntervals.Max(i => i.To);

                

                if (interval.From < maxTo && interval.From > minFrom)
                {
                    rearrangedIntervals.Add(new Interval(minFrom, interval.From - 1, OriginalData));
                }
                if (interval.From < minFrom)
                {
                    rearrangedIntervals.Add(new Interval(interval.From, minFrom - 1, interval.Data));
                }
                else if (interval.From < maxTo)
                {
                    rearrangedIntervals.Add(new Interval(interval.From, maxTo, allData));
                }
                if (interval.To > maxTo)
                {
                    rearrangedIntervals.Add(new Interval(maxTo + 1, interval.To, interval.Data));
                }
               // rearrangedIntervals.Add(new Interval(minFrom, maxTo, allData));

                foreach (Interval overlappingInterval in overlappingIntervals)
                {
                    rearrangedIntervals.Remove(overlappingInterval);
                }
            }
            else
            {
                rearrangedIntervals.Add(interval);
            }
        }
        rearrangedIntervals = rearrangedIntervals.OrderBy(i => i.From).ToList();
        // Split any large intervals into smaller non-overlapping intervals
        List<Interval> finalIntervals = new List<Interval>();
        int previousTo = rearrangedIntervals[0].From - 1;
        foreach (Interval interval in rearrangedIntervals)
        {
            if (interval.From > previousTo + 1)
            {
                finalIntervals.Add(new Interval(previousTo + 1, interval.From - 1, new List<string>()));
            }
            finalIntervals.Add(interval);
            previousTo = interval.To;
        }
        return finalIntervals;
    }


}
