﻿
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
/// Should we move classifer. 
namespace OutlookExecutable
{
    /// <summary>
    /// This class handleds classifing the information we get out of nlp
    /// </summary>
    public class Classifier
    {
        // Global Variables
        private Dictionary<string, int> localDict;
        private int importantLimit = 150;
        private int notImportantLimit = 50;
        NLP nlp = new NLP();

        // Inner class varaibles. 
        private Settings settings;
        private FolderSystem file;


        // Stores emails that were marked a certain way. 
        private Dictionary<string, string> importantDic;
        private Dictionary<string, string> normalDic;
        private Dictionary<string, string> yellowDic;

        //Message that is sent back to the outlook add on. 
        List<string> jsonMessage = new List<string>();



        public Classifier()
        {        
            settings = new Settings();
            file = new FolderSystem();

            importantDic = new Dictionary<string, string>();
            normalDic = new Dictionary<string, string>();
            yellowDic = new Dictionary<string, string>();
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
            double score = 0;
            string classifiedEmail = "";
            // done stuff

            email = email.Replace("\r\n", " ");
            email = email.Trim().ToLowerInvariant();

            foreach(string word in wordWeights.Keys)
            {
                if(email.Contains(word))
                {
                    score += nlp.AdjustWeight(email, wordWeights[word], word);
                }
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
        
        /// <summary>
        /// Executes the NLP 
        /// </summary>
        public void execute()
        {
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
                
                string result = ScanInformationForDetails(email);
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

            string importance = scan(completeEmail, wordWeights);
            
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
