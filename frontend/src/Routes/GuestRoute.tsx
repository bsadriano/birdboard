import React from "react";
import { Navigate, useLocation } from "react-router";
import { useAuth } from "../Context/useAuth";

interface Props {
  children: React.ReactNode;
}

const GuestRoute = ({ children }: Props) => {
  const location = useLocation();
  const { isLoggedIn } = useAuth();

  return isLoggedIn() ? (
    <Navigate to="/projects" state={{ from: location }} replace />
  ) : (
    <>{children}</>
  );
};

export default GuestRoute;
