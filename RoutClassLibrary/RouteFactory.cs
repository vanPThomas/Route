using System.Globalization;

namespace RoutClassLibrary
{
    public class RouteFactory
    {
        public static XRoute BuildRouteFromFile(string fileName) 
        {
            XRoute xRoute = new XRoute();
            double distance = 0;
            double oldDist = 0;
            string oldLoc = null;
            bool oldIsStop = false;
            using (StreamReader sr = File.OpenText(fileName))
            {
                List<string> locations = new List<string>();
                List <double> distances = new List<double>();
                List <bool> isStops = new List<bool>();
                string input;
                string[] inputArray;
                while ((input = sr.ReadLine()) != null)
                {
                    inputArray = input.Split(",");
                    if (double.TryParse(inputArray[1], out distance))
                    {
                        string location = inputArray[0];
                        try
                        {
                            if(location == CultureInfo.CurrentCulture.TextInfo.ToTitleCase(location.ToLower()))
                            {
                                bool isStop = false;
                                if (inputArray[2].ToLower() == "true")
                                    isStop = true;
                        
                                locations.Add(location);
                                distances.Add(distance);
                                isStops.Add(isStop);
                            }
                            else
                                throw new RouteException();

                        }catch(Exception ex)
                        {
                            Console.WriteLine();
                            Console.WriteLine("Route Exception!");
                            RouteException.ExceptionDetails(ex);
                        }
                    }
                    else
                    {
                        int endLoc = inputArray[0].IndexOf('(');
                        string location = inputArray[0].Substring(0, endLoc);
                        try
                        {
                            if (location == CultureInfo.CurrentCulture.TextInfo.ToTitleCase(location.ToLower()))
                            {
                                    bool isStop = false;
                                if(inputArray[0].Contains("stop"))
                                    isStop= true;
                        
                                locations.Add(location);
                                distances.Add(oldDist);
                                isStops.Add(isStop);
                                distance = Double.Parse(inputArray[2]);
                                oldDist = distance;
                                oldLoc = inputArray[1];
                                if (inputArray[1].Contains("stop"))
                                    oldIsStop = true;
                            }
                            else
                            {
                                throw new Exception();
                            }
                        }catch(Exception ex)
                        {
                            Console.WriteLine();
                            Console.WriteLine("Route Exception!");
                            RouteException.ExceptionDetails(ex);
                        }
                    }
                }
                if(oldLoc != null)
                {
                    int endLoc = oldLoc.IndexOf('(');
                    string location = oldLoc.Substring(0, endLoc);
                    locations.Add(location);
                    distances.Add(oldDist);
                    isStops.Add(oldIsStop);
                }
                xRoute = BuildRoute(locations, isStops, distances);
            }
            return xRoute;
        }
        public static XRoute BuildRoute(List<string> locations, List<bool> stops, List<double> distances)
        {
            XRoute xRoute = new XRoute();
            for(int i = 0; i < locations.Count; i++)
                xRoute.Route.route.Add(Tuple.Create(distances[i], locations[i], stops[i]));
            return xRoute;
        }
        public static XRoute ReverseRoute(Route route)
        {
            XRoute xRoute = new XRoute();
            int tempVal = route.route.Count - 1;
            double startval = 0;

            for(int i = 0; i < route.route.Count; i++)
            {
                if(i == 0)
                    xRoute.Route.route.Add(Tuple.Create(startval, route.route[tempVal - i].Item2, route.route[tempVal - i].Item3));
                else
                    xRoute.Route.route.Add(Tuple.Create(route.route[tempVal - i + 1].Item1, route.route[tempVal - i].Item2, route.route[tempVal - i].Item3));
            }
            return xRoute;
        }
    }
}
