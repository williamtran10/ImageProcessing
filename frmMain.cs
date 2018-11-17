using System;
using System.Drawing;
using System.Windows.Forms;

//William Tran
//October 1, 2018
//Image Processing
//Takes a picture from the user and can apply different processes to it

namespace ImageProcessing
{
    public partial class frmMain : Form
    {
        private Color[,] original; //original picture - never change values stored in this array
        private Color[,] transformedPic;  //transformed picture that is shown

        public frmMain()
        {
            InitializeComponent();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            //this method draws the transformed picture

            base.OnPaint(e);
            Graphics g = e.Graphics;

            //only draw if picture is transformed
            if (transformedPic != null)
            {
                //get height and width of the transfrormedPic array
                int height = transformedPic.GetUpperBound(0) + 1;
                int width = transformedPic.GetUpperBound(1) + 1;

                //create a new Bitmap to be dispalyed on the form
                Bitmap newBmp = new Bitmap(width, height);
                for (int i = 0; i < height; i++)
                {
                    for (int j = 0; j < width; j++)
                    {
                        //loop through each element transformedPic and set the 
                        //colour of each pixel in the bitmalp
                        newBmp.SetPixel(j, i, transformedPic[i, j]);
                    }
                }
                //call DrawImage to draw the bitmap
                g.DrawImage(newBmp, 0, menuStrip1.Height, width, height);
            }
        }


