import { ProjectTaskGet } from "../Models/ProjectTask";
import { patchAPI, postAPI } from "./ApiService";

const api = (projectId: string) =>
  `${process.env.REACT_APP_API_URL}/projects/${projectId}/tasks`;

export const projectTaskPostAPI = async (projectId: string, body: string) =>
  postAPI<ProjectTaskGet>(api(projectId), { body });

export type UpdateTaskData = {
  body?: string;
  completed?: boolean;
};

export const projectTaskPatchAPI = async (
  projectId: string,
  taskId: number,
  taskData: UpdateTaskData
) => patchAPI<ProjectTaskGet>(`${api(projectId)}/${taskId}`, taskData);
