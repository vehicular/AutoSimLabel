using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.Xml.Serialization;
using UnityScript.Lang;

[XmlRoot("imgInfoSYS", IsNullable = false)]
public class imgInfoSYS
{

    public string img_name;

    public float objCenterPiexInImg_x;
    public float objCenterPiexInImg_y;

    public int prgCenterPiexInImg_x;
    public int prgCenterPiexInImg_y;


    public float obj_size_x;
    public float obj_size_y;
    public float obj_size_z;


    public float objProjectionHeight; //y_max - y_min
    public float objProjectionWidth; //x_max - x_max

    public float distance_to_cam;

    public float angle_to_img;


    public float cam_pos_x;
    public float cam_pos_y;
    public float cam_pos_Z;

    public float target_pos_x;
    public float target_pos_y;
    public float target_pos_z;
    // public Array target_POS;
    // public Array cam_Pos;
    /*
    public imgInfoSYS(string n,float piexs_x,float piexs_y,float size_x,float size_y,float size_z,float dis,float ang)
    {
        n = this.img_name;
        piexs_x = this.img_piexs_x;
        piexs_y = this.img_piexs_y;
        size_x = this.img_size_x;
        size_y = this.img_size_y;
        size_z = this.img_size_z;
        dis = this.distance_to_cam;
        ang = this.angle_to_img;
    }
    */
}
