import { createBrowserRouter } from "react-router-dom";
import App from "../App";
import HomePage from "../Pages/HomePage/HomePage";
import ListProjects from "../Pages/Projects/ListProjects/ListProjects";
import CreateProject from "../Pages/Projects/CreateProject/CreateProject";
import ShowProject from "../Pages/Projects/ShowProject/ShowProject";

export const router = createBrowserRouter([
  {
    path: "/",
    element: <App />,
    children: [
      {
        path: "",
        element: <HomePage />,
      },
      {
        path: "projects",
        element: <ListProjects />,
      },
      {
        path: "projects/create",
        element: <CreateProject />,
      },
      {
        path: "projects/:id",
        element: <ShowProject />,
      },
    ],
  },
]);
