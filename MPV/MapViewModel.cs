using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.Mapping;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Map = Esri.ArcGISRuntime.Mapping.Map;

namespace MPV
{
    /// <summary>
    /// Provides map data to an application
    /// </summary>
    public class MapViewModel : INotifyPropertyChanged
    {
        public MapViewModel()
        {
            Initialize();
        }

        private async Task Initialize()
        {
            string _ResourceFile = "Sample.mmpk";
            string _LocalFile = System.IO.Path.Combine(FileSystem.Current.AppDataDirectory, _ResourceFile);

            using Stream _ResourceStream = await FileSystem.Current.OpenAppPackageFileAsync(_ResourceFile);
            using FileStream _LocalStream = System.IO.File.OpenWrite(_LocalFile);

            using BinaryWriter _LocalWriter = new BinaryWriter(_LocalStream);

            using (BinaryReader _ResourceReader = new BinaryReader(_ResourceStream))
            {
                var _BytesRead = 0;

                int BufferSize = 1024;

                var _Buffer = new byte[BufferSize];

                using (_ResourceStream)
                {
                    do
                    {
                        _Buffer = _ResourceReader.ReadBytes(BufferSize);
                        _BytesRead = _Buffer.Count();
                        _LocalWriter.Write(_Buffer);
                    }

                    while (_BytesRead > 0);
                }
            }

            _LocalWriter.Close();
            _LocalWriter.Dispose();

            _LocalStream.Close();
            _LocalStream.Dispose();

            _ResourceStream.Close();
            _ResourceStream.Dispose();

            try
            {
                MobileMapPackage Package = await MobileMapPackage.OpenAsync(_LocalFile);
                await Package.LoadAsync();
                Map = Package.Maps.FirstOrDefault();
            }
            catch (Exception E)
            {
                await Application.Current.MainPage.DisplayAlert("DEBUG", E.Message, "OK");
            }
        }

        private Esri.ArcGISRuntime.Mapping.Map _map;

        /// <summary>
        /// Gets or sets the map
        /// </summary>
        
        public Esri.ArcGISRuntime.Mapping.Map Map
        {
            get => _map;
            set { _map = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Raises the <see cref="MapViewModel.PropertyChanged" /> event
        /// </summary>
        /// <param name="propertyName">The name of the property that has changed</param>
        
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
                 PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
