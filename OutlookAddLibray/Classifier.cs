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

            string[] fromsSpilt = from.Split("%split%");
            string[] subjectSpilt = subject.Split("%split%");
            string[] bodySpilt = body.Split("%split%");

            if (fromsSpilt.Length != subjectSpilt.Length || subjectSpilt.Length != bodySpilt.Length)
                return "Error";

            string classifiedEmail = "";
            for (int index = 0; index < fromsSpilt.Length; index++)
            {
                string currentFrom = fromsSpilt[index];
                string currentSubject = subjectSpilt[index];
                string currentBody = bodySpilt[index];
                string tagg = "";
                folderSystem.SaveToFolder(currentFrom, currentBody, currentSubject);
                if (!exceptions.Keys.Contains(currentFrom))
                {


                    var sampleData = new MLModel1.ModelInput()
                    {
                        Col1 = "@" + currentBody
                    };

                    //Load model and predict output
                    var result = MLModel1.Predict(sampleData);
                    tagg = result.PredictedLabel.ToLowerInvariant();
                }
                else
                {
                    tagg += exceptions[currentFrom];
                }
                classifiedEmail += tagg;
                classifiedEmail += "%spilt%";
            }
            return classifiedEmail.Substring(0, classifiedEmail.Length - 8);
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
            string newtag = "";
            string list = "";
            using (StreamReader reader = new StreamReader(inputfilePath))
            {
                list = reader.ReadToEnd();
            }
            using (StreamWriter writer = File.AppendText(inputfilePath))
            {
                if (Tag.Contains("remove"))
                    newtag = "remove";
                else
                    newtag = Tag;
                if (!list.Contains(sender + "\t" + newtag))
                {
                    writer.WriteLine();
                    writer.Write(sender + "\t" + newtag);
                }
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
