using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/// <summary>
/// 
/// This namespace contains the insides of our outlook add-on. 
/// 
/// 
/// Authors: Team Executive Assistants:
///           Zachary Gundersen, Trina loung, Andrew Gill, Sephora Batmean 
/// 
/// 2022-2023 Capstone Project, University of Utah
/// 
///*copyright note* This code is the property of Team Executive Assistants any 
///                 unauthoized use of the code will be persecuted to the fullest 
///                 extent of the law. 
/// </summary>
/// 
namespace OutlookExecutable
{
    public class Settings
    {
       
        public Settings()
        { }
        /// <summary>
        /// Returns the "dictionary" attached to the client name. 
        /// </summary>
        /// <param name="clientName">The client that sent the email</param>
        /// <returns></returns>
        public Dictionary<string,int> GetCleintDictionary(string clientName)
        {
            Dictionary<string,int> dict = new Dictionary<string, int>();

            using (StreamReader reader = new StreamReader("C:..\\..\\..\\..\\OutlookAddLibray\\WordWeights.txt"))
            {
                while (!reader.EndOfStream)
                {

                    string[] value = reader.ReadLine().Split("\t");
                    if (value.Length > 1)
                    {
                        if(!dict.ContainsKey(value[0]))
                        { 
                            dict.Add(value[0].ToLowerInvariant(), Int32.Parse(value[1])); // find bug
                        }
                    }
                }
            }
            return dict;
        }
    }
}
