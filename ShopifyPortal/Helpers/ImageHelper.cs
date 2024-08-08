namespace ShopifyPortal.Helpers
{
    public class ImageHelper
    {
        public static bool VerifyImageFileExtension(string extension)
        {
            if (extension == ".jpg" || extension == ".jpeg" || extension == ".png")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
