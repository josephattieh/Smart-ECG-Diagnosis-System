using System.IO;
using UnityEngine;
namespace Models
{
    public class ECGResultModel : MonoBehaviour
    {
        public static ECGResultModel instance;
         
        private void Awake()
        {
            instance = this;
        }
        public Texture LoadECGTexture()
        {
            //Create an array of file paths from which to choose
            string imagePath = Application.streamingAssetsPath + Config.ECG_OUTPUT_LOC + Config.ECG_IMAGE_NAME;  //Get path of folder
           
            //Converts desired path into byte array
            byte[] pngBytes = System.IO.File.ReadAllBytes(imagePath);

            //Creates texture and loads byte array data to create image
            Texture2D tex = new Texture2D(320, 240);
            tex.LoadImage(pngBytes);

            //Creates a new Sprite based on the Texture2D
            //Sprite fromTex = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);

            return tex;
        }

    }
}