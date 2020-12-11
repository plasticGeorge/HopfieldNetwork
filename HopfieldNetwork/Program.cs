using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace HopfieldNetwork
{
    internal class Network
    {
        private int numOfNeur;
        private int maxImages;
        private int imagesCount;
        private int[,] weightsMatrix;

        public Network(int imgRes)
        {
            numOfNeur = (int)Math.Pow(imgRes, 2);
            maxImages = (int)(numOfNeur / (2 * Math.Log(numOfNeur, 2)));
            weightsMatrix = new int[numOfNeur, numOfNeur];
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
                for (int y = 0; y < bm.Width; y++)
                {
                    Color pixel = bm.GetPixel(x, y);
                    newImage[(x * bm.Width) + y] = (pixel.R > 0) ? (-1) : (1);
                }

            for (int i = 0; i < numOfNeur; i++)
                for (int j = 0; j < numOfNeur; j++)
                    if (i != j)
                        weightsMatrix[i, j] += newImage[i] * newImage[j];

            imagesCount++;
        }

        public int[] SearchImage(string imagePath)
        {
            Bitmap bm = new Bitmap(imagePath, true);
            int[] searchImage = new int[bm.Width * bm.Height]; 
            
            for (int x = 0; x < bm.Height; x++)
                for (int y = 0; y < bm.Width; y++)
                {
                    Color pixel = bm.GetPixel(x, y);
                    searchImage[(x * bm.Width) + y] = (pixel.R == 255) ? (-1) : (1);
                }

            int[] oldSearchImage;
            do
            {
                oldSearchImage = (int[]) searchImage.Clone();
                int[] na = new int[numOfNeur];
                for (int i = 0; i < numOfNeur; i++)
                {
                    //int tempNeurState = searchImage[i];
                    for (int j = 0; j < numOfNeur; j++)
                        na[i] += weightsMatrix[i, j] * searchImage[j];
                    if (na[i] < 0)
                        na[i] = -1;
                    else if(na[i] > 0)
                        na[i] = 1;
                }

                searchImage = (int[])na.Clone();
            } while (!isEqual(oldSearchImage, searchImage));

            return searchImage;
        }

        private bool isEqual(int[] arr1, int[] arr2)
        {
            if (arr1.Length != arr2.Length)
                return false;
            
            for (int i = 0; i < arr1.Length; i++)
                if (arr1[i] != arr2[i])
                    return false;

            return true;
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
            Network NNH = new Network(150);
            NNH.SaveNewImage(@"C:\Users\floppy\RiderProjects\HopfieldNetwork\HopfieldNetwork\images\1.png");
            NNH.SaveNewImage(@"C:\Users\floppy\RiderProjects\HopfieldNetwork\HopfieldNetwork\images\2.png");
            NNH.SaveNewImage(@"C:\Users\floppy\RiderProjects\HopfieldNetwork\HopfieldNetwork\images\3.png");
            NNH.SaveNewImage(@"C:\Users\floppy\RiderProjects\HopfieldNetwork\HopfieldNetwork\images\4.png");
            NNH.SaveNewImage(@"C:\Users\floppy\RiderProjects\HopfieldNetwork\HopfieldNetwork\images\5.png");
            
            BitmapToImage(NNH.SearchImage(@"C:\Users\floppy\RiderProjects\HopfieldNetwork\HopfieldNetwork\searchImages\s.png"), @"C:\Users\floppy\RiderProjects\HopfieldNetwork\HopfieldNetwork\searchResult\r.png");
        }
    }
}