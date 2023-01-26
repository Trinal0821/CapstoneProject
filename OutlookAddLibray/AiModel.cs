using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutlookAddLibray
{
    public class AiModel
    {
        /// <summary>
        /// Private golbal variables to keep track of the parent(Company) 
        /// and the children(persons).
        /// </summary>
        private Dictionary<string, double> dictionary;
        private List<AiModel> children;
        private AiModel parent;
        private string company;
        private string person;
        private const double standardWeight = 20; 

        /// <summary>
        /// This is an consturctor for the AiModel that creates
        /// a list of childen modles and the current dictionary. 
        /// </summary>
        /// <param name="parent">The parent dictionary </param>
        public AiModel(AiModel parent)
        {
            children = new List<AiModel>(); 
            dictionary = new Dictionary<string, double>();
        }
        /// <summary>
        /// An empty constuctor of the AiModel that creates the parent
        /// </summary>
        public AiModel()
        {
           parent = new AiModel();
           dictionary = new Dictionary<string, double>();
        }

       /// <summary>
       /// 
       /// </summary>
       /// <returns></returns>
        private List<AiModel> GetChildenList() 
        {
            return children;
        }
        /// <summary>
        /// 
        ///
        /// </summary>
        /// <param name="name"></param>
        /// <param name="parent"></param>
        private void AddChildNode(string name, string companyName)
        {
           
            AiModel theChild = new AiModel(this);
            theChild.person = name;
            theChild.company = companyName;

            theChild.dictionary = this.dictionary; 
            children.Add(theChild);
        }
        /// <summary>
        /// *NOTE* Only be used if we can assume the same keys for parent and child.  
        /// </summary>
        private void InfluenceCompany()
        {
            foreach(string parentKey in this.dictionary.Keys)
            {
                dictionary[parentKey] /= 2; // weighted average
            }
            foreach(AiModel child in children)
            {
                foreach (string key in dictionary.Keys)
                {
                    dictionary[key] = child.dictionary[key]/(2 * child.dictionary.Keys.Count);
                }
            }
        }
        /// <summary>
        /// This method will add a key word to the current company and every person. 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="weight"></param>
        private void KeyUpdate(string key,double weight = standardWeight)
        {
            dictionary.Add(key, weight);

            foreach (AiModel child in children)
            {
                child.dictionary.Add(key, weight);
            }
                
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="weight"></param>
        private void MasterKeyUpdate(string key, double weight = standardWeight)
        {
            dictionary.Add(key, weight);
            foreach (AiModel child in children)
            {
                child.KeyUpdate(key, weight);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="weight"></param>
        private void AddChildKey(string key, double weight = standardWeight)
        {
            this.dictionary.Add(key, weight);
        }
    }
}
