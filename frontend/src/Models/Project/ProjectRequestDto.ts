export interface TaskDto {
  body?: string;
}

export interface SaveProjectRequestDto {
  title: string;
  description: string;
  tasks?: TaskDto[];
}

export interface UpdateProjectRequestDto {
  title?: string;
  description?: string;
  notes?: string;
}
