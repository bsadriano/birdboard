export interface Project {
    id: number,
    owner: Owner
    tasks: ProjectTask[]
    title: string;
    description: string;
    path: string;
}

export interface Owner {
    id: number
}

export interface ProjectTask {
    id: number,
    projectId: number
    body: string;
}
