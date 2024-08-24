import axios from "axios";
import React, { createContext, useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { toast } from "react-toastify";
import Agent from "../Api/Agent";
import { LoginRequestDto } from "../Models/Login/LoginRequestDto";
import { RegisterRequestDto } from "../Models/Register/RegisterRequestDto";
import { UserProfile } from "../Models/User";

type UserContextType = {
  user: UserProfile | null;
  token: string | null;
  registerUser: (body: RegisterRequestDto) => void;
  loginUser: (body: LoginRequestDto) => void;
  logout: (e: any) => void;
  isLoggedIn: () => boolean;
  isGuest: () => boolean;
  isAuthUser: (email: string) => boolean;
};

type Props = { children: React.ReactNode };

const UserContext = createContext<UserContextType>({} as UserContextType);

export const UserProvider = ({ children }: Props) => {
  const navigate = useNavigate();
  const [token, setToken] = useState<string | null>(null);
  const [user, setUser] = useState<UserProfile | null>(null);
  const [isReady, setIsReady] = useState<boolean>(false);

  useEffect(() => {
    const user = localStorage.getItem("user");
    const token = localStorage.getItem("token");
    if (user && token) {
      setUser(JSON.parse(user));
      setToken(token);
      axios.defaults.headers.common["Authorization"] = "Bearer " + token;
    }
    setIsReady(true);
  }, []);

  const registerUser = async (body: RegisterRequestDto) => {
    const data = await Agent.Auth.register(body);

    if (data) {
      const { userName, email, token } = data;

      localStorage.setItem("token", token);
      const userObj = { userName, email };
      localStorage.setItem("user", JSON.stringify(userObj));
      setToken(token);
      setUser(userObj);
      axios.defaults.headers.common["Authorization"] = `Bearer ${token}`;
      navigate("/projects");
      toast.success("Login Success!", {
        autoClose: 1000,
      });
    }
  };

  const loginUser = async (body: LoginRequestDto) => {
    const data = await Agent.Auth.login(body);

    if (data) {
      const { userName, email, token } = data;

      localStorage.setItem("token", token);
      const userObj = {
        userName,
        email,
      };
      localStorage.setItem("user", JSON.stringify(userObj));
      setToken(token);
      setUser(userObj);
      axios.defaults.headers.common["Authorization"] = `Bearer ${token}`;
      navigate("/projects");
      toast.success("Login Success!", {
        autoClose: 1000,
      });
    }
  };

  const isLoggedIn = () => !!user;

  const isGuest = () => !user;

  const logout = (e: any) => {
    e.preventDefault();
    localStorage.removeItem("token");
    localStorage.removeItem("user");
    setUser(null);
    setToken("");
    navigate("/");
  };

  const isAuthUser = (emali: string) => user?.email === emali;

  return (
    <UserContext.Provider
      value={{
        loginUser,
        user,
        token,
        logout,
        isLoggedIn,
        registerUser,
        isGuest,
        isAuthUser,
      }}
    >
      {isReady ? children : null}
    </UserContext.Provider>
  );
};

export const useAuth = () => React.useContext(UserContext);
