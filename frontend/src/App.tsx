import { Outlet } from "react-router";
import "./App.css";
import { Link } from "react-router-dom";

function App() {
  return (
    <div className="app">
      <nav>
        <div className="container">
          <Link to="/">Birdboard</Link>
        </div>
      </nav>
      <Outlet />
    </div>
  );
}

export default App;
