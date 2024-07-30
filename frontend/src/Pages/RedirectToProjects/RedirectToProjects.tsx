import { Navigate, useLocation } from "react-router-dom";
import ProtectedRoute from "../../Routes/ProtectedRoute";

interface Props {}

const RedirectToProjects = (props: Props) => {
  const location = useLocation();

  return (
    <ProtectedRoute>
      <Navigate to="/projects" state={{ from: location }} replace />
    </ProtectedRoute>
  );
};

export default RedirectToProjects;
