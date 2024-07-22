import { ProjectTaskGet } from "./ProjectTask";

export type ProjectGet = {
  id: number;
  owner: OwnerGet;
  tasks: ProjectTaskGet[];
  title: string;
  description: string;
  path: string;
};

export type OwnerGet = {
  id: number;
};
