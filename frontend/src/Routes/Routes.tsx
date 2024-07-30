import { createBrowserRouter } from "react-router-dom";
import App from "../App";
import ListProjects from "../Pages/Projects/ListProjects/ListProjects";
import CreateProject from "../Pages/Projects/CreateProject/CreateProject";
import ShowProject from "../Pages/Projects/ShowProject/ShowProject";
import LoginPage from "../Pages/LoginPage/LoginPage";
import RegisterPage from "../Pages/RegisterPage/RegisterPage";
import ProtectedRoute from "./ProtectedRoute";
import EditProject from "../Pages/Projects/EditProject/EditProject";
import GuestRoute from "./GuestRoute";
import RedirectToProjects from "../Pages/RedirectToProjects/RedirectToProjects";

export const router = createBrowserRouter([
  {
    path: "/",
    element: <App />,
    children: [
      {
        path: "",
        element: <RedirectToProjects />,
      },
      {
        path: "projects",
        element: (
          <ProtectedRoute>
            <ListProjects />
          </ProtectedRoute>
        ),
      },
      {
        path: "projects/create",
        element: (
          <ProtectedRoute>
            <CreateProject />
          </ProtectedRoute>
        ),
      },
      {
        path: "projects/:projectId",
        element: (
          <ProtectedRoute>
            <ShowProject />
          </ProtectedRoute>
        ),
      },
      {
        path: "projects/:projectId/edit",
        element: (
          <ProtectedRoute>
            <EditProject />
          </ProtectedRoute>
        ),
      },
      {
        path: "login",
        element: (
          <GuestRoute>
            <LoginPage />
          </GuestRoute>
        ),
      },
      {
        path: "register",
        element: (
          <GuestRoute>
            <RegisterPage />
          </GuestRoute>
        ),
      },
    ],
  },
]);
