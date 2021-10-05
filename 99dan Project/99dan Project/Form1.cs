using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace _99dan_Project
{
    public partial class BBForm : Form
    {
        private const int dot_size = 10;
        private Bitmap bitmap;
        //색상표
        public List<string> colorList = new List<string>()
        {
            "FFFFFF","CDCBCE","A6A6A6","838184","595959","323431",
            "EA3337","F19FAD","ED6F7D","EB4958","AE232A","79151D",
            "EE8833","F7CCA2","F4B570","F19F4B","B16623","7A4415",
            "F9D94A","FCF0A6","FCE878","FAE058","B79F33","806A1F",
            "7AE444","B4E594","99D86D","76C045","4D9027","326018",
            "56BBB5","A8E5DE","82D8CD","64C7C4","408F8A","28615A",
            "4BA8F8","ABDEFD","7FC9FA","5FB8FA","387FB7","21547F",
            "2E45C5","A5AAE2","7289D8","4F64CF","263497","1B245F",
            "6923F5","C0A2F8","9D74F8","8349F6","541DB6","34137E",
            "EA3CD1","F1A6E9","EE79E1","EB56D8","AD2CA0","781E68",
            "95776D","D4C7BF","BAABA4","A99385","705A4C","483D3B"
        };
        public List<string> setColorList = new List<string>();
        public BBForm()
        {
            InitializeComponent();
        }

        private void BBForm_Load(object sender, EventArgs e)
        {
            Origin_Image.SizeMode = PictureBoxSizeMode.Zoom;

            foreach(string s in colorList)
            {
                string sColor = "#" + s;
                setColorList.Add(sColor);
            }
        }

        private void LoadImage_btn_Click(object sender, EventArgs e)
        {
            string imageFile = string.Empty;

            OpenFileDialog dialog = new OpenFileDialog();
            dialog.InitialDirectory = @"C:\";

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                imageFile = dialog.FileName;
            }
            else if (dialog.ShowDialog() == DialogResult.Cancel)
            {
                return;
            }

            bitmap = (Bitmap)Image.FromFile(imageFile);
            Origin_Image.Image = bitmap;
            Origin_Image.Width = width_bar.Value * dot_size;
            Origin_Image.Height = height_bar.Value * dot_size;
            Console.WriteLine(bitmap.Width);
            CreateDotImage(bitmap);
        }
        private void width_bar_Scroll(object sender, EventArgs e)
        {
            Width_Num.Text = width_bar.Value.ToString();
            Origin_Image.Width = width_bar.Value * dot_size;
            if (bitmap != null)
                CreateDotImage(bitmap);
        }

        private void height_bar_Scroll(object sender, EventArgs e)
        {
            Height_Num.Text = height_bar.Value.ToString();
            Origin_Image.Height = height_bar.Value * dot_size;
            if (bitmap != null)
                CreateDotImage(bitmap);
        }

        private void CreateDotImage(Bitmap bitmap)
        {
            Bitmap pixelBitmap = new Bitmap(width_bar.Value * dot_size, height_bar.Value * dot_size);

            for (int x = 0; x < width_bar.Value; x++)
            {
                Color nearestColor = Color.Empty;
                for (int y = 0; y < height_bar.Value; y++)
                {
                    Color pixelColor = bitmap.GetPixel((bitmap.Width / width_bar.Value) * x, (bitmap.Height / height_bar.Value) * y);
                    double distance = 500.0;

                    foreach (string c in setColorList)
                    {
                        Color getColor = ColorTranslator.FromHtml(c);

                        double dbl_red = Math.Pow(Convert.ToDouble(getColor.R) - Convert.ToDouble(pixelColor.R), 2.0);
                        double dbl_green = Math.Pow(Convert.ToDouble(getColor.G) - Convert.ToDouble(pixelColor.G), 2.0);
                        double dbl_blue = Math.Pow(Convert.ToDouble(getColor.B) - Convert.ToDouble(pixelColor.B), 2.0);

                        double temp = Math.Sqrt(dbl_red + dbl_green + dbl_blue);

                        if (temp == 0.0)
                        {
                            nearestColor = getColor;
                            break;
                        }
                        else if (temp < distance)
                        {
                            distance = temp;
                            nearestColor = getColor;
                        }
                    }

                    for (int dx = 0; dx < 10; dx++)
                    {
                        for (int dy = 0; dy < 10; dy++)
                        {
                            pixelBitmap.SetPixel(x * dot_size + dx, y * dot_size + dy, nearestColor);
                        }
                    }
                }
            }
            Pixel_Image.Image = pixelBitmap;
        }
        private void btnColor_Click(object sender, EventArgs e)
        {
            var button = sender as Button;
            if (button.Text == "O")
            {
                button.Text = "X";
                Color btnColor = button.BackColor;
                foreach (string s in setColorList)
                {
                    Color sColor = ColorTranslator.FromHtml(s);
                    if(sColor == btnColor)
                    {
                        setColorList.Remove(s);
                        if (bitmap != null)
                            CreateDotImage(bitmap);
                        return;
                    }
                }
                return;
            }
            else if (button.Text == "X")
            {
                button.Text = "O";
                Color btnColor = button.BackColor;
                string hexColor = HexConverter(btnColor);
                if (!setColorList.Contains(hexColor))
                {
                    setColorList.Add(hexColor);
                }
                if (bitmap != null)
                    CreateDotImage(bitmap);
            }
        }

        private static string HexConverter(Color c)
        {
            return "#" + c.R.ToString("X2") + c.G.ToString("X2") + c.B.ToString("X2");
        }
    }
}
