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
        private Dictionary<string, int> wordWeights;
        private Classifier classifier;

        /// <summary>
        /// NLP Initalizer
        /// </summary>
        public NLP()
        {
            wordWeights = new Dictionary<string, int>();
            wordWeights.Add("important", 30);
            wordWeights.Add("good", 2);
            wordWeights.Add("asap", 100);
            wordWeights.Add("demo", 5);
            classifier = new Classifier(wordWeights);
        }
        /// <summary>
        /// Executes the NLP 
        /// </summary>
        public void execute()
        {
            List<string> emails = new List<string>();
            string trail = "SUBJECT: 15-Minute Demo –  When you have a minute, Microsoft Office for Legal was hoping you" +
                           " could suggest a good time to set up a 15 - minute phone call and demo with" +
                           " you or one of the people in your firm that is responsible for docketing litigation" +
                           " deadlines. Microsoft recently introduced me to LawToolBox365, a matter - based deadline" +
                           " management system inside Outlook(case studies, brochure).LawToolBox, who has been automating" +
                           " court rules calendaring for state and federal courts around the country since the late 90s is" +
                           " offering LawToolBox365 as a bundle with Office 365 for a monthly per user fee.If you have a minute," +
                           " please check out this 2 - min video.If you are interested, can you suggest a good person in your firm" +
                           " to schedule a 15 - minute phone call and demo this week ? Or next week ? We look forward" +
                           " to getting your feedback on how or if you think this Office 365 deadline management system" +
                           " will save time generating deadlines, getting them into Outlook, tracking rule changes, and" +
                           " supporting malpractice insurance requirements for multiple reminder systems.Thank you!";
            
            string trail2 = "SUBJECT: Important Buiness Meeting –  I need you to give me a call, ASAP." +
                            " We are getting sued.NOW!";

            string trail3 = "SUBJECT:Thank you!";

            emails.Add(trail);
            emails.Add(trail2);
            emails.Add(trail3);

            for(int emailIndex = 0; emailIndex < emails.Count; emailIndex++)
            {
                // get email. 
                string currentEmail = GrabInformationFromEmail(emails, emailIndex);

                Console.WriteLine("Email scanned: " + currentEmail);
                
                // Scan through for important words. 
                string result = ScanInformationForDetails(currentEmail);
                // Send Coloration and Email to the outlook.
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

            if (result.Equals("Important"))
            {
                /* Return the email as red to outlook and send a notification.*/
                Console.WriteLine("Importance level: " + result);
                Console.WriteLine("Color tagged as: RED");
            }
            else if (result.Equals("Not Important"))
            {
                /* Return the email as green to outlook and send a notification.*/
                Console.WriteLine("Importance level: " + result);
                Console.WriteLine("Color tagged as: GREEN");
            }
            else
            {
                /* Return the email as yellow to outlook and send a notification.*/
                Console.WriteLine("Importance level: " + result);
                Console.WriteLine("Color tagged as: YELLOW");
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
            string importance = classifier.scan(currentEmail, wordWeights);
            return importance;
        }

        /// <summary>
        /// Grabs all the information from the email.  
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        private string GrabInformationFromEmail(List<string> emails, int trail)
        {
            // Gets an email from the email scrapper
            // Then grabs all the information off of the scraper.
            return emails[trail];
        }
    }
}