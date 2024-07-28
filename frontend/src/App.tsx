import { Outlet } from "react-router";
import { ToastContainer } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";
import "./App.css";
import Nav from "./Components/Nav/Nav";
import { UserProvider } from "./Context/useAuth";

function App() {
  return (
    <UserProvider>
      <div className="theme-light bg-page min-h-screen">
        <Nav></Nav>
        <div className="container mx-auto py-4 section">
          <Outlet />
          <ToastContainer />
        </div>
      </div>
    </UserProvider>
  );
}

export default App;
