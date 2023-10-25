using RoutClassLibrary;

namespace xUnitTest_Route
{
    public class UnitTesting_xRoute
    {
        [Theory]
        [InlineData("Gent", 0, true, "Luik", 5, false)]
        [InlineData("Luik", 5, false, "Gent", 0, true)]
        [InlineData("Brugge", 100, true, "Gent", 0, true)]
        public void AddLocation_test(string location, double distance, bool isStop, string loc2, double dist2, bool isStop2)
        {
            XRoute xRoute = new XRoute();

            xRoute.AddLocation(location, distance, isStop);
            xRoute.AddLocation(loc2, dist2, isStop2);

            Assert.Equal(location, xRoute.Route.route[0].Item2);
            Assert.Equal(distance, xRoute.Route.route[0].Item1);
            Assert.Equal(isStop, xRoute.Route.route[0].Item3);
            Assert.Equal(loc2, xRoute.Route.route[1].Item2);
            Assert.Equal(dist2, xRoute.Route.route[1].Item1);
            Assert.Equal(isStop2, xRoute.Route.route[1].Item3);
            Assert.Equal(2, xRoute.Route.route.Count);
        }

        [Theory]
        [InlineData("Luik", -1, false)]
        [InlineData(null, 5, true)]
        [InlineData("luik", 8, false)]
        public void AddLocation_failTest(string location, double distance, bool isStop)
        {
            XRoute xRoute = new XRoute();

            Assert.Throws<RouteException>(() => xRoute.AddLocation(location, distance, isStop));
        }

        [Fact]
        public void GetDistance_test()
        {
            XRoute xRoute = new XRoute();
            xRoute.AddLocation("Gent", 0, true);
            xRoute.AddLocation("Luik", 5, false);
            xRoute.AddLocation("Brugge", 100, true);

            double distance = xRoute.GetDistance();

            Assert.Equal(105, distance);
        }

        [Theory]
        [InlineData("Gent", "Brugge", 105)]
        [InlineData("Gent", "Luik", 5)]
        [InlineData("Luik", "Luik", 0)]
        [InlineData("Luik", "Brugge", 100)]
        public void GetDistanceLimited_test(string start, string finish, double correctDistance)
        {
            XRoute xRoute = new XRoute();

            xRoute.AddLocation("Gent", 0, true);
            xRoute.AddLocation("Luik", 5, false);
            xRoute.AddLocation("Brugge", 100, true);

            double distance = xRoute.GetDistance(start, finish);

            Assert.Equal(correctDistance, distance);
        }

        [Theory]
        [InlineData("A", "Brugge")]
        [InlineData("Luik", "A")]
        [InlineData("A", "A")]
        public void GetDistanceLimited_testfailure(string loc1, string loc2)
        {
            XRoute xRoute = new XRoute();

            xRoute.AddLocation("Gent", 0, true);
            xRoute.AddLocation("Luik", 5, false);
            xRoute.AddLocation("Brugge", 100, true);
            double distance = xRoute.GetDistance(loc1, loc2);
            Assert.Equal(-1, distance);
        }

        [Theory]
        [InlineData("Brugge", true)]
        [InlineData("Luik", true)]
        [InlineData("Gent", true)]
        [InlineData("Merelbeke", false)]
        [InlineData("Genk", false)]
        [InlineData("Parijs", false)]
        public void HasLocation_test(string location, bool hasLocation)
        {
            XRoute xRoute = new XRoute();
            xRoute.AddLocation("Gent", 0, true);
            xRoute.AddLocation("Luik", 5, false);
            xRoute.AddLocation("Brugge", 100, true);

            Assert.Equal(hasLocation, xRoute.HasLocation(location));
        }

