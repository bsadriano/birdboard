import { string } from "yup";
import { ProjectTaskGet } from "./ProjectTask";

export type ProjectGet = {
  id: number;
  owner: OwnerGet;
  tasks: ProjectTaskGet[];
  activities: Activity[];
  title: string;
  notes: string;
  description: string;
  path: string;
};

export type OwnerGet = {
  id: number;
};

export type Activity = {
  id: number;
  description: string;
  createdAt: string;
};

export type ProjectResponse = {
  id: number;
  title: string;
  description: string;
  notes: string;
};
