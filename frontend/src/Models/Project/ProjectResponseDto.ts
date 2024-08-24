import { ProjectTaskDto } from "./ProjectTaskDto";

export type ProjectResponseDto = {
  id: number;
  owner: OwnerDto;
  tasks: ProjectTaskDto[];
  activities: ProjectActivityDto | TaskActivityDto[];
  title: string;
  notes: string;
  description: string;
  path: string;
  members: ProjectMemberDto[];
};

export type OwnerDto = {
  id: string;
  userName: string;
  email: string;
};

export type ProjectMemberDto = {
  email: string;
  token: string;
  userName: string;
};

// Base type for common properties
type BaseActivity = {
  id: number;
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
  user: OwnerDto;
};

// Project Activity Type
export type ProjectActivityDto = BaseActivity & {
  description: "created" | "updated";
  entityData: {
    id: number;
    title: string;
    description: string;
  };
  subjectType: "Project";
};

// Task Activity Type
export type TaskActivityDto = BaseActivity & {
  description:
    | "created_task"
    | "updated_task"
    | "completed_task"
    | "incompleted_task";
  entityData: {
    id: number;
    body: string;
  };
  subjectType: "ProjectTask";
};
