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
        string imgFoldPath = "D:\\ad_simulation_Logitech\\ad_simulation\\va_sim\\Drive_1_4_2\\signShots\\";//The Image Folder
        string imgInfoPath = "D:\\ad_simulation_Logitech\\ad_simulation\\va_sim\\Drive_1_4_2\\img_info_folder\\ ";//The ImageTXT Folder

        DirectoryInfo imgFold;
        DirectoryInfo imgInfoFold;
        FileInfo[] imgFiles;
        FileInfo[] imgInfoFiles;

        
        Image openImage;

        int[] affirScale = { 205, 120, 91, 66, 55 };
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
                for (int count=0; count<imgInfoFiles.Length;count++)
                {
                    ///------------open Image 
                    this.pictureBox1.Load(imgFiles[count].FullName);
                    this.pictureBox1.SizeMode = PictureBoxSizeMode.AutoSize;
                    openImage = this.pictureBox1.Image;

                    //open Txt file
                    imgInfoSYS imgInfo = XMLWR.ReadObject(imgInfoFiles[count].FullName);                
                   
                    //the obj center Point in image 
                    int pos_x =Convert.ToInt32(imgInfo.prgCenterPiexInImg_x);
                    int pos_y =Convert.ToInt32(imgInfo.prgCenterPiexInImg_y);
                    Point centerPoint = new Point(pos_x, pos_y);


                    Point P0 = new Point();
                    Point P1 = new Point();
                    double scale = GetScale(imgInfo.distance_to_cam);

                    double width_left = centerPoint.X - scale * imgInfo.objProjectionWidth;
                    double height_top = centerPoint.Y - scale * imgInfo.objProjectionHeight;

                    P0.X =Convert.ToInt32(Math.Ceiling(width_left));// object size
                    P0.Y = Convert.ToInt32(Math.Ceiling(height_top));


                    double width_right = centerPoint.X + scale * imgInfo.objProjectionWidth;
                    double height_buttom = centerPoint.Y + scale * imgInfo.objProjectionHeight;
                    P1.X = Convert.ToInt32(Math.Ceiling(width_right));
                    P1.Y = Convert.ToInt32(Math.Ceiling(height_buttom));

                    DrawRect(openImage, P0,P1 , count+"", centerPoint);
                   
                }
                textBox1.Text = "All picture label finished";
            }
        }


        public double GetScale(float distance)
        {
            double scale = 0;
            int distanceToInt = Convert.ToInt32(Math.Ceiling(distance)); ///user to the 2.3.4.5.6m
            float distanceRemainder = distance - Convert.ToInt32(Math.Floor(distance));

            //0?
            if( distance < 2)
            {

            }
            else if (distance >=2 && distance < 3)
            {
                // 2m ： 205
                // 3m :  120
                scale = (distance - 2.0f) * (205 - 120) / (3.0f - 2.0f);
                //scale = affirScale[distanceToInt - 2];
            }
            else if (distance >= 3 && distance < 6)
            {
                //
                scale = affirScale[distanceToInt - 2];
            }
            else if (distance >= 6 && distance < 8)
            {
                scale = affirScale[distanceToInt - 2];
            }
            else if(distance > 8 && distance < 10)
            {
                scale = 55 - 10 * distanceRemainder;
            }
            else if (distance >= 20 && distance <28)
            {
                // 20m ： 22
                int a = 17;
                // 30m :  15
                int b = 12;
                scale = a-(distance - 20.0f) * (a - b) / (28.0f - 20.0f);
                //scale = affirScale[distanceToInt - 2];
            }

            else
            {
                scale = 6;
            }
            
            return scale;
        }
        //------Draw Rect
        public void  DrawRect(Image image ,Point p0 ,Point p1,string saveImgPath, Point center)
        {
                Pen pen = new Pen(Color.Red, 2);
          
                // Create bitmap
                using (Bitmap newImage = new Bitmap(image.Width,image.Height))
                {

                    // Crop and resize the image.
                   // Rectangle destination = new Rectangle(0, 0, 200, 120);
                    using (Graphics graphic = Graphics.FromImage(image))
                    {
                        graphic.DrawRectangle(pen, p0.X, p0.Y, p1.X-p0.X, p1.Y-p0.Y);
                    graphic.DrawRectangle(pen, center.X - 1, center.Y - 1, 2,2);
                    }
                    image.Save(AppDomain.CurrentDomain.BaseDirectory + saveImgPath +".jpg");
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
