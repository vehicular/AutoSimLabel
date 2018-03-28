using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;



[XmlRoot("imgInfoAfterLabel", IsNullable = false)]
public class imgInfoAfterLabel
{
    public string labelObj_name;
    public int bound_left_x;
    public int bound_right_x;
    public int bound_left_y;
    public int bound_right_y;


}
