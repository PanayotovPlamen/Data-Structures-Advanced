using System;
using System.Collections.Generic;
using System.Linq;

namespace Exam.MoovIt
{
    public class MoovIt : IMoovIt
    {
        private Dictionary<string, Route> routesById = new Dictionary<string, Route>();

        public int Count => this.routesById.Count;

        public void AddRoute(Route route)
        {
            //if (this.routesById.ContainsKey(route.Id))
            //{
            //    throw new ArgumentException();
            //}

            if (this.Contains(route))
            {
                throw new ArgumentException();
            }

            this.routesById.Add(route.Id, route);
        }

        public void ChooseRoute(string routeId)
        {
            if (!this.routesById.ContainsKey(routeId))
            {
                throw new ArgumentException();
            }

            this.routesById[routeId].Popularity++;
        }

        public bool Contains(Route route)
        {
            
            if (this.routesById.ContainsKey(route.Id))
            {
                return true;
            }
            else
            {
                foreach (var item in routesById.Values)
                {
                    if (item.Distance == route.Distance && item.LocationPoints[0] == route.LocationPoints[0] && item.LocationPoints[item.LocationPoints.Count - 1] == route.LocationPoints[route.LocationPoints.Count - 1])
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public IEnumerable<Route> GetFavoriteRoutes(string destinationPoint)
        {
            return this.routesById.Values.Where(x => x.IsFavorite && x.LocationPoints.Contains(destinationPoint) && x.LocationPoints[0] != destinationPoint).OrderBy(x => x.Distance).ThenByDescending(x => x.Popularity);

            //var favouriteRoutes = this.routesById.Values.Where(x => x.IsFavorite);

            //var favouriteRoutesByLocation = favouriteRoutes.Where(x => x.LocationPoints.Contains(destinationPoint) && x.LocationPoints[0] != destinationPoint);

            //return favouriteRoutesByLocation.OrderBy(x => x.Distance).ThenByDescending(x => x.Popularity);
        }

        public Route GetRoute(string routeId)
        {
            if (!this.routesById.ContainsKey(routeId))
            {
                throw new ArgumentException();
            }

            return this.routesById[routeId];
        }

        public IEnumerable<Route> GetTop5RoutesByPopularityThenByDistanceThenByCountOfLocationPoints()
        {
            return this.routesById.Values.OrderByDescending(x => x.Popularity).ThenBy(x => x.Distance).ThenBy(x => x.LocationPoints.Count).Take(5);
        }

        public void RemoveRoute(string routeId)
        {
            if (!this.routesById.ContainsKey(routeId))
            {
                throw new ArgumentException();
            }

            this.routesById.Remove(routeId);
        }

        public IEnumerable<Route> SearchRoutes(string startPoint, string endPoint)
        {
            List<Route> result = new List<Route>();

            foreach (var item in this.routesById.Values)
            {
                int counter = 0;

                bool isStarted = false;

                foreach (var city in item.LocationPoints)
                {
                    if (city == endPoint)
                    {
                        if (isStarted && counter == 0)
                        {
                            counter++;
                        }

                        break;
                    }

                    if (isStarted)
                    {
                        counter++;
                    }

                    if (city == startPoint)
                    {
                        isStarted = true;
                    }                                        
                }

                if (counter > 0)
                {
                    item.NumberOfDesiredLocations = counter;

                    result.Add(item);
                }
            }

            return result.OrderBy(x => x.IsFavorite).ThenBy(x => x.NumberOfDesiredLocations).ThenByDescending(x => x.Popularity);
        }
    }
}
