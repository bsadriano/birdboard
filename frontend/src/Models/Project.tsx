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
  id: string;
  userName: string;
  email: string;
};

// Base type for common properties
type BaseActivity<T extends string> = {
  id: number;
  description: T;
  createdAt: string;
  entityData: {
    id: number;
    title?: string; // Optional for tasks
    description?: string; // Optional for tasks
    body?: string; // Optional for projects
  };
  subjectId: number;
  subjectType: string; // To be narrowed in derived types
  changes: any;
  user: OwnerGet;
};

// Project Activity Type
export type ProjectActivity = BaseActivity<"created" | "updated"> & {
  entityData: {
    id: number;
    title: string;
    description: string;
  };
  subjectType: "Project";
};

// Task Activity Type
export type TaskActivity = BaseActivity<
  "created_task" | "updated_task" | "completed_task" | "incompleted_task"
> & {
  entityData: {
    id: number;
    body: string;
  };
  subjectType: "ProjectTask";
};

export type ProjectResponse = {
  id: number;
  title: string;
  description: string;
  notes: string;
};
