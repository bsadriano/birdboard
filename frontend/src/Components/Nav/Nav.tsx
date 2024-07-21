import React from "react";
import { Link } from "react-router-dom";

interface Props {}

const Nav = (props: Props) => {
  return (
    <nav className="bg-header py-2">
      <div className="container mx-auto flex justify-between items-center py-2">
        <Link className="navbar-brand" to="/projects">
          <img
            src="/images/logo.svg"
            alt="Birdboard"
            className="relative"
            style={{ top: "2px" }}
          />
        </Link>
        <div>
          <div className="flex items-center ml-auto text-default">
            <Link className="nav-link" to="login">
              Login
            </Link>
            <Link className="nav-link" to="register">
              Register
            </Link>
          </div>
        </div>
      </div>
    </nav>
  );
};

export default Nav;
