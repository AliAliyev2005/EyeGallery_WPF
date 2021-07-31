using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using EyeGallery_WPF.Views.User_Controls;
using EyeGallery_WPF.Services;
using GalaSoft.MvvmLight.Command;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using System.Windows.Threading;
using System;

namespace EyeGallery_WPF.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        /// <summary>
        ///  Relay Commands
        /// </summary>
        public RelayCommand TilesCommand { get; set; }
        public RelayCommand SmallCommand { get; set; }
        public RelayCommand DetailsCommand { get; set; }
        public RelayCommand AddCommand { get; set; }
        public RelayCommand SaveCommand { get; set; }
        public RelayCommand NextCommand { get; set; }
        public RelayCommand PreviousCommand { get; set; }
        public RelayCommand<bool> ShowCommand { get; set; }
        public RelayCommand<ListBox> DoubleClickCommand { get; set; }

        /// <summary>
        /// User Control for Main Window
        /// </summary>
        private UserControl mainContent;
        public UserControl MainContent
        {
            get { return mainContent; }
            set { mainContent = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Observable Collection for Main Window
        /// </summary>
        private ObservableCollection<Models.Image> images;
        public ObservableCollection<Models.Image> Images
        {
            get { return images; }
            set { images = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Timer for Auto Show images
        /// </summary>
        public DispatcherTimer Timer { get; set; }

        /// <summary>
        /// Selected image for shows bigger in auto show
        /// </summary>
        private string imageNow;
        public string ImageNow
        {
            get { return imageNow; }
            set { imageNow = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// AutoShow image index for shows next or previous image 
        /// </summary>
        public int ImageIndex { get; set; }


        public MainViewModel()
        {
            // ObservableCollection Images for showing in folder 
            Images = new ObservableCollection<Models.Image>();
            Images = File.ReadJSON("images.json");
            
            // Default show images in TilesUC UserControl
            MainContent = new TilesUC();

            // Switch to TilesUC User Control
            TilesCommand = new RelayCommand(() => { MainContent = new TilesUC(); });

            // Switch to SmallIconsUC User Control
            SmallCommand = new RelayCommand(() => { MainContent = new SmallIconsUC(); });

            // Switch to DetailsUC User Control
            DetailsCommand = new RelayCommand(() => { MainContent = new DetailsUC(); });

            // Saves newly added image 
            SaveCommand = new RelayCommand(() => { File.WriteJSON(Images, "images.json"); });
            
            // Opens dialog for add new image
            AddCommand = new RelayCommand(AddImage);

            // Select image for show bigger in AutoShow User Control
            DoubleClickCommand = new RelayCommand<ListBox>(DoubleClick);

            // Shows next image
            NextCommand = new RelayCommand(NextClick);

            // Shows previous image
            PreviousCommand = new RelayCommand(PreviousClick);

            // Starts auto show images
            ShowCommand = new RelayCommand<bool>(Show);

            // Timer for auto show images
            Timer = new DispatcherTimer(DispatcherPriority.Render)
            {
                Interval = TimeSpan.FromSeconds(3)
            };
            Timer.Tick += (sender, args) => { NextClick(); };
        }

        /// <summary>
        /// Adds new image to Observable Collection Images
        /// </summary>
        public void AddImage()
        {
            System.Windows.Forms.OpenFileDialog op = new System.Windows.Forms.OpenFileDialog()
            {
                Title = "Select a picture",
                Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
                             "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
                             "Portable Network Graphic (*.png)|*.png"
            };

            if (op.ShowDialog().Equals(System.Windows.Forms.DialogResult.OK))
            {
                Images.Add(new Models.Image(op.FileName.Split('\\').Last(), op.FileName));
            }
        }

        /// <summary>
        /// Opens AutoShow User Control with selected Image
        /// </summary>
        /// <param name="listBox"></param>
        public void DoubleClick(ListBox listBox)
        {
            Models.Image item = listBox.SelectedItem as Models.Image;
            MainContent = new AutoShowUC();
            ImageNow = item.Source;
            ImageIndex = Images.IndexOf(item);
        }

        /// <summary>
        /// Shows next image in AutoShow User Control
        /// </summary>
        public void NextClick()
        {
            if (ImageIndex < Images.Count-1) ImageNow = Images[++ImageIndex].Source;
            else
            {
                ImageIndex = 0;
                ImageNow = Images[ImageIndex].Source;
            }
        }

        /// <summary>
        /// Shows previous image in AutoShow User Control
        /// </summary>
        public void PreviousClick()
        {
            if (ImageIndex > 0) ImageNow = Images[--ImageIndex].Source;
            else
            {
                ImageIndex = Images.Count - 1;
                ImageNow = Images[ImageIndex].Source;
            }
        }

        /// <summary>
        /// Enable / Disable toggle button witch starts / stops auto show images
        /// </summary>
        /// <param name="isChecked"></param>
        public void Show(bool isChecked)
        {
            if (isChecked) Timer.Start();
            else Timer.Stop();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
