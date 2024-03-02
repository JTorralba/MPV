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
            //            _map = new Map(SpatialReferences.WebMercator)
            //            {
            //                InitialViewpoint = new Viewpoint(new Envelope(-180, -85, 180, 85, SpatialReferences.Wgs84)),
            //#warning To use ArcGIS location services (including basemaps) specify your ArcGIS Developer API Key or require the user to sign in with an ArcGIS Identity.
            //                //Basemap = new Basemap(BasemapStyle.ArcGISStreets)
            //            };

            Initialize();
        }

        private async Task Initialize()
        {
            string FilePath = "C:\\Sample.mmpk";

            try
            {
                MobileMapPackage Package = await MobileMapPackage.OpenAsync(FilePath);
                await Package.LoadAsync();
                Map = Package.Maps.FirstOrDefault();
            }
            catch (Exception E)
            {
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
