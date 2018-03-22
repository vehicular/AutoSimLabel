using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
        // imgSYS = new imgInfoSYS();
        imgXML.Serialize(dbWriter, imgSYS);
        dbWriter.Close();
    }

    public static imgInfoSYS ReadObject(string filePath)
    {

        XmlSerializer sys = new XmlSerializer(typeof(imgInfoSYS));
        FileStream fs;

        fs = new FileStream(System.IO.Path.GetFileName(filePath), FileMode.Open);
        //sys.UnknownNode += new XmlNodeEventHandler(SerializerUnknownNode);
        //sys.UnknownAttribute += new XmlAttributeEventHandler(SerializerUnknownAttribute);
        imgInfoSYS _message = (imgInfoSYS)sys.Deserialize(fs);
        fs.Close();
        return _message;
    }
}

