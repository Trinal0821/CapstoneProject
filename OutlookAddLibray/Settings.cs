using System.Text.RegularExpressions;

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
        string documentFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        public Settings()
        { }
        /// <summary>
        /// Returns the "dictionary" attached to the client name. 
        /// </summary>
        /// <param name="clientName">The client that sent the email</param>
        /// <returns></returns>
        public Dictionary<string,int> GetCleintDictionary(string clientName)
        {
            // Get path for directory
            Dictionary<string,int> dict = new Dictionary<string, int>();
            string eaServerDirectory = System.IO.Path.GetFullPath("Settings.js");
            string parentDirectory = Path.GetFullPath(Path.Combine(eaServerDirectory, @"..\..\"));

            // Get WordWights from Generic List. 
            string filepath = Path.Combine(Path.Combine(parentDirectory, "OutlookAddLibray"), "WordWeights.txt");
            dict = GetAllWordWeightsForDictionary(filepath, dict);
            
            //Get any additional wordweights from the clients.
            filepath = Path.Combine(Path.Combine(documentFolder, "Client_Correspondence"), clientName);
            filepath = Path.Combine(filepath, "WordWeightsAdditions.txt");
            dict = GetAllWordWeightsForDictionary(filepath, dict);
            return dict;
        }
        /// <summary>
        /// This is a helper method that runthrough the file found at any given filepath
        /// </summary>
        /// <param name="filepath">The path location of the file being read</param>
        /// <param name="dict">The dictionary being updated with new wordweights</param>
        /// <returns>A modified dictionary</returns>
        public Dictionary<string, int> GetAllWordWeightsForDictionary(string filepath, Dictionary<string, int> dict)
        {
            using (StreamReader reader = new StreamReader(filepath))
            {
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    line = line.Replace("\t", " ");

                    string phrase = Regex.Match(line, @"[a-zA-Z\s]+").Value;
                    phrase = phrase.ToLowerInvariant().Trim();

                    string number = Regex.Match(line, @"\d+").Value;

                    if (!dict.ContainsKey(phrase))
                      {
                        dict.Add(phrase, Int32.Parse(number));
                      }
                }
            }
            return dict;
        }
    }
}
