import { yupResolver } from "@hookform/resolvers/yup";
import { useForm } from "react-hook-form";
import { Link } from "react-router-dom";
import { ProjectFormInputs } from "../../../Services/ProjectService";

interface Props {
  defaultValues: any;
  validation: any;
  onSubmit: any;
  projectId?: string;
  isEdit?: boolean;
}

const SaveProjectForm = ({
  defaultValues,
  validation,
  onSubmit,
  isEdit = false,
  projectId,
}: Props) => {
  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm<ProjectFormInputs>({
    defaultValues: defaultValues,
    resolver: yupResolver(validation),
  });
  return (
    <form onSubmit={handleSubmit(onSubmit)}>
      <div className="mb-6">
        <label className="block mb-2 text-sm text-default" htmlFor="title">
          Title
        </label>
        <div>
          <input
            type="text"
            className="w-full rounded border border-muted-light bg-card p-2 text-xs text-default"
            placeholder="My next awesome project"
            required
            {...register("title")}
          />
          {errors.title && (
            <p className="text-red-400 text-sm">{errors.title.message}</p>
          )}
        </div>
      </div>

      <div className="mb-6">
        <label
          className="block mb-2 text-sm text-default"
          htmlFor="description"
        >
          Description
        </label>
        <div>
          <textarea
            className="w-full rounded border border-muted-light bg-card p-2 text-xs text-default"
            rows={10}
            placeholder="I should start learning piano"
            required
            {...register("description")}
          />
          {errors.description && (
            <p className="text-red-400 text-sm">{errors.description.message}</p>
          )}
        </div>
      </div>

      <div>
        <button className="button mr-4" type="submit">
          {isEdit ? "Update Project" : "Create Project"}
        </button>
        <Link to={isEdit ? `/projects/${projectId}` : "/projects"}>Cancel</Link>
      </div>
    </form>
  );
};

export default SaveProjectForm;
