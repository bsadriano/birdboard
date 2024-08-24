import axios, { AxiosError, AxiosResponse } from "axios";
import { LoginRequestDto } from "../Models/Login/LoginRequestDto";
import { LoginResponseDto } from "../Models/Login/LoginResponseDto";
import { PaginatedResponse } from "../Models/Pagination";
import {
  SaveProjectRequestDto,
  UpdateProjectRequestDto,
} from "../Models/Project/ProjectRequestDto";
import { ProjectResponseDto } from "../Models/Project/ProjectResponseDto";
import { RegisterRequestDto } from "../Models/Register/RegisterRequestDto";
import { RegisterResponseDto } from "../Models/Register/RegisterResponseDto";
import {
  CreateProjectTaskRequestDto,
  UpdateProjectTaskRequestDto,
} from "../Models/Project/ProjectTaskRequestDto";
import { CreateProjectInvitationRequestDto } from "../Models/Project/ProjectInvitationRequestDto";

const sleep = () => new Promise((resolve) => setTimeout(resolve, 300));

axios.defaults.baseURL = process.env.REACT_APP_API_URL;
axios.defaults.withCredentials = true;

const responseBody = <T>(response: AxiosResponse<T>) => response.data;

axios.interceptors.request.use((config) => {
  const token = localStorage.getItem("token");
  if (token) config.headers.Authorization = `Bearer ${token}`;
  return config;
});

axios.interceptors.response.use(
  async (response) => {
    if (process.env.NODE_ENV === "development") await sleep();

    const pagination = response.headers["pagination"];
    if (pagination) {
      response.data = new PaginatedResponse(
        response.data,
        JSON.parse(pagination)
      );
      return response;
    }
    return response;
  },
  (error: AxiosError) => {
    return Promise.reject(error.response as AxiosResponse);
  }
);

const requests = {
  get: <T>(url: string, params?: URLSearchParams) =>
    axios.get(url, { params }).then(responseBody<T>),
  post: <T>(url: string, body: object) =>
    axios.post(url, body).then(responseBody<T>),
  put: <T>(url: string, body: object) =>
    axios.put(url, body).then(responseBody<T>),
  patch: <T>(url: string, body: object) =>
    axios.patch(url, body).then(responseBody<T>),
  delete: <T>(url: string) => axios.delete(url).then(responseBody<T>),
};

const Auth = {
  login: (body: LoginRequestDto) =>
    requests.post<LoginResponseDto>("auth/login", body),
  register: (body: RegisterRequestDto) =>
    requests.post<RegisterResponseDto>("auth/register", body),
};

const Project = {
  list: () => requests.get<ProjectResponseDto[]>("projects"),
  show: (id: number | string) =>
    requests.get<ProjectResponseDto>(`projects/${id}`),
  create: (body: SaveProjectRequestDto) =>
    requests.post<ProjectResponseDto>("projects", body),
  update: (id: number | string, body: UpdateProjectRequestDto) =>
    requests.patch<ProjectResponseDto>(`projects/${id}`, body),
  delete: (id: number | string) =>
    requests.delete<ProjectResponseDto>(`projects/${id}`),
};

const ProjectTask = {
  create: (projectId: number | string, body: CreateProjectTaskRequestDto) =>
    requests.post(`/projects/${projectId}/tasks`, body),
  update: (
    projectId: number | string,
    taskId: number | string,
    body: UpdateProjectTaskRequestDto
  ) => requests.patch(`/projects${projectId}/tasks/${taskId}`, body),
};

const ProjectInvitation = {
  create: (
    projectId: number | string,
    body: CreateProjectInvitationRequestDto
  ) => requests.post(`/projects/${projectId}/invitations`, body),
};

const Agent = {
  Auth,
  Project,
  ProjectTask,
  ProjectInvitation,
};

export default Agent;
