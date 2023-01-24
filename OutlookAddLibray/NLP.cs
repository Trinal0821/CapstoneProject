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
namespace OutlookExecutable
{
    /// <summary>
    /// This is the Natural Language Processor class that the email is run through 
    /// to determine email importance. 
    /// </summary>
    public class NLP

    {
        // inner class varaibles. 
        private Settings settings;
        private FolderSystem file;

        // Stores emails that were marked a certain way. 
        private Dictionary<string,string>  importantDic;
        private Dictionary<string, string> normalDic;
        private Dictionary<string, string> yellowDic;

        //Message that is sent back to the outlook add on. 
        List<string> jsonMessage = new List<string>();
        
        /// <summary>
        /// NLP Initalizer
        /// </summary>
        public NLP()
        {
            settings = new Settings();
            file = new FolderSystem();

            importantDic = new Dictionary<string, string>();
            normalDic = new Dictionary<string, string>();
            yellowDic = new Dictionary<string, string>();
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
                String result = ScanInformationForDetails(email);
                file.SaveToFolder(clientName, email);
                ReportFindingsToOutlook(result, email);       
            }

            PrintWelcomeMessage(emails);
            

            foreach (KeyValuePair<String,int> value in emailList)
            {
                Console.WriteLine("We got " + value.Value + " emails from your client: " + value.Key);
            }
            Console.WriteLine("");


            //pop-up message
            Console.WriteLine("Would you like to see the emails we scanned?");
            Console.WriteLine("Please type 'yes' for yes and 'no' for no");
            Console.WriteLine();


            string answer = Console.ReadLine();
            if (answer.Equals("yes"))
            {
                Console.WriteLine("The emails are organized based on importance");
                Console.WriteLine();

                PrintDicTionary(importantDic);

                PrintDicTionary(normalDic);

                PrintDicTionary(yellowDic);
            }

            Console.WriteLine("We will continue Scanning now...........");
        }
        /// <summary>
        /// 
        /// </summary>
        private void PrintWelcomeMessage(string[] emails)
        {
            Console.WriteLine("----------------");
            Console.WriteLine("!!WELCOME BACK!!");
            Console.WriteLine("----------------");
            Console.WriteLine("");
            Console.WriteLine("While you were gone we scanned " + emails.Length + " emails");
            Console.WriteLine("");
            Console.WriteLine("These are the following emails that have been scanned");
            Console.WriteLine("..............");

        }

        /// <summary>
        /// Prints the emails and tagging that was saved while scanning emails.
        /// </summary>
        /// <param name="importantDic">The dictionary that contains the tagging and email</param>
        private void PrintDicTionary(Dictionary<string, string> importantDic)
        {
            foreach(KeyValuePair<string,string> email in importantDic)
            {
                Console.WriteLine(email.Key);
                Console.WriteLine("Was Tagged as: " +email.Value);
            }
            Console.WriteLine();
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
        /// 
        /// Examples: 
        ///     Client name
        ///     Matter number
        ///     Important Key words. 
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
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
        public string colortagged { get; set; }
    }
}