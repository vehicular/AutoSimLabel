using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace AutoLabelme
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }


        /// <summary>
        /// //
        /// </summary>
        string imgFoldPath = @"C:\Users\Administrator\Documents\GitHub\Alisa_Mao\DeepLearningFirstVersion\simulation\ad_simulation\va_sim\Drive_1_4_2\signShots\\";//The Image Folder
        string imgInfoPath = @"C:\Users\Administrator\Documents\GitHub\Alisa_Mao\DeepLearningFirstVersion\simulation\ad_simulation\va_sim\Drive_1_4_2\img_info_folder\\ ";//The ImageTXT Folder

        DirectoryInfo imgFold;
        DirectoryInfo imgInfoFold;
        FileInfo[] imgFiles;
        FileInfo[] imgInfoFiles;

        Point P0 = new Point();
        Point P1 = new Point();
        Image openImage;

        string objName;
        //1.0 -1.9 ,   2.0-2.9 ,  3.0-3.9,   4.0-4.9      5.0-5.9, 
        int[] affirScale = { 205, 130, 95, 70, 55 };//,      50  ,  40};
        //开始自动画图
        private void button3_Click(object sender, EventArgs e)
        {

            try
            {
                imgFold = new DirectoryInfo(imgFoldPath);
                imgInfoFold = new DirectoryInfo(imgInfoPath);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Open Folder error :" + ex.Message);
            }

            imgFiles = imgFold.GetFiles();
            imgInfoFiles = imgInfoFold.GetFiles();
            if (imgFiles.Length == imgInfoFiles.Length)
            {
                for (int count = 0; count < imgInfoFiles.Length; count++)
                {
                    ///------------open Image 
                    this.pictureBox1.Load(imgFiles[count].FullName);
                    this.pictureBox1.SizeMode = PictureBoxSizeMode.AutoSize;
                    openImage = this.pictureBox1.Image;

                    //open Txt file
                    imgInfoSYS imgInfo = XMLWR.ReadObject(imgInfoFiles[count].FullName);

                    //the obj center Point in image ,
                    //int pos_x =Convert.ToInt32(imgInfo.prgCenterPiexInImg_x);
                    // int pos_y =Convert.ToInt32(imgInfo.prgCenterPiexInImg_y);
                    int pos_x = Convert.ToInt32(imgInfo.objCenterPiexInImg_x);
                    int pos_y = Convert.ToInt32(imgInfo.objCenterPiexInImg_y);
                    Point centerPoint = new Point(pos_x, pos_y);

                    objName = imgInfo.img_name;
                    //---Function GetOrignPointX Test 


                    float scale = GetScale(imgInfo.distance_to_cam);

                    float height = scale * imgInfo.objProjectionHeight * 1.2f;
                    float width = scale * imgInfo.objProjectionWidth * 0.5f;
                    // P0.X = imgInfo.imgMinXPoint;
                    // P0.Y = imgInfo.imgMinYPoint;
                    // P1.X = imgInfo.imgMaxXPoint;
                    // P1.Y = imgInfo.imgMaxYPoint;

                    List<Point> list = GetOrignPointX(P0, P1, centerPoint, width, height, imgInfo.imgSizeX, imgInfo.imgSizeY);
                    P0 = list[0];
                    P1 = list[1];
                    /*
                                     // ------the useful
                                     Point P0 = new Point();
                                     Point P1 = new Point();
                                     float scale = GetScale(imgInfo.distance_to_cam);

                                     P0.X = Convert.ToInt32(centerPoint.X - scale * imgInfo.objProjectionWidth);
                                     P0.Y = Convert.ToInt32(centerPoint.Y - scale * imgInfo.objProjectionHeight);                                  

                                     P1.X = Convert.ToInt32(centerPoint.X + scale * imgInfo.objProjectionWidth);
                                     P1.Y = Convert.ToInt32(centerPoint.Y + scale * imgInfo.objProjectionHeight);
                                      */

                    DrawRect(openImage, P0, P1, count + "", centerPoint);

                    //After DrawRect ,create the Label Xml File
                    imgInfoAfterLabel labelSYS = new imgInfoAfterLabel();
                    labelSYS.labelObj_name = objName;
                    labelSYS.bound_left_x = P0.X;
                    labelSYS.bound_right_x = P1.X;
                    labelSYS.bound_right_y = P0.Y;
                    labelSYS.bound_right_y = P1.Y;
                    XMLWR.CreateLabelXML(AppDomain.CurrentDomain.BaseDirectory + count + ".txt", labelSYS);

                }
                textBox1.Text = "All picture label finished";
            }
            else
            {
                Console.WriteLine("The TXT Length is not equal to Image Length");
            }
        }


        public List<Point> GetOrignPointX(Point p0, Point p1, Point centerPoint, float widthMargin, float heightMargin, int imgBoundX, int imgBoundY)
        {
            List<Point> lPoint = new List<Point>();
            p0.X = Convert.ToInt32(centerPoint.X - widthMargin);
            p1.X = Convert.ToInt32(centerPoint.X + widthMargin);

            p0.Y = Convert.ToInt32(centerPoint.Y - heightMargin);
            p1.Y = Convert.ToInt32(centerPoint.Y + heightMargin);

            if (centerPoint.X - widthMargin <= 0)
            {
                p0.X = 1;
            }
            if (centerPoint.Y - heightMargin <= 0)
            {
                p0.Y = 1;
            }
            if (centerPoint.X + widthMargin >= imgBoundX)
            {
                //p0.X =Convert.ToInt32(centerPoint.X + widthMargin) - imgBoundX;
                p1.X = imgBoundX - 1;
            }
            if (centerPoint.Y + heightMargin >= imgBoundY)
            {
                //p0.Y =Convert.ToInt32(centerPoint.Y + widthMargin) - imgBoundY;
                p1.Y = imgBoundY - 1;

            }
            lPoint.Add(p0);
            lPoint.Add(p1);
            return lPoint;
        }
        public float GetScale(float distance)
        {
            float scale = 0;
            int distanceToInt = Convert.ToInt32(Math.Ceiling(distance)); ///user to the 2.3.4.5.6m
            //float distanceRemainder = distance - Convert.ToInt32(Math.Floor(distance));

            //0?
            if (distanceToInt >= 2 && distanceToInt <= 4)
            {
                scale = affirScale[distanceToInt - 2];
            }/*
            else if (distance >= 2 && distance < 3)
            {
                // 2m ： 205
                // 3m :  120
                scale = scaleCalculate(distance, 2.0f, 205f, 3f, 120f);
                //  scale = (distance - 2.0f) * (205 - 120) / (3.0f - 2.0f);
                //scale = affirScale[distanceToInt - 2];
            }
            else if (distance >= 3 && distance < 4)
            {
                scale = scaleCalculate(distance, 3.0f, 120f, 4.0f, 91f);
            }
            else if (distance >= 4 && distance < 6)
            {
                scale = scaleCalculate(distance, 4.0f, 91f, 6.0f, 55f);
            }
            else if (distance >=7.5f && distance < 8)
            {
                scale = scaleCalculate(distance, 6.0f, 70f, 8.0f, 41f);
            }
             */
            else if (distance >= 5.0 && distance < 6)
            {
                scale = scaleCalculate(distance, 5.0f, 66f, 6.0f, 58f);
            }
            else if (distance >= 6.0 && distance < 7)
            {
                scale = scaleCalculate(distance, 6.0f, 54f, 7.0f, 50f);
            }
            else if (distance >= 7.0 && distance < 8)
            {
                scale = scaleCalculate(distance, 7.0f, 44f, 8.0f, 40f);
            }

            else if (distance >= 8 && distance < 10)
            {
                scale = scaleCalculate(distance, 8.0f, 42f, 10.0f, 35f);
            }
            else if (distance >= 10 && distance < 15)
            {
                scale = scaleCalculate(distance, 10.0f, 36f, 20.0f, 20f);
            }
            else if (distance >= 15 && distance < 20)
            {
                scale = scaleCalculate(distance, 15.0f, 20f, 20.0f, 16f);
            }
            else if (distance >= 20 && distance < 30)
            {
                scale = scaleCalculate(distance, 20.0f, 16f, 30.0f, 10f);
            }
            else if (distance >= 30 && distance < 45)
            {
                scale = scaleCalculate(distance, 12.0f, 12f, 45.0f, 8f);
            }
            else if (distance >= 45 && distance < 60)
            {
                scale = scaleCalculate(distance, 45.0f, 10f, 60.0f, 8f);
            }
            else if (distance >= 60 && distance < 70)
            {
                scale = 5;
            }

            /*
            else if (distance >= 20 && distance < 28)
            {
                // 20m ： 22
                int a = 17;
                // 30m :  15
                int b = 12;
                scale = a - (distance - 20.0f) * (a - b) / (28.0f - 20.0f);
                //scale = affirScale[distanceToInt - 2];
            }
*/
            else if (distance >= 70 && distance < 80)
            {
                scale = 4;
            }
            else
            {
                scale = 3;
            }

            return scale;
        }

        public float scaleCalculate(float distance, float mileMin, float RectMax, float mileMax, float RectMin)
        {
            return RectMax - (distance - mileMin) * (RectMax - RectMin) / (mileMax - mileMin);
        }
        //------Draw Rect
        public void DrawRect(Image image, Point p0, Point p1, string saveImgPath, Point center)
        {
            Pen pen = new Pen(Color.Red, 2);
            // Create bitmap
            using (Bitmap newImage = new Bitmap(image.Width, image.Height))
            {

                // Crop and resize the image.
                // Rectangle destination = new Rectangle(0, 0, 200, 120);
                using (Graphics graphic = Graphics.FromImage(image))
                {
                    graphic.DrawRectangle(pen, p0.X, p0.Y, p1.X - p0.X, p1.Y - p0.Y);
                    graphic.DrawRectangle(pen, center.X - 1, center.Y - 1, 2, 2);  //--test ,create the center-Point in image
                }
                image.Save(AppDomain.CurrentDomain.BaseDirectory + saveImgPath + ".jpg");
            }
        }

        //------------
        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}


/*
Point scale = new Point();
scale.X = GetScale(imgInfo.distance_to_cam).X;
scale.Y = GetScale(imgInfo.distance_to_cam).Y;

Point scale1 = new Point(2,2 );//temp
imgInfo.obj_size_x = imgInfo.obj_size_y = 5;//temp

Point P0 = new Point();
P0.X = (int)(centerPoint.X - scale1.X * imgInfo.obj_size_x);// object size
Console.WriteLine(imgInfo.distance_to_cam);
P0.Y = (int)(centerPoint.Y - scale1.Y * imgInfo.obj_size_y);// object size
Point P1 = new Point();
P1.X = (int)(centerPoint.X + scale1.X * imgInfo.obj_size_x);// object size
P1.Y = (int)(centerPoint.Y + scale1.Y * imgInfo.obj_size_y);// object size
*/
