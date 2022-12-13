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
namespace OutlookExecutable
{
    public class Settings
    {
        public Dictionary<string,Dictionary<string, int>> clientSettings { get;}
        public Settings()
        {
            clientSettings = new Dictionary<string, Dictionary<string, int>>();
            Dictionary<string, int> clientOne = new Dictionary<string, int>();
            clientOne.Add("important", 500);
            clientOne.Add("call", 20);
            clientOne.Add("meet", 50);
            clientOne.Add("talk", 5);
            clientSettings.Add("Balthazar", clientOne);
            Dictionary<string, int> clientTwo = new Dictionary<string, int>();
            clientTwo.Add("meet", 500);
            clientTwo.Add("call", 2);
            clientTwo.Add("cat", -5);
            clientTwo.Add("ASAP", 200);
            clientSettings.Add("Mortdecai", clientTwo);
            Dictionary<string, int> clientThree = new Dictionary<string, int>();
            clientThree.Add("important", 5);
            clientThree.Add("up", 1);
            clientThree.Add("meet", 1);
            clientThree.Add("talk", 5);
            clientSettings.Add("Harnassus", clientThree);

        }
        /// <summary>
        /// Returns the "dictionary" attached to the client name. 
        /// </summary>
        /// <param name="clientName">The client that sent the email</param>
        /// <returns></returns>
        public Dictionary<string,int> GetCleintDictionary(string clientName)
        {
            clientSettings.TryGetValue(clientName, out Dictionary<string,int> result);
            return result;
        }
    }
}
