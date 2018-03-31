using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;



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



    public int imgSizeX;
    public int imgSizeY;

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


    public int imgMinXPoint;
    public int imgMinYPoint;
    public int imgMaxXPoint;
    public int imgMaxYPoint;
}
