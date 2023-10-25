using RoutClassLibrary;

namespace RouteUI
{
    internal class Program
    {
        static void Main(string[] args)
        {
            XRoute xRoute = RouteFactory.BuildRouteFromFile("test.txt");
            
            for(int i = 0; i < xRoute.Route.route.Count; i++)
                Console.WriteLine(xRoute.Route.route[i]);
            Console.WriteLine();

            xRoute = RouteFactory.BuildRouteFromFile("test2.txt");

            for (int i = 0; i < xRoute.Route.route.Count; i++)
                Console.WriteLine(xRoute.Route.route[i]);
            
            Console.WriteLine() ;
            xRoute.UpdateLocation("Bree", "Blaa", false);
            for (int i = 0; i < xRoute.Route.route.Count; i++)
                Console.WriteLine(xRoute.Route.route[i]);

            Console.WriteLine();

            xRoute.InsertLocation("Place", 5, "Mount Doom", false);
            for (int i = 0; i < xRoute.Route.route.Count; i++)
                Console.WriteLine(xRoute.Route.route[i]);

            Console.WriteLine();

            xRoute.RemoveLocation("Place");
            for (int i = 0; i < xRoute.Route.route.Count; i++)
                Console.WriteLine(xRoute.Route.route[i]);

            Console.WriteLine();

            //xRoute = RouteFactory.ReverseRoute(xRoute.Route);

            //for (int i = 0; i < xRoute.Route.route.Count; i++)
            //    Console.WriteLine(xRoute.Route.route[i]);
        }
    }
}