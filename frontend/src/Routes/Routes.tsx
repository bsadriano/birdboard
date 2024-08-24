import { createBrowserRouter } from "react-router-dom";
import App from "../App";
import LoginPage from "../Pages/LoginPage/LoginPage";
import EditProject from "../Pages/Projects/EditProject/EditProject";
import ListProjects from "../Pages/Projects/ListProjects/ListProjects";
import ShowProject from "../Pages/Projects/ShowProject/ShowProject";
import RedirectToProjects from "../Pages/RedirectToProjects/RedirectToProjects";
import RegisterPage from "../Pages/RegisterPage/RegisterPage";
import GuestRoute from "./GuestRoute";
import ProtectedRoute from "./ProtectedRoute";

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
