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
        private Settings settings;

        /// <summary>
        /// NLP Initalizer
        /// </summary>
        public NLP()
        {
            settings = new Settings();
        }
        /// <summary>
        /// Executes the NLP 
        /// </summary>
        public void execute()
        {

            string text = File.ReadAllText("C:\\Users\\skate\\source\\repos\\OutlookExecutable\\OutlookAddLibray\\Emails.txt");
            string[] emails = text.Split("--");
            Dictionary<string,int> emailList = new Dictionary<string, int>();
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
                }
            }
            Console.WriteLine("----------------");
            Console.WriteLine("!!WELCOME BACK!!");
            Console.WriteLine("----------------");
            Console.WriteLine("");
            Console.WriteLine("While you were gone we scanned " + emails.Length + " emails");
            Console.WriteLine("");
            Console.WriteLine("These are the following emails I have tagged");
            Console.WriteLine("..............");
            System.Threading.Thread.Sleep(3000);

            foreach (KeyValuePair<String,int> value in emailList)
            {
                Console.WriteLine("We got " + value.Value + " emails from your client: " + value.Key);
            }
            Console.WriteLine("");
            Console.WriteLine("Here is how the emails scored.");
            foreach (string email in emails)
            {
                String result = ScanInformationForDetails(email);
                Console.WriteLine(email);
                Console.WriteLine("-----");
                Console.WriteLine("TAGGED AS");
                Console.WriteLine("-----");

                ReportFindingsToOutlook(result);
            }
        }
        /// <summary>
        /// This takes the results from scanning the infomration and 
        /// sends the correct color to tag the email in outlook. 
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        private void ReportFindingsToOutlook(string result)
        {
            EmailTagger tag = new EmailTagger();

            string jsonString = "";
            if (result.Equals("Important"))
            {
                /* Return the email as red to outlook and send a notification.*/
                Console.WriteLine("Importance level: " + result);
                Console.WriteLine("Color tagged as: RED");
                tag.colortagged = "RED";
                jsonString = JsonSerializer.Serialize(tag);
            }
            else if (result.Equals("Not Important"))
            {
                /* Return the email as green to outlook and send a notification.*/
                Console.WriteLine("Importance level: " + result);
                Console.WriteLine("Color tagged as: GREEN");
                tag.colortagged = "GREEN";
                jsonString = JsonSerializer.Serialize(tag);
            }
            else
            {
                /* Return the email as yellow to outlook and send a notification.*/
                Console.WriteLine("Importance level: " + result);
                Console.WriteLine("Color tagged as: YELLOW");
                tag.colortagged = "YELLOW";
                jsonString = JsonSerializer.Serialize(tag);
            }
            Console.WriteLine();
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

            Classifier classifier = new Classifier(wordWeights);
            string importance = classifier.scan(emailSpilt[2], wordWeights);
            
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