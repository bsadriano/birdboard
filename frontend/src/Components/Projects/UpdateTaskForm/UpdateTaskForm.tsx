import * as Yup from "yup";
import { ProjectTaskGet } from "../../../Models/ProjectTask";
import { yupResolver } from "@hookform/resolvers/yup";
import { useForm } from "react-hook-form";
import { toast } from "react-toastify";
import { projectTaskPatchAPI } from "../../../Services/ProjectTaskService";

interface Props {
  projectId: string;
  task: ProjectTaskGet;
  getProject: (projectId: string) => void;
}

type UdpateTaskFormInputs = {
  body?: string;
};

const validation = Yup.object().shape({
  body: Yup.string()
    .min(3, "Task cannot be less than 3 characters")
    .max(50, "Task cannot be over 50 characters"),
});

const UpdateTaskForm = ({ projectId, task, getProject }: Props) => {
  const {
    register,
    handleSubmit,
    reset,
    formState: { errors },
  } = useForm<UdpateTaskFormInputs>({
    defaultValues: {
      body: task.body,
    },
    resolver: yupResolver(validation),
  });

  const handleUpdateTask = ({ body }: UdpateTaskFormInputs) => {
    projectTaskPatchAPI(projectId, task.id, { body })
      .then((res) => {
        if (res) {
          getProject(projectId);
        }
      })
      .catch((e) => {
        toast.warning(e);
      });
  };

  const handleToggleCompleted = (e: any) => {
    projectTaskPatchAPI(projectId, task.id, { completed: e.target.checked })
      .then((res) => {
        if (res) {
          getProject(projectId);
        }
      })
      .catch((e) => {
        toast.warning(e);
      });
  };

  return (
    <form onSubmit={handleSubmit(handleUpdateTask)}>
      <div className="flex items-center">
        <div className="w-full">
          <input
            type="text"
            className={`w-full ${task.completed ? "text-grey" : ""}`}
            {...register("body")}
          />
          {errors.body && (
            <p className="text-red-400 text-sm">{errors.body.message}</p>
          )}
        </div>
        <input
          type="checkbox"
          checked={task.completed}
          onChange={handleToggleCompleted}
        />
      </div>
    </form>
  );
};

export default UpdateTaskForm;
