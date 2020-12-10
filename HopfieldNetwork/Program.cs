using System;
using System.Drawing;

namespace HopfieldNetwork
{
    internal class Network
    {
        private int numOfNeur;
        private int maxImages;
        private int imagesCount;
        private int[,] weightsMatrix;

        public Network(int numOfNeur)
        {
            this.numOfNeur = numOfNeur;
            maxImages = (int)(numOfNeur / (2 * Math.Log(numOfNeur, 2)));
            weightsMatrix = new int[numOfNeur, numOfNeur];
        }

        public void SaveNewImage()
        {
            if (imagesCount == maxImages)
            {
                Console.WriteLine("Network memory is full!");
                return;
            }
            Bitmap bm = new Bitmap(@"C:\Users\floppy\RiderProjects\HopfieldNetwork\HopfieldNetwork\images\0.png", true);
            int[] res = new int[bm.Width * bm.Height]; 
            for (int x = 0; x < bm.Height; x++)
            {
                for (int y = 0; y < bm.Width; y++)
                {
                    Color pixel = bm.GetPixel(x, y);
                    res[(x * bm.Width) + y] = (pixel.R > 0) ? (-1) : (1);
                }
            }

            for (int i = 0; i < res.Length; i++)
            {
                for (int j = 0; j < res.Length; j++)
                {
                    if(i == j)
                        continue;
                    weightsMatrix[i, j] += res[j] * res[i];
                }
            }

            imagesCount++;
        }

        public void SearchImage()
        {
            
        }
    }
    internal class Program
    {
        public static void Main(string[] args)
        {
            
        }
    }
}