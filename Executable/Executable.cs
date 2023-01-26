
using OutlookExecutable;
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
namespace Executable
{
    /// <summary>
    /// This is the wraper class for our project. The Natural Lanuague Processor is called from here. 
    /// </summary>
    public class Executable
    {

        public static void Main(String[] args)
        {
            NLP nlp = new NLP();
            nlp.execute();
        }
    }
}
