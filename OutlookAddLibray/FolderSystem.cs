using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutlookAddLibray
{
    class FolderSystem
    {
        string documentFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        public FolderSystem()
        {
           // string ClientEmail = @"C:\Client_Information\Client_Email";
         
            string clientEmailDirectory = Path.Combine(documentFolder, "Client_Emails");
            if (!Directory.Exists(clientEmailDirectory))
            { 
                Directory.CreateDirectory(clientEmailDirectory);
            }
           
            string clientDictionaryDirectory = Path.Combine(documentFolder, "Client_Dictionary");
            if (!Directory.Exists(clientDictionaryDirectory))
            {
                Directory.CreateDirectory(clientDictionaryDirectory);
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
            string filePath = Path.Combine(Path.Combine(documentFolder, "Client_Emails"), clientName);

            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }


            string[] spiltEmail = email.Split("\r\n");
            string subject = emailSubject.Trim();
          //  foreach (string spilt in spiltEmail)
           // {
               // string spiltLower = spilt.ToLower();
               // if (spiltLower.Contains("subject"))
               // {
                 //   string[] subjectSpilt = spiltLower.Split(":");
                    //subject = .Trim();

                    if (subject[subject.Length - 1].Equals(';'))
                        subject = subject.Remove(subject.Length - 1, 1);

                   // break;
              //  }

            //}
            filePath = filePath + @"\" + subject + ".txt";
            File.WriteAllText(filePath, email); 
            
            // Check if it's a exisiting matter create a folder. 
        }

    }

}
