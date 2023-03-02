using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutlookAddLibray
{
    class FolderSystem
    {
        public FolderSystem()
        {
            string ClientEmail = @"C:\Client_Information\Client_Email";
            if (!Directory.Exists(ClientEmail))
            { 
                Directory.CreateDirectory(ClientEmail);
            }
            string ClientDictionary = @"C:\Client_Information\Client_Dictionary";
            if (!Directory.Exists(ClientDictionary))
            {
                Directory.CreateDirectory(ClientDictionary);
            }
        }

        /// <summary>
        /// this saves the email in the cilents folder. 
        /// </summary>
        /// <param name="clientName"></param>
        /// <param name="email"></param>
        public void SaveToFolder(String clientName, string email)
        {
            string filePath = @"C:\Client_Information\Client_Email\" + clientName;

            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }


            string[] spiltEmail = email.Split("\r\n");
            string subject = "";
            foreach (string spilt in spiltEmail)
            {
                string spiltLower = spilt.ToLower();
                if (spiltLower.Contains("subject"))
                {
                    string[] subjectSpilt = spiltLower.Split(":");
                    subject = subjectSpilt[1].Trim();

                    if (subject[subject.Length - 1].Equals(';'))
                        subject = subject.Remove(subject.Length - 1, 1);

                    break;
                }

            }
            filePath = filePath + @"\" + subject + ".txt";
            File.WriteAllText(filePath, email); 
            
            // Check if it's a exisiting matter create a folder. 
        }

    }

}
