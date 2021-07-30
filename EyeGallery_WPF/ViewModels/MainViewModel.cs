using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using EyeGallery_WPF.Views.User_Controls;
using EyeGallery_WPF.Views;
using System.Windows.Controls;
using EyeGallery_WPF.Models;
using EyeGallery_WPF.Services;
using GalaSoft.MvvmLight.Command;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media.Imaging;

namespace EyeGallery_WPF.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public RelayCommand TilesCommand { get; set; }
        public RelayCommand SmallCommand { get; set; }
        public RelayCommand DetailsCommand { get; set; }
        public RelayCommand AddCommand { get; set; }

        private System.Windows.Controls.UserControl mainContent;
        public System.Windows.Controls.UserControl MainContent
        {
            get { return mainContent; }
            set { mainContent = value; OnPropertyChanged(); }
        }

        private List<Models.Image> images;
        public List<Models.Image> Images
        {
            get { return images; }
            set { images = value; OnPropertyChanged(); }
        }

        public MainViewModel()
        {
            Images = new List<Models.Image>();
            Images = File.ReadJSON("images.json");
            MainContent = new TilesUC();

            TilesCommand = new RelayCommand(() => { MainContent = new TilesUC(); });
            SmallCommand = new RelayCommand(() => { MainContent = new SmallIconsUC(); });
            DetailsCommand = new RelayCommand(() => { MainContent = new DetailsUC(); });
            AddCommand = new RelayCommand(AddImage);
        }

        public void AddImage()
        {
            OpenFileDialog op = new OpenFileDialog
            {
                Title = "Select a picture",
                Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
                             "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
                             "Portable Network Graphic (*.png)|*.png"
            };
            op.ShowDialog();
            Images.Add(new Models.Image(op.FileName.Split('\\').Last(), op.FileName));
            File.WriteJSON(Images, "images.json");
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
