import axios from "axios";
import { handleError } from "../Helpers/ErrorHandler";
import { ProjectGet } from "../Models/Project";

const api = `${process.env.REACT_APP_API_URL}/projects`;

export const projectsGetAPI = async () => {
  try {
    const data = await axios.get<ProjectGet[]>(`${api}`);
    return data;
  } catch (error) {
    handleError(error);
  }
};

export const projectGetAPI = async (id: string) => {
  try {
    const data = await axios.get<ProjectGet>(`${api}/${id}`);
    return data;
  } catch (error) {
    handleError(error);
  }
};
