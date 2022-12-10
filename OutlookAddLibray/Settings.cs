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
            clientOne.Add("important", 5);
            clientOne.Add("call", 200);
            clientOne.Add("asap", 10);
            clientOne.Add("talk", 5);
            clientSettings.Add("Balthazar", clientOne);
            Dictionary<string, int> clientTwo = new Dictionary<string, int>();
            clientTwo.Add("important", 5);
            clientTwo.Add("call", 2);
            clientTwo.Add("cat", -5);
            clientTwo.Add("talk", 5);
            clientSettings.Add("Mortdecai", clientTwo);
            Dictionary<string, int> clientThree = new Dictionary<string, int>();
            clientThree.Add("important", 5);
            clientThree.Add("meeting", 200);
            clientThree.Add("ASAP", 100);
            clientThree.Add("talk", 5);
            clientSettings.Add("Harnassus", clientThree);

        }
        public Dictionary<string,int> GetCleintDictionary(string clientName)
        {
            clientSettings.TryGetValue(clientName, out Dictionary<string,int> result);
            return result;
        }
    }
}
