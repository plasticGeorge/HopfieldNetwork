using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;

namespace HopfieldNetwork
{
    internal class Network
    {
        private int numOfNeur;
        private int maxImages;
        private int imagesCount;
        private float[,] weightsMatrix;

        public Network(int imgRes)
        {
            numOfNeur = (int)Math.Pow(imgRes, 2);
            maxImages = (int)(numOfNeur / (2 * Math.Log(numOfNeur, 2)));
            weightsMatrix = new float[numOfNeur, numOfNeur];
        }

        public void SaveNewImage(string imagePath)
        {
            if (imagesCount == maxImages)
            {
                Console.WriteLine("Network memory is full!");
                return;
            }
            Bitmap bm = new Bitmap(imagePath, true);
            int[] newImage = new int[bm.Width * bm.Height]; 
            for (int x = 0; x < bm.Height; x++)
            {
                for (int y = 0; y < bm.Width; y++)
                {
                    Color pixel = bm.GetPixel(x, y);
                    newImage[(x * bm.Width) + y] = (pixel.R > 0) ? (-1) : (1);
                }
            }

            for (int i = 0; i < newImage.Length; i++)
            {
                for (int j = 0; j < newImage.Length; j++)
                {
                    if(i == j)
                        continue;
                    weightsMatrix[i, j] += (float)(newImage[j] * newImage[i])/newImage.Length;
                }
            }

            imagesCount++;
        }

        public int[] SearchImage(string imagePath)
        {
            Bitmap bm = new Bitmap(imagePath, true);
            int[] searchImage = new int[bm.Width * bm.Height]; 
            for (int x = 0; x < bm.Height; x++)
            {
                for (int y = 0; y < bm.Width; y++)
                {
                    Color pixel = bm.GetPixel(x, y);
                    searchImage[(x * bm.Width) + y] = (pixel.R > 0) ? (-1) : (1);
                }
            }

            int[] oldSearchImage = null;
            do
            {
                oldSearchImage = (int[]) searchImage.Clone();
                for (int i = 0; i < numOfNeur; i++)
                {
                    float tempNeurValue = 0;
                    for (int j = 0; j < numOfNeur; j++)
                        tempNeurValue += searchImage[j] * weightsMatrix[i, j];
                    searchImage[i] = tempNeurValue < 0 ? -1 : 1;
                }
            } while (!searchImage.SequenceEqual(oldSearchImage));
            
            return searchImage;
        } 
    }

    internal class Program
    {
        public static void BitmapToImage(int[] bitmap, string savePath){
            Bitmap bm = new Bitmap((int)Math.Sqrt(bitmap.Length), (int)Math.Sqrt(bitmap.Length));
            for (int x = 0; x < bm.Height; x++)
            {
                for (int y = 0; y < bm.Width; y++)
                {
                    bm.SetPixel(x, y, bitmap[(x * bm.Width) + y] > 0 ? Color.Black: Color.White);
                }
            }
            
            bm.Save(savePath, ImageFormat.Png);
        }
        public static void Main(string[] args)
        {
            Network NNH = new Network(16);
            NNH.SaveNewImage(@"C:\Users\floppy\RiderProjects\HopfieldNetwork\HopfieldNetwork\images\0.png");
            NNH.SaveNewImage(@"C:\Users\floppy\RiderProjects\HopfieldNetwork\HopfieldNetwork\images\1.png");
            NNH.SaveNewImage(@"C:\Users\floppy\RiderProjects\HopfieldNetwork\HopfieldNetwork\images\2.png");
            NNH.SaveNewImage(@"C:\Users\floppy\RiderProjects\HopfieldNetwork\HopfieldNetwork\images\3.png");
            NNH.SaveNewImage(@"C:\Users\floppy\RiderProjects\HopfieldNetwork\HopfieldNetwork\images\4.png");
            NNH.SaveNewImage(@"C:\Users\floppy\RiderProjects\HopfieldNetwork\HopfieldNetwork\images\5.png");
            NNH.SaveNewImage(@"C:\Users\floppy\RiderProjects\HopfieldNetwork\HopfieldNetwork\images\6.png");
            NNH.SaveNewImage(@"C:\Users\floppy\RiderProjects\HopfieldNetwork\HopfieldNetwork\images\7.png");
            NNH.SaveNewImage(@"C:\Users\floppy\RiderProjects\HopfieldNetwork\HopfieldNetwork\images\8.png");
            NNH.SaveNewImage(@"C:\Users\floppy\RiderProjects\HopfieldNetwork\HopfieldNetwork\images\9.png");
            int[] resVector = NNH.SearchImage(@"C:\Users\floppy\RiderProjects\HopfieldNetwork\HopfieldNetwork\searchImages\5-template.png");
            BitmapToImage(resVector, @"C:\Users\floppy\RiderProjects\HopfieldNetwork\HopfieldNetwork\searchResult\5-result.png");
            Console.ReadKey();
        }
    }
}