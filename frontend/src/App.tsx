import { Outlet } from "react-router";
import "./App.css";
import { Link } from "react-router-dom";
import Nav from "./Components/Nav/Nav";

function App() {
  return (
    <div className="bg-grey-light min-h-screen">
      <Nav></Nav>
      <div className="container mx-auto py-4 section">
        <Outlet />
      </div>
    </div>
  );
}

export default App;
