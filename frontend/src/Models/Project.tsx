import { ProjectTaskGet } from "./ProjectTask";

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
  description: "created" | "updated";
  createdAt: string;
  entityData: {
    id: number;
    title: string;
    description: string;
  };
  subjectId: number;
  subjectType: "Project";
  changes: any;
};

export type TaskActivity = {
  id: number;
  description:
    | "created_task"
    | "updated_task"
    | "completed_task"
    | "incompleted_task";
  createdAt: string;
  entityData: {
    id: number;
    body: string;
  };
  subjectId: number;
  subjectType: "ProjectTask";
  changes: any;
};

export type ProjectResponse = {
  id: number;
  title: string;
  description: string;
  notes: string;
};
