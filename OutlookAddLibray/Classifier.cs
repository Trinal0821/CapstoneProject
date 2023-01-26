
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
        /// <param name="email">The email that was sent. </param>
        /// <param name="wordWeights">The dictionary begin passed in</param>
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
                    
                trimmedWord = CheckForUnwantedChar(word);

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
        /// <summary>
        /// Checks to see if a word has an unwanted char.
        /// </summary>
        /// <param name="word">The word that is being scored.</param>
        /// <returns></returns>
        private string CheckForUnwantedChar(string word)
        {
            if (word.Contains("."))
                return word.Replace(".","");
            else if (word.Contains(";"))
                return word.Replace(";", "");
            else if (word.Contains("\r\n"))
                return word.Replace("\r\n", "");

            return word;
        }
    }
}
