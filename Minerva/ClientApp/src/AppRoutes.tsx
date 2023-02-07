import Home from "./pages/Home";
import Login from "./pages/Login";
import Register from "./pages/Register";
import Map from "./pages/Map";
import Planner from "./components/Planner/Planner";


export interface AppRoute {
  path?: string;
  element: JSX.Element;
  navLabel?: string;
  index?: boolean;
  authRequired?: boolean;
}

const AppRoutes: AppRoute[] = [
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
    element: <Planner />,
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
