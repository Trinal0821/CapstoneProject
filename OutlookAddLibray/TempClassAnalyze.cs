using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OutlookAddLibray;
using OutlookExecutable;

namespace OutlookAddLibray
{


    internal class TempClassAnalyze
    {
        NLP nlp = new NLP();
        Dictionary<string, int> frquency = new Dictionary<string, int>();
        public void analyze()
        {
            string emails = File.ReadAllText("C:\\Users\\skate\\Documents\\school\\Spring 2023\\CS 4500 - Capstone part two\\Emails\\Unimportant.txt");

            string[] spiltEmail = emails.Split("\r\n");

            foreach (string line in spiltEmail)
            {
                foreach (string word in line.Split(" "))
                {
                    if (!nlp.StopWords.Contains(word))
                    {
                        string wordtrimmed = word.Trim().ToLowerInvariant();
                        if (frquency.ContainsKey(wordtrimmed))
                        {
                            frquency[wordtrimmed]++;
                        }
                        else
                        {
                            frquency.Add(wordtrimmed, 1);
                        }
                    }
                }
            }

            foreach (string word in frquency.Keys)
            {
                Console.WriteLine("Word: " + word + " " + "Count: " + frquency[word]);
            }
        }
    }
    
}
