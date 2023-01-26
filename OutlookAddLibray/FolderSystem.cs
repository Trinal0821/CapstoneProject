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
        /// 
        /// </summary>
        /// <param name="clientName"></param>
        /// <param name="email"></param>
        public void SaveToFolder(String clientName, string email)
        {
            string[] spiltEmail = email.Split("\r\n");
            string[] subject = spiltEmail[1].Split(";");

            string emailName = subject[0] + ".txt";

            string filePath = @"C:\Client_Information\Client_Email\" + clientName;

            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
            // save the file 
            filePath = filePath + @"\" + emailName;
            File.WriteAllText(emailName, email);    
            //currently not saving the file correctly. 
        }

    }

}
