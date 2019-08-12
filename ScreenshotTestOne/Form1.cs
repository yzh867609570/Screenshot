using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ScreenshotTestOne
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        #region 点击打开图像
        public string strHeadImagePath; //打开图片的路径
        Bitmap Bi;  //定义位图对像
        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "C# Corner Open File Dialog";
            openFileDialog.FilterIndex = 2;
            //openFileDialog.Filter = "*.gif|*.jpg|*.JPEG|*.JPEG|*.bmp|*.bmp";         //设置读取图片类型
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    strHeadImagePath = openFileDialog.FileName;
                    //this.Show(strHeadImagePath);
                    //Bi = new Bitmap(strHeadImagePath);  //使用打开的图片路径创建位图对像
                    //ImageCut1 IC = new ImageCut1(40, 112, this.pictureBox1.Width, this.pictureBox1.Height);      //实例化ImageCut类，四个参数据分别表示为：x、y、width、heigth，（40、112）表示pictureBox1的Lcation的坐标，（120、144）表示pictureBox1控件的宽度和高度
                    //this.pictureBox1.Image = IC.KiCut1((Bitmap)(this.GetSelectImage(this.pictureBox1.Width, this.pictureBox1.Height)));     //（120、144）表示pictureBox1控件的宽度和高度
                    //pictureBox1.Image = new Bitmap();

                    //本机屏幕分辨率
                    var rect = Screen.GetWorkingArea(this);

                    pictureBox1.Image = new Bitmap(new Bitmap(strHeadImagePath), rect.Width/2, rect.Height/2);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("格式不对");
                    ex.ToString();
                }
            }
        }
        #endregion
        #region 定义显示图像方法，即将打开的图像在pictureBox1控件显示
        public void Show(string strHeadImagePath)
        {
            this.pictureBox1.Load(@strHeadImagePath);   //
        }
        #endregion
        #region 获取图像
        /// <summary>
        /// 获取指定宽度和高度的图像即使图片和pictureBox1控件一样宽和高，返回值为图片Image
        /// </summary>
        /// <param name="Width表示宽"></param>
        /// <param name="Height表示高"></param>
        /// <returns></returns>
        public Image GetSelectImage(int Width, int Height)
        {
            //Image initImage = this.pictureBox1.Image;
            Image initImage = Bi;
            //原图宽高均小于模版，不作处理，直接保存 
            if (initImage.Width <= Width && initImage.Height <= Height)
            {
                //initImage.Save(fileSaveUrl, System.Drawing.Imaging.ImageFormat.Jpeg);
                return initImage;
            }
            else
            {
                //原始图片的宽、高 
                int initWidth = initImage.Width;
                int initHeight = initImage.Height;

                //非正方型先裁剪为正方型 
                if (initWidth != initHeight)
                {
                    //截图对象 
                    Image pickedImage = null;
                    Graphics pickedG = null;

                    //宽大于高的横图 
                    if (initWidth > initHeight)
                    {
                        //对象实例化 
                        pickedImage = new System.Drawing.Bitmap(initHeight, initHeight);
                        pickedG = System.Drawing.Graphics.FromImage(pickedImage);
                        //设置质量 
                        pickedG.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                        pickedG.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                        //定位 
                        Rectangle fromR = new Rectangle((initWidth - initHeight) / 2, 0, initHeight, initHeight);
                        Rectangle toR = new Rectangle(0, 0, initHeight, initHeight);
                        //画图 
                        pickedG.DrawImage(initImage, toR, fromR, System.Drawing.GraphicsUnit.Pixel);
                        //重置宽 
                        initWidth = initHeight;
                    }
                    //高大于宽的竖图 
                    else
                    {
                        //对象实例化
                        pickedImage = new System.Drawing.Bitmap(initWidth, initWidth);
                        pickedG = System.Drawing.Graphics.FromImage(pickedImage);
                        //设置质量 
                        pickedG.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                        pickedG.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                        //定位 
                        Rectangle fromR = new Rectangle(0, (initHeight - initWidth) / 2, initWidth, initWidth);
                        Rectangle toR = new Rectangle(0, 0, initWidth, initWidth);
                        //画图 
                        pickedG.DrawImage(initImage, toR, fromR, System.Drawing.GraphicsUnit.Pixel);
                        //重置高 
                        initHeight = initWidth;
                    }

                    initImage = (System.Drawing.Image)pickedImage.Clone();
                    //释放截图资源 
                    pickedG.Dispose();
                    pickedImage.Dispose();
                }

                return initImage;
            }
        }
        #endregion
        #region 缩放、裁剪图像用到的变量
        /// <summary>
        /// 
        /// </summary>
        int x1;     //鼠标按下时横坐标
        int y1;     //鼠标按下时纵坐标
        int width;  //所打开的图像的宽
        int heigth; //所打开的图像的高
        bool HeadImageBool = false;    // 此布尔变量用来判断pictureBox1控件是否有图片
        #endregion
        #region 画矩形使用到的变量
        Point p1;   //定义鼠标按下时的坐标点
        Point p2;   //定义移动鼠标时的坐标点
        Point p3;   //定义松开鼠标时的坐标点
        #endregion
        #region 鼠标按下时发生的事件
        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            this.Cursor = Cursors.Cross;
            this.p1 = new Point(e.X, e.Y);
            x1 = e.X;
            y1 = e.Y;
            if (this.pictureBox1.Image != null)
            {
                HeadImageBool = true;
            }
            else
            {
                HeadImageBool = false;
            }
        }
        #endregion
        #region 移动鼠标发生的事件
        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (this.Cursor == Cursors.Cross)
            {
                this.p2 = new Point(e.X, e.Y);
                if ((p2.X - p1.X) > 0 && (p2.Y - p1.Y) > 0)     //当鼠标从左上角向开始移动时P3坐标
                {
                    this.p3 = new Point(p1.X, p1.Y);
                }
                if ((p2.X - p1.X) < 0 && (p2.Y - p1.Y) > 0)     //当鼠标从右上角向左下方向开始移动时P3坐标
                {
                    this.p3 = new Point(p2.X, p1.Y);
                }
                if ((p2.X - p1.X) > 0 && (p2.Y - p1.Y) < 0)     //当鼠标从左下角向上开始移动时P3坐标
                {
                    this.p3 = new Point(p1.X, p2.Y);
                }
                if ((p2.X - p1.X) < 0 && (p2.Y - p1.Y) < 0)     //当鼠标从右下角向左方向上开始移动时P3坐标
                {
                    this.p3 = new Point(p2.X, p2.Y);
                }
                this.pictureBox1.Invalidate();  //使控件的整个图面无效，并导致重绘控件
            }
        }
        #endregion
        #region 松开鼠标发生的事件，实例化ImageCut1类对像
        ImageCut1 IC1;  //定义所画矩形的图像对像
        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (HeadImageBool)
            {
                width = this.pictureBox1.Image.Width;
                heigth = this.pictureBox1.Image.Height;
                if ((e.X - x1) > 0 && (e.Y - y1) > 0)   //当鼠标从左上角向右下方向开始移动时发生
                {
                    IC1 = new ImageCut1(x1, y1, Math.Abs(e.X - x1), Math.Abs(e.Y - y1));    //实例化ImageCut1类
                }
                if ((e.X - x1) < 0 && (e.Y - y1) > 0)   //当鼠标从右上角向左下方向开始移动时发生
                {
                    IC1 = new ImageCut1(e.X, y1, Math.Abs(e.X - x1), Math.Abs(e.Y - y1));   //实例化ImageCut1类
                }
                if ((e.X - x1) > 0 && (e.Y - y1) < 0)   //当鼠标从左下角向右上方向开始移动时发生
                {
                    IC1 = new ImageCut1(x1, e.Y, Math.Abs(e.X - x1), Math.Abs(e.Y - y1));   //实例化ImageCut1类
                }
                if ((e.X - x1) < 0 && (e.Y - y1) < 0)   //当鼠标从右下角向左上方向开始移动时发生
                {
                    IC1 = new ImageCut1(e.X, e.Y, Math.Abs(e.X - x1), Math.Abs(e.Y - y1));      //实例化ImageCut1类
                }
                this.pictureBox2.Width = (IC1.KiCut1((Bitmap)(this.pictureBox1.Image))).Width;
                this.pictureBox2.Height = (IC1.KiCut1((Bitmap)(this.pictureBox1.Image))).Height;
                this.pictureBox2.Image = IC1.KiCut1((Bitmap)(this.pictureBox1.Image));
                this.Cursor = Cursors.Default;

                this.label1.Text = "裁剪后的图片宽度：" + this.pictureBox2.Width.ToString();
                this.label2.Text = "裁剪后的图片高度：" + this.pictureBox2.Height.ToString();
            }
            else
            {
                this.Cursor = Cursors.Default; 
            }
        }
        #endregion
        #region 获取所选矩形图像
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Width"></param>
        /// <param name="Height"></param>
        /// <returns></returns>
        public Image GetSelectImage1(int Width, int Height)
        {
            Image initImage = this.pictureBox1.Image;
            //Image initImage = Bi;
            //原图宽高均小于模版，不作处理，直接保存 
            if (initImage.Width <= Width && initImage.Height <= Height)
            {
                //initImage.Save(fileSaveUrl, System.Drawing.Imaging.ImageFormat.Jpeg);
                return initImage;
            }
            else
            {
                //原始图片的宽、高 
                int initWidth = initImage.Width;
                int initHeight = initImage.Height;

                //非正方型先裁剪为正方型 
                if (initWidth != initHeight)
                {
                    //截图对象 
                    System.Drawing.Image pickedImage = null;
                    System.Drawing.Graphics pickedG = null;

                    //宽大于高的横图 
                    if (initWidth > initHeight)
                    {
                        //对象实例化 
                        pickedImage = new System.Drawing.Bitmap(initHeight, initHeight);
                        pickedG = System.Drawing.Graphics.FromImage(pickedImage);
                        //设置质量 
                        pickedG.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                        pickedG.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                        //定位 
                        Rectangle fromR = new Rectangle((initWidth - initHeight) / 2, 0, initHeight, initHeight);
                        Rectangle toR = new Rectangle(0, 0, initHeight, initHeight);
                        //画图 
                        pickedG.DrawImage(initImage, toR, fromR, System.Drawing.GraphicsUnit.Pixel);
                        //重置宽 
                        initWidth = initHeight;
                    }
                    //高大于宽的竖图 
                    else
                    {
                        //对象实例化
                        pickedImage = new System.Drawing.Bitmap(initWidth, initWidth);
                        pickedG = System.Drawing.Graphics.FromImage(pickedImage);
                        //设置质量 
                        pickedG.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                        pickedG.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                        //定位 
                        Rectangle fromR = new Rectangle(0, (initHeight - initWidth) / 2, initWidth, initWidth);
                        Rectangle toR = new Rectangle(0, 0, initWidth, initWidth);
                        //画图 
                        pickedG.DrawImage(initImage, toR, fromR, System.Drawing.GraphicsUnit.Pixel);
                        //重置高 
                        initHeight = initWidth;
                    }

                    initImage = (System.Drawing.Image)pickedImage.Clone();
                    //释放截图资源 
                    pickedG.Dispose();
                    pickedImage.Dispose();
                }

                return initImage;
            }
        }
        #endregion
        #region 重新绘制pictureBox1控件，即移动鼠标画矩形
        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            if (HeadImageBool)
            {
                Pen p = new Pen(Color.Black, 1);//画笔
                p.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
                //Bitmap bitmap = new Bitmap(strHeadImagePath);
                Bitmap bitmap = Bi;
                Rectangle rect = new Rectangle(p3, new Size(System.Math.Abs(p2.X - p1.X), System.Math.Abs(p2.Y - p1.Y)));
                e.Graphics.DrawRectangle(p, rect);
            }
        }
        #endregion

        private void button2_Click(object sender, EventArgs e)
        {
            pictureBox2.Image.Save(@"C:\Users\tom86\Desktop\screenshot.jpg", ImageFormat.Jpeg);
        }
    }
}