        [Theory]
        [InlineData("Brugge", true)]
        [InlineData("Luik", false)]
        [InlineData("Gent", true)]
        public void HasStop_test(string location, bool hasLocation)
        {
            XRoute xRoute = new XRoute();
            xRoute.AddLocation("Gent", 0, true);
            xRoute.AddLocation("Luik", 5, false);
            xRoute.AddLocation("Brugge", 100, true);

            Assert.Equal(hasLocation, xRoute.HasStop(location));
        }

        [Theory]
        [InlineData("Merelbeke", 4, "Gent", false)]
        [InlineData("De Pinte", 4, "Luik", true)]

        public void InsertLoction_test(string location, double distance, string fromLocation, bool isStop)
        {
            XRoute xRoute = new XRoute();
            xRoute.AddLocation("Gent", 0, true);
            xRoute.AddLocation("Luik", 5, false);
            xRoute.AddLocation("Brugge", 100, true); ;

            double newDist1Correct = distance;
            double newDist2Correct = -1;
            double actDist1 = -1;
            double actDist2 = -1;
            for (int i = 0; i < xRoute.Route.route.Count; i++)
            {
                if (xRoute.Route.route[i].Item2 == fromLocation)
                {
                    newDist2Correct = xRoute.Route.route[i + 1].Item1 - distance;
                    break;
                }
            }
            xRoute.InsertLocation(location, distance, fromLocation, isStop);
            for (int i = 0; i < xRoute.Route.route.Count; i++)
            {
                if (xRoute.Route.route[i].Item2 == fromLocation)
                {
                    actDist1 = xRoute.Route.route[i + 1].Item1;
                    actDist2 = xRoute.Route.route[i + 2].Item1;
                    break;
                }
            }
            int routeLength = xRoute.Route.route.Count;
            Assert.Equal(newDist1Correct, actDist1);
            Assert.Equal(newDist2Correct, actDist2);
            Assert.Equal(4, routeLength);
        }
        [Theory]
        [InlineData("Merelbeke", 4, "Brugge", false)]

        public void InsertLoctionEnd_test(string location, double distance, string fromLocation, bool isStop)
        {
            XRoute xRoute = new XRoute();
            xRoute.AddLocation("Gent", 0, true);
            xRoute.AddLocation("Luik", 5, false);
            xRoute.AddLocation("Brugge", 100, true);

            xRoute.InsertLocation(location, distance, fromLocation, isStop);

            int routeLength = xRoute.Route.route.Count;
            Assert.Equal(distance, xRoute.Route.route[3].Item1);
            Assert.Equal(4, routeLength);
        }
        [Theory]
        [InlineData("Luik")]
        [InlineData("Gent")]
        public void RemoveLocation_test(string location)
        {
            XRoute xRoute = new XRoute();
            xRoute.AddLocation("Gent", 0, true);
            xRoute.AddLocation("Luik", 5, false);
            xRoute.AddLocation("Brugge", 100, true);

            double newDistCorrect = -1;
            double actDist = -1;
            int newLoc = -1;
            for (int i = 0; i < xRoute.Route.route.Count; i++)
            {
                if (xRoute.Route.route[i].Item2 == location)
                {
                    newDistCorrect = xRoute.Route.route[i + 1].Item1 + xRoute.Route.route[i].Item1;
                    newLoc = i;
                    break;
                }
            }
            xRoute.RemoveLocation(location);

            actDist = xRoute.Route.route[newLoc].Item1;

            int routeLength = xRoute.Route.route.Count;

            Assert.Equal(2, routeLength);
            Assert.Equal(newDistCorrect, actDist);
        }

        [Theory]
        [InlineData("Brugge")]
        public void RemoveLocationEnd_test(string location)
        {
            XRoute xRoute = new XRoute();
            xRoute.AddLocation("Gent", 0, true);
            xRoute.AddLocation("Luik", 5, false);
            xRoute.AddLocation("Brugge", 100, true);

            xRoute.RemoveLocation(location);

            int routeLength = xRoute.Route.route.Count;

            Assert.Equal(2, routeLength);
            Assert.Equal(5, xRoute.Route.route[1].Item1);
        }

