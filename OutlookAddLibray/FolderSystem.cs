using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/// <summary>
/// 
/// </summary>
namespace OutlookExecutable
{
    class FolderSystem
    {
        string documentFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        public FolderSystem()
        {
           // string ClientEmail = @"C:\Client_Information\Client_Email";
         
            string clientEmailDirectory = Path.Combine(documentFolder, "Client_Correspondence");
            if (!Directory.Exists(clientEmailDirectory))
            { 
                Directory.CreateDirectory(clientEmailDirectory);
            }
        }

        /// <summary>
        /// this saves the email in the cilents folder. 
        /// </summary>
        /// <param name="clientName"></param>
        /// <param name="email"></param>
        public void SaveToFolder(string clientName, string email, string emailSubject)
        {
            // string filePath = @"C:\Client_Information\Client_Email\" + clientName;
            string filePath = Path.Combine(Path.Combine(documentFolder, "Client_Correspondence"), clientName);

            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
                string tempPath = filePath + @"\" + "WordWeightsAdditions.txt";
                File.WriteAllText(tempPath, "");
            }


            string[] spiltEmail = email.Split("\r\n");
            string subject = emailSubject.Trim();
            if (subject[subject.Length - 1].Equals(';'))
                 subject = subject.Remove(subject.Length - 1, 1);

            // GOAL: Check if it's a exisiting matter create a folder. 

            filePath = filePath + @"\" + subject + ".txt";
            File.WriteAllText(filePath, email); 
            // Goal: download actual emails to this spot. 
        }
    }

}
