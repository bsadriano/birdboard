export type ProjectTaskGet = {
  id: number;
  projectId: number;
  body: string;
  completed: boolean;
  path: string;
};

export type ProjectTaskPost = {
  body: string;
};
