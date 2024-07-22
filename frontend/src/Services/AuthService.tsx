import axios from "axios";
import { handleError } from "../Helpers/ErrorHandler";
import { UserPorfileToken } from "../Models/User";

const auth = `${process.env.REACT_APP_API_URL}/auth`;

export const loginAPI = async (username: string, password: string) => {
  try {
    const data = await axios.post<UserPorfileToken>(`${auth}/login`, {
      username,
      password,
    });
    return data;
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
    const data = await axios.post<UserPorfileToken>(`${auth}/login`, {
      email,
      username,
      password,
    });
    return data;
  } catch (error) {
    handleError(error);
  }
};
