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
        string imgFoldPath = "C:\\Users\\Administrator\\Documents\\DeepLearningTest\\signShots\\ ";
        string imgInfoPath = "C:\\Users\\Administrator\\Documents\\DeepLearningTest\\img_info_folder\\ ";

        DirectoryInfo imgFold;
        DirectoryInfo imgInfoFold;
        FileInfo[] imgFiles;
        FileInfo[] imgInfoFiles;


        Image openImage;

        
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
                    
                    int pos_x =Convert.ToInt32(imgInfo.img_piexs_x);
                    int pos_y =Convert.ToInt32(imgInfo.img_piexs_y);
                    Point centerPoint = new Point(pos_x, pos_y);
                    Point scale = new Point();
                    scale.X = GetScale(imgInfo.distance_to_cam).X;
                    scale.Y = GetScale(imgInfo.distance_to_cam).Y;

                    Point scale1 = new Point(2, 2);//temp
                    imgInfo.img_size_x = imgInfo.img_size_y = 5;//temp

                    Point P0 = new Point();
                    P0.X = (int)(centerPoint.X - scale1.X * imgInfo.img_size_x);// object size
                    P0.Y = (int)(centerPoint.Y - scale1.Y * imgInfo.img_size_y);// object size
                    Point P1 = new Point();
                    P1.X = (int)(centerPoint.X + scale1.X * imgInfo.img_size_x);// object size
                    P1.Y = (int)(centerPoint.Y + scale1.Y * imgInfo.img_size_y);// object size



                    DrawRect(openImage, P0,P1 , count+"");
                   
                }
                textBox1.Text = "All picture label finished";
            }

        }


        public Point GetScale(float distance)
        {
            Point p = new Point(1, 1);
            return p;
        }
        //------Draw Rect
        public void  DrawRect(Image image ,Point p0 ,Point p1,string saveImgPath)
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
                    }
                    image.Save(AppDomain.CurrentDomain.BaseDirectory + saveImgPath +"2.jpg");
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
