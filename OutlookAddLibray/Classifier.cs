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
    /// <summary>
    /// This class handleds classifing the information we get out of nlp
    /// </summary>
    public class Classifier
    {
        // Global Variables
        private Dictionary<string, int> localDict;
        private int importantLimit = 50;
        private int notImportantLimit = 10;
        
        /// <summary>
        /// Classifier initializer
        /// </summary>
        public Classifier(Dictionary<string, int> temp)
        {
           localDict = temp;
           
        }
        /// <summary>
        /// Scans through the email to see what words are part of the wordWeight and adds
        /// that words weight to the score. 
        /// </summary>
        /// <param name="email"></param>
        /// <param name="wordWeights"></param>
        /// <returns></returns>
        public string scan(string email, Dictionary<string, int> wordWeights)
        {
            int score = 0;
            string classifiedEmail = "";
            // done stuff
            string[] wordsInEmail = email.Split(" ");
            
            foreach(string word in wordsInEmail)
            {
                string trimmedWord = word.Trim().ToLower();
                if (trimmedWord.Contains("."))
                    trimmedWord = trimmedWord.Replace(".", "");

                if (wordWeights.ContainsKey(trimmedWord))
                    score += wordWeights[trimmedWord];
            }

            if (score > importantLimit)
                classifiedEmail = "Important";
            else if (score < notImportantLimit)
                classifiedEmail = "Not Important";
            else
                classifiedEmail = "Standard";

            return classifiedEmail;
        }
    }
}
