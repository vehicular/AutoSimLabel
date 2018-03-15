using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Diagnostics;
using System.Xml;
using System.Text;
using System.IO;



public class XMLWR
{
    //private static imgInfoSYS imgSYS;
    public static void CreateImgXML(string filePath, imgInfoSYS imgSYS)
    {
        XmlSerializer imgXML = new XmlSerializer(typeof(imgInfoSYS));
        TextWriter dbWriter = new StreamWriter(filePath);
        imgSYS = new imgInfoSYS();
        imgXML.Serialize(dbWriter, imgSYS);
        dbWriter.Close();
    }

    public static imgInfoSYS ReadObject(string filePath)
    {
        imgInfoSYS _message = null;
        FileStream fs;

        fs = new FileStream(filePath, FileMode.Open);
        try
        {
            XmlSerializer sys = new XmlSerializer(typeof(imgInfoSYS));
            _message = (imgInfoSYS)sys.Deserialize(fs);

        }
        catch(System.Exception ex)
        {
            string a = ex.ToString();
        }
        //sys.UnknownNode += new XmlNodeEventHandler(SerializerUnknownNode);
        //sys.UnknownAttribute += new XmlAttributeEventHandler(SerializerUnknownAttribute);
        
        fs.Close();
        return _message;
    }
}

