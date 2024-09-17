/*1. To Verify if the XML File is valid and all records have the same number of columns.
 
2. To Read a Sample XML File (of my choice).
 
3. To Filter some sections of the XML File by comparing tags given in the user input.
 
4. To Save the Filtered sections to another XML File. */




using System;
using System.IO;
using System.Xml;
using System.Xml.Schema;

namespace XmlValidatorAndCopier
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                XmlDocument xmlDoc = new XmlDocument();             //Here We are Loading the employee.xml file
                xmlDoc.Load("emp.xml");
                XmlSchemaSet schemaSet = new XmlSchemaSet();        //Here we are loading the emp.xsd file
                schemaSet.Add(null, "employees.xsd");

                bool isValid = true;                                //Here we are validating the .xsd with .xml file
                xmlDoc.Schemas = schemaSet;
                xmlDoc.Validate((sender, e) =>
                {
                    Console.WriteLine("Validation error: " + e.Message);              //here we can also print the e.Severity and e.Message as per user requirements
                    isValid = false;
                });

                if (!isValid)
                {
                    Console.WriteLine("XML validation failed. Exiting.");
                    return;
                }

                Console.WriteLine("Enter the parameter to search for (empname, dept, salary, companyname): ");        //here we are asking user to select the parameter name to search the emp.xml file
                string parameter = Console.ReadLine();

                Console.WriteLine("Enter the value for the parameter: ");                //here we are asking user to input the value for parameter selected
                string value = Console.ReadLine();

                XmlDocument newXmlDoc = new XmlDocument();
                XmlElement rootElement = newXmlDoc.CreateElement("employees");                      //Here we are creating a new xmldoc element and creating a root element for it as employees
                newXmlDoc.AppendChild(rootElement);

                foreach (XmlNode employeeNode in xmlDoc.SelectNodes("//employee"))                      //the searching the matching node in the collection of nodes matching as employee
                {
                    XmlNode parameterNode = employeeNode.SelectSingleNode(parameter);
                    if (parameterNode != null && parameterNode.InnerText == value)                              //checking if the node as well as value matches
                    {
                        XmlElement newEmployeeElement = newXmlDoc.CreateElement("employee");
                        rootElement.AppendChild(newEmployeeElement);

                        foreach (XmlNode childNode in employeeNode.ChildNodes)                                     //Searching the collection for mathcing childNodes
                        {
                            XmlElement newChildElement = newXmlDoc.CreateElement(childNode.Name);
                            newChildElement.InnerText = childNode.InnerText;
                            newEmployeeElement.AppendChild(newChildElement);
                        }
                    }
                }

                if (newXmlDoc.DocumentElement.ChildNodes.Count == 0)
                {
                    Console.WriteLine("No employee found with the specified parameter value.");
                    return;
                }

                newXmlDoc.Save("output.xml");
                Console.WriteLine("Employee details copied to output.xml");

            }
            
            catch (FileNotFoundException ex)          //Here we are raising proper Exceptions
            {
                Console.WriteLine(ex.Message);
            }
            catch (FileLoadException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (IOException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
