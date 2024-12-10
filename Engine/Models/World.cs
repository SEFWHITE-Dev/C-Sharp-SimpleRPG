using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Models
{
    public class World
    {
        // using.systems.collections.generic
        // 'collections' allows to store multiple objects
        // 'generic' allows to any type of object
        private List<Location> _locations = new List<Location>();

        //internal void AddLocation(int xCoordinate, int yCoordinate, string name, string description, string iamgeName)
        //{
        //    _locations.Add(new Location(xCoordinate, yCoordinate, name, description, $"pack://application:,,,/Engine;component/Images/Locations/{iamgeName}"));               
        //}

        internal void AddLocation(Location location)
        {
            _locations.Add(location);
        }


        public Location LocationAt(int xCoordinate, int yCoordinate)
        {
            foreach (Location loc in _locations) {
                if (loc.XCoordinate == xCoordinate && loc.YCoordinate == yCoordinate) return loc;
            }
            return null;
            
        }
    }
}
