import axios from "axios";
import { ProjectTaskPost } from "../Models/ProjectTask";
import { handleError } from "../Helpers/ErrorHandler";

const api = (projectId: string) =>
  `${process.env.REACT_APP_API_URL}/projects/${projectId}/tasks`;

export const projectTaskPostAPI = async (projectId: string, body: string) => {
  try {
    const data = await axios.post<ProjectTaskPost>(api(projectId), {
      body,
    });
    return data;
  } catch (error) {
    handleError(error);
  }
};

export type UpdateTaskData = {
  body?: string;
  completed?: boolean;
};

export const projectTaskPatchAPI = async (
  projectId: string,
  taskId: number,
  taskData: UpdateTaskData
) => {
  try {
    const data = await axios.patch<ProjectTaskPost>(
      `${api(projectId)}/${taskId}`,
      taskData
    );
    return data;
  } catch (error) {
    handleError(error);
  }
};
