

using Microsoft.ML;
using OutlookAddLibray;
using System;
using System.Reflection.PortableExecutable;
using System.Text.RegularExpressions;
using static System.Formats.Asn1.AsnWriter;
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
        // Inner class varaibles. 
        private FolderSystem folderSystem;

        // 
        private Dictionary<string, string> exceptions;

        //Message that is sent back to the outlook add on. 
        List<string> jsonMessage = new List<string>();

        public Classifier()
        {
            folderSystem = new FolderSystem();
            exceptions = new Dictionary<string, string>();
            FillDictionary();
        }
        /// <summary>
        /// 
        /// </summary>
        private void FillDictionary()
        {
            // string inputfilePath = @"C:\Users\skate\Source\Repos\executive-assistants\OutlookAddLibray\OverrideList.txt";

            string inputfilePath = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), @"..\\OutlookAddLibray\\OverrideList.txt"));
            // Open the file in append mode
            lock (this)
            {
                using (
                    StreamReader reader = new StreamReader(inputfilePath))
                {
                    while (!reader.EndOfStream)
                    {
                        string line = reader.ReadLine();
                        string[] spilt = line.Split("\t");
                        if (spilt.Length == 2)
                        {
                            if (spilt[1].Equals("remove"))
                            {
                                exceptions.Remove(spilt[0]);
                            }
                            else if (!exceptions.ContainsKey(spilt[0]))
                            {
                                exceptions.Add(spilt[0], spilt[1]);
                            }
                            else
                            {
                                exceptions[spilt[0]] = spilt[1];
                            }
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Executes the NLP 
        /// </summary>
        public string execute(string from, string subject, string body)
        {

           // folderSystem.SaveToFolder(from, body, subject);
            if (exceptions.Keys.Contains(from))
            {
                return exceptions[from];
            }

            string classifiedEmail = "";
            var sampleData = new MLModel1.ModelInput()
            {
                Col1 = "@" + body
            };

            //Load model and predict output
            var result = MLModel1.Predict(sampleData);
            string tagg = result.PredictedLabel.ToLowerInvariant();

            if (tagg.Equals("important"))
                classifiedEmail = "High Priority";
            else if (tagg.Equals("unimportant"))
                classifiedEmail = "Low Priority";
            else
                classifiedEmail = "Medium Priority";

            return classifiedEmail;
            // }

        }
        /// <summary>
        /// Send to trina 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="Tag"></param>
        public void changeOverideDictionary(string sender, string Tag)
        {
            string inputfilePath = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), @"..\\OutlookAddLibray\\OverrideList.txt"));

            /*if (Tag.Equals("remove"))
            {
                if (exceptions.ContainsKey(sender))
                {
                    exceptions.Remove(sender);
                } 
                return;
            }
            if (exceptions.ContainsKey(sender))
            {
                exceptions[sender] = Tag;
            } 
            else
            { */
            using (StreamWriter writer = File.AppendText(inputfilePath))
            {
                //exceptions.Add(sender, Tag);
                writer.WriteLine();
                writer.Write(sender + "\t" + Tag);
            }

        }
        /// <summary>
        /// Share  with trina 
        /// </summary>
        /// <param name="emailBody"></param>
        /// <param name="tag"></param>
        public void retrainData(string emailBody, string tag)
        {
            string inputfilePath = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), @"..\\OutlookAddLibray\\testing-INFOtext.txt"));
            string outputfilePath = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), @"..\\OutlookAddLibray\\MLModel1.mlnet")); ;
            emailBody = emailBody.Replace("\r", "");
            emailBody = emailBody.Replace("\n", "");

            string textToAppend = tag + "\t" + emailBody;

            // Open the file in append mode
            lock (this)
            {
                using (
                    StreamWriter writer = File.AppendText(inputfilePath))
                {
                    // Write the new text to the end of the file
                    writer.WriteLine();
                    writer.Write(textToAppend);
                }
                MLModel1.Train(outputfilePath, inputfilePath, '\t', false);
            }
        }
    }
    /// <summary>
    /// A sub-class of NLP that will allow us to send the information grabbed from the
    /// email as a json string. 
    /// </summary>
    public class EmailTagger
    {
        //change this to a list. 
        public string? colortagged { get; set; }
    }
}
