using System.Collections.Generic;
using System.Configuration;

namespace ScreenScrapping.Console.ScrappingDefinition
{    
    #region ConfigurationSection implementation
    public class ScrappingDefinitionSection : ConfigurationSection
    {        
        [ConfigurationProperty("Definitions", IsRequired = true)]
        [ConfigurationCollection(typeof(Definition))]
        public GenericConfigurationElementCollection<Definition> Definitions
        {
            get { return (GenericConfigurationElementCollection<Definition>)this["Definitions"]; }
        }
    }

    public class Definition : ConfigurationElement
    {
        [ConfigurationProperty("Name")]
        public string Name
        {
            get { return (string) this["Name"]; }
        }

        [ConfigurationProperty("BaseUrl")]
        public string BaseUrl
        {
            get { return (string)this["BaseUrl"]; }
            //set { this["JobListUrl"] = value; }
        }

        [ConfigurationProperty("JobDetailLinkUrlXPath")]
        public string JobDetailLinkUrlXPath
        {
            get { return (string)this["JobDetailLinkUrlXPath"]; }
            //set { this["JobDetailLinkUrlXPath"] = value; }
        }

        [ConfigurationProperty("NextPageUrlXPath")]
        public string NextPageUrlXPath
        {
            get { return (string)this["NextPageUrlXPath"]; }
            //set { this["NextPageUrlXPath"] = value; }
        }

        [ConfigurationProperty("JobDetailFields", IsRequired = true)]
        [ConfigurationCollection(typeof(DetailField))]
        public GenericConfigurationElementCollection<DetailField> DetailFields
        {
            get { return (GenericConfigurationElementCollection<DetailField>)this["JobDetailFields"]; }
        }
    }    

    public class DetailField : ConfigurationElement
    {
        [ConfigurationProperty("Name")]
        public string Name
        {
            get { return this["Name"].ToString(); } 
            set { this["Name"] = value; }
        }

        [ConfigurationProperty("XPath")]
        public string XPath
        {
            get { return this["XPath"].ToString(); }
            set { this["XPath"] = value; }
        }
    }

    public class GenericConfigurationElementCollection<T> : ConfigurationElementCollection, IEnumerable<T>
        where T : ConfigurationElement, new()
    {
        private readonly List<T> _elements = new List<T>();

        protected override ConfigurationElement CreateNewElement()
        {
            var newElement = new T();
            _elements.Add(newElement);
            return newElement;
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return _elements.Find(e => e.Equals(element));
        }

        public new IEnumerator<T> GetEnumerator()
        {
            return _elements.GetEnumerator();
        }
    }
    #endregion


}
