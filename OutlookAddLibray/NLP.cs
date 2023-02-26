using OutlookAddLibray;
using System.Text.Json;

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

/////// DRIVER CLASS?///////////

namespace OutlookExecutable
{
    /// <summary>
    /// This is the Natural Language Processor class that the email is run through 
    /// to determine email importance. 
    /// </summary>
    public class NLP

    {
        // Inner class varaibles. 
        private Settings settings;
        private FolderSystem file;
        

        // Stores emails that were marked a certain way. 
        private Dictionary<string,string>  importantDic;
        private Dictionary<string, string> normalDic;
        private Dictionary<string, string> yellowDic;

        //Message that is sent back to the outlook add on. 
        List<string> jsonMessage = new List<string>();

        //The size to check for word adjustments. 
        private int k = 3;

        // WORD LISTS
        List<string> negation = new List<string>(); 
        List<string> Inflation = new List<string>();
        List<string> deflation = new List<string>();
        public List<string> StopWords = new List<string>();
        List<string> ImportantClients = new List<string>();

        /// <summary>
        /// NLP Constuctor
        /// </summary>
        public NLP()
        {
            settings = new Settings();
            file = new FolderSystem();

            importantDic = new Dictionary<string, string>();
            normalDic = new Dictionary<string, string>();
            yellowDic = new Dictionary<string, string>();
            LoadWordLists();
        }
        /// <summary>
        /// Loads the words from the Word List. 
        /// 
        /// Look into a better way to 
        /// </summary>
        private void LoadWordLists()
        {
            bool inflame = false;
            bool deflame = false;
            bool nagation = false;
            bool stopWords = false;
            //fix folder system 
            using (StreamReader reader = new StreamReader("C:..\\..\\..\\..\\OutlookAddLibray\\WordList.txt"))
            {
                while(!reader.EndOfStream)
                {
                    string word = reader.ReadLine().Trim().ToLowerInvariant();
                    if(word.Equals(""))
                    {
                        continue;
                    }
                    if (word.Contains("inflation"))
                    {
                        inflame = true;
                        deflame = false;
                    }
                    if (word.Contains("negation"))
                    {
                        nagation = true;
                        inflame = false;
                    }
                    if (word.Contains("deflamation"))
                    {
                        deflame = true;
                    }
                    if (word.Contains("stop words"))
                    {
                        nagation = false;
                        stopWords = true;
                    }
                    if (inflame)
                    {
                        Inflation.Add(word);
                    }
                    if (nagation)
                    {
                        negation.Add(word);
                    }
                    if (deflame)
                    {
                        deflation.Add(word);
                    }
                    if (stopWords)
                    {
                        StopWords.Add(word);
                    }

                }
                
            }
        }

        /// <summary>
        /// Adjusts a words weight depending on weather there is a negation, inflation, deflation.
        /// </summary>
        /// <param name="email">The current email being scanned</param>
        /// <param name="trimmedWord">The word that is trimmed</param>
        /// <param name="currentWeight">The current weight of the word</param>
        /// <param name="location">The location of the word in the email</param>
        /// <returns></returns>
        internal double AdjustWeight(string email, double currentWeight, string word)
        {
            // To prevent double negation  
            bool negate = false;
            bool inflate = false;
            bool deflate = false;

            List<string> checkKArea = new List<string>();
            string[] scanThrough = email.Split(" ");
            int location = Array.IndexOf(scanThrough, word);

            if (location - k > 0 && location + k < email.Length)
            {
                checkKArea = GetWordAroundLocation(scanThrough, location);
            }

            // Grab a section of the email to check for adjustments. 
            for (int index = 0; index < checkKArea.Count; index++)
            {
                if (checkKArea.Count == 0)
                {
                    continue;
                }
                if (negation.Contains(checkKArea[index]) && !negate)
                {
                    negate = true;
                    inflate = false;
                    deflate = false;
                }
                if(negation.Any(checkKArea[index].Contains))
                {
                    negate = true;

                }
                if (Inflation.Contains(checkKArea[index]) && !inflate)
                {
                    inflate = true;
                    negate = false;
                    deflate = false;
                }
                if (deflation.Contains(checkKArea[index]) && !deflate)
                {
                    deflate = true;
                    negate = false;
                    inflate = false;
                }
            }

            if(negate && !deflate && !inflate)
               {
                 currentWeight *= -1;
               }
            if ((deflate && !negate && !inflate) || (negate && inflate && !deflate))
               {
                 currentWeight *= 0.5;
               }
            if (inflate && !negate && !deflate)
               {
                 currentWeight *= 2;
               }
               
            
            return currentWeight;
        }

