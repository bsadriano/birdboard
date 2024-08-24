export type CreateProjectTaskRequestDto = {
  body: string;
};

export type UpdateProjectTaskRequestDto = {
  body?: string;
  completed?: boolean;
};
