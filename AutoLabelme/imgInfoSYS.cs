using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;



[XmlRoot("imgInfoSYS", IsNullable = false)]
public class imgInfoSYS
{
    public string img_name;
    public float img_piexs_x;
    public float img_piexs_y;
    public float img_size_x;
    public float img_size_y;
    public float img_size_z;
    public float distance_to_cam;
    public float angle_to_img;
    public float cam_pos_x;
    public float cam_pos_y;
    public float cam_pos_Z;
    public float target_pos_x;
    public float target_pos_y;
    public float target_pos_z;
    //public Array target_POS;
    //public Array cam_Pos;
}