        /// <summary>
        ///  Finds all the words in a k radius of the given word.  
        /// </summary>
        /// <param name="scanThrough">The email spilt up into parts</param>
        /// <param name="location">The known location of the looked up word</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        private List<string> GetWordAroundLocation(string[] scanThrough, int location)
        {

            List<string> KArea = new List<string>();
            for (int i = -k; i <= k; i++)
            {
                KArea.Add(scanThrough[location + i]);
            }

            return KArea;

        }

        /// <summary>
        /// Executes the NLP 
        /// </summary>
        public void execute()
        {
            /*TempClassAnalyze temp = new TempClassAnalyze();
            temp.analyze();*/// DELETE THIS//
            //change this to run on json objects beging sent from add-on. 
            string text = File.ReadAllText("C:\\Users\\skate\\source\\repos\\OutlookExecutable\\OutlookAddLibray\\Emails.txt");
            string[] emails = text.Split("--");
            Dictionary<string,int> emailList = new Dictionary<string, int>();
            // 
            string emailAddress = ""; 
            foreach(string email in emails)
            {
                string[] emailSpilt = email.Split(";");
                string clientName = emailSpilt[0].Split("FROM:")[1].Trim();
                if (emailList.ContainsKey(clientName))
                {
                    int newCount = emailList[clientName] + 1;       
                    emailList[clientName]= newCount;
                }
                else
                {
                    emailList.Add(clientName, 1);
                    // ask if they want to create a floder and dictionary. 
                }
                string result = "";
                if (!ImportantClients.Contains(clientName) && !ImportantClients.Contains(emailAddress))
                {
                    result = ScanInformationForDetails(email);
                }
                else
                {
                    result = "Important";
                }
                file.SaveToFolder(clientName, email);
                ReportFindingsToOutlook(result, email);
            }
        }


        /// <summary>
        /// Takes the results of the classifier and reports it to the outlook add-on using a json object 
        /// </summary>
        /// <param name="result">The tagging result</param>
        /// <param name="email">The email passed in</param>
        private void ReportFindingsToOutlook(string result, string email)
        {
            EmailTagger tag = new EmailTagger();

            string jsonString = "";
            if (result.Equals("Important"))
            {
          
                    importantDic.Add(email, result);
                    tag.colortagged = "RED";
                    jsonString = JsonSerializer.Serialize(tag);
                    jsonMessage.Add(jsonString);
            }
            else if (result.Equals("Not Important"))
            {

                /* Return the email as green to outlook and send a notification.*/
           
                    normalDic.Add(email, result);
                    tag.colortagged = "GREEN";
                    jsonString = JsonSerializer.Serialize(tag);
                    jsonMessage.Add(jsonString);
            }
            else
            {
                /* Return the email as yellow to outlook and send a notification.*/
                    yellowDic.Add(email, result);
                    tag.colortagged = "YELLOW";
                    jsonString = JsonSerializer.Serialize(tag);
                    jsonMessage.Add(jsonString);
            }
        }

        /// <summary>
        /// Sorts through the information from the email looking for certain 
        /// inforamtion. 
        /// </summary>
        /// <param name="currentEmail"> The current email being scanned</param>
        /// <returns></returns>
        private string ScanInformationForDetails(string currentEmail)
        {
            string[] emailSpilt = currentEmail.Split(";");
            string clientName = emailSpilt[0].Split("FROM:")[1];
            Dictionary<string, int> wordWeights =  settings.GetCleintDictionary(clientName.Trim());
            string completeEmail = emailSpilt[1] + " " + emailSpilt[2];


            Classifier classifier = new Classifier(wordWeights);
            string importance = classifier.scan(completeEmail, wordWeights);
            
            return importance;
        }
    }
    /// <summary>
    /// A sub-class of NLP that will allow us to send the information grabbed from the
    /// email as a json string. 
    /// </summary>
    public class EmailTagger
    {
        //change this to a list. 
        public string colortagged { get; set; }
    }
}