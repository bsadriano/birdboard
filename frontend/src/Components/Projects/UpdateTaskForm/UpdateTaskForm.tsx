import { yupResolver } from "@hookform/resolvers/yup";
import { useForm } from "react-hook-form";
import { toast } from "react-toastify";
import * as Yup from "yup";
import Agent from "../../../Api/Agent";
import { ProjectTaskDto } from "../../../Models/Project/ProjectTaskDto";
import { UpdateProjectTaskRequestDto } from "../../../Models/Project/ProjectTaskRequestDto";

interface Props {
  projectId: string;
  task: ProjectTaskDto;
  getProject: (projectId: string) => void;
}

const validation = Yup.object().shape({
  body: Yup.string()
    .min(3, "Task cannot be less than 3 characters")
    .max(50, "Task cannot be over 50 characters"),
});

const UpdateTaskForm = ({ projectId, task, getProject }: Props) => {
  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm<UpdateProjectTaskRequestDto>({
    defaultValues: {
      body: task.body,
    },
    resolver: yupResolver(validation),
  });

  const handleUpdateTask = async (body: UpdateProjectTaskRequestDto) => {
    await updateTask(body);
  };

  const handleToggleCompleted = async (e: any) => {
    await updateTask({
      completed: e.target.checked,
    });
  };

  const updateTask = async (body: UpdateProjectTaskRequestDto) => {
    try {
      const data = await Agent.ProjectTask.update(projectId, task.id, body);
      if (data) {
        getProject(projectId);
      }
    } catch (e: any) {
      toast.warning(e);
    }
  };

  return (
    <form onSubmit={handleSubmit(handleUpdateTask)}>
      <div className="flex items-center">
        <div className="w-full">
          <input
            type="text"
            className={`w-full bg-card ${
              task.completed ? "text-muted line-through" : "text-default"
            }`}
            {...register("body")}
          />
          {errors.body && (
            <p className="text-error text-sm mt-2">{errors.body.message}</p>
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
