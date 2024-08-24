import { yupResolver } from "@hookform/resolvers/yup";
import { useEffect, useState } from "react";
import { useForm } from "react-hook-form";
import { Link, useNavigate, useParams } from "react-router-dom";
import { toast } from "react-toastify";
import * as Yup from "yup";
import Agent from "../../../Api/Agent";
import ActivityCard from "../../../Components/Projects/ActivityCard/ActivityCard";
import GeneralNotes from "../../../Components/Projects/GeneralNotes/GeneralNotes";
import InviteCard from "../../../Components/Projects/InviteCard/InviteCard";
import ProjectCard from "../../../Components/Projects/ProjectCard/ProjectCard";
import UpdateTaskForm from "../../../Components/Projects/UpdateTaskForm/UpdateTaskForm";
import { useAuth } from "../../../Context/useAuth";
import { gravatar_url } from "../../../Helpers/Gravatar_Url";
import { ProjectResponseDto } from "../../../Models/Project/ProjectResponseDto";
import { CreateProjectTaskRequestDto } from "../../../Models/Project/ProjectTaskRequestDto";

interface Props {}

const validation = Yup.object().shape({
  body: Yup.string()
    .required("Task cannot be empty")
    .min(3, "Task cannot be less than 3 characters")
    .max(50, "Task cannot be over 50 characters"),
});

const ShowProject = (props: Props) => {
  const navigate = useNavigate();
  const { projectId } = useParams();
  const [project, setProject] = useState<ProjectResponseDto>();
  const {
    register,
    handleSubmit,
    reset,
    formState: { errors },
  } = useForm<CreateProjectTaskRequestDto>({
    resolver: yupResolver(validation),
  });
  const { isAuthUser } = useAuth();

  useEffect(() => {
    getProject(projectId!);
  }, [projectId]);

  const getProject = async (projectId: string) => {
    try {
      const data = await Agent.Project.show(projectId);
      if (data) {
        setProject(data);
      }
    } catch (err) {
      toast.error("Could not get project!");
    }
  };

  const handleAddTask = async (body: CreateProjectTaskRequestDto) => {
    try {
      const res = await Agent.ProjectTask.create(projectId!, body);
      if (res) {
        toast.success("Task created successfully!");
        getProject(projectId!);
        reset();
      }
    } catch (e: any) {
      toast.warning(e);
    }
  };

  function handleDelete(): void {
    navigate("/projects");
  }

  return (
    <>
      <header className="flex items-center mb-3 py-4">
        <div className="flex justify-between items-end w-full">
          <p className="text-default text-sm font-normal">
            <Link to="/projects">My Projects</Link> / {project?.title}
          </p>

          <div className="flex items-center">
            {project?.members &&
              project?.members.map((member) => {
                return (
                  <img
                    key={member.email}
                    src={gravatar_url(member.email)}
                    alt={`${member.userName}'s avatar`}
                    className="rounded-full w-8 mr-2"
                  />
                );
              })}
            <img
              src={gravatar_url(project?.owner.email!)}
              alt={`${project?.owner.userName}'s avatar`}
              className="rounded-full w-8 mr-2"
            />
            <Link className="button ml-6" to={`/projects/${project?.id}/edit`}>
              Edit Project
            </Link>
          </div>
        </div>
      </header>

      <main>
        <div className="lg:flex -mx-3 mb-6">
          <div className="lg:w-3/4 px-3">
            <div className="mb-8">
              <h2 className="text-default font-normal text-lg mb-3">Tasks</h2>
              {project?.tasks &&
                project?.tasks?.map((task) => (
                  <div key={task.id} className="card mb-3">
                    <UpdateTaskForm
                      projectId={projectId!}
                      task={task}
                      getProject={getProject}
                    />
                  </div>
                ))}
              <div className="card mb-3">
                <form onSubmit={handleSubmit(handleAddTask)}>
                  <input
                    type="text"
                    placeholder="Begin adding tasks..."
                    className="bg-card text-default w-full"
                    {...register("body")}
                  />
                  {errors.body && (
                    <p className="text-error text-sm mt-2">
                      {errors.body.message}
                    </p>
                  )}
                </form>
              </div>
            </div>

            <div>
              <h2 className="text-default font-normal text-lg mb-3">
                General Notes
              </h2>
              {project && <GeneralNotes project={project} />}
            </div>
          </div>

          <div className="lg:w-1/4 px-3">
            {project && (
              <>
                <ProjectCard project={project} onDelete={handleDelete} />
                <ActivityCard project={project} />

                {isAuthUser(project.owner.email) && (
                  <InviteCard
                    projectId={project.id}
                    onInviteUser={() => {
                      getProject(project.id.toString());
                    }}
                  />
                )}
              </>
            )}
          </div>
        </div>
      </main>
    </>
  );
};

export default ShowProject;
