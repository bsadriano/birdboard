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

export type ProjectTaskGet = {
  id: number;
  projectId: number;
  body: string;
  completed: boolean;
  path: string;
};