        [Theory]
        [InlineData("Gent", "Luik", 10)]
        [InlineData("Luik", "Brugge", 20)]
        public void SetDistance_test(string loc1, string loc2, double distance)
        {
            XRoute xRoute = new XRoute();

            xRoute.AddLocation("Gent", 0, true);
            xRoute.AddLocation("Luik", 5, false);
            xRoute.AddLocation("Brugge", 100, true);

            double oldDistance = xRoute.GetDistance();
            xRoute.SetDistance(distance, loc1, loc2);
            double newDistance = xRoute.GetDistance();
            Assert.NotEqual(oldDistance, newDistance);
        }

        [Fact]
        public void ShowLocations_test()
        {
            XRoute xRoute = new XRoute();
            xRoute.AddLocation("Gent", 0, true);
            xRoute.AddLocation("Luik", 5, false);
            xRoute.AddLocation("Brugge", 100, true);

            List<string> locations = xRoute.ShowLocations();

            Assert.Equal(xRoute.Route.route.Count(), locations.Count());
        }

        [Fact]
        public void ShowStops_test()
        {
            XRoute xRoute = new XRoute();
            xRoute.AddLocation("Gent", 0, true);
            xRoute.AddLocation("Luik", 5, false);
            xRoute.AddLocation("Brugge", 100, true);

            List<string> locations = xRoute.ShowStops();

            Assert.Equal(2, locations.Count());
        }

        [Fact]
        public void ShowFullRoute_test()
        {
            XRoute xRoute = new XRoute();
            xRoute.AddLocation("Gent", 0, true);
            xRoute.AddLocation("Luik", 5, false);
            xRoute.AddLocation("Brugge", 100, true);

            (string start, List<(double distance, string location)>) FullRoute = xRoute.ShowFullRoute();

            Assert.Equal("Gent", FullRoute.Item1);
            Assert.Equal(3, FullRoute.Item2.Count);
        }

        [Theory]
        [InlineData("Gent", "Luik", 2)]
        [InlineData("Gent", "Brugge", 3)]
        [InlineData("Luik", "Brugge", 2)]
        public void ShowFullRouteExtra_test(string startLocation, string endLocation, int desiredResult)
        {
            XRoute xRoute = new XRoute();
            xRoute.AddLocation("Gent", 0, true);
            xRoute.AddLocation("Luik", 5, false);
            xRoute.AddLocation("Brugge", 100, true);

            (string start, List<(double distance, string location)>) FullRoute = xRoute.ShowFullRoute(startLocation, endLocation);

            Assert.Equal(startLocation, FullRoute.Item1);
            Assert.Equal(desiredResult, FullRoute.Item2.Count);
        }

        [Theory]
        [InlineData("Gent", "Luik", 2)]
        [InlineData("Gent", "Brugge", 2)]
        [InlineData("Luik", "Brugge", 2)]
        public void ShowRouteExtra_test(string startLocation, string endLocation, int desiredResult)
        {
            XRoute xRoute = new XRoute();
            xRoute.AddLocation("Gent", 0, true);
            xRoute.AddLocation("Luik", 5, false);
            xRoute.AddLocation("Brugge", 100, true);

            (string start, List<(double distance, string location)>) FullRoute = xRoute.ShowRoute(startLocation, endLocation);

            Assert.Equal(startLocation, FullRoute.Item1);
            Assert.Equal(desiredResult, FullRoute.Item2.Count);
        }

        [Fact]
        public void ShowRoute_test()
        {
            XRoute xRoute = new XRoute();
            xRoute.AddLocation("Gent", 0, true);
            xRoute.AddLocation("Luik", 5, false);
            xRoute.AddLocation("Brugge", 100, true);

            (string start, List<(double distance, string location)>) FullRoute = xRoute.ShowRoute();

            Assert.Equal("Gent", FullRoute.Item1);
            Assert.Equal(2, FullRoute.Item2.Count);
        }
    }
}