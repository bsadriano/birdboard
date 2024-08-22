import axios from "axios";
import { handleError } from "../Helpers/ErrorHandler";
import { UserProfileToken } from "../Models/User";
import Agent from "../Api/Agent";

const auth = `${process.env.REACT_APP_API_URL}/auth`;

export const loginAPI = async (username: string, password: string) => {
  try {
    return await Agent.Auth.login({ username, password });
  } catch (error) {
    handleError(error);
  }
};

export const registerAPI = async (
  email: string,
  username: string,
  password: string
) => {
  try {
    const data = await axios.post<UserProfileToken>(`${auth}/register`, {
      email,
      username,
      password,
    });
    return data;
  } catch (error) {
    handleError(error);
  }
};
