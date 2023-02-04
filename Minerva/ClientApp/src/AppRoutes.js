import Home from "./pages/Home";
import Login from "./pages/Login";
import Register from "./pages/Register";
import Map from "./pages/Map";

const AppRoutes = [
  {
    index: true,
    element: <Home />,
    navLabel: "Home",
  },
  {
    path: '/login',
    element: <Login />,
  },
  {
    path: '/register',
    element: <Register />,
  },
  {
    path: '/planner',
    element: <div />,
    navLabel: "Planner",
    authRequired: true
  },
  {
    path: '/map',
    element: <Map/>,
    navLabel: "Map",
    authRequired: true
  },
  {
    path: '/search',
    element: <div />,
    navLabel: "Search",
    authRequired: true
  },
  {
    path: '/profile',
    element: <div />,
    authRequired: true
  },
  {
    path: '/verify',
    element: <div />,
  }
];

export default AppRoutes;
