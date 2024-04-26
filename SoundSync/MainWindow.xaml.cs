using Microsoft.Win32;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace SoundSync
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MediaPlayer player = new MediaPlayer();
        bool isPlaying = false;
        public MainWindow()
        {
            InitializeComponent();
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Update; //Cette fonction sera appelé chaque seconde (un callback)
            timer.Start();

            player.MediaEnded += Player_MediaEnded;
            player.Play();
        }

        private void Update(object sender, EventArgs e)
        {
            if (player.Source != null && player.NaturalDuration.HasTimeSpan)
            {
                progressBar.Minimum = 0;
                progressBar.Maximum = player.NaturalDuration.TimeSpan.TotalSeconds;
                progressBar.Value = player.Position.TotalSeconds;
            }
        }

        private void Player_MediaEnded(object sender, EventArgs e)
        {
            // Réinitialise la position de lecture au début de la piste
            player.Position = TimeSpan.Zero;
            // Reprend la lecture
            player.Play();
        }
        private void btnParcourir_Click(object sender, RoutedEventArgs e)
        {
            player.Stop();
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "Fichiers MP3 (*.mp3)|*.mp3|Tous les fichiers (*.*)|*.*";
            if (fileDialog.ShowDialog() == true)
            {
                

                //Je split le chemin pour recuperer que le nom du fichier
                string separateur = "\\";
                string[] titreTmp = fileDialog.FileName.Split(new string[] { separateur }, StringSplitOptions.None);

                //J'enleve l'extension 
                string [] titre = titreTmp[titreTmp.Length - 1].Split(new string[] { "." }, StringSplitOptions.None);
                
                
                if (titre[1] == "mp3")
                {
                    player.Open(new Uri(fileDialog.FileName));
                    player.Play();
                    //Je modifie le contenu de mon TextBox
                    filePath.Text = "Titre : " + titre[0];
                    isPlaying = true;
                }
                else
                {
                    isPlaying = false;
                    filePath.Text = "Aucun titre selectionné ...";
                    MessageBox.Show("Le fichier sélectionné n'est pas au format MP3. Veuillez sélectionner un fichier MP3 valide. 🎵", "Erreur de format de fichier 🚫", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                
            }
        }

        private void volume_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            player.Volume = volume.Value / 100;
        }

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            if(isPlaying)
            {
                btnPause.Content = "PLAY";
                player.Stop();
                
            }
            
        }

        private void btnPause_Click(object sender, RoutedEventArgs e)
        {
            if(isPlaying)
            {
                if (btnPause.Content == "PAUSE")
                {
                    player.Pause();
                    btnPause.Content = "PLAY";
                }
                else
                {
                    player.Play();
                    btnPause.Content = "PAUSE";
                }
            }
                
        }
    }
}