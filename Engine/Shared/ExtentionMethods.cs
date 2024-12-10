using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Engine.Shared
{
    // Extention methods let you "extend" the available functions on objects. When created, by defining the datatype (or interface) it works on
    // and the function will be visible through IntelliSense
    // Extention method class must be static.
    // Functions must also be static. They are global, and we don't want to instantiate an object.
    // The first parameter must also be 'this' - to signify the object/interface you can call the extention method from.
    public static class ExtentionMethods
    {
        // any XmlNode class object can now call this method because the first parameter signifies 'this'
        public static int AttributeAsInt(this XmlNode node, string attributeName)
        {
            return Convert.ToInt32(node.AttributeAsString(attributeName));
        }

        public static string AttributeAsString(this XmlNode node, string attributeName)
        {
            XmlAttribute attribute = node.Attributes?[attributeName];

            if (attribute == null)
            {
                throw new ArgumentException($"The attribute {attributeName} does not exist.");
            }

            return attribute.Value;
        }
    }
}
