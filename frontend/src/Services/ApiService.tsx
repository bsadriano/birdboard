import axios from "axios";
import { handleError } from "../Helpers/ErrorHandler";

export const getAPI = async <T,>(url: string) => {
  try {
    return await axios.get<T>(url);
  } catch (e) {
    handleError(e);
    return null;
  }
};

export const postAPI = async <T,>(url: string, data: any) => {
  try {
    return await axios.post<T>(url, data);
  } catch (error) {
    handleError(error);
    return null;
  }
};

export const patchAPI = async <T,>(url: string, data: any) => {
  try {
    return await axios.patch<T>(url, data);
  } catch (error) {
    handleError(error);
    return null;
  }
};
