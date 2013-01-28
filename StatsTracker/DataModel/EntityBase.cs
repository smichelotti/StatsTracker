using Newtonsoft.Json;
using StatsTracker.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace StatsTracker.Data
{
    public abstract class EntityBase : BindableBase
    {
        public EntityBase(string imagePath)
        {
            this.ImagePath = imagePath;
        }

        //private static Uri _baseUri = new Uri("ms-appx:///");
        private static Uri _baseUri = new Uri("ms-appdata:///local/");

        private ImageSource _image = null;
        
        [JsonIgnore]
        public ImageSource Image
        {
            get
            {
                if (this._image == null && this.ImagePath != null)
                {
                    if (this.ImagePath.StartsWith("ms-appx"))
                    {
                        this._image = new BitmapImage(new Uri(this.ImagePath));
                    }
                    else
                    {
                        this._image = new BitmapImage(new Uri(_baseUri, this.ImagePath));
                    }
                }
                return this._image;
            }

            set
            {
                this.ImagePath = null;
                this.SetProperty(ref this._image, value);
            }
        }

        public string ImagePath { get; set; }

        public void SetImage(String path)
        {
            this._image = null;
            this.ImagePath = path;
            this.OnPropertyChanged("Image");
        }
    }
}
