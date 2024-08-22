import { createContext, useEffect, useState } from "react";
import { UserProfile } from "../Models/User";
import { useNavigate } from "react-router-dom";
import { loginAPI, registerAPI } from "../Services/AuthService";
import { toast } from "react-toastify";
import React from "react";
import axios from "axios";

type UserContextType = {
  user: UserProfile | null;
  token: string | null;
  registerUser: (email: string, username: string, password: string) => void;
  loginUser: (username: string, password: string) => void;
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

  const registerUser = async (
    email: string,
    username: string,
    password: string
  ) => {
    await registerAPI(email, username, password)
      .then((res) => {
        if (res) {
          localStorage.setItem("token", res?.data.token);
          const userObj = {
            userName: res?.data.userName,
            email: res?.data.email,
          };
          localStorage.setItem("user", JSON.stringify(userObj));
          setToken(res?.data.token!);
          setUser(userObj);
          axios.defaults.headers.common["Authorization"] =
            "Bearer " + res?.data.token!;
          navigate("/projects");
          toast.success("Login Success!", {
            autoClose: 1000,
          });
        }
      })
      .catch((e) => toast.warning("Server error occured"));
  };

  const loginUser = async (username: string, password: string) => {
    const data = await loginAPI(username, password);
    if (!data) {
      throw new Error("Error logging in");
    }
    const { email, userName, token } = data;

    localStorage.setItem("token", token);
    const userObj = {
      userName,
      email,
    };
    localStorage.setItem("user", JSON.stringify(userObj));
    setToken(token);
    setUser(userObj);
    axios.defaults.headers.common["Authorization"] = "Bearer " + token;
    navigate("/projects");
    toast.success("Login Success!", {
      autoClose: 1000,
    });
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
