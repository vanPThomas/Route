namespace RoutClassLibrary
{
    public class XRoute : IRoute
    {
        public Route Route { get; set; } = new Route();

        public void AddLocation(string location, double distance, bool isStop)
        {
            if (!string.IsNullOrWhiteSpace(location) &&distance >= 0 && Char.IsUpper(location[0]))
                Route.route.Add(Tuple.Create(distance, location, isStop));
            else
            {
                throw new RouteException();
            }
        }
        public double GetDistance()
        {
            double distance = 0;
            for (int i = 0; i < Route.route.Count; i++)
            {
                distance += Route.route[i].Item1;
            }
            return distance;
        }
        public double GetDistance(string startLocation, string endLocation)
        {
            int startLoc = -1;
            int endLoc = -1;
            double distance = -1;

            for (int i = 0; i < Route.route.Count; i++)
                if (Route.route[i].Item2 == startLocation)
                {
                    startLoc = i;
                    break;
                }

            for (int i = 0; i < Route.route.Count; i++)
                if (Route.route[i].Item2 == endLocation)
                {
                    endLoc = i;
                    break;
                }
            if (startLoc != -1 && endLoc != -1)
            {
                distance = 0;
                for (int i = startLoc; i < Route.route.Count; i++)
                {
                    if (i != startLoc)
                        distance += Route.route[i].Item1;
                    if (i == endLoc)
                        break;
                }
            }

            return distance;
        }
        public bool HasLocation(string location)
        {
            bool hasLocation = false;
            for (int i = 0; i < Route.route.Count; i++)
                if (Route.route[i].Item2 == location)
                {
                    hasLocation = true;
                    break;
                }
            return hasLocation;
        }
        public bool HasStop(string location)
        {
            bool hasStop = false;
            for (int i = 0; i < Route.route.Count; i++)
                if (Route.route[i].Item2 == location)
                {
                    hasStop = Route.route[i].Item3;
                    break;
                }
            return hasStop;
        }
        public void InsertLocation(string location, double distance, string fromLocation, bool isStop)
        {
            Route route = new Route();
            if (Route.route[Route.route.Count - 1].Item2 != fromLocation)
            {
                for (int i = 0; i < Route.route.Count; i++)
                {
                    if (i + 1 < Route.route.Count)
                    {
                        if (Route.route[i].Item2 == fromLocation)
                        {
                            if (i + 2 < Route.route.Count)
                            {
                                route.route.Add(Tuple.Create(Route.route[i].Item1, Route.route[i].Item2, Route.route[i].Item3));
                                route.route.Add(Tuple.Create(distance, location, isStop));
                                if (Route.route[Route.route.Count - 2].Item2 != fromLocation)
                                    route.route.Add(Tuple.Create(Route.route[i + 1].Item1 - distance, Route.route[i + 1].Item2, Route.route[i + 1].Item3));
                                i++;
                            }
                            else
                            {
                                route.route.Add(Tuple.Create(Route.route[i].Item1, Route.route[i].Item2, Route.route[i].Item3));
                                route.route.Add(Tuple.Create(distance, location, isStop));
                                route.route.Add(Tuple.Create(Route.route[i + 1].Item1 - distance, Route.route[i + 1].Item2, Route.route[i + 1].Item3));
                                i++;
                                break;
                            }
                        }
                        else
                            route.route.Add(Tuple.Create(Route.route[i].Item1, Route.route[i].Item2, Route.route[i].Item3));
                    }
                    else { break; }
                }
                if (Route.route[Route.route.Count - 1].Item2 != fromLocation && Route.route[Route.route.Count - 2].Item2 != fromLocation)
                    route.route.Add(Tuple.Create(Route.route[Route.route.Count - 1].Item1, Route.route[Route.route.Count - 1].Item2, Route.route[Route.route.Count - 1].Item3));
            }
            else
            {
                for (int i = 0; i < Route.route.Count; i++)
                {
                    route.route.Add(Tuple.Create(Route.route[i].Item1, Route.route[i].Item2, Route.route[i].Item3));
                }
                route.route.Add(Tuple.Create(distance, location, isStop));
            }
            try
            {
                if (Route.route.Count() == route.route.Count())
                {
                    Console.WriteLine("aaaa");

                    throw new RouteException();
                }
                else
                    Route = route;
            }
            catch (Exception ex)
            {
                RouteException.ExceptionDetails(ex);
            }
        }
        public void RemoveLocation(string location)
        {
            Route route = new Route();
            for (int i = 0; i < Route.route.Count; i++)
            {
                if (i + 1 < Route.route.Count)
                {
                    if (Route.route[i].Item2 == location && Route.route.Count - 1 != i)
                    {
                        route.route.Add(Tuple.Create(Route.route[i + 1].Item1 + Route.route[i].Item1, Route.route[i + 1].Item2, Route.route[i + 1].Item3));
                        i++;
                    }
                    else if (Route.route[i].Item2 != location)
                        route.route.Add(Tuple.Create(Route.route[i].Item1, Route.route[i].Item2, Route.route[i].Item3));
                }
                else if (Route.route[i].Item2 == location && Route.route.Count - 1 == i)
                    break;
                else
                    route.route.Add(Tuple.Create(Route.route[i].Item1, Route.route[i].Item2, Route.route[i].Item3));
            }
            Route = route;
        }
        public void SetDistance(double distance, string location1, string location2)
        {
            Route route = new Route();
            for (int i = 0; i < Route.route.Count; i++)
            {
                if (Route.route[i].Item2 == location1 && Route.route[i + 1].Item2 == location2)
                {
                    route.route.Add(Tuple.Create(Route.route[i].Item1, Route.route[i].Item2, Route.route[i].Item3));
                    i++;
                    route.route.Add(Tuple.Create(distance, Route.route[i].Item2, Route.route[i].Item3));
                }
                else
                    route.route.Add(Tuple.Create(Route.route[i].Item1, Route.route[i].Item2, Route.route[i].Item3));
            }
            Route = route;
        }
        public (string start, List<(double distance, string location)>) ShowFullRoute()
        {
            List<(double distance, string location)> route = new List<(double distance, string location)>();
            for (int i = 0; i < Route.route.Count; i++)
                route.Add((Route.route[i].Item1, Route.route[i].Item2));

            return (route[0].Item2, route);
        }
        public (string start, List<(double distance, string location)>) ShowFullRoute(string startLocation, string endLocation)
        {
            List<(double distance, string location)> route = new List<(double distance, string location)>();
            int startLoc = 0;
            int endLoc = 0;
            for (int i = 0; i < Route.route.Count; i++)
            {
                if (Route.route[i].Item2 == startLocation)
                    startLoc = i;
                else if (Route.route[i].Item2 == endLocation)
                    endLoc = i;
            }
            for (int i = startLoc; i < endLoc; i++)
            {
                if (i == startLoc)
                    route.Add((0, Route.route[i].Item2));
                else
                    route.Add((Route.route[i].Item1, Route.route[i].Item2));
            }
            route.Add((Route.route[endLoc].Item1, Route.route[endLoc].Item2));
            return (Route.route[startLoc].Item2, route);
        }
        public List<string> ShowLocations()
        {
            List<string> locations = new List<string>();
            for (int i = 0; i < Route.route.Count(); i++)
                locations.Add(Route.route[i].Item2);
            return locations;
        }

        public (string start, List<(double distance, string location)>) ShowRoute()
        {
            List<(double distance, string location)> route = new List<(double distance, string location)>();
            for (int i = 0; i < Route.route.Count; i++)
                if (Route.route[i].Item3)
                    route.Add((Route.route[i].Item1, Route.route[i].Item2));

            return (Route.route[0].Item2, route);
        }
        public (string start, List<(double distance, string location)>) ShowRoute(string startLocation, string endLocation)
        {
            List<(double distance, string location)> route = new List<(double distance, string location)>();
            int startLoc = 0;
            int endLoc = 0;
            for (int i = 0; i < Route.route.Count; i++)
            {
                if (Route.route[i].Item2 == startLocation)
                    startLoc = i;
                else if (Route.route[i].Item2 == endLocation)
                    endLoc = i;
            }
            for (int i = startLoc; i < endLoc; i++)
            {
                if (i == startLoc)
                    route.Add((0, Route.route[i].Item2));
                else if (Route.route[i].Item3)
                    route.Add((Route.route[i].Item1, Route.route[i].Item2));
            }
            route.Add((Route.route[endLoc].Item1, Route.route[endLoc].Item2));
            return (Route.route[startLoc].Item2, route);
        }

        public List<string> ShowStops()
        {
            List<string> stops = new List<string>();
            for (int i = 0; i < Route.route.Count(); i++)
                if (Route.route[i].Item3)
                    stops.Add(Route.route[i].Item2);
            return stops;
        }

        public void UpdateLocation(string location, string newName, bool isStop)
        {
            List<string> locations = new List<string>();
            List<double> distances = new List<double>();
            List<bool> isStops = new List<bool>();
            for (int i = 0; i < Route.route.Count; i++)
            {
                if (Route.route[i].Item2 == location)
                {
                    locations.Add(newName);
                    distances.Add(Route.route[i].Item1);
                    isStops.Add(isStop);
                }
                else
                {
                    locations.Add(Route.route[i].Item2);
                    distances.Add(Route.route[i].Item1);
                    isStops.Add(Route.route[i].Item3);
                }
            }
            Route = RouteFactory.BuildRoute(locations, isStops, distances).Route;
        }
    }
}