        private void mnuFileOpen_Click(object sender, EventArgs e)
        {
            //reads in a picture file and stores it in an array

            //try catch handles any errors for invalid picture files
            try
            {

                //open the file dialog to select a picture file

                OpenFileDialog fd = new OpenFileDialog();

                //create a bitmap to store the file in
                Bitmap bmp;

                if (fd.ShowDialog() == DialogResult.OK)
                {
                    //store the selected file into a bitmap
                    bmp = new Bitmap(fd.FileName);

                    //create the arrays that store the colours for the image
                    //the size of the arrays is based on the height and width of the bitmap
                    //initially both the original and transformedPic will be identical
                    original = new Color[bmp.Height, bmp.Width];
                    transformedPic = new Color[bmp.Height, bmp.Width];

                    //load each color into a color array
                    for (int i = 0; i < bmp.Height; i++)//each row
                    {
                        for (int j = 0; j < bmp.Width; j++)//each column
                        {
                            //assign the colour in the bitmap to the array
                            original[i, j] = bmp.GetPixel(j, i);
                            transformedPic[i, j] = original[i, j];
                        }
                    }
                    //this will cause the form to be redrawn and OnPaint() will be called
                    this.Refresh();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Loading Picture File. \n" + ex.Message);
            }
            
        }

        private void mnuProcessDarken_Click(object sender, EventArgs e)
        {
            if (transformedPic != null)
            {
                //store RGB intensity
                int Red, Green, Blue;

                int Height = transformedPic.GetLength(0);
                int Width = transformedPic.GetLength(1);

                //loop through each color element in tranformedPic
                for (int i = 0; i < Height; i++)
                {
                    for (int j = 0; j < Width; j++)
                    {
                        //subract 10 from each color value (capped at 0)
                        Red = transformedPic[i, j].R - 10;
                        if (Red < 0) Red = 0;
                        Blue = transformedPic[i, j].B - 10;
                        if (Blue < 0) Blue = 0;
                        Green = transformedPic[i, j].G - 10;
                        if (Green < 0) Green = 0;

                        //combine new values and assign to transformedPic
                        transformedPic[i, j] = Color.FromArgb(Red, Green, Blue);
                    }
                }
                this.Refresh();
            }
        }

        private void mnuProcessInvert_Click(object sender, EventArgs e)
        {
            if (transformedPic != null)
            {
                //store RGB intensity
                int Red, Green, Blue;

                int Height = transformedPic.GetLength(0);
                int Width = transformedPic.GetLength(1);

                //loop through each color element in tranformedPic
                for (int i = 0; i < Height; i++)
                {
                    for (int j = 0; j < Width; j++)
                    {
                        //invert by subtracting each color value from 255
                        Red = 255 - transformedPic[i, j].R;
                        Blue = 255 - transformedPic[i, j].B;
                        Green = 255 - transformedPic[i, j].G;

                        //combine new values and assign to transformedPic
                        transformedPic[i, j] = Color.FromArgb(Red, Green, Blue);
                    }
                }
                this.Refresh();
            }
        }

        private void mnuProcessWhiten_Click(object sender, EventArgs e)
        {
            if (transformedPic != null)
            {
                //store RGB intensity
                int Red, Green, Blue;

                int Height = transformedPic.GetLength(0);
                int Width = transformedPic.GetLength(1);

                //loop through each pixel in tranformedPic
                for (int i = 0; i < Height; i++)
                {
                    for (int j = 0; j < Width; j++)
                    {
                        //add 10 from each color value (capped at 255)
                        Red = transformedPic[i, j].R + 10;
                        if (Red > 255) Red = 255;
                        Blue = transformedPic[i, j].B + 10;
                        if (Blue > 255) Blue = 255;
                        Green = transformedPic[i, j].G + 10;
                        if (Green > 255) Green = 255;

                        //combine new values and assign to transformedPic
                        transformedPic[i, j] = Color.FromArgb(Red, Green, Blue);
                    }
                }

                this.Refresh();
            }

        }

        private void mnuProcessReset_Click(object sender, EventArgs e)
        {
            if (transformedPic != null)
            {
                //reset size to original
                transformedPic = new Color[original.GetLength(0), original.GetLength(1)];

                int Height = transformedPic.GetLength(0);
                int Width = transformedPic.GetLength(1);

                //loop through each color element in tranformedPic
                for (int i = 0; i < Height; i++)
                {
                    for (int j = 0; j < Width; j++)
                    {
                        //make each element the same as the original's
                        transformedPic[i, j] = original[i, j];
                    }
                }
                this.Refresh();
            }
        }

        private void mnuProcessFlipX_Click(object sender, EventArgs e)
        {
            if (transformedPic != null)
            {
                //store height and width
                int Height = transformedPic.GetLength(0);
                int Width = transformedPic.GetLength(1);

                Color Temp = Color.Empty; //stores colors during swapping so that data isn't lost

                //loop through each color element in transformedPic on left half of the picture (Width / 2)
                for (int i = 0; i < Height; i++)
                {
                    for (int j = 0; j < (Width / 2); j++)
                    {
                        //swapping color elements using Temp:
                        Temp = transformedPic[i, Width - 1 - j];
                        transformedPic[i, Width - 1 - j] = transformedPic[i, j];
                        transformedPic[i, j] = Temp;
                    }
                }
                this.Refresh();
            }
        }

        private void mnuProcessFlipY_Click(object sender, EventArgs e)
        {
            if (transformedPic != null)
            {
                int Height = transformedPic.GetLength(0);
                int Width = transformedPic.GetLength(1);

                Color Temp = Color.Empty; //stores colors during swapping so that data isn't lost

                //loop through each pixel in transformedPic in the top half of the picture (Height / 2)
                for (int i = 0; i < (Height / 2); i++)
                {
                    for (int j = 0; j < Width; j++)
                    {
                        //swapping color elements using Temp:
                        Temp = transformedPic[Height - 1 - i, j];
                        transformedPic[Height - 1 - i, j] = transformedPic[i, j];
                        transformedPic[i, j] = Temp;
                    }
                }
                this.Refresh();
            }
        }

        private void mnuProcessMirrorH_Click(object sender, EventArgs e)
        {
            if (transformedPic != null)
            {
                //store height and width
                int Height = transformedPic.GetLength(0);
                int Width = transformedPic.GetLength(1);

                Color[,] TempTransformedPic = new Color[Height, Width]; //stores whole array during resizing so that data isn't lost

                //copy TransformedPic to Temp
                for (int i = 0; i < Height; i++)
                {
                    for (int j = 0; j < Width; j++)
                    {
                        TempTransformedPic[i, j] = transformedPic[i, j];
                    }
                }

                //resize transformedPic so that it has double the width
                transformedPic = new Color[Height, Width * 2];

                //loop through each color element in Temp and copy to original place and mirrored place
                for (int i = 0; i < Height; i++)
                {
                    for (int j = 0; j < Width; j++)
                    {
                        transformedPic[i, j] = TempTransformedPic[i, j];
                        //the leftmost column is mirrored in the rightmost column (Width * 2 - 1) and the mirror image is made from right to left (-j)
                        transformedPic[i, Width * 2 - 1 - j] = TempTransformedPic[i, j];
                    }
                }
                this.Refresh();
            }
        }

        private void mnuProcessMirrorV_Click(object sender, EventArgs e)
        {
            if (transformedPic != null)
            {
                int Height = transformedPic.GetLength(0);
                int Width = transformedPic.GetLength(1);
                
                Color[,] TempTransformedPic = new Color[Height, Width]; //stores whole array during resizing so that data isn't lost

                //copy TransformedPic to Temp
                for (int i = 0; i < Height; i++)
                {
                    for (int j = 0; j < Width; j++)
                    {
                        TempTransformedPic[i, j] = transformedPic[i, j];
                    }
                }

                //resize transformedPic so that it has double the height
                transformedPic = new Color[Height * 2, Width];

                //loop through each color element in Temp and copy to original place and mirrored place
                for (int i = 0; i < Height; i++)
                {
                    for (int j = 0; j < Width; j++)
                    {
                        transformedPic[i, j] = TempTransformedPic[i, j];
                        //the top row is mirrored in the bottom row (Height * 2 - 1) and the mirror image is made from bottom to top (-i)
                        transformedPic[Height * 2 - 1 - i, j] = TempTransformedPic[i, j];
                    }
                }
                this.Refresh();
            }
        }

        private void mnuProcessScale50_Click(object sender, EventArgs e)
        {
            //code to scale by 50% if more than 1x1
            if (transformedPic != null && transformedPic.GetLength(0) > 1 && transformedPic.GetLength(1) > 1)
            {
                int Height = transformedPic.GetLength(0);
                int Width = transformedPic.GetLength(1);

                Color[,] TempTransformedPic = new Color[Height, Width]; //stores whole array during resizing so that data isn't lost

                //copy TransformedPic to Temp
                for (int i = 0; i < Height; i++)
                {
                    for (int j = 0; j < Width; j++)
                    {
                        TempTransformedPic[i, j] = transformedPic[i, j];
                    }
                }

                //make height and width an even number before dividing both by 2
                if (Height % 2 == 1) Height++;
                if (Width % 2 == 1) Width++;
                transformedPic = new Color[Height / 2, Width / 2];


                //loop through every other row and every other column
                for (int i = 0; i < Height; i += 2)
                {
                    for (int j = 0; j < Width; j += 2)
                    {
                        //copy the pixel in Temp to its place in the new smaller array, where its index values are halved
                        transformedPic[i / 2, j / 2] = TempTransformedPic[i, j];
                    }
                }
                this.Refresh();
            }
        }

        private void mnuProcessScale200_Click(object sender, EventArgs e)
        {
            if (transformedPic != null)
            {
                int Height = transformedPic.GetLength(0);
                int Width = transformedPic.GetLength(1);

                Color[,] TempTransformedPic = new Color[Height, Width]; //stores whole array during resizing so that data isn't lost

                //copy TransformedPic to Temp
                for (int i = 0; i < Height; i++)
                {
                    for (int j = 0; j < Width; j++)
                    {
                        TempTransformedPic[i, j] = transformedPic[i, j];
                    }
                }

                //resize tranformedPic to have double height and width
                transformedPic = new Color[Height * 2, Width * 2];

                //loop through each pixel in Temp and copy it 4 times into transformedPic
                for (int i = 0; i < Height; i++)
                {
                    for (int j = 0; j < Width; j++)
                    {
                        //the index values of the inital pixel are doubled and the pixel to the right, bottom, and bottom right are copies of it.
                        transformedPic[i * 2, j * 2] = TempTransformedPic[i, j];
                        transformedPic[i * 2 + 1, j * 2] = TempTransformedPic[i, j];
                        transformedPic[i * 2, j * 2 + 1] = TempTransformedPic[i, j];
                        transformedPic[i * 2 + 1, j * 2 + 1] = TempTransformedPic[i, j];
                    }
                }

                this.Refresh();
            }
        }

        private void mnuProcessRotateCW_Click(object sender, EventArgs e)
        {
            //code to rotate 90 degrees clockwise
            if (transformedPic != null)
            {
                int Height = transformedPic.GetLength(0);
                int Width = transformedPic.GetLength(1);

                Color[,] TempTransformedPic = new Color[Height, Width]; //stores whole array during resizing so that data isn't lost

                //copy TransformedPic to Temp
                for (int i = 0; i < Height; i++)
                {
                    for (int j = 0; j < Width; j++)
                    {
                        TempTransformedPic[i, j] = transformedPic[i, j];
                    }
                }

                //resize transformed array swapping height and width
                transformedPic = new Color[Width, Height];

                //loop through each pixel in Temp and copy its corresponding place in transformedPic
                for (int i = 0; i < Height; i++)
                {
                    for (int j = 0; j < Width; j++)
                    {
                        transformedPic[j, Height - 1 - i] = TempTransformedPic[i, j];
                    }
                }

                this.Refresh();
            }
        }

        private void mnuProcessRotateCCW_Click(object sender, EventArgs e)
        {
            //code to rotate 90 degrees counter-clockwise
            if (transformedPic != null)
            {
                //store height and width
                int Height = transformedPic.GetLength(0);
                int Width = transformedPic.GetLength(1);

                Color[,] TempTransformedPic = new Color[Height, Width]; //stores whole array during resizing so that data isn't lost

                //copy TransformedPic to Temp
                for (int i = 0; i < Height; i++)
                {
                    for (int j = 0; j < Width; j++)
                    {
                        TempTransformedPic[i, j] = transformedPic[i, j];
                    }
                }

                //resize transformed array swapping height and width
                transformedPic = new Color[Width, Height];

                //loop through each pixel in Temp and copy its corresponding place in transformedPic
                for (int i = 0; i < Height; i++)
                {
                    for (int j = 0; j < Width; j++)
                    {
                        transformedPic[Width - 1 - j, i] = TempTransformedPic[i, j];
                    }
                }

                this.Refresh();
            }
        }

        private void mnuProcessBlur_Click(object sender, EventArgs e)
        {
            if (transformedPic != null)
            {
                int Height = transformedPic.GetLength(0);
                int Width = transformedPic.GetLength(1);

                double Red, Green, Blue;
                int PixelsCounted;
                bool TopIsValid, RightIsValid, BottomIsValid, LeftIsValid; //stores info on what pixels border this one

                Color[,] TempTransformedPic = new Color[Height, Width]; //temporarily stores transformedPic so that pixels that are already blurred dont affect pixels that still need to be blurred

                //copy transformed to Temp
                for (int i = 0; i < Height; i++)
                {
                    for (int j = 0; j < Width; j++)
                    {
                        TempTransformedPic[i, j] = transformedPic[i, j];
                    }
                }

                //loop through each pixel in Temp and calculate and assign its blurred value to transformedPic
                for (int i = 0; i < Height; i++)
                {
                    for (int j = 0; j < Width; j++)
                    {
                        //reset values
                        Red = 0;
                        Green = 0;
                        Blue= 0;
                        PixelsCounted = 1;

                        //find out which pixels are valid
                        if (i > 0) TopIsValid = true;
                        else TopIsValid = false;
                        if (j > 0) LeftIsValid = true;
                        else LeftIsValid = false;
                        if (i < Height - 1) BottomIsValid = true;
                        else BottomIsValid = false;
                        if (j < Width - 1) RightIsValid = true;
                        else RightIsValid = false;

                        //automatically include the pixel's own rgb values
                        Red += transformedPic[i, j].R;
                        Green += transformedPic[i, j].G;
                        Blue += transformedPic[i, j].B;

                        if (TopIsValid) //top
                        {
                            PixelsCounted++;
                            Red += TempTransformedPic[i - 1, j].R;
                            Green += TempTransformedPic[i - 1, j].G;
                            Blue += TempTransformedPic[i - 1, j].B;

                            if (RightIsValid) //top right
                            {
                                PixelsCounted++;
                                Red += TempTransformedPic[i - 1, j + 1].R;
                                Green += TempTransformedPic[i - 1, j + 1].G;
                                Blue += TempTransformedPic[i - 1, j + 1].B;
                            }
                            if (LeftIsValid) //top left
                            {
                                PixelsCounted++;
                                Red += TempTransformedPic[i - 1, j - 1].R;
                                Green += TempTransformedPic[i - 1, j - 1].G;
                                Blue += TempTransformedPic[i - 1, j - 1].B;
                            }
                        }
                        if (BottomIsValid) //Bottom
                        {
                            PixelsCounted++;
                            Red += TempTransformedPic[i + 1, j].R;
                            Green += TempTransformedPic[i + 1, j].G;
                            Blue += TempTransformedPic[i + 1, j].B;

                            if (RightIsValid) //Bottom right
                            {
                                PixelsCounted++;
                                Red += TempTransformedPic[i + 1, j + 1].R;
                                Green += TempTransformedPic[i + 1, j + 1].G;
                                Blue += TempTransformedPic[i + 1, j + 1].B;
                            }
                            if (LeftIsValid) //Bottom left
                            {
                                PixelsCounted++;
                                Red += TempTransformedPic[i + 1, j - 1].R;
                                Green += TempTransformedPic[i + 1, j - 1].G;
                                Blue += TempTransformedPic[i + 1, j - 1].B;
                            }
                        }
                        if(RightIsValid) //right
                        {
                            PixelsCounted++;
                            Red += TempTransformedPic[i, j + 1].R;
                            Green += TempTransformedPic[i, j + 1].G;
                            Blue += TempTransformedPic[i, j + 1].B;
                        }
                        if (LeftIsValid) //left
                        {
                            PixelsCounted++;
                            Red += TempTransformedPic[i, j - 1].R;
                            Green += TempTransformedPic[i, j - 1].G;
                            Blue += TempTransformedPic[i, j - 1].B;
                        }

                        //the average is the sum divided by how many pixels were counted
                        //add 0.5 so that it is rounded properly when truncated into int
                        Red = Red / PixelsCounted + 0.5;
                        Green = Green / PixelsCounted + 0.5;
                        Blue = Blue / PixelsCounted + 0.5;
                        
                        //recombine color based on new average values and assign to transformPic
                        transformedPic[i, j] = Color.FromArgb((int)Red, (int)Green, (int)Blue);
                    }
                }

                this.Refresh();
            }
        }


    }
}
