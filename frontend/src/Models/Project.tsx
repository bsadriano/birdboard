import { string } from "yup";
import { ProjectTaskGet } from "./ProjectTask";

export type ProjectGet = {
  id: number;
  owner: OwnerGet;
  tasks: ProjectTaskGet[];
  title: string;
  notes: string;
  description: string;
  path: string;
};

export type OwnerGet = {
  id: number;
};

export type ProjectResponse = {
  id: number;
  title: string;
  description: string;
  notes: string;
};
