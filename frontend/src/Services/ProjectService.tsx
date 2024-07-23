import { ProjectGet } from "../Models/Project";
import { getAPI, patchAPI, postAPI } from "./ApiService";

const api = `${process.env.REACT_APP_API_URL}/projects`;

export const projectsGetAPI = async () => getAPI<ProjectGet[]>(api);

export const projectGetAPI = async (id: string) =>
  getAPI<ProjectGet>(`${api}/${id}`);

export type ProjectFormInputs = {
  title: string;
  description: string;
};

export const projectsPostAPI = async (projectData: ProjectFormInputs) =>
  postAPI<ProjectGet>(api, projectData);

export type UpdateProjectData = {
  title?: string;
  description?: string;
  notes?: string;
};

export const projectPatchAPI = async (
  projectId: number | string,
  projectData: UpdateProjectData
) => patchAPI<ProjectGet>(`${api}/${projectId}`, projectData);
