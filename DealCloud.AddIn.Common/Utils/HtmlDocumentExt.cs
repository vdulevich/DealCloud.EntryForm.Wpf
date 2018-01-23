using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace DealCloud.AddIn.Common.Utils
{
    public static class HtmlDocumentExt
    {
        public static IEnumerable<HtmlElement> GetElemenstByClassName(this HtmlDocument doc, string tag, string className)
        {
            foreach (HtmlElement el in doc.GetElementsByTagName(tag))
            {
                string classNameValue = el.GetAttribute("className");
                if (classNameValue.Contains(" "))
                {
                    if (classNameValue.Split(' ').Any(pred => string.Equals(pred, className, StringComparison.CurrentCultureIgnoreCase)))
                    {
                        yield return el;
                    }
                }
                else if(string.Equals(classNameValue, className, StringComparison.CurrentCultureIgnoreCase))
                {
                    yield return el;
                }
            }
        }

        public static IEnumerable<HtmlElement> GetElemenstByAttribute(this HtmlDocument doc, string tag, string attributeName, string attributeValue)
        {
            foreach (HtmlElement el in doc.GetElementsByTagName(tag))
            {
                if (el.GetAttribute(attributeName) == attributeValue)
                {
                    yield return el;
                }
            }
        }
    }
}
