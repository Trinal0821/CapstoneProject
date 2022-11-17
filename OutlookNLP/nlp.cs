/// <summary>
/// This project contains the guts of the add on's Natural language Processor and Classifier.
/// After the email is scrapped it's run through the NLP class were it will be checked for keywords,
/// then will be sent to the classifier to label it and save it to the hard drive. 
/// 
/// Authors: Team Executive Assistants:
///           Zachary Gundersen, Trina loung, Andrew Gill, Sephora Batmean 
/// 
/// 2022-2023 Capstone Project, University of Utah
/// 
///*copyright note* This code is the property of Team Executive Assistants any 
///                 unauthoized use of the code will be persecute to the fullest 
///                 extent of the law. 
/// </summary>
namespace OutlookADDON
{
    /// <summary>
    /// This is the Natural Language Processor class that the email is run through 
    /// to determine email importance. 
    /// </summary>
    public class nlp
    {

        public static void main (String[] args)
        {
            // get email. 
            GrabinformationFromEmail();
            // Scan through for important words. 
            ScanInformationForDetails();
            // Caterogieze and Send to classifier(80%)
            SendImportantInformationToClassifier();
            // Send coloration and email to the outlook.
            ReportFindingsToOutlook();
        }
        /// <summary>
        /// This takes the results from scanning the infomration and 
        /// sends the correct color to tag the email in outlook. 
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        private static void ReportFindingsToOutlook()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// After gathering all import for the email this method will send it to the 
        /// classifier to tag information and save the email onto the hard drive. 
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        private static void SendImportantInformationToClassifier()
        {
            throw new NotImplementedException();
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
        private static void ScanInformationForDetails()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Grabs all the information from the email.  
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        private static void GrabinformationFromEmail()
        {
            throw new NotImplementedException();
        }
    }
}