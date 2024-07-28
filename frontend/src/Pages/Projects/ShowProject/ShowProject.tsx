import { useEffect, useState } from "react";
import { Link, useNavigate, useParams } from "react-router-dom";
import { toast } from "react-toastify";
import * as Yup from "yup";
import ProjectCard from "../../../Components/Projects/ProjectCard/ProjectCard";
import { ProjectGet } from "../../../Models/Project";
import { projectGetAPI } from "../../../Services/ProjectService";
import { useForm } from "react-hook-form";
import { yupResolver } from "@hookform/resolvers/yup";
import { projectTaskPostAPI } from "../../../Services/ProjectTaskService";
import UpdateTaskForm from "../../../Components/Projects/UpdateTaskForm/UpdateTaskForm";
import GeneralNotes from "../../../Components/Projects/GeneralNotes/GeneralNotes";
import ActivityCard from "../../../Components/Projects/ActivityCard/ActivityCard";
import { gravatar_url } from "../../../Helpers/Gravatar_Url";
import InviteCard from "../../../Components/Projects/InviteCard/InviteCard";
import { useAuth } from "../../../Context/useAuth";

interface Props {}

type AddTaskFormInputs = {
  body?: string;
};

const validation = Yup.object().shape({
  body: Yup.string()
    .min(3, "Task cannot be less than 3 characters")
    .max(50, "Task cannot be over 50 characters"),
});

const ShowProject = (props: Props) => {
  const navigate = useNavigate();
  const { projectId } = useParams();
  const [project, setProject] = useState<ProjectGet>();
  const {
    register,
    handleSubmit,
    reset,
    formState: { errors },
  } = useForm<AddTaskFormInputs>({ resolver: yupResolver(validation) });
  const { isAuthUser } = useAuth();

  useEffect(() => {
    getProject(projectId!);
  }, [projectId]);

  const getProject = (projectId: string) => {
    projectGetAPI(projectId)
      .then((res) => {
        if (res?.data) {
          setProject(res?.data);
        }
      })
      .catch((error) => {
        toast.warning("Could not get project!");
      });
  };

  const handleAddTask = ({ body }: AddTaskFormInputs) => {
    projectTaskPostAPI(projectId!, body!)
      .then((res) => {
        if (res) {
          toast.success("Task created successfully!");
          getProject(projectId!);
          reset();
        }
      })
      .catch((e) => {
        toast.warning(e);
      });
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
                    <p className="text-red-400 text-sm">
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
