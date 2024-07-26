import { string } from "yup";
import { ProjectTaskGet } from "./ProjectTask";
import { ActivityMapKey } from "../Components/Projects/ActivityCard/ActivityCard";

export type ProjectGet = {
  id: number;
  owner: OwnerGet;
  tasks: ProjectTaskGet[];
  activities: ProjectActivity | TaskActivity[];
  title: string;
  notes: string;
  description: string;
  path: string;
};

export type OwnerGet = {
  id: number;
};

export type ProjectActivity = {
  id: number;
  description: ActivityMapKey;
  createdAt: string;
  entityData: {
    id: number;
    title: string;
    description: string;
  };
  subjectId: number;
  subjectType: "Project";
};

export type TaskActivity = {
  id: number;
  description: ActivityMapKey;
  createdAt: string;
  entityData: {
    id: number;
    body: string;
  };
  subjectId: number;
  subjectType: "ProjectTask";
};

export type ProjectResponse = {
  id: number;
  title: string;
  description: string;
  notes: string;
};
