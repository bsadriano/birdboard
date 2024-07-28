import { ProjectMemberGet } from "../Models/Project";
import { postAPI } from "./ApiService";

const api = (projectId: number) =>
  `${process.env.REACT_APP_API_URL}/projects/${projectId}/invitations`;

export const projectInvitationsPostApi = async (
  projectId: number,
  email: string
) => postAPI<ProjectMemberGet>(api(projectId), { email });
