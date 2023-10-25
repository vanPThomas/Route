namespace RoutClassLibrary
{
    public class RouteException : Exception
    {
        public RouteException() :
            base("An Error Occurred")
        {

        }

        public static void ExceptionDetails(Exception e)
        {
            Console.WriteLine("-----------");
            Console.WriteLine($"Type: {e.GetType()}");
            Console.WriteLine($"Source: {e.Source}");
            Console.WriteLine($"TargetSite: {e.TargetSite.GetType()}");
            Console.WriteLine($"Message: {e.Message}");
            Console.WriteLine("-----------");
        }
    }
}